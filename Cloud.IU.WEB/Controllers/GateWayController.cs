using Cloud.Core.Models;
using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.Hubs;
using Cloud.Log.Tracking;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class GateWayController : Controller
    {
        //
        // GET: /GateWay/
        [AcceptVerbs(HttpVerbs.Post)]
        //[Route("gateWay/get/{path}")]
        public void Get()
        {

            string userID = string.Empty;
            try
            {
                //Logging.INFOlogRegistrer("getRequest",
                //                          "noID",
                //                          MethodBase.GetCurrentMethod().DeclaringType);

                if (Request.InputStream.Length == 0)
                    throw new System.Web.Http.HttpResponseException(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));
                using (var iStream = Request.InputStream)
                {
                    if (iStream == null)
                        return;
                    var sReader = new StreamReader(iStream);
                    string bodyContent = sReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(bodyContent))
                    {
                        var jval = JsonConvert.DeserializeObject(bodyContent);

                        var jobject = JObject.Parse(jval.ToString())["content"];
                        string connectionID = (string)jobject["ReceiverUserID"];
                        string recieveraApp = (string)jobject["RecieverApp"];
                        var hub = GlobalHost.ConnectionManager.GetHubContext<Notifications>();
                        if (Session["userID"] != null)
                            userID = Session["userID"].ToString();
                        else
                            userID = connectionID;
                        Logging.INFOLogRegisterToDB(userID,
                                                    "GateWay.get",
                                                    "info",
                                                    "incoming msg");
                        hub.Clients.Client(connectionID).appSwitch(recieveraApp,
                                                                        jobject);
                    }
                }
            }
            catch (Exception ex)
            {

                Logging.INFOLogRegisterToDB(userID,
                                           "GateWay.get",
                                           "exception",
                                           ex.Message);
            }
            //after this line creating appropriate class required.
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public void Post()
        //{
        //    if (Request.InputStream.Length == 0)
        //        throw new System.Web.Http.HttpResponseException(new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));
        //    using (var iStream = Request.InputStream)
        //    {
        //        var sReader = new StreamReader(iStream);
        //        string bodyContent = sReader.ReadToEnd();
        //        //var jval = JsonConvert.DeserializeObject(bodyContent);

        //        //var jobject = JObject.Parse(jval.ToString())["content"];
        //        string connectionID =bodyContent ;//(string)jobject["UserID"];
        //        var hub = GlobalHost.ConnectionManager.GetHubContext<Notifications>();

        //        hub.Clients.Client(connectionID).alarm("hello");

        //    }
        //}


        [AcceptVerbs(HttpVerbs.Post)]
        public void PostMessage(string serviceName, string methodName, string content)
        {
            if ((string.IsNullOrEmpty(serviceName)) ||
                 (string.IsNullOrEmpty(methodName)))
                return;
            var pMsg = new ProxyMessage();
            pMsg.DestinationURL = serviceName;
            pMsg.Method = methodName;
            pMsg.Content = content;
            pMsg.DateTime = DateTime.Now;

            using (var apiService = new APIServiceManagement(pMsg))
            {
                apiService.PostData();
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string getCurrentUserID()
        {
            if (Session["userID"] != null)
            {
                string userID = Session["userID"].ToString();
                return (userID);
            }
            else
                return (string.Empty);
        }
    }
}