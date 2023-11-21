using Cloud.IO.Repository;
using Cloud.Log.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    public class CloudServiceRequestCenter
    {
        private string appName;
        private LayoutConfigRepository layoutconfigRep;
        private string directory;
        private string applicationPath;
        private string servicename;

        public string Servicename
        {
            get { return servicename; }
            set { servicename = value; }
        }
        private string className;

        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }
        private string method;
        private string userID;
        public string Method
        {
            get { return method; }
            set { method = value; }
        }
        private List<object> param;

        public List<object> Param
        {
            get { return param; }
            set { param = value; }
        }
        public CloudServiceRequestCenter(string AppName , string userId  = null)
        {
            if (!string.IsNullOrEmpty(AppName))
            {
                if (string.IsNullOrEmpty(userId))
                    this.userID = "noUserID";
                else
                    this.userID = userId;
                appName = AppName;
                layoutconfigRep = new LayoutConfigRepository("Application-Service\\AppsConfig.xml");
                var appcon = layoutconfigRep.ReadOne("//root//applications//application[@name='" + AppName + "']");
                if (appcon == null)
                    throw new Exception("[Server].[SDKManager].[SDKManager] => application not found in AppsConfig ");
                directory = appcon.Directory;
                applicationPath = string.Format("{0}{1}\\{2}",
                                                      AppDomain.CurrentDomain.BaseDirectory,
                                                      "Application-Service",
                                                       directory);
            }
        }


        public object CallServiceMethod()
        {
            //Logging.INFOlogRegistrer(servicename,
            //                            this.userID,
            //                            MethodBase.GetCurrentMethod().GetType());
            try
            {
                var cloudservice = new CloudServicesConfig(this.directory);
                string xpath = "//applicationConfig//services//service";
                var serviceconfig = cloudservice.ReadOne(xpath,
                                                         "servicename",
                                                          servicename);
                if (serviceconfig == null)
                {
                    string msg = string.Format("[server].[CloudServiceRequestCenter].[CallServiceMethod] => service is not exist in config file ");
                    return (null);
                }
                var service = Service_FactoryMethod.Method(serviceconfig.ServiceType);
                if (service == null)
                {
                    string msg = string.Format("[server].[{0}].[CallServiceMethod] => service type {1} is not valid in Application ",
                                                    this.GetType().ToString(),
                                                    serviceconfig.ServiceType.ToString());
                    Logging.ErrorlogRegister(msg ,
                                                MethodBase.GetCurrentMethod().GetType(),
                                                null);
                }
                var serviceparameters = Service_FactoryMethod.MethodProperty(serviceconfig.ServiceType,
                                                                                serviceconfig.Url);
                serviceparameters.ClassName = className;
                serviceparameters.Method = method;
                if (param != null)
                    serviceparameters.Param = param.ToArray();
                else
                    serviceparameters.Param = null;
                var result = service.WS_ExecuteMethod(serviceparameters);
                return (result);

            }
            catch (Exception ex)
            {
                string emsg = string.Format("[server].[{0}].[CallServiceMethod] [{1}] => Calling Service  failed by this error {2} ",
                                                    this.GetType().ToString(),
                                                    servicename,
                                                    ex.Message);
                                                    
                Logging.ErrorlogRegister(emsg,
                                        MethodBase.GetCurrentMethod().GetType(),
                                        this.userID);
                return (null);
            }




        }


    }
}
