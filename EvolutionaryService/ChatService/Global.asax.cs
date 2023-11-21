using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ChatService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Task.Factory.StartNew(() => {
                for(int i = 0; i < 10; i++)
                {

                }

            });
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
