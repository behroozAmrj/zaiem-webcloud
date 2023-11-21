using Cloud.Foundation.Infrastructure;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.IO.WSControlCenter
{
    public class WCFService : IWebService
    {
        WebServiceConfigAndInfo _webservicePropertyconfig;

        public WebServiceConfigAndInfo WebservicePropertyconfig
        {
            get { return _webservicePropertyconfig; }
            set { _webservicePropertyconfig = value; }
        }
        public WCFService()
        {
        }
        public object WS_ExecuteMethod(WebServiceConfigAndInfo WebServicePropertyConfig)
        {
            if (WebServicePropertyConfig == null)
            {
                if (_webservicePropertyconfig == null)
                    throw new Exception("the webservice property is null or empty");
            }
            else
            { _webservicePropertyconfig = WebServicePropertyConfig; }

            var mexaddress = new Uri(_webservicePropertyconfig.Url);
            var mexmode = MetadataExchangeClientMode.HttpGet;
            string contractname =  _webservicePropertyconfig.ClassName;
            string oprationName =  _webservicePropertyconfig.Method;
            object[] param = _webservicePropertyconfig.Param;

            var mexClient = new MetadataExchangeClient(mexaddress,
                                                      mexmode);
            mexClient.ResolveMetadataReferences = true;
            var metaset = mexClient.GetMetadata();


            var importer = new WsdlImporter(metaset);
            var contracts = importer.ImportAllContracts();
            var allendpoint = importer.ImportAllEndpoints();


            var generator = new ServiceContractGenerator();
            var endpointforContracts = new Dictionary<string, IEnumerable<ServiceEndpoint>>();

            foreach (var contract in contracts)
            {
                generator.GenerateServiceContractType(contract);
                endpointforContracts[contract.Name] = allendpoint.Where(se => se.Contract.Name == contract.Name).ToList();
                if (generator.Errors.Count != 0)
                    throw new Exception(" there was an error during compilation ");

                var options = new CodeGeneratorOptions();
                options.BracingStyle = "D";
                var codeDemoProvider = CodeDomProvider.CreateProvider("C#");

                var compilerParameters = new CompilerParameters(new string[] {"System.dll",
                                                                                "System.ServiceModel.dll",
                                                                                "System.Runtime.Serialization.dll"});
                compilerParameters.GenerateInMemory = true;
                var results = codeDemoProvider.CompileAssemblyFromDom(compilerParameters,
                                                                        generator.TargetCompileUnit);
                if (results.Errors.Count > 0)
                    throw new Exception("there were an errors in duration of compilations");
                else
                {
                    var clientProxyType = results.CompiledAssembly.GetTypes().First(t => t.IsClass &&
                                                                                            t.GetInterface(contractname) != null &&
                                                                                            t.GetInterface(typeof(ICommunicationObject).Name) != null);
                    var se = endpointforContracts[contractname].First();



                    var instance = results.CompiledAssembly.CreateInstance(clientProxyType.Name,
                                                                                false,
                                                                                System.Reflection.BindingFlags.CreateInstance,
                                                                                null,
                                                                                new object[]{se.Binding,
                                                                                             se.Address},
                                                                                 CultureInfo.CurrentCulture,
                                                                                 null);

                    var reVal = instance.GetType().GetMethod(oprationName).Invoke(instance,
                                                                                    param);
                    return (reVal);
                }

            }

            return (null);
        }
    }
}
