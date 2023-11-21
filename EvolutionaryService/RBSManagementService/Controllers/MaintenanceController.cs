using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace RBSManagementService.Controllers
{
   [RoutePrefix("Maintenance")]
    public class MaintenanceController : ApiController
    {
        [HttpGet]
        [Route("Ping")]
        public HttpResponseMessage ping()
        {
            string response = string.Format("RBS Management service ,{0}", 
                DateTime.Now.ToShortTimeString());
            return (Request.CreateResponse(HttpStatusCode.OK,
                                           new object[] { response }));
        }

        [HttpGet]
        [Route("showAppFile")]
        public HttpResponseMessage showApp()
        {
            string path = HttpContext.Current.Server.MapPath("/Data");
            if (File.Exists(path + "/AppData.xml"))
                return (Request.CreateResponse(HttpStatusCode.OK,
                                        path));
            else
                return (Request.CreateResponse(HttpStatusCode.OK,
                                        "file not found!"));
        }
    }
}