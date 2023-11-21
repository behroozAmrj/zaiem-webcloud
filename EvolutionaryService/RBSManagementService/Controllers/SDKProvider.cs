using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RBSManagementService.LojicModel;
using RBSManagementService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RBSManagementService.Controllers
{
    public class SDKProviderController : ApiController
    {
        [HttpPost]
        [Route("SDK/ExecuteMethod")]
        public void executeMethod()
        {

        }

        [HttpGet]
        [Route("SDK/GetApps/{*UserID}")]
        public HttpResponseMessage GetAppsList(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))

                return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                            "no user ID is provided"));
            try
            {
                var sdkMgr = new SDKManager();
                return (Request.CreateResponse(HttpStatusCode.OK,
                                            sdkMgr.ListAllApplication(UserID)));
            }
            catch (Exception ex)
            {
                return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                            ex.Message));
            }
        }


        [HttpPost]
        [Route("SDK/ExecuteRBS/{*UserID}")]
        public HttpResponseMessage Run(string UserID)
        {
            int cnt = Request.Content.Headers.Count();
            if (string.IsNullOrEmpty(UserID))
                return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                            "no user ID is provided"));
            var result = Request.Content.ReadAsStringAsync().Result;
            if (string.IsNullOrEmpty(result))
                return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                         "null application provided info"));
            var desStream = JsonConvert.DeserializeObject(result);
            var context = JObject.Parse(desStream.ToString())["content"];
            if (string.IsNullOrEmpty(context.ToString()))
                throw new Exception("content is null or empty!");

            var rbsRunInfo = JsonConvert.DeserializeObject<RbsRequestInfo>(context.ToString());
            if (rbsRunInfo == null)
                return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                         "can not convert stream to RBS object!"));
            try
            {
                var sdkExec = new SDKExecuter(rbsRunInfo.AppName,
                                                   rbsRunInfo.ClassName,
                                                   rbsRunInfo.MethodName,
                                                   UserID);
                sdkExec.ConstructorParams = rbsRunInfo.contructParams;
                sdkExec.param = rbsRunInfo.param;
                var res = sdkExec.MethodExecute();
                return (Request.CreateResponse(HttpStatusCode.OK,
                                                res));
            }
            catch (Exception ex)
            {
                return (Request.CreateResponse(HttpStatusCode.InternalServerError,
                                                ex.Message));
            }
        }
    }
}