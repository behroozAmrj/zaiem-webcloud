//using CRC_SDK.Classes;
using net.openstack.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Timers;
using System.Web;

namespace Cloud.Core.Models
{
    [DataContract]
    public class UserProSecurity : IDisposable
    {
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public String Token { get; set; }
        [DataMember]
        public String StorageURL { get; set; }
        [DataMember]
        public String UserName { get; set; }
        [DataMember]
        public String Password { get; set; }
        [DataMember]
        public String UserTenant { get; set; }
        [DataMember]
        public String zStoreToken { get; set; }
        [DataMember]
        public String zStoreTenant { get; set; }

        public String Role
        {
            get
            { return (_role); }
            set
            {
                if ((!String.IsNullOrEmpty(value)) && (!String.IsNullOrWhiteSpace(value)))
                    this._role = value;
            }
        }
        public DateTime LoginTime { get { return (loginTime); } }
        private DateTime loginTime;



        List<Server> zServerList;
        List<ServerImage> ozImageList;
        List<FlavorDetails> ozFalvorList;

        List<newZImage> zImageList;
        List<newZFlavor> zFlavorList;

        const int timeout = 3000000;//= 30 minuts
        public delegate void onUserExpiretionTime(String userID);
        public event onUserExpiretionTime onUserTimeout;


        private String _role = "user";
        public UserProSecurity(DateTime? exTime = null)
        {
            /*
            timeoutChecker = new Timer(timeout);
            timeoutChecker.Elapsed += timeoutChecker_Elapsed;
            timeoutChecker.Enabled = true;
            timeoutChecker.Start();
            */
            if (exTime != null)
                this.loginTime = DateTime.Now;
        }

        
        public void FillupServer(List<Server> ZServer)
        {
            if (ZServer != null)
            {
                if (this.zServerList == null)
                    this.zServerList = new List<Server>();
                this.zServerList = ZServer;
            }
        }

        public void FillupServer(Server zServer)
        {
            if (zServer != null)
            {
                if (this.zServerList == null)
                {
                    zServerList = new List<Server>();
                    this.zServerList.Add(zServer);
                }
                else
                {
                    var serv = this.zServerList.FirstOrDefault(zserver => zserver.Id == zServer.Id);
                    if (serv == null)
                        this.zServerList.Add(zServer);
                }
            }
        }


        public Server SelectServer(string MachineID)
        {
            if (!string.IsNullOrEmpty(MachineID))
            {
                if (this.zServerList != null)
                {
                    var zserv = zServerList.FirstOrDefault(zserver => zserver.Id == MachineID);
                    return (zserv);
                }
                else
                { throw new Exception("no zserver list "); }
            }
            else
            { throw new Exception("null Machine ID"); }
        }


        public void FillUpImage(List<ServerImage> ZImage)
        {
            if (ZImage != null)
            {
                if (zImageList == null)
                    zImageList = new List<newZImage>();
                this.ozImageList = ZImage;
                zImageList.Clear();
                foreach (var rec in ZImage)
                {
                    var nzimg = new newZImage();
                    nzimg.ID = rec.Id;// ImageDetails.Id;
                    nzimg.Name = rec.Name;// ImageDetails.Name;
                    zImageList.Add(nzimg);
                }
            }
        }

        public void FillUp_Flavor(List<FlavorDetails> ZFlavor)
        {
            if (ZFlavor != null)
            {
                this.ozFalvorList = ZFlavor;
                if (this.zFlavorList == null)
                    this.zFlavorList = new List<newZFlavor>();
                this.zFlavorList.Clear();
                foreach (var rec in ZFlavor)
                {
                    var nzflav = new newZFlavor();
                    nzflav.ID = rec.Id;// FlavorDetails.ID;
                    nzflav.Name = rec.Name;// FlavorDetails.Name;
                    zFlavorList.Add(nzflav);
                }
            }
        }

        public List<Server> GetServerList()
        {
            return (this.zServerList);
        }

        public List<newZImage> GetImageList()
        {
            return (this.zImageList);
        }

        public List<newZFlavor> GetFlavorList()
        {
            return (this.zFlavorList);
        }


        public void DeletezServerMachine(string MachineID)
        {
            try
            {
                zServerList.Remove(zServerList.FirstOrDefault(zserver => zserver.Id == MachineID));
            }
            catch (Exception ex)
            {

                throw new Exception(" opration failed due to this error:" + ex.Message);
            }

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}