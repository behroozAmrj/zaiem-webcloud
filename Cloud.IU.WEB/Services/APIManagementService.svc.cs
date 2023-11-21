using Cloud.Core.Models;
using Cloud.IU.WEB.InfraSructure;
using Cloud.Log.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cloud.IU.WEB.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "APIManagementService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select APIManagementService.svc or APIManagementService.svc.cs at the Solution Explorer and start debugging.
    public class APIManagementService : IAPIManagementService
    {
        string client;
        string appName;
        string className;
        string method;
        List<object> param;

        SDKProperty property;

        public SDKProperty Property
        {
            get { return (this.property); }
            set { this.property = value; }
        }
        public APIManagementService()
        {

        }

        public string ClientApp { get { return (client); } set { this.client = value; } }

        private string GenerateFormat(List<object> Params)
        {
            var lenght = Params.Count;
            string result = string.Empty;
            string tresult = string.Empty;
            for (int i = 0; i < lenght; i++)
            {
                tresult += " [{" + i.ToString() + "}] ";
            }
            result = tresult;
            return (result);
        }




        public object Execute_Method(SDKProperty Property)
        {

            if (Property == null)
            {
                string eMsg = string.Format("[Server.Service].[APIManagementService].[ExecuteMethod] {0} => property of executemethod is not provided ",
                                                                                                    this.client);
                Logging.ErrorlogRegister("not Property setted",
                                        MethodBase.GetCurrentMethod().GetType(),
                                        null);
                return (null);

            }
            else
            {
                if (!string.IsNullOrEmpty(Property.ApplicationName))
                {
                    this.appName = Property.ApplicationName;
                }
                else
                {
                    string eMsg = string.Format("[Server.Service].[APIManagementService].[ExecuteMethod] {0} => property Application Name of executemethod is not provided ",
                                                                                                     this.client);
                    Logging.ErrorlogRegister("not applicationName setted",
                                        MethodBase.GetCurrentMethod().GetType(),
                                        null);
                    return (null);
                }

                if (!string.IsNullOrEmpty(Property.Class))
                { this.className = Property.Class; }
                else
                {
                    string eMsg = string.Format("[Server.Service].[APIManagementService].[ExecuteMethod] {0} => property Class Name of executemethod is not provided ",
                                                                                                     this.client);
                    Logging.ErrorlogRegister("not Class setted",
                                        MethodBase.GetCurrentMethod().GetType(),
                                        null);
                    return (null);
                }

                if (!string.IsNullOrEmpty(Property.Method))
                { this.method = Property.Method; }
                else
                {

                    string eMsg = string.Format("[Server.Service].[APIManagementService].[ExecuteMethod] {0} => property Method Name of executemethod is not provided ",
                                                                                                     this.client);
                    Logging.ErrorlogRegister("not Method setted",
                                        MethodBase.GetCurrentMethod().GetType(),
                                        null);
                    return (null);
                }

                if (Property.Parameters != null)
                {
                    this.param = Property.Parameters;

                    string emsg = string.Format("[Server.Service].[ApiManagement].[Execute_Method] => {0} {1} {2} Are SDKProperty clinet[{3}] ",
                                                    this.appName,
                                                    this.className,
                                                    this.method,
                                                    this.client);
                }
                else
                {
                    var emsg = "[Server.Service].[ApiManagement].[Execute_Method] => No Prarameters are Set";
                    Logging.SimpleWarringRegister(emsg);
                }
            }

            try
            {
                var type = MethodBase.GetCurrentMethod().DeclaringType;
                string msg = string.Empty;
                string logmethod = this.method;
                if (this.param != null)
                    if (this.param.Count > 0)
                    {
                        string resultformat = GenerateFormat(this.param);
                        var arr = this.param.ToArray();
                        string res = string.Format(resultformat,
                                                    arr);
                        logmethod = string.Format("{0}  {1}",
                                                      logmethod,
                                                      res);
                    }
                msg = string.Format("[Execute_Method] {0} [= {1} {2} {3} ",
                                            this.client,
                                            this.appName,
                                           this.className,
                                           logmethod);

                Logging.SimpleINOFlogRegister(msg);
                var sdkmgr = new SDKManager(this.appName,
                                            this.className,
                                            this.method);
                sdkmgr.Param = this.param;
                var result = sdkmgr.MethodExecute();
                if (result != null)
                    return (result);
                return (DateTime.Now.ToShortTimeString());
            }
            catch (Exception ex)
            {

                var esmg = string.Format("[Server.Service].[ApiManagement].[Execute_Method] Method Execution Stop by this error {0} ",
                                               ex.Message);
                Logging.ErrorlogRegister(ex.Message,
                                        MethodBase.GetCurrentMethod().GetType(),
                                        null);
                return (DateTime.Now.ToShortTimeString());
            }

        }
    }
}
#region comment
//public APIManagementService(ISDKProperties SDKProperty, string ClientApp)
//{
//    if (SDKProperty != null)
//    {
//        if ((string.IsNullOrEmpty(SDKProperty.ApplicationName)) ||
//             string.IsNullOrEmpty(SDKProperty.Class) ||
//             string.IsNullOrEmpty(SDKProperty.Method))
//        {
//            var msg = string.Format("[Server.Service].[APIManagement].[ctor] => sdk parameters are null ");
//            Logging.SimpleErrorlogRegister(msg);
//            throw new Exception(msg);
//        }
//        this.client = ClientApp;
//        this.appName = SDKProperty.ApplicationName;
//        this.className = SDKProperty.Class;
//        this.method = SDKProperty.Method;
//        this.param = SDKProperty.Parameters;
//        string msgs = string.Format("[Server.Service].[APIManagement].[ctor] => this service create successfuly by from Application {0} ",
//                                    this.client);
//        Logging.SimpleINOFlogRegister(msgs);
//    }
//    else
//    {
//        var msg = string.Format("[Server.Service].[APIManagement].[ctor] => sdk Object is null ");
//        Logging.SimpleErrorlogRegister(msg);
//        throw new Exception(msg);
//    }
//}

