using Cloud.Core.Models;
using Cloud.Core.Models.Modules;
using Cloud.IO.Repository;
using Cloud.IU.WEB.Hubs;
using Cloud.IU.WEB.InfraSructure;
using CRC_SDK.Classes;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Cloud.IU.WEB.Controllers
{
    public class VMManagementController : Controller
    {
        //
        // GET: /VMManagement/
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Main(string SessionID, string UserID)
        {
            try
            {
                var user = new User();
                user.UserName = "amiri";
                user.Password = ">>>>";
                user.GroupName = "user";
                user.CloudIP = new CloudIPV4("172.18.23.249");

                if (!string.IsNullOrEmpty(SessionID) &&
                     !string.IsNullOrEmpty(UserID))
                {
                    ViewBag.sessionid = SessionID;
                    ViewBag.userid = UserID;
                }
                else
                    Redirect("/Security/NewLogin");
                var zserverlist = STRepository.StrRepository.GetZserverList(UserID);
                var zimagelist = STRepository.StrRepository.GetZimageList(UserID);
                var zflavorlist = STRepository.StrRepository.GetZFlavorList(UserID);
                ViewBag.zserverlist = zserverlist.ToList();
                ViewBag.zimagelist = new SelectList(zimagelist.ToList(),
                                                    "ID",
                                                    "Name");

                ViewBag.zflavorlist = new SelectList(zflavorlist.ToList(),
                                                    "ID",
                                                     "Name");
                return View(user);
            }
            catch (Exception ex)
            {

                return (Redirect("/Security/NewLogin?erc=001"));
            }

        }
        [AcceptVerbs(HttpVerbs.Post)]

        public ActionResult RP_zServerList(string UserID)
        {
            return (PartialView("ZserverList",
                                    STRepository.StrRepository.GetZserverList(UserID)));
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DeskTop(string SessionID, string UserID, string appName)
        {
            String sessionUserID = string.Empty;
            if (Session["userID"] != null)
                sessionUserID = Session["userID"].ToString();

            try
            {
                if ((!string.IsNullOrEmpty(SessionID) ||
                    (!string.IsNullOrEmpty(UserID))))
                {
                    if (sessionUserID != UserID)
                        return (Redirect("/Security/NewLogin?erc=can not logon."));
                    //try
                    //{
                    //    var formconfig = new FormsConfiguration("DeskTop");
                    //    string xpath = "root//settings//desktop";
                    //    var configdictinary = formconfig.GetFormConfigurationSetting(xpath);
                    //    if ((configdictinary != null) &&
                    //        (configdictinary.Count > 0))
                    //    {
                    //        if (configdictinary.ContainsKey("background"))
                    //        {
                    //            string imagename = configdictinary["background"].ToString();
                    //            ViewBag.bgimage = imagename;
                    //        }
                    //    }
                    //}
                    //catch
                    //{
                    //}
                    ViewBag.sessionid = SessionID;
                    ViewBag.userid = UserID;
                    ViewBag.appName = appName;
                    return (View("w7Desktop"));
                }
                else
                {
                    return (Redirect("/Security/NewLogin?erc=can not logon."));
                }
            }
            catch
            {
                return (Redirect("/Security/NewLogin?erc=can not logon."));
            }
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PostBackCustomView(string URL, string CustomViewName)
        {
            var str = "result";
            return (Json(str));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeSkin(string Data, string Type)
        {
            var desktopentity = Entity_FactoryMethod.Method(new object[] { Convert.ToInt32(Type) });
            if (desktopentity != null)
            {
                string path = desktopentity.LoadTemplate(Data);
                return (Json(path));
            }
            else
            {
                var rbsapp = new RBSApplication(" ",
                                                Data,
                                                Convert.ToInt32(Type));
                string filepath = rbsapp.RBSapplicaitonLoadSkin();
                return (Json(filepath));
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult menuItem(String itemName, String userID)
        {

            switch (itemName)
            {
                case "Storage":
                    { return (Json(Storages())); break; }
                case "RBSmgr":
                    {
                        return (Json(RBSMgr())); break;

                        break;
                    }
                default:
                    {
                        var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                        {
                            Content = new System.Net.Http.StringContent("no menu item found"),
                            ReasonPhrase = "wrong menu item Name"
                        };
                        throw new System.Web.Http.HttpResponseException(httpMsg);
                    }
            }
        }

        private void Settings()
        {
            throw new NotImplementedException();
        }

        private List<String> Storages()
        {
            var contentList = new List<String>();
            contentList.Add("/Menuitemui/Storage/zDrive.html");
            var pwindow = new WindowTemplate("Templates\\pwindow.html");
            contentList.Add(pwindow.ReadFileAsHtmlString("550px", "900px").ToHtmlString());
            contentList.Add(String.Empty);
            contentList.Add("-1");//this number is layoutList dropDown on window dosen`t show dropdown menu
            contentList.Add("Storage");
            return (contentList);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Storage(String operation, String URL)
        {
            String result = "no userID";
            if (Session["userID"] != null)
            {
                string userID = Session["userID"].ToString();
                var repService = new RepositoryService.RepositoryClient();
                var user = repService.getUser(userID);
                if (user == null)
                {
                    var msgis = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                    {
                        Content = new System.Net.Http.StringContent("request failed. sessions is expired"),
                        ReasonPhrase = "session is expired"
                    };
                    throw new System.Web.Http.HttpResponseException(msgis);
                }


                String token = user.Token;// Cloud.Core.Models.STRepository.StrRepository.getUserCredentialToken(userID);
                if ((!String.IsNullOrEmpty(token)) && (!String.IsNullOrEmpty(URL)))
                {
                    String url = URL;
                    switch (operation)
                    {
                        case "loadContent":
                            {
                                try
                                {
                                    var zDriveStorageService = new zDrive(userID);
                                    var contentList = zDriveStorageService.getContentListFromzDriveService(url,
                                                                             token);
                                    return (Json(contentList));
                                }
                                catch (Exception)
                                {
                                    var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                                    {
                                        Content = new System.Net.Http.StringContent("loading content failed. please try again"),
                                        ReasonPhrase = "loading content"
                                    };
                                    throw new System.Web.Http.HttpResponseException(httpMsg);
                                }
                            }
                        case "createContainer":
                            {
                                try
                                {
                                    var zDriveStorageService = new zDrive(userID);
                                    zDriveStorageService.createContainer(url,
                                                                        token);
                                    return (Json(true));
                                }
                                catch (Exception ex)
                                {

                                    var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                                    {
                                        Content = new System.Net.Http.StringContent("creating folder failed. please try again"),
                                        ReasonPhrase = "creating folder"
                                    };
                                    throw new System.Web.Http.HttpResponseException(httpMsg);
                                }

                            }

                        case "deleteContainer":
                            {
                                try
                                {

                                    var storageService = new zDrive(userID);
                                    storageService.deleteContainer(URL,
                                                                    token,
                                                                    userID);
                                    return (Json(true));
                                }
                                catch (Exception)
                                {
                                    var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                                    {
                                        Content = new System.Net.Http.StringContent("deleting folder failed. please try again"),
                                        ReasonPhrase = "deleting folder"
                                    };
                                    throw new System.Web.Http.HttpResponseException(httpMsg);
                                }
                            }
                        case "deleteFile":
                            {
                                try
                                {
                                    var storageService = new StorageServiceManagement(userID);
                                    storageService.deleteFile(URL,
                                                              token,
                                                              userID);
                                    return (Json(true));
                                }
                                catch (Exception)
                                {
                                    var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                                    {
                                        Content = new System.Net.Http.StringContent("deleting file failed. please try again"),
                                        ReasonPhrase = "deleting file"
                                    };
                                    throw new System.Web.Http.HttpResponseException(httpMsg);
                                }
                            }
                        case "renameFile":
                            {
                                var zDriveStorageService = new zDrive(userID);
                                var contentList = zDriveStorageService.getContentListFromzDriveService(url,
                                                                         token);
                                return (Json(contentList));
                            }
                        case "uploadFile":
                            {
                                var zDriveStorageService = new zDrive(userID);
                                var contentList = zDriveStorageService.getContentListFromzDriveService(url,
                                                                         token);
                                return (Json(contentList));
                            }
                        case "downloadFile":
                            {
                                var zDriveStorageService = new zDrive(userID);
                                var contentList = zDriveStorageService.getContentListFromzDriveService(url,
                                                                             token);
                                return (Json(contentList));
                            }
                        default:
                            return (Json(result));

                    }
                }
                else
                {
                    var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                    {
                        Content = new System.Net.Http.StringContent("request failed parameters are null"),
                        ReasonPhrase = "wrong parameters"
                    };
                    throw new System.Web.Http.HttpResponseException(httpMsg);
                }
            }
            else
            {
                var msgis = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new System.Net.Http.StringContent("request failed. sessions is expired"),
                    ReasonPhrase = "session is expired"
                };
                throw new System.Web.Http.HttpResponseException(msgis);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public Boolean uploadStreamFileTozDrive(String fileContent, String unBase64Content)
        {

            Boolean result = false;
            if (Session["userID"] != null)
            {
                string userID = Session["userID"].ToString();
                var repService = new RepositoryService.RepositoryClient();
                String token = repService.getUser(userID).Token;// Cloud.Core.Models.STRepository.StrRepository.getUserCredentialToken(userID);
                //getting data from url 
                String fileName = Request.QueryString["fileName"].ToString();
                String fileSize = Request.QueryString["fileSize"].ToString();
                String status = Request.QueryString["status"].ToString();
                String startByte = Request.QueryString["startByte"].ToString();
                String endByte = Request.QueryString["endByte"].ToString();
                String URL = Request.QueryString["URL"].ToString();

                Boolean validate = validateInputValue(fileName,
                                                        fileSize,
                                                        status,
                                                        startByte,
                                                        endByte,
                                                        URL,
                                                        token);

                if (validate)
                {
                    //endByte = (Convert.ToInt64(endByte) - 1).ToString();
                    var storageService = new StorageServiceManagement(userID);
                    result = storageService.uploadFileStreamTozDrive(fileContent,
                                                                     fileName,
                                                                     fileSize,
                                                                     status,
                                                                     startByte,
                                                                     endByte,
                                                                     URL,
                                                                     token);
                    return (result);
                }
                else
                {
                    var msgis = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                    {
                        Content = new System.Net.Http.StringContent("uploadin failed parameters are null"),
                        ReasonPhrase = "wrong parameters"
                    };
                    throw new System.Web.Http.HttpResponseException(msgis);
                }
            }
            else
            {
                var msgis = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    Content = new System.Net.Http.StringContent("upload failed. session is expired"),
                    ReasonPhrase = "session expired"
                };
                throw new System.Web.Http.HttpResponseException(msgis);
            }

        }

        private bool validateInputValue(string fileName, string fileSize, string status, string startByte, string endByte, string URL, string token)
        {
            if ((String.IsNullOrEmpty(fileName)) &&
                (String.IsNullOrEmpty(fileSize)) &&
                (String.IsNullOrEmpty(status)) &&
                (String.IsNullOrEmpty(startByte)) &&
                (String.IsNullOrEmpty(endByte)) &&
                (String.IsNullOrEmpty(token)) &&
                (String.IsNullOrEmpty(URL)))
                return (false);
            else
                return (true);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult _downloadFile()
        {
            //HttpContext.Response.OutputStream.Write

            Log.Tracking.Logging.SimpleINOFlogRegister("download File Executed");
            String URL = HttpContext.Request.QueryString["url"];
            if (String.IsNullOrEmpty(URL))

                return (null);
            if (Session["userID"] != null)
            {
                string userID = Session["userID"].ToString();
                var repService = new RepositoryService.RepositoryClient();

                String token = repService.getUser(userID).Token;// Cloud.Core.Models.STRepository.StrRepository.getUserCredentialToken(userID);
                try
                {
                    var storageService = new StorageServiceManagement(userID);
                    String downloadedFilePath = storageService.downloadFile(URL,
                                                                            token,
                                                                            userID);
                    var spliteFileName = downloadedFilePath.Split('_');
                    String objName = spliteFileName[spliteFileName.Length - 1];
                    String savefilePath = downloadedFilePath;
                    if (System.IO.File.Exists(downloadedFilePath))
                    {
                        String contentDescripttion = String.Format("attechment ; filename=\"{0}\"", objName);
                        var hh = HttpContext.Response;

                        HttpContext.Response.ContentType = "application/octet-stream";
                        HttpContext.Response.AddHeader("Content-Disposition", contentDescripttion);
                        HttpContext.Response.AddHeader("Content-Description", "File Transfer");
                        HttpContext.Response.AddHeader("Content-Transfer-Encoding", "binary");
                        HttpContext.Response.AddHeader("Content-Length", new System.IO.FileInfo(downloadedFilePath).Length.ToString());
                        int byteRead;
                        byte[] byteBuffer = new byte[1024];
                        using (var memStream = new MemoryStream(byteBuffer, true))
                        using (var strmReader = new FileStream(savefilePath, FileMode.Open))
                        {
                            while ((byteRead = strmReader.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                            {
                                HttpContext.Response.BinaryWrite(memStream.ToArray());
                                HttpContext.Response.Flush();
                                System.Threading.Thread.Sleep(50);

                            }
                            HttpContext.Response.Close();

                        }
                        System.IO.File.Delete(savefilePath);
                        return (null);
                    }
                    else
                        return (null);
                }

                catch (Exception)
                {

                    return (null);
                }
            }
            else
            {
                Log.Tracking.Logging.SimpleINOFlogRegister("no user id is not exist");
                return (null);
            }

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult downloadFile()
        {
            //HttpContext.Response.OutputStream.Write


            Log.Tracking.Logging.SimpleINOFlogRegister("download File Executed");
            String URL = HttpContext.Request.QueryString["url"];
            if (String.IsNullOrEmpty(URL))

                return (null);
            if (Session["userID"] != null)
            {
                string userID = Session["userID"].ToString();
                var repService = new RepositoryService.RepositoryClient();
                String token = repService.getUser(userID).Token;// Cloud.Core.Models.STRepository.StrRepository.getUserCredentialToken(userID);
                try
                {
                    var storageService = new StorageServiceManagement(userID);
                    var downloadedFilePath = storageService.downloadFileStream(HttpContext,
                                                                                    URL,
                                                                                    token,
                                                                                    userID);
                    return (null);

                }

                catch (Exception)
                {

                    return (null);
                }
            }
            else
            {
                Log.Tracking.Logging.SimpleINOFlogRegister("no user id is not exist");
                return (null);
            }

        }

        public void seTimer()
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext("Security");
            hub.Clients.Client("").send(9);
        }

        public Boolean deleteContainer(String URL)
        {
            if (Session["userID"] != null)
            {
                string userID = Session["userID"].ToString();
                var repService = new RepositoryService.RepositoryClient();

                String token = repService.getUser(userID).Token;//  Cloud.Core.Models.STRepository.StrRepository.getUserCredentialToken(userID);
                try
                {
                    var storageService = new StorageServiceManagement(userID);
                    storageService.deleteContainer(URL,
                                                    token,
                                                    userID);

                    return (true);
                }
                catch (Exception e)
                { return (false); }
            }
            else
                return (false);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult InitialStorage()
        {
            if (Session["userID"] != null)
            {
                var repService = new RepositoryService.RepositoryClient();
                string userID = Session["userID"].ToString();
                var user = repService.getUser(userID);
                if (user == null)
                {
                    var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new System.Net.Http.StringContent("request failed your session is expired"),
                    ReasonPhrase = "session expired"
                };
                    throw new System.Web.Http.HttpResponseException(httpMsg);
                }
                String url = user.StorageURL; // Cloud.Core.Models.STRepository.StrRepository.getUserStorageURL(userID);
                return (Json(url));
            }
            else
            {
                var httpMsg = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new System.Net.Http.StringContent("request failed your session is expired"),
                    ReasonPhrase = "session expired"
                };
                throw new System.Web.Http.HttpResponseException(httpMsg);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public List<String> RBSMgr()
        {
            var contentList = new List<String>();
            contentList.Add("/Menuitemui/RBS/RBSmgr.html");
            var pwindow = new WindowTemplate("Templates\\pwindow.html");
            contentList.Add(pwindow.ReadFileAsHtmlString("550px", "900px").ToHtmlString());
            contentList.Add(String.Empty);
            contentList.Add("-1");//this number is layoutList dropDown on window dosen`t show dropdown menu
            contentList.Add("RBSManager");

            return (contentList);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public Boolean RBSuploadAndAddFile(String fileContent)
        {

            if ((Session["userID"] != null) &&
                    (!String.IsNullOrEmpty(fileContent)))
            {
                string userID = Session["userID"].ToString();
                String fileName = Request.QueryString["fileName"].ToString();
                String appName = Request.QueryString["appName"].ToString();
                String status = Request.QueryString["status"].ToString();
                String filePath = Server.MapPath("/Application-Service");

                if (!String.IsNullOrEmpty(fileName) &&
                    !String.IsNullOrEmpty(appName) &&
                    !String.IsNullOrEmpty(status))
                {
                    var rbsManager = new RBSManagement();
                    rbsManager.createFileAndRegister(appName,
                                                    userID,
                                                    fileName,
                                                    filePath,
                                                    fileContent,
                                                    status);
                }
                return (true);
            }
            else
            {
                var msgi = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    Content = new System.Net.Http.StringContent("mission failed by wrong input data value"),
                    ReasonPhrase = "wrong parameters "
                };
                throw new System.Web.Http.HttpResponseException(msgi);
            }
        }
    }
}