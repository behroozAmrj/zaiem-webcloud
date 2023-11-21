using Cloud.Core.Models;
using Cloud.Foundation.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class ApplicationCatalogController : Controller
    {
        //
        // GET: /ApplicationCatalog/
        public ActionResult Load_Catalogs()
        {
            if (Session["userID"] != null)
            {
                try
                {
                    String userID = Session["userID"].ToString();
                    var catalogMgr = new CatalogManagement(userID);
                    var list = catalogMgr.getApplicationList();
                    ViewBag.userID = userID;
                    return (View(list));
                }
                catch (Exception ex)
                {

                    return (Content("some errors ocurred during operations, " + ex.Message));
                }
            }
            return null;
        }

        public ActionResult InstallApplicationForUser(String userId , String name)
        {
            if ((Session["userID"] != null) &&
                (Request.QueryString["appName"] != null)&&
                (Request.QueryString["appType"] != null))
            {
                try
                {
                    string userID = Session["userID"].ToString();
                    IAppCatalog app;
                    string appType = Request.QueryString["appType"].ToString();
                    String appName = Request.QueryString["appName"].ToString();
                    app = FactoryMethod_AppCatalog(userID, appType);
                    var catalogMgr = new CatalogManagement(userID);
                    app.Name = appName;
                    var list = catalogMgr.InstallAppforUser(app);
                    ViewBag.userID = userID;
                    return (View("Load_Catalogs", list));
                }
                catch (Exception ex)
                {
                    
                    return(Content("installing application for user failed by this error, " + ex.Message));
                }
            }
            return (RedirectToAction("Load_Catalogs"));
        }

        private static IAppCatalog FactoryMethod_AppCatalog(string userID, string appType)
        {
            IAppCatalog app;
            switch ((AppCatalogType)Enum.Parse(typeof(AppCatalogType), appType))
            {
                case AppCatalogType.ExistApp:
                    {
                        app = new ExistApp(userID);
                        break;
                    }
                case AppCatalogType.CloudApp:
                    {
                        app = new StreamCloudApp();
                        break;
                    }
                default:
                    {
                        app = new ExistApp(userID);
                        break;
                    }
            }
            return app;
        }

        public ActionResult RemoveAnCatalog(string catalogID)
        {
            if ((Session["userID"] != null) ||
                    (!string.IsNullOrEmpty(catalogID)))
            {

                try
                {
                    string userID = Session["userID"].ToString();
                    var catalogMgr = new CatalogManagement(userID);
                    catalogMgr.UnInstallCatalog(catalogID);
                    return (RedirectToAction("Load_Catalog"));
                }
                catch (Exception ex)
                {
                    return (Content(string.Format("some error occurred during: {0}" , ex.Message)));
                }

            }
            return(Content("your sesion is expired! please relogin and continue"));
        }
    }
}