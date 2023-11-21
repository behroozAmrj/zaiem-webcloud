using gitLab = Cloud.Core.GitLabDomain;
using Cloud.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class zStoreController : Controller
    {
        //
        // GET: /zStore/
        public ActionResult Load()
        {
            try
            {
                String userID = String.Empty;
                if (Session["userID"] != null)
                {
                    userID = Session["userID"].ToString();
                    String url = "http://172.18.23.249:9011/services";
                    var zStoreMgr = new zStoreServiceManagement(userID);
                    var applianceList = zStoreMgr.RetrieveAppliances(url);
                    ViewBag.url = "ws://172.18.23.249:8888/install";
                    ViewBag.networksList = zStoreMgr.cloudNetworks;
                    return (View(applianceList));
                }

                return (Content("invalid user Id and session "));
            }
            catch (Exception ex)
            {
                /*  # this used whether this method called by ajax and result must be returned in json 
                var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new System.Net.Http.StringContent(ex.Message),
                    ReasonPhrase = "operation failed"
                };
                throw new System.Web.Http.HttpResponseException(httpMsg);
                */

                return (Content(String.Format("Some error ocurred during retrieving data: {0}" , ex.Message)));
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult addAppliance(String URL, String applianceID, String applianceName, String networkID)
        {
            String userID = String.Empty;
            if (Session["userID"] != null)
            {
                userID = Session["userID"].ToString();
                if ((String.IsNullOrEmpty(URL)) ||
                    (String.IsNullOrEmpty(applianceID)) ||
                    (String.IsNullOrEmpty(applianceName)) ||
                    (String.IsNullOrEmpty(networkID)))
                {
                    var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                    {
                        Content = new System.Net.Http.StringContent("one or more parameters are null"),
                        ReasonPhrase = "null parameters"
                    };
                    throw new System.Web.Http.HttpResponseException(httpMsg);
                }
                else
                {
                    try
                    {
                        networkID = "fb1b6880-ea53-4de4-bb47-1e8e6572e7d0"; // # temp hardCode just for test
                        var zStore = new zStoreServiceManagement(userID);
                        zStore.createAnMachineWithAppliance(URL: URL,
                                                            applianceID: applianceID,
                                                            networkID: networkID,
                                                            applianceName: applianceName);
                    }
                    catch (Exception ex)
                    {
                        var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                        {
                            Content = new System.Net.Http.StringContent(ex.Message),
                            ReasonPhrase = "operation failed"
                        };
                        throw new System.Web.Http.HttpResponseException(httpMsg);
                    }
                }
            }
            return (RedirectToAction("Load"));
        }
    }
}