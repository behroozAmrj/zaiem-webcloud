using ChatService.Domain;
using ChatService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatService.Controllers
{
    public class ChatProxyController : ApiController
    {
        /*[HttpGet]
        [Route("ChatService/Get/{*tail}")]
        public HttpResponseMessage Get(string tail)
        {
            string result = "hello world";
            return (Request.CreateResponse(HttpStatusCode.OK,
                                                    result));
        }
        */

        [HttpGet]
        [Route("ChatService/ping")]
        public HttpResponseMessage Get()
        {
            string serverName = "ChatService";
            string datetime = DateTime.Now.ToString();
            var server = JsonConvert.SerializeObject(serverName);
            var time = JsonConvert.SerializeObject(datetime);
            var response = Request.CreateResponse(HttpStatusCode.OK,
                                                    new object[] { server, time });
            return (response);
        }

        [HttpPost]
        [Route("ChatService/PostMessage")]
        public void Post()
        {
            try
            {
                var result = Request.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(result))
                    throw new Exception("body is null or empty");
                var jval = JsonConvert.DeserializeObject (result);
                var parser = JObject.Parse(jval.ToString())["content"];
                if (string.IsNullOrEmpty(parser.ToString()))
                    throw new Exception("content is not exist in body");

                var msg = JsonConvert.DeserializeObject<Message>(parser.ToString()); // JsonConvert.DeserializeObject<Message>((string)msgStr["content"]);
                if (msg == null)
                    throw new Exception("body is nul or empty");
                var validation = new Validation(msg);

                var mesBuilder = new MessageBuilder();
                mesBuilder.Send(msg);
                var response = Request.CreateResponse(HttpStatusCode.OK,
                                                        msg);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }
        // var msgStr = JObject.Parse(result);
        // Request.Content.ReadAsStringAsync().Result;

    }
}