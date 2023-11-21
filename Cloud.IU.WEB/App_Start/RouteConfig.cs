using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cloud.IU.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Security", action = "NewLogin", id = UrlParameter.Optional }
            );
            routes.MapRoute(name: "Main",
                            url: "{controller}/{action}/{SessionID}/{UserID}",
                            defaults: new
                            {
                                controller = "VMManagement",
                                action = "Main",
                                SessionID = UrlParameter.Optional,
                                UserID = UrlParameter.Optional
                            });


            routes.MapRoute(name: "DeskTop",
                url: "{controller}/{action}/{SessionID}/{UserID}/{appName}",
                defaults: new
                {
                    controller = "VMManagement",
                    action = "Main",
                    SessionID = UrlParameter.Optional,
                    UserID = UrlParameter.Optional,
                    appName = UrlParameter.Optional
                });

            routes.MapRoute("NotFound",
                            "{*url}",
                            new { Controller = "Error", action = "PageNotFound" });

        }
    }
}
