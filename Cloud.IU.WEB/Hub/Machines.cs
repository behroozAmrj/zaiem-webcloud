using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;
using CRC_SDK.Classes;
using net.openstack.Core.Domain;

namespace Cloud.IU.WEB.Hubs
{
    [HubName("Machines")]
    public class Machines : Hub
    {
        #region public property
        zStack crccloud;
        string _connectionID;
        string userid;
        private ManualResetEvent maResetEvent = new ManualResetEvent(false);
        #endregion
        // this willbe start at the first of page load
        [HubMethodName("Initial")]
        public void Initial(string SessionID, string UserID)
        {
            this.userid = UserID;
            crccloud = Cloud.Core.Models.STRepository.StrRepository.RetrievezStack(SessionID);
            if (crccloud != null)
            {
                /*
                crccloud.On_Created_Server -= cloud_On_Created_Server;
                crccloud.On_Creating_Server -= cloud_On_Creating_Server;
                crccloud.On_Error_Create_Server -= cloud_On_Error_Create_Server;
                crccloud.On_Login -= cloud_On_Login;
                crccloud.On_Login_Error -= cloud_On_Login_Error;
                crccloud.On_Deleting_Server -= crccloud_On_Deleting_Server;
                crccloud.On_Deleted_Server -= crccloud_On_Deleted_Server;
                crccloud.On_Error_Delete_Server -= crccloud_On_Error_Delete_Server;
                crccloud.On_Terminated_Server -= crccloud_On_Terminated_Server;
                crccloud.On_Terminating_Server -= crccloud_On_Terminating_Server;
                //crccloud.On_Rebooting_Server -= crccloud_On_Rebooting_Server;
                //crccloud.On_Rebooted_Server -= crccloud_On_Rebooted_Server;
                crccloud.On_Started_Server -= crccloud_On_Started_Server;
                crccloud.On_Starting_Server -= crccloud_On_Starting_Server;
                crccloud.On_Paused_Server -= crccloud_On_Paused_Server;
                crccloud.On_Pausing_Server -= crccloud_On_Pausing_Server;
            
                crccloud.On_Created_Server += cloud_On_Created_Server;
                crccloud.On_Creating_Server += cloud_On_Creating_Server;
                crccloud.On_Error_Create_Server += cloud_On_Error_Create_Server;
                crccloud.On_Login += cloud_On_Login;
                crccloud.On_Login_Error += cloud_On_Login_Error;
                crccloud.On_Deleting_Server += crccloud_On_Deleting_Server;
                crccloud.On_Deleted_Server += crccloud_On_Deleted_Server;
                crccloud.On_Error_Delete_Server += crccloud_On_Error_Delete_Server;
                crccloud.On_Terminated_Server += crccloud_On_Terminated_Server;
                crccloud.On_Terminating_Server += crccloud_On_Terminating_Server;
                //crccloud.On_Rebooting_Server += crccloud_On_Rebooting_Server;
                //crccloud.On_Rebooted_Server += crccloud_On_Rebooted_Server;
                crccloud.On_Started_Server += crccloud_On_Started_Server;
                crccloud.On_Starting_Server += crccloud_On_Starting_Server;
                crccloud.On_Paused_Server += crccloud_On_Paused_Server;
                crccloud.On_Pausing_Server += crccloud_On_Pausing_Server;
                */
            }
        }



        [HubMethodName("Start")]
        public void Start(string ConnectionID)
        {
            this._connectionID = ConnectionID;
        }
        #region Creating zServer

