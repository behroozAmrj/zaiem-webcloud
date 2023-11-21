using Cloud.Foundation.Infrastructure;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace Cloud.IO.WSControlCenter
{
    public class AsmxService : IWebService
    {
        string urlPattern = @"^(((ht|f)tp(s?))\://)?((([a-zA-Z0-9_\-]{2,}\.)+[a-zA-Z]{2,})|((?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(\.?\d)\.)){4}))(:[a-zA-Z0-9]+)?(/[a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~]*)?$";
        public object WS_ExecuteMethod(WebServiceConfigAndInfo WebServicePropertyConfig)
        {
            var regex = new Regex(urlPattern);
            if (!regex.IsMatch(WebServicePropertyConfig.Url))
            {
                string msg = string.Format("[Server].[{0}].[WS_ExecuteMethod] the url {1} is not in well format or is not valid",
                                                this.GetType().ToString(),
                                                WebServicePropertyConfig.Url);
                //Logging.SimpleErrorlogRegister(msg);
                return (null);

            }
            var client = new WebClient();
            var stream = client.OpenRead(WebServicePropertyConfig.Url);
            var description = ServiceDescription.Read(stream);
            var importer = new ServiceDescriptionImporter();
            string classname = WebServicePropertyConfig.ClassName;
            string method = WebServicePropertyConfig.Method;
            var param = WebServicePropertyConfig.Param;

            importer.ProtocolName = "Soap12";
            importer.AddServiceDescription(description,
                                            null,
                                            null);
            importer.Style = ServiceDescriptionImportStyle.Client;
            importer.CodeGenerationOptions = System.Xml.Serialization.CodeGenerationOptions.GenerateProperties;
            var nmspace = new CodeNamespace();
            var unite1 = new CodeCompileUnit();
            unite1.Namespaces.Add(nmspace);
            var warring = importer.Import(nmspace,
                                           unite1);
            if (warring == 0)
            {
                var provider1 = CodeDomProvider.CreateProvider("C#");
                var assemblyRef = new string[]{"System.Web.Services.dll" ,
                                               "System.Xml.dll",
                                               "System.Runtime.Serialization.dll"};
                var comparam = new CompilerParameters(assemblyRef);
                var result = provider1.CompileAssemblyFromDom(comparam,
                                                                unite1);

                var wsvcClass = result.CompiledAssembly.CreateInstance(classname);
                var mi = wsvcClass.GetType().GetMethod(method);
                var output = mi.Invoke(wsvcClass,
                                           param);
                return (output);
            }
            return ("can not connect to the server");
        }
    }
}
