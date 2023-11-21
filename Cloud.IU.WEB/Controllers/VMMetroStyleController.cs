using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class VMMetroStyleController : Controller
    {
        //
        // GET: /VMMetroStyle/
        public ActionResult VMLoadMetroApps(string SessionID, string userID, string appName)
        {
            String dependence = string.Empty;
            if (Request.QueryString["hdr"] != null)
                dependence = Request.QueryString["hdr"].ToString();
            ViewBag.sessionID = SessionID;
            ViewBag.userID = userID;
            ViewBag.appName = menuItemSwitch(appName);
            if (!String.IsNullOrEmpty(dependence))
                ViewBag.dependence = dependence;
            return View();
        }

        private String menuItemSwitch(String appName)
        {
            if (String.IsNullOrEmpty(appName))
                return ("/Menuitemui/Storage/zDrive.html");
            switch (appName.ToLower().Trim())
            {
                case "storage": { return ("/Menuitemui/Storage/zDrive.html"); break; }
                case "rbsmgr": { return ("/Menuitemui/RBS/RBSmgr.html"); break; }
                case "vmmgr": { return ("/Menuitemui/machineMgr/machineMgr.html"); break; }
                default: { return ("/Menuitemui/Storage/zDrive.html"); }
            }

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult retrieveMachineList()
        {
            String sessionUserID = String.Empty;
            if (Session["userID"] != null)
                sessionUserID = Session["userID"].ToString();
            else
                Redirect("/Security/NewLogin?erc=user loggedOut");
            try
            {
                var serverList = Cloud.Core.Models.STRepository.StrRepository.GetZserverList(sessionUserID);
                return (Json(serverList));
            }
            catch (Exception e)
            {
                return (null);
            }

        }
    }
}