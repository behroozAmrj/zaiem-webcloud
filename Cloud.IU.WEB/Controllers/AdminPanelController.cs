using Cloud.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class AdminPanelController : Controller
    {
        //
        // GET: /AdminPanel/
        public ActionResult Load()
        {

            String role;
            String userID ;
            if (Session["userID"] != null)
            {
                userID = Session["userID"].ToString();
                var userSecurity = new LoginSecurity_v1();
                role = userSecurity.getUserRole(userID);
                if (role != "admin")
                    return(Redirect("/Start/logOut/sr04n43ja4urz3cqxmpybzgb/d8ccf5a0be9c4a3ea5ca11a77716af7a"));
            }

            return View();
        }

        public ActionResult Logging()
        {
            String role;
            String userID;
            if (Session["userID"] != null)
            {
                userID = Session["userID"].ToString();
                var userSecurity = new LoginSecurity_v1();
                role = userSecurity.getUserRole(userID);
                //if (role != "admin")
                //    return (Redirect("/Start/logOut/sr04n43ja4urz3cqxmpybzgb/d8ccf5a0be9c4a3ea5ca11a77716af7a"));
                ViewBag.role = role;
                var logList = Cloud.Log.Tracking.Logging.ShowLogs();
                return View(logList);
            }
            return (Redirect("http://localhost:5146/"));
        }

        public ActionResult SearchUser(String UserID)
        {
            String role;
            String userID;
            if (Session["userID"] != null)
            {
                userID = Session["userID"].ToString();
                var userSecurity = new LoginSecurity_v1();
                role = userSecurity.getUserRole(userID);
                if (role != "admin")
                    return (Redirect("/Start/logOut/sr04n43ja4urz3cqxmpybzgb/d8ccf5a0be9c4a3ea5ca11a77716af7a"));

                var logList = Cloud.Log.Tracking.Logging.ShowLogs(UserID);
                return (View("Logging", logList));
            }
            return (View());
        }
	}
}