        [HubMethodName("CreatezServer")]
        public void CreatezServer(string zImageID, string zFlavorID, string MachineName, string SessionID, string UserID)
        {
            try
            {
                if ((!string.IsNullOrEmpty(zImageID)) &&
                    (!string.IsNullOrEmpty(zFlavorID)) &&
                    (!string.IsNullOrEmpty(MachineName)) &&
                    (!string.IsNullOrEmpty(SessionID)) &&
                    (!string.IsNullOrEmpty(UserID)))
                {

                    var nzserver = new NewzServer();
                    nzserver.ImageID = zImageID;
                    nzserver.FlavorID = zFlavorID;
                    nzserver.Name = MachineName;
                    crccloud = Cloud.Core.Models.STRepository.StrRepository.RetrievezStack(SessionID);
                    if (crccloud != null)
                    {
                        this.userid = UserID;
                        crccloud.CreateServer(nzserver);
                        string msg = string.Format(" {0} شروع عملیات درج ماشین ",
                                                    MachineName);
                        Cloud.Core.Models.STRepository.StrRepository.Logs(UserID,
                                                                msg);
                    }
                }
                else
                {
                    throw new Exception(" one or more parameters are null");
                    string msg = string.Format(" روند ایجاد ماشین {0} با مشکل موجه شد ",
                                                MachineName);
                    Cloud.Core.Models.STRepository.StrRepository.Logs(UserID,
                                                            msg);
                    PostMessageToClient(msg);
                }
            }
            catch (Exception ex)
            {
                OnMachineHub_ErrorOccurd("CreateServer",
                                            ex.Message);
            }
        }

        protected void cloud_On_Creating_Server(zServer Server, CRC_SDK.Events.Servers.CreatingServerEventArgs e)
        {
            Clients.Caller.onerror("برنامه در حال درج ماشین میباشد.لطفا صبر کنید . . .");
        }

        protected void cloud_On_Login_Error(zStack cloud, CRC_SDK.Events.General.ErrorEventArgs e)
        {
            OnMachineHub_ErrorOccurd("on login error",
                                        e.message);
        }

        protected void cloud_On_Login(zStack cloud, CRC_SDK.Events.General.LoginEventArgs e)
        {
            //throw new NotImplementedException();
        }
        protected void cloud_On_Error_Create_Server(CRC_SDK.Events.General.ErrorEventArgs e)
        {
            OnMachineHub_ErrorOccurd("on create server error",
                                      e.message);
        }

        public void cloud_On_Created_Server(Server Server, CRC_SDK.Events.Servers.CreatedServerEventArgs e)
        {
            Cloud.Core.Models.STRepository.StrRepository.FillupServer(Server,
                                                                    this.userid);
            string msg = string.Format("ماشین {0} با موفقیت اضافه شد",
                                        Server.Name);
            Cloud.Core.Models.STRepository.StrRepository.Logs(this.userid,
                                                    msg);
            PostLogstoClient(msg);
            OnZServerList_Changed();


        }

        #endregion

        #region Deleting zServer

