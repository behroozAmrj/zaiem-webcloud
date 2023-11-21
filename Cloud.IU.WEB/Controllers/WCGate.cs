using Cloud.Log.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace Cloud.IU.WEB.Controllers
{
    public class WCGateController : ApiController
    {
        [HttpGet]
        //[Route("log")]
        public void Get()
        {
            Logging.INFOlogRegistrer("Getting request in gateWay ",
                                                "UserID",
                                                MethodBase.GetCurrentMethod().DeclaringType);
        }

    }
}