//public object ExecuteMethod()
//{
//if (this.property == null)
//{
//    string eMsg = string.Format("[Server.Service].[APIManagementService].[ExecuteMethod] {0} => property of executemethod is not provided ",
//                                                                                        this.client);
//    Logging.SimpleErrorlogRegister(eMsg);
//    return (null);

//}
//var type = MethodBase.GetCurrentMethod().DeclaringType;
//string msg = string.Empty;
//string logmethod = this.method;
//if (this.property.Parameters.Count > 0)
//{
//    string resultformat = GenerateFormat(this.param);
//    var arr = this.property.Parameters.ToArray();
//    string res = string.Format(resultformat,
//                                arr);
//    logmethod = string.Format("{0}  {1}",
//                                  logmethod,
//                                  res);
//}
//msg = string.Format("[Execute_Method] {0} [= {1} {2} {3} ",
//                            this.client,
//                            this.appName,
//                           this.className,
//                           logmethod);

//Logging.SimpleINOFlogRegister(msg);
//var sdkmgr = new SDKManager(this.appName,
//                            this.className,
//                            this.method);
//sdkmgr.Param = this.param;
//var result = sdkmgr.MethodExecute();
//if (result != null)
//    return (result);
//return (DateTime.Now.ToShortTimeString());
//}

//public object ExecuteMethod()
//{
//    if (this.property == null)
//    {
//        string eMsg = string.Format("[Server.Service].[APIManagementService].[ExecuteMethod] {0} => property of executemethod is not provided ",
//                                                                                            this.client);
//        Logging.SimpleErrorlogRegister(eMsg);
//        return (null);

//    }
//    var type = MethodBase.GetCurrentMethod().DeclaringType;
//    string msg = string.Empty;
//    string logmethod = this.Property.Method;
//    if (this.property.Parameters.Count > 0)
//    {
//        string resultformat = GenerateFormat(this.property.Parameters);
//        var arr = this.property.Parameters.ToArray();
//        string res = string.Format(resultformat,
//                                    arr);
//        logmethod = string.Format("{0}  {1}",
//                                      logmethod,
//                                      res);
//    }
//    msg = string.Format("[Execute_Method] {0} [= {1} {2} {3} ",
//                                this.client,
//                                this.property.ApplicationName,
//                               this.property.Class,
//                               logmethod);

//    Logging.SimpleINOFlogRegister(msg);
//    var sdkmgr = new SDKManager(this.property.ApplicationName,
//                                this.property.Class,
//                                this.property.Method);
//    sdkmgr.Param = this.property.Parameters;
//    var result = sdkmgr.MethodExecute();
//    if (result != null)
//        return (result);
//    return (DateTime.Now.ToShortTimeString());
//}
#endregion
