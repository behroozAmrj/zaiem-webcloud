using Cloud.Core.Models;
using Cloud.IU.WEB.Hubs;
using Cloud.Log.Tracking;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class StartController : Controller
    {
        //
        // GET: /Start/
        public ActionResult Start_Screen(string SessionID, string UserID)
        {
            String sessionUserID = string.Empty;
            if (Session["userID"] != null)
                sessionUserID = Session["userID"].ToString();

            if ((!string.IsNullOrEmpty(SessionID) ||
                (!string.IsNullOrEmpty(UserID))))
            {
                if (sessionUserID != UserID)
                    return (Redirect("/Security/NewLogin?erc=can not logon."));
                if (!String.IsNullOrEmpty(SessionID))
                    ViewBag.sessionID = SessionID;
                if (!String.IsNullOrEmpty(UserID))
                    ViewBag.userID = UserID;
                var userSecurity = new LoginSecurity_v1();
                String role = userSecurity.getUserRole(sessionUserID);
                ViewBag.role = role;
                return View();
            }
            else
                return (Redirect("/Security/NewLogin?erc=can not logon."));
        }

        public ActionResult logOut()//(String sessionID, String userID)
        {
          

            String refererrURL;
            String userID = string.Empty;
            if (Session["userID"] != null)
                userID = Session["userID"].ToString();
            if (!String.IsNullOrEmpty(userID))
            {
                Session["userID"] = null;
                Logging.INFOLogRegisterToDB(userID,
                                            "Logout",
                                            "info",
                                            "logout from webcloud");
                var loginSecurity = new LoginSecurity_v1();
                loginSecurity.LogOut(userID);

            }
            refererrURL = Request.UrlReferrer.AbsolutePath;
            return (Redirect(String.Format("/Security/NewLogin?erc=user loggedOut&refURL={0}", refererrURL)));

        }
    }
}