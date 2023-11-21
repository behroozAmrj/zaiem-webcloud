using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Cloud.IU.WEB.Services
{
    /// <summary>
    /// Summary description for myService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class myService : System.Web.Services.WebService
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
            /*
            String userID = string.Empty;
            if (Session["userID"] != null)
                userID = Session["userID"].ToString();
            else
                return (DateTime.Now.ToShortTimeString());

            if (IsAPIRequest(AppName))
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
                var sdkmgr = new SDKManager(AppName,
                                            ClassName,
                                            MethodName);
                sdkmgr.Param = Params;
                var result = sdkmgr.MethodExecute();
                if (result != null)
                    return (result);
                return (DateTime.Now.ToShortTimeString());
            }
             * */
            return ("hello");
        }
    }
}
