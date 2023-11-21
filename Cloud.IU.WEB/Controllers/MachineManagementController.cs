using Cloud.Core.Models;
using net.openstack.Core.Domain;
using net.openstack.Providers.Zstack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class MachineManagementController : Controller
    {
        //
        // GET: /MachineManagement/
        public ActionResult MachineLoad(String sessionID, String userID)
        {
            
            String sessionUserID = String.Empty;
            if (Session["userID"] != null)
                sessionUserID = Session["userID"].ToString();
            else
                return (Redirect("/Security/NewLogin?erc=user loggedOut"));
            try
            {
                using (var machine = new Machine())
                {
                    String startMenu = string.Empty;
                    if (Request.QueryString["hdr"] != null)
                        startMenu = Request.QueryString["hdr"].ToString();
                    var resultList = machine.retrieveMachine(sessionUserID,
                                                                true);
                    var serverList = resultList[0] as List<Server>;
                    var imageList = resultList[1] as List<newZImage>;
                    var falvor = resultList[2] as List<newZFlavor>;
                    ViewBag.sessionID = sessionID;
                    ViewBag.userID = userID;
                    ViewBag.zImageList = new SelectList(imageList,
                                                        "ID",
                                                        "Name");
                    ViewBag.zFlavorList = new SelectList(falvor,
                                                        "ID",
                                                        "Name");



                    ViewBag.networkList = resultList[3] as List<String>;
                    ViewBag.dependence = startMenu;
                    return (View(serverList));
                }
            }
            catch (Exception e)
            {

                return (Redirect("/Security/NewLogin?erc=user loggedOut"));
            }

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult machineInsert(String machineName, String imageID, String flavorID, String network)
        {
            String sessionUserID = String.Empty;
            if (Session["userID"] != null)
                sessionUserID = Session["userID"].ToString();
            else
                throw (new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.NoContent));
            try
            {
                if ((String.IsNullOrEmpty(machineName)) ||
                     (String.IsNullOrEmpty(imageID)) ||
                     (String.IsNullOrEmpty(flavorID)))
                    return (Content("one or more parameters are null or empty"));
                using (var machine = new Machine())
                {
                    machine.Insert(sessionUserID,
                                    machineName,
                                    imageID,
                                    flavorID,
                                    network);

                    var serverList = machine.retrieveMachine(sessionUserID,
                                                                false)[0] as List<Server>;
                    return (PartialView("MachineList",
                                    serverList));
                }
            }
            catch (Exception ex)
            {

                return (Content(ex.Message));
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult machineOperation(String machineID, String operation)
        {
            
            
            String sessionUserID = String.Empty;
            if (Session["userID"] != null)
                sessionUserID = Session["userID"].ToString();
            else
                throw (new System.Web.Http.HttpResponseException(System.Net.HttpStatusCode.NoContent));
            try
            {
                if ((String.IsNullOrEmpty(machineID)) ||
                    (String.IsNullOrEmpty(operation)))
                    return (Content("one or more parameters are null"));
                using (var machine = new Machine())
                {
                    machine.machineOperation(sessionUserID,
                                                machineID,
                                                 operation);



                    var serverList = machine.retrieveMachine(sessionUserID,
                                                                    false)[0] as List<Server>;

                    return (PartialView("MachineList",
                                        serverList));
                }
            }
            catch (Exception ex)
            {

                return (Content("some exception was occured . refresh the page and continue <a href=\"/MachineManagement/machineLoad\">redirect</a>"));
            }
             

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ViewMachine(String ID)
        {
            String sessionUserID = String.Empty;
            if (Session["userID"] != null)
                sessionUserID = Session["userID"].ToString();
            else
                return (Content("you session is expired relogin and try again <a href=\"/Security/NewLogin?erc=user loggedOut\">relogin</a> "));
            if (String.IsNullOrEmpty(ID))
                return (Content("your machine is is null or empty please try again!"));
            try
            {
                var machine = STRepository.StrRepository.RetrievezServer(ID,
                                                                         sessionUserID);
                String vnc = machine.GetVncConsole(ConsoleType.NoVNC);
                ViewBag.vnc = vnc;
                return (View());
            }
            catch (Exception ex)
            {
                return (Content("some problems occurred during request.<a href=\"/MachineManagement/machineLoad\">please reload again</a> "));
            }
        }
    }
}