using MessageServer.Domain;
using MessageServer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MessageServer.Controllers
{
    [RoutePrefix("Maintenance")]
    public class MaintenanceController : ApiController
    {
        [HttpGet]
        [Route("ShowRoatTable")]
        public HttpResponseMessage Get()
        {
            try
            {
                var xmlData = ServiceManagementCenter.ServiceCenterMgr.getStorage();
                var list = xmlData.getAvailableServices();
                return (Request.CreateResponse(HttpStatusCode.OK,
                                                       list));
            }
            catch (Exception ex)
            {
                return (Request.CreateResponse(HttpStatusCode.ExpectationFailed,
                                       "some wrong done with this request: " + ex.Message));
            }
        }
        [HttpPost]
        [Route("RegisterService")]
        public void insertService()
        {
            string temp = Request.Content.ReadAsStringAsync().Result;
            var service = JsonConvert.DeserializeObject<Service>(temp);
            if (service != null)
            {
                var xData = ServiceManagementCenter.ServiceCenterMgr.getStorage();
                xData.RegisterService(service);
            }
        }

        [HttpGet]
        [Route("maintenance/showHeader")]
        public HttpResponseMessage showHeader()
        {
            var list = Request.Content.Headers.Select((x) => 
                                                    {
                                                          return (string.Format("{0}:{1}" ,
                                                                                    x.Key ,
                                                                                    x.Value));
                                                    });
            var head = HttpContext.Current.Request.Headers;
            //string headersVal =  Request.Headers["Content-Type"];

            return (Request.CreateResponse(HttpStatusCode.OK ,
                                            "ys"));
        }
    }
    
}
