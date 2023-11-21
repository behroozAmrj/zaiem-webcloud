using Cloud.Core.Models;
using Cloud.IU.WEB.App_Start;
using Cloud.Log.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Cloud.IU.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapHubs();
            AreaRegistration.RegisterAllAreas();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            /*
                        WebApiConfig.Register(GlobalConfiguration.Configuration);
                        RegisterRoutes(RouteTable.Routes);
                        BundleConfig.RegisterBundles(BundleTable.Bundles);*/
        }


        protected void Session_End(Object sender, EventArgs e)
        {
            if (Session["userID"] != null)
            {
                string userID = Session["userID"].ToString();
                Logging.INFOLogRegisterToDB(userID,
                                            "Logout",
                                            "info",
                                            "user session expired");
                var loginSecurity = new LoginSecurity_v1();
                loginSecurity.LogOut(userID);
            }
        }
    }
}
