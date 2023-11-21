using Cloud.Foundation.Infrastructure;
using Cloud.IO.WSControlCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    static class Service_FactoryMethod
    {
        public static IWebService Method(ServiceType ServiceType)
        {
            if (ServiceType == null)
                return (null);
            switch (ServiceType)
            {
                case ServiceType.WCFService:
                    { return ( new WCFService() ); break; }
                case ServiceType.asmx:
                    { return (new AsmxService ()); break; }
                default: return (null);
            }
        }


        public static WebServiceConfigAndInfo MethodProperty(ServiceType ServiceType , string URL)
        {
            switch (ServiceType)
            {
                case ServiceType.WCFService:
                    { return (new WCFConfig(URL)); break; }
                case ServiceType.asmx:
                    { return (new WS_ServiceConfigAndInfo(URL)); break; }
                default: return (null);
            }
        }
   
    }
}
