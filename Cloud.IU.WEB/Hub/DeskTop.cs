using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;
using System.IO;
using System.Configuration;
using Cloud.IU.WEB.InfraSructure;
using System.Reflection;
using Cloud.Foundation.Infrastructure;
using Cloud.IO.Repository;
using Cloud.Core.Models;
using Cloud.Log.Tracking;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cloud.IU.WEB.Hubs
{
    [HubName("DeskTop")]
    public class DeskTop : Hub
    {
        [HubMethodName("Ping")]
        public void Ping()
        {
            while (true)
            {
                var dt = DateTime.Now;
                Clients.Caller.pingOnclient(dt.Second.ToString());
                Thread.Sleep(1000);
            }
        }

        [HubMethodName("LoadDeskTop")]
        public void LoadDeskTop(string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
            {

                try
                {
                    //Logging.INFOlogRegistrer(string.Format("Login Request:conID{0}", ConnectionID),
                    //                        UserID,
                    //                        MethodBase.GetCurrentMethod().DeclaringType);


                    List<IEntity> dtlist = new List<IEntity>();
                    List<RunBySelf> runbyselflist = new List<RunBySelf>();

                    var dtbuilder = new DeskTopBuilder(UserID);
                    //var crc_items = dtbuilder.GetDeskTopItemsList(UserID);   // this load crc_clouds items
                    dtbuilder.GetDeskTopItemsList(UserID,
                                                 ref dtlist,
                                                 ref runbyselflist);

                    // in this section RBS application items must be load  from desktop builder
                    // BEGIN
                    //var rbslist = dtbuilder.RBSConfigRepositoryList;
                    // END
                    Clients.Caller.itemsLoad(dtlist,
                                             runbyselflist);
                }
                catch (Exception ex)
                {
                    var url = System.Configuration.ConfigurationManager.AppSettings.Get("redirectURL");
                    Logging.ErrorlogRegister(ex.Message,
                                             MethodBase.GetCurrentMethod().DeclaringType,
                                             UserID);
                    PageRedirect(url);

                }
            }
            else
            {
                var url = System.Configuration.ConfigurationManager.AppSettings.Get("redirectURL");
                PageRedirect(url);
            }
        }

        [HubMethodName("LoadDeskTop")]
        public void LoadDeskTop(string UserID, string ConnectionID)
        {
            string userID = UserID;
            string action = "Desktop.LoadDesktop";
            string actionType = "info";
            string content = "loading Desktop";
            if (!string.IsNullOrEmpty(UserID))
            {
                try
                {
                    //Logging.INFOlogRegistrer(string.Format("Login Request:conID{0}", ConnectionID),
                    //                        UserID,
                    //                        MethodBase.GetCurrentMethod().DeclaringType);


                    List<IEntity> dtlist = new List<IEntity>();
                    List<RunBySelf> runbyselflist = new List<RunBySelf>();

                    if (HttpContext.Current.Request.Cookies["desktopItems"] == null)
                    {
                        int count = HttpContext.Current.Request.Cookies.Count;
                        var dtbuilder = new DeskTopBuilder(UserID);
                        //var crc_items = dtbuilder.GetDeskTopItemsList(UserID);   // this load crc_clouds items
                        dtbuilder.GetDeskTopItemsList(UserID,
                                                     ref dtlist,
                                                     ref runbyselflist);

                        var desktopCoockies = new HttpCookie("desktopItems");
                        desktopCoockies.Values["machine"] = JsonConvert.SerializeObject(dtlist);
                        desktopCoockies.Values["rbs"] = JsonConvert.SerializeObject(runbyselflist);
                        desktopCoockies.Expires = DateTime.Now.AddDays(2);
                        HttpContext.Current.Response.Cookies.Add(desktopCoockies);

                    }
                    else
                    {
                        var desktopCoockies = HttpContext.Current.Request.Cookies["desktopItems"];
                        string machines = desktopCoockies.Values["machine"];
                        string rbs = desktopCoockies.Values["rbs"];
                        if (!string.IsNullOrEmpty(machines))
                        {
                            var machineList = JsonConvert.DeserializeObject<List<IEntity>>(machines);
                            dtlist = machineList;
                        }

                        if (!string.IsNullOrEmpty(rbs))
                        {
                            var rbsList = JsonConvert.DeserializeObject<List<RunBySelf>>(rbs);
                            runbyselflist = rbsList;
                        }
                    }



                    Logging.INFOLogRegisterToDB(userID,
                                               action,
                                               actionType,
                                               content);
                    // in this section RBS application items must be load  from desktop builder
                    // BEGIN
                    // var rbslist = dtbuilder.RBSConfigRepositoryList;
                    // END

                    //var repService = new RepositoryService.RepositoryClient();

                    //repService.updateConnectionID(UserID,
                    //                                ConnectionID);
                    Clients.Caller.itemsLoad(dtlist,
                                             runbyselflist);


                }
                catch (Exception ex)
                {
                    var url = System.Configuration.ConfigurationManager.AppSettings.Get("redirectURL");
                    //Logging.ErrorlogRegister(ex.Message,
                    //                         MethodBase.GetCurrentMethod().DeclaringType,
                    //                         UserID);
                    Logging.INFOLogRegisterToDB(userID,
                                               action,
                                               "exception",
                                               ex.Message);
                    PageRedirect(url);

                }
            }
            else
            {
                Logging.INFOLogRegisterToDB(UserID,
                                               action,
                                               "failed",
                                               "one or more parameters are null or empty!");
                var url = System.Configuration.ConfigurationManager.AppSettings.Get("redirectURL");
                PageRedirect(url);
            }
        }

        [HubMethodName("DoWork")]
        public void DoWork(string Handler, string UserID, string Type, string itemName)
        {

            var entitytype = (Cloud.Foundation.Infrastructure.EntityType)Convert.ToInt32(Type);
            if ((!string.IsNullOrEmpty(Handler)) &&
                (!string.IsNullOrEmpty(Type)) &&
                (!string.IsNullOrEmpty(UserID)))
            {
                string userID = UserID;
                string action = "Desktop.DoWrok";
                string actionType = "info";
                string content = itemName;
                try
                {
                    //Logging.INFOlogRegistrer("DoWork",
                    //                            UserID,
                    //                            MethodBase.GetCurrentMethod().DeclaringType);


                    var desktopentity = Entity_FactoryMethod.Method(entitytype,
                                                                    Handler,
                                                                    UserID);
                    /*
                    if ((desktopentity == null) || // this if statement checks in sate of Machines and Run by self application the first section blong to RBS appilcation
                        (Convert.ToInt32(Type) < 0))
                    {
                        var rbsapp = new RBSApplication(Handler);
                        var result = rbsapp.DoWork();// ReadApplicationLayoutlist();
                        var pwindow = new WindowTemplate("Templates\\pwindow.html");

                        var windowcontent = pwindow.ReadFileAsHtmlString(rbsapp.Height,
                                                                         rbsapp.Width);
                        Clients.Caller.onClientBackResult(result.ToString(),
                                                            windowcontent.ToString(),
                                                            rbsapp.LayoutFileList,
                                                            Type,
                                                            itemName);
                    }
                    else
                    {
                        var result = desktopentity.DoWork();
                        var pwindow = new WindowTemplate("Templates\\pwindow.html");
                        var windowcontent = pwindow.ReadFileAsHtmlString(desktopentity.Height,
                                                                            desktopentity.Width);
                        if (result != null)
                        {
                            Clients.Caller.onClientBackResult(result.ToString(),
                                                                windowcontent.ToString(),
                                                                desktopentity.LayoutFileList,
                                                                Type,
                                                                itemName);
                        }
                    }*/

                    var result = desktopentity.DoWork();
                    Logging.INFOLogRegisterToDB(userID,
                                                action,
                                                actionType,
                                                Type + content);
                    var pwindow = new WindowTemplate("Templates\\pwindow.html");
                    var windowcontent = pwindow.ReadFileAsHtmlString(desktopentity.Height,
                                                                        desktopentity.Width);
                    if (result != null)
                    {

                        Clients.Caller.onClientBackResult(result.ToString(),
                                                            windowcontent.ToString(),
                                                            desktopentity.LayoutFileList,
                                                            Type,
                                                            itemName);
                    }

                }
                catch (Exception ex)
                {
                    var msg = "exectuion failed please refresh or try later";
                    //Logging.ErrorlogRegister(msg,
                    //                          MethodBase.GetCurrentMethod().DeclaringType,
                    //                           UserID);
                    Logging.INFOLogRegisterToDB(userID,
                                               action,
                                               "exception",
                                               ex.Message);
                    Clients.Caller.onMessageBack(msg);
                }
            }
            else
            {
                var msg = "execution failed => send parameters are not in valid format";
                //Logging.ErrorlogRegister(msg,
                //                         MethodBase.GetCurrentMethod().DeclaringType,
                //                         UserID);
                Logging.INFOLogRegisterToDB(UserID,
                                            "Desktop.DoWork",
                                               "exception",
                                             "one or more parameters are null or empty");
                Clients.Caller.onMessageBack(msg);

            }

        }

        public void Timer(string connectionID)
        {

            Clients.All.pageRedirect(string.Format("serverTime:{0}", DateTime.Now.ToShortTimeString()));
            Thread.Sleep(1000);

        }

        [HubMethodName("Delete")]
        public void Delete(string SessionID, string Handler, string UserID, string Type)
        {
            if ((!string.IsNullOrEmpty(SessionID)) &&
                (!string.IsNullOrEmpty(Handler)) &&
                (!string.IsNullOrEmpty(UserID)) &&
                (!string.IsNullOrEmpty(Type)))
            {
                var entitytype = (EntityType)Convert.ToInt32(Type);
                var desktopentity = Entity_FactoryMethod.Method(new object[] {entitytype,
                                                                                Handler,
                                                                                UserID,
                                                                                SessionID});
                var result = desktopentity.Delete();
                if (result)
                {
                    LoadDeskTop(UserID);
                }
            }
            else
            { throw new Exception("one or more parameters are null ..."); }
        }

        private void PageRedirect(string Url)
        {
            Clients.Caller.pageRedirect(Url);
        }


        [HubMethodName("PostBackDesignedCustomView")]
        public void PostBackDesignedCustomView(string CustomView_ID)
        {
            if (!string.IsNullOrEmpty(CustomView_ID))
            {
                //var customviewRep = new CustomViewRepository(CustomView_ID);
                //customviewRep.RetrieveCustomViewDesignedData(CustomView_ID,
                //                                                "hello");
            }
            else
            { }
        }


        [HubMethodName("MenuItemContent")]
        public void MenuItemContent(string UserID, string MenuItemName)
        {
            switch (MenuItemName)
            {
                case "Storages":
                    { Storages(); break; }
                case "Settings":
                    { Settings(); break; }
            }
        }

        private void Settings()
        {
            throw new NotImplementedException();
        }

        private void Storages()
        {

        }

        public string machines { get; set; }
    }
}