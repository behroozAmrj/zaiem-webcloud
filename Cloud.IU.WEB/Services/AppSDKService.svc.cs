using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cloud.IU.WEB.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AppSDKService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AppSDKService.svc or AppSDKService.svc.cs at the Solution Explorer and start debugging.
    public class AppSDKService : IAppSDKService
    {
        public string DoWork()
        {
            return ("hello Wrold !");
        }
    }
}
