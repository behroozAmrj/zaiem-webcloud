using MessageServer.Domain;
using MessageServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace MessageServer.Controllers
{
    [RoutePrefix("router")]
    public class RouterController : ApiController
    {
        [HttpGet]
        [Route("ping")]
        public HttpResponseMessage Get()
        {
            string serverName = "ProxyServer";
            string datetime = DateTime.Now.ToString();
            var server = JsonConvert.SerializeObject(serverName);
            var time = JsonConvert.SerializeObject(datetime);

            var response = Request.CreateResponse(HttpStatusCode.OK,
                                                    new object[] { server, time });
            return (response);
        }


        [HttpPost]
        [Route("PostMsg/{*value}")]
        public HttpResponseMessage Post(string value)
        {
            Message message;
            string streamStr;
            var loginfo = new LogInfo();
            string[] param;
            if (!string.IsNullOrEmpty(value))
            {
                param = value.Split('/');
                if (param.Count() > 0)
                    loginfo.LogTrace = param[0].ToString();
            }
            loginfo.AppName = "ProxyService";
            loginfo.User_ID = "serviceUser";
            loginfo.Action = "postMsg";
            loginfo.DateTime = DateTime.Now;
            try
            {
                //Logging.INFOlogRegistrer("router/postMsg",
                //                        "PostMsg",
                //                        "info",
                //                        "new request");
                streamStr = Request.Content.ReadAsStringAsync().Result;

                if (string.IsNullOrEmpty(streamStr))
                    return (Request.CreateResponse(HttpStatusCode.BadRequest,
                                                    "no content is included!"));
                message = JsonConvert.DeserializeObject<Message>(streamStr);//Request.Content.ReadAsAsync<Message>().Result;
                if (message == null)
                    return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                                    "failed!"));
                Logging.registerTransaction(message,
                                            "PostMsg");
                loginfo.Content = message.Content;
                var valid = new Validation(message);

            }
            catch (Exception ex)
            {
                
                loginfo.ActionType = "exception";
                loginfo.Content = ex.Message;
                ServiceManagementCenter.ServiceCenterMgr.getLogStorage().RegisterLog(loginfo);
                return (Request.CreateResponse(HttpStatusCode.NotImplemented,
                                               ex.Message));
            }
            // if (!valid.isValid())
            //     throw new Exception(valid.messageText);
            try
            {
                var sender = new RestSender();
                sender.Send(message);
                loginfo.ActionType = "info";
                ServiceManagementCenter.ServiceCenterMgr.getLogStorage().RegisterLog(loginfo);
                return (Request.CreateResponse(HttpStatusCode.OK,
                                                   message));
            }
            catch (Exception ex)
            {
                loginfo.ActionType = "exception";
                loginfo.Content = ex.Message;
                ServiceManagementCenter.ServiceCenterMgr.getLogStorage().RegisterLog(loginfo);
                return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                               "after sending" + ex.Message));
            }
        }

        [HttpPost]
        [Route("PostMsgCallBack/{*value}")]
        public HttpResponseMessage callBackHandler(string value)
        {
            string[] param;
            var loginfo = new LogInfo();
            if (!string.IsNullOrEmpty(value))
            {
                param = value.Split('/');
                if (param.Count() > 0)
                    loginfo.LogTrace = param[0].ToString();
            }

            Message message;
            string strStream;
            loginfo.AppName = "ProxyService";
            loginfo.User_ID = "serviceUser";
            loginfo.Action = "PostMsgCallBack";
            loginfo.DateTime = DateTime.Now;
            try
            {
                //Logging.INFOlogRegistrer("router/postMsg",
                //                      "PostMsgCallback",
                //                      "info",
                //                      "new request");

                strStream = Request.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(strStream))
                    return (Request.CreateResponse(HttpStatusCode.BadRequest,
                                                    "no content is included!"));
                message = JsonConvert.DeserializeObject<Message>(strStream);//Request.Content.ReadAsAsync<Message>().Result;
                if (message == null)
                    return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                                    "failed!"));
                var callBackSender = new RestSenderCallBack();
                var result = callBackSender.Send(message);
                loginfo.Content = message.Content;
                loginfo.ActionType = "info";
                ServiceManagementCenter.ServiceCenterMgr.getLogStorage().RegisterLog(loginfo);
                return (Request.CreateResponse(HttpStatusCode.OK,
                                                   result));
            }
            catch (Exception ex)
            {
                //Logging.INFOlogRegistrer("router/postMsg",
                //                              "postMessageCallback",
                //                                "Exception",
                //                              ex.Message);
                loginfo.ActionType = "exception";
                loginfo.Content = ex.Message;
                ServiceManagementCenter.ServiceCenterMgr.getLogStorage().RegisterLog(loginfo);
                return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                               "after sending: " + ex.Message));
            }
        }

        [HttpPost]
        [Route("testBody")]
        public void testBody()
        {
            string inputStream = Request.Content.ReadAsStringAsync().Result;
            var body = JsonConvert.DeserializeObject<body>(inputStream);

        }
    }

    public class body
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}