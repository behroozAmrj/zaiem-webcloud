using Cloud.Core.Models;
using Cloud.Core.Models.RBS;
using Cloud.Log.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;

namespace Cloud.IU.WEB.Services
{
    /// <summary>
    /// Summary description for GateWayService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class GateWayService1 : System.Web.Services.WebService
    {

        string callerApp;
        string requestApp;
        private Boolean IsAPIRequest(string AppName)
        {
            var result = false;
            var strarray = AppName.Split('/');
            if (strarray.Length > 1)
            {
                callerApp = strarray[0];
                requestApp = strarray[1];
                result = true;
            }
            return (result);
        }
       
        [WebMethod(EnableSession = true)]
        public object Execute_Method(string AppName, string ClassName, string MethodName, List<object> Params)
        {
            if ((string.IsNullOrEmpty(AppName)) &&
                 (string.IsNullOrEmpty(ClassName)) &&
                 (string.IsNullOrEmpty(MethodName)))
            {
                return (DateTime.Now.ToShortTimeString());
            }

            String userID = string.Empty;
            if (Session["userID"] != null)
                userID = Session["userID"].ToString();
            else
                return ("sesion timeout");

            if (IsAPIRequest(AppName)) // in this section checks if any other app wants execute other app sdk
            {
                var sdkproperty = new SDKProperty();
                sdkproperty.ApplicationName = this.requestApp;
                sdkproperty.Class = ClassName;
                sdkproperty.Method = MethodName;
                sdkproperty.Parameters = Params;
                var apiservice = new APIManagementService();
                apiservice.ClientApp = callerApp;
                apiservice.Property = sdkproperty;
                var result = apiservice.Execute_Method(sdkproperty);
                return (result);
            }

            else
            {
                var type = MethodBase.GetCurrentMethod().DeclaringType;
                string msg = string.Empty;
                string logmethod = MethodName;
                if (Params != null)
                    if (Params.Count > 0)
                    {
                        string resultformat = GenerateFormat(Params);
                        var arr = Params.ToArray();
                        string res = string.Format(resultformat,
                                                    arr);
                        logmethod = string.Format("{0}  {1}",
                                                      logmethod,
                                                      res);
                    }
                msg = string.Format("[Execute_Method] {0} {1} {2} ",
                                           AppName,
                                           ClassName,
                                           logmethod);

                Logging.SimpleINOFlogRegister(msg);
                //var sdkmgr = new SDKManager(AppName,
                //                            ClassName,
                //                            MethodName);
                //sdkmgr.Param = Params;
                //var result = sdkmgr.MethodExecute();
                var sdkMgr = new RBSinService(AppName,
                                              ClassName,
                                              MethodName,
                                              userID);
                sdkMgr.Param = Params;
                var result = sdkMgr.ExecuteMethod();
                if (result != null)
                    return (result);
                return ("no content");
            }

        }
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

        [WebMethod(EnableSession = true)]
        public object CloudService_ExecuteMethod(string AppName, string ServiceName, string ClassName, string MethodName, List<object> Params)
        {
            if ((string.IsNullOrEmpty(AppName)) &&
                (string.IsNullOrEmpty(ClassName)) &&
                (string.IsNullOrEmpty(MethodName)))
            {
                Logging.ErrorlogRegister("parametrs are null or empty",
                                            MethodBase.GetCurrentMethod().GetType(),
                                            null);
                object obj = new object();
                obj = DateTime.Now.ToShortTimeString();
                var olist = new List<object>();
                olist.Add(obj);
                return (olist);

            }

            string userID = string.Empty;
            if (Session["userID"] != null)
                userID = Session["userID"].ToString();
            var serviceing = new CloudServiceRequestCenter(AppName, userID);
            serviceing.ClassName = ClassName;
            serviceing.Method = MethodName;
            serviceing.Param = Params;
            serviceing.Servicename = ServiceName;
            var finalResult = serviceing.CallServiceMethod();
            //var result = new List<object>(outputResult as IList<object>);
            return (finalResult);
        }


    }
}
