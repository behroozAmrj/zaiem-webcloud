using Cloud.Core.Models;
using Cloud.IU.WEB.Hubs;
using Cloud.Log.Tracking;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class SecurityController : Controller
    {
        //
        // GET: /Security/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login()
        {


            string sessid = Session.SessionID;
            ViewBag.sessionid = sessid;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult WinLogin()
        {
            string sessid = Session.SessionID;
            ViewBag.sessionid = sessid;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void TestMethod()
        {
            int i = 10;
            Console.WriteLine("hello");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult NewLogin()
        {
            String refURL = String.Empty;
            if (Request.QueryString["refURL"] != null)
                refURL = Request.QueryString["refURL"].ToString();
            if (Session["userID"] != null)
            {
                var sessionUserID = Session["userID"].ToString();
                var loginSecurity = new LoginSecurity_v1();
                if (loginSecurity.IsAlive(sessionUserID))
                {
                    String url = String.Format("/Start/Start_Screen/{0}/{1}",
                                                "102030",
                                                sessionUserID);
                    return (Redirect(url));
                }
                else
                {
                    Logging.SimpleINOFlogRegister("New Log");
                    string sessid = Session.SessionID;
                    ViewBag.sessionid = sessid;
                    return View("mLogin");
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(refURL))
                {
                    String[] refArray = refURL.Split('/');
                    String returnPath = String.Format("/{0}/{1}/",
                                                    refArray[1],
                                                    refArray[2]);
                    ViewBag.returnPath = returnPath;
                }
                Logging.SimpleINOFlogRegister("New Log");
                string sessid = Session.SessionID;
                ViewBag.sessionid = sessid;
                return View("mLogin");
            }

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RegisterUserIDInSession(String userID)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                String value = userID;
                Session["userID"] = userID;
                return (Json(value));
            }
            else
            {
                return (null);
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SignUp_Load()
        {
            return (View());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RegisterUserInCloud(String userName, String passWord, String email)
        {
            String userID = String.Empty;
            if (Session["userID"] != null)
                userID = Session["userID"].ToString();
            else
                return (Content("you session is expired relogin and try again <a href=\"/Security/NewLogin?erc=user loggedOut\">relogin</a> "));
            if ((String.IsNullOrEmpty(userName)) ||
                (String.IsNullOrEmpty(passWord)) ||
                (String.IsNullOrEmpty(email)))
                return (Content("Error : one or more Parameters are null! try again. <a href=\"/SignUp_load\">retry </a>"));

            // in this block code goes to register user in cloud 
            try
            {
                var userSecurity = new LoginSecurity_v1();
                userSecurity.Register(userName,
                                        passWord,
                                        new string[] { userID, email });
                return (Content("you registered successfuly. <a href=\"/Security/WinLogin\">login now</a>"));
            }
            catch (Exception ex)
            {


                return (Content(String.Format("Error {0} occured during user registration please try again. <a href=\"\">reload</a>",
                                                ex.Message)));
            }



        }
    }
}