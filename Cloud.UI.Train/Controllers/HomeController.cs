using Cloud.UI.Train.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud.UI.Train.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var hubs = GlobalHost.ConnectionManager.GetHubContext<Master>();
            hubs.Clients.All.SendBackResponse("test",
                                           "tty");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
    }
}