        [HubMethodName("DeleteMachine")]
        public void DeleteMachine(string MachineID, string SessionID, string UserID, string MachineName)
        {

            try
            {
                if ((!string.IsNullOrEmpty(MachineID)) &&
                    (!string.IsNullOrEmpty(SessionID)) &&
                    (!string.IsNullOrEmpty(UserID)))
                {
                    var selectszserver = Cloud.Core.Models.STRepository.StrRepository.RetrievezServer(MachineID,
                                                                                            UserID);
                    crccloud = Cloud.Core.Models.STRepository.StrRepository.RetrievezStack(SessionID);

                    if (selectszserver != null)
                    {
                        string msg = string.Format(" {0} شورع عملیات حذف ماشین ",
                                                    MachineName);
                        Cloud.Core.Models.STRepository.StrRepository.Logs(UserID,
                                                                msg);
                        selectszserver.Delete();


                    }
                    else
                        throw new Exception("Record Not Found . . . ");
                }
                else
                {
                    throw new Exception(" one or more parameters are null");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("optration failed due to this error:" + ex.Message);
            }

        }

        protected void crccloud_On_Deleted_Server(zServer Server, CRC_SDK.Events.Servers.DeleteServerEventArgs e)
        {
            string msg = string.Format(" ماشین {0} باموفقیت حذف شد ",
                                        Server.Name);
            Clients.Caller.onerror(msg);
            Cloud.Core.Models.STRepository.StrRepository.DeletezServerMachine(Server.ID,
                                                                    this.userid);
            Cloud.Core.Models.STRepository.StrRepository.Logs(this.userid,
                                                    msg);
            PostLogstoClient(msg);
            OnZServerList_Changed();
        }

        protected void crccloud_On_Deleting_Server(zServer Server, CRC_SDK.Events.Servers.DeletingServerEventArgs e)
        {
            Clients.Caller.onerror("برنامه در حال حذف ماشین میباشد . لطفا صبر کنید  . . .");
        }

        protected void crccloud_On_Error_Delete_Server(CRC_SDK.Events.General.ErrorEventArgs e)
        {
            string msg = "اجرای حذف ماشین با مشکل موجه شد";
            PostMessageToClient(msg);
            Cloud.Core.Models.STRepository.StrRepository.Logs(this.userid,
                                                    msg);
            PostLogstoClient(msg);
        }

        #endregion

        #region RunMachine

        [HubMethodName("RunMachine")]
        public void RunMachine(string MachineID, string UserID, string MachineName)
        {
            try
            {
                if ((!string.IsNullOrEmpty(MachineID)) &&
                        (!string.IsNullOrEmpty(UserID)))
                {
                    var zserver = Cloud.Core.Models.STRepository.StrRepository.RetrievezServer(MachineID,
                                                                                        UserID);
                    if (zserver != null)
                    {
                        string msg = string.Format("نمایش ماشین {0}ا",
                                                    MachineName);
                        Cloud.Core.Models.STRepository.StrRepository.Logs(UserID,
                                                               msg);
                        PostLogstoClient(msg);
                        string url = zserver.GetVncConsole(ConsoleType.NoVNC);
                        PostMachineURL(url);
                    }
                    else
                    {
                        throw new Exception("now such zserver was found . . . ");
                    }
                }
                else
                { throw new Exception("one or more parameters are empty"); }
            }
            catch (Exception ex)
            {

                throw new Exception("could not run functionality");
            }
        }

        private void PostMachineURL(string URL)
        {
            Clients.Caller.pbMachineUrl(URL);
        }

        #endregion

        #region  Terminate Machine

        [HubMethodName("TerminateMachine")]
        public void TerminateMachine(string MachineID, string SessionID, string UserID, string MachineName)
        {
            if ((!string.IsNullOrEmpty(MachineID)) &&
                (!string.IsNullOrEmpty(UserID)))
            {
                try
                {
                    var server = Cloud.Core.Models.STRepository.StrRepository.RetrievezServer(MachineID,
                                                                                    UserID);
                    crccloud = Cloud.Core.Models.STRepository.StrRepository.RetrievezStack(SessionID);
                    if (crccloud != null)
                    {
                        if (server.Status.Name != "STOP")
                        {
                            string msg = string.Format("{0}انجام عملیات خاتمه دادن به ماشین ",
                                                        MachineName);
                            Cloud.Core.Models.STRepository.StrRepository.Logs(UserID,
                                                                    msg);
                            //crccloud.TerminateServer(server.);

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Terminate opration canceled due to this cause " + ex.Message);
                }
            }
            else
            { throw new Exception("one or more parameters are null"); }
        }


        void crccloud_On_Terminating_Server(zServer Server, CRC_SDK.Events.Servers.TerminatingServerEventArgs e)
        {
            string msg = string.Format("برنامه در حال خاتمه دادن به ماشین {0} میباشد",
                                        Server.Name);
            PostMessageToClient(msg);
        }

        void crccloud_On_Terminated_Server(zServer Server, CRC_SDK.Events.Servers.TerminateServerEventArgs e)
        {
            if (e.IsSuccessful)
            {
                Server.Attributes.Remove("browser");
                string msg = string.Format("خاتمه ماشین {0} با موفقیت انجام شد",
                                            Server.Name);
                Cloud.Core.Models.STRepository.StrRepository.Logs(this.userid,
                                                        msg);
                PostMessageToClient(msg);
                PostLogstoClient(msg);
                OnZServerList_Changed();

            }
            else
            {
                PostMessageToClient(e.Errormessage);
            }
        }

        #endregion

        #region Restart Machine

        [HubMethodName("RestartMachine")]
        public void RestartMachine(string MachineID, string SessionID, string UserID, string MachineName)
        {
            if ((!string.IsNullOrEmpty(MachineID)) &&
                (!string.IsNullOrEmpty(SessionID)) &&
                (!string.IsNullOrEmpty(UserID)))
            {
                try
                {
                    var zserver = Cloud.Core.Models.STRepository.StrRepository.RetrievezServer(MachineID,
                                                                                UserID);
                    crccloud = Cloud.Core.Models.STRepository.StrRepository.RetrievezStack(SessionID);
                    if (crccloud != null)
                    {
                        string msg = string.Format(" {0} برنامه درحال راهندازی مجددماشین ",
                                                    MachineName);
                        Cloud.Core.Models.STRepository.StrRepository.Logs(UserID,
                                                                msg);
                        //crccloud.RebootServer(zserver);
                     /*   zserver.On_Rebooting -= zserver_On_Rebooting;
                        zserver.On_Rebooted -= zserver_On_Rebooted;
                        zserver.On_Rebooting += zserver_On_Rebooting;
                        zserver.On_Rebooted += zserver_On_Rebooted;
                        zserver.Reboot();*/
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception(" Machine Restarting failed due to this error " + ex.Message);
                }


            }
            else
            { throw new Exception("one or more parameters are empty"); }
        }

        void zserver_On_Rebooted(zServer Server, CRC_SDK.Events.Servers.RebootServerEventArgs e)
        {
            string msg = "سیستم عامل در حال بوت شدن مجدد میباشد";
            PostMessageToClient(msg);
            var loop = true;
            string url = string.Empty;
            while (loop)
            {
                url = Server.GetVNC();
                if (string.IsNullOrEmpty(url))
                    Thread.Sleep(2000);
                else
                {
                    loop = false;
                    PostMachineURL(url);
                    msg = string.Format(" [servmsg.onrebooted] درحال اجرای مجدد ماشین {0} میباشد ",
                                        Server.Name);
                    PostLogstoClient(msg);
                    OnZServerList_Changed();
                }
            }
        }

        void zserver_On_Rebooting(zServer Server, CRC_SDK.Events.Servers.RebootingServerEventArgs e)
        {
            PostMessageToClient(string.Format("برنامه در حال اجرای مجدد {0} ماشین میباشد",
                                               Server.Name));
        }

        void crccloud_On_Rebooted_Server(zServer Server, CRC_SDK.Events.Servers.RebootServerEventArgs e)
        {
            string msg = "سیستم عامل در حال بوت شدن مجدد میباشد";
            PostMessageToClient(msg);
            var loop = true;
            string url = string.Empty;
            while (loop)
            {
                url = Server.GetVNC();
                if (string.IsNullOrEmpty(url))
                    Thread.Sleep(2000);
                else
                {
                    loop = false;
                    PostMachineURL(url);
                    msg = string.Format(" [servmsg.onrebooted] درحال اجرای مجدد ماشین {0} میباشد ",
                                        Server.Name);
                    PostLogstoClient(msg);
                    OnZServerList_Changed();
                }
            }


        }

        void crccloud_On_Rebooting_Server(zServer Server, CRC_SDK.Events.Servers.RebootingServerEventArgs e)
        {
            PostMessageToClient(string.Format("برنامه در حال اجرای مجدد {0} ماشین میباشد",
                                                Server.Name));


        }

        #endregion

        #region PowerOn Machine

        [HubMethodName("PowerOnMachine")]
        public void PowerOnMachine(string MachineID, string SessionID, string UserID, string MachineName)
        {
            if ((!string.IsNullOrEmpty(MachineID)) &&
                (!string.IsNullOrEmpty(SessionID)) &&
                (!string.IsNullOrEmpty(UserID)))
            {
                var server = Cloud.Core.Models.STRepository.StrRepository.RetrievezServer(MachineID,
                                                                                    UserID);
                crccloud = Cloud.Core.Models.STRepository.StrRepository.RetrievezStack(SessionID);
                if (crccloud != null)
                {
                    try
                    {
                        Cloud.Core.Models.STRepository.StrRepository.Logs(UserID,
                                                                MachineName);
                        server.Start();
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("The server power on canceled by cause to this error " + ex.Message);
                    }
                }

            }
            else
            { throw new Exception("one or more parameres are empty"); }

        }



        void crccloud_On_Starting_Server(zServer Server, CRC_SDK.Events.Servers.StartingServerEventArgs e)
        {
            string msg = "برنامه در حال راهندازی ماشین میباشد";

            PostMessageToClient(msg);
        }

        void crccloud_On_Started_Server(zServer Server, CRC_SDK.Events.Servers.StartServerEventArgs e)
        {
            string msg = "ماشین راهندازی شد . برای استفاده از ماشین لطفا صبرکنید";
            PostMessageToClient(msg);
            string url;
            var loop = true;
            while (loop)
            {
                url = Server.GetVNC();
                if (string.IsNullOrEmpty(url))
                {
                    Thread.Sleep(2000);
                }
                else
                {
                    loop = false;
                    PostMachineURL(url);
                    OnZServerList_Changed();
                    msg = "ماشین " + Server.Name + " راهندازی شد";
                    PostLogstoClient(msg);
                }
            }
        }
        #endregion


        #region  Pause Machine

        [HubMethodName("PauseMachine")]
        public void PauseMachine(string MachineID, string SessionID, string UserID, string MachineName)
        {
            try
            {


                if ((!string.IsNullOrEmpty(MachineID)) &&
                    (!string.IsNullOrEmpty(SessionID)) &&
                    (!string.IsNullOrEmpty(UserID)) &&
                    (!string.IsNullOrEmpty(MachineName)))
                {
                    crccloud = Cloud.Core.Models.STRepository.StrRepository.RetrievezStack(SessionID);
                    var server = Cloud.Core.Models.STRepository.StrRepository.RetrievezServer(MachineID,
                                                                                        UserID);
                    if ((crccloud != null) &&
                        (server != null))
                    {
                        string msg = string.Format(" برنامه درحال مکث کردن ماشین {0} میباشد ",
                                                    MachineName);
                        Cloud.Core.Models.STRepository.StrRepository.Logs(UserID,
                                                               msg);
                        server.Pause();
                    }
                }
                else
                { throw new Exception(" one or more parameters are emoty"); }
            }
            catch (Exception)
            {

                string msg = string.Format(" روند مکث ماشین با مشکل مواجه شد "
                                            ,MachineName);
                Cloud.Core.Models.STRepository.StrRepository.Logs(UserID,
                                                       msg);
                PostLogstoClient(msg);
            }
        }


        void crccloud_On_Pausing_Server(zServer Server, CRC_SDK.Events.Servers.PausingServerEventArgs e)
        {
            string msg = string.Format( " برنامه در حال اجرای مکث ماشین {0} میباشد " , 
                                        Server.Name);
            PostMessageToClient(msg);
        }

        void crccloud_On_Paused_Server(zServer Server, CRC_SDK.Events.Servers.PauseServerEventArgs e)
        {
            string msg = string.Format(" مکث ماشین {0} با موفقیت به اتمام رسید ",
                                        Server.Name);
            PostLogstoClient(msg);
            PostMessageToClient(msg);


        }
        #endregion


        #region OnClientside Back Response
        private void PostMessageToClient(string Message)
        {
            Clients.Caller.onerror(Message);

        }


        private void PostLogstoClient(string Message)
        {
            Clients.Caller.client_log(Message);
        }

        private void SendRandonNumber(string _connectionID)
        {
            while (true)
            {
                var rnd = new Random();

                int id = rnd.Next(0,
                                   9);
                Clients.Client(_connectionID).machinestate(id);
                Thread.Sleep(3000);
            }
        }

        protected void OnCreateServerError(string ErrorMessage)
        {
            Clients.Caller.onerror(ErrorMessage);
        }



        protected void OnMachineHub_ErrorOccurd(string Source, string ErrorMessage)
        {
            Clients.Caller.onerror(string.Format("Error Raised in {0} by cause {1}", Source,
                                                                                        ErrorMessage));
        }

        private void OnZServerList_Changed()
        {
            Clients.Caller.onzserverchanged(1);
        }

        #endregion



    }
}