using CRC_SDK.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud.IU.WEB.Models
{
    public class UserProSecurity
    {
        public string UserID { get; set; }
        List<zServer> zServerList;
        List<zImage> ozImageList;
        List<zFlavor> ozFalvorList;

        List<newZImage> zImageList;
        List<newZFlavor> zFlavorList;


        public void FillupZSrever(List<zServer> ZServer)
        {
            if (ZServer != null)
            {
                if (this.zServerList == null)
                    this.zServerList = new List<zServer>();
                this.zServerList = ZServer;
            }
        }

        public void FillupZSrever(zServer zServer)
        {
            if (zServer != null)
            {
                if (this.zServerList == null)
                {
                    zServerList = new List<zServer>();
                    this.zServerList.Add(zServer);
                }
                else
                {
                    var serv = this.zServerList.FirstOrDefault(zserver => zserver.ID == zServer.ID);
                    if (serv == null)
                        this.zServerList.Add(zServer);
                }
            }
        }


        public zServer SelectzServer(string MachineID)
        {
            if (!string.IsNullOrEmpty(MachineID))
            {
                if (this.zServerList != null)
                {
                    var zserv = zServerList.FirstOrDefault(zserver => zserver.ID == MachineID);
                    return (zserv);
                }
                else
                { throw new Exception("no zserver list "); }
            }
            else
            { throw new Exception("null Machine ID"); }
        }


        public void FillUpZImage(List<zImage> ZImage)
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
                    nzimg.ID = rec.ImageDetails.ID;
                    nzimg.Name = rec.ImageDetails.Name;
                    zImageList.Add(nzimg);
                }
            }
        }

        public void FillUp_ZFlavor(List<zFlavor> ZFlavor)
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
                    nzflav.ID = rec.FlavorDetails.ID;
                    nzflav.Name = rec.FlavorDetails.Name;
                    zFlavorList.Add(nzflav);
                }
            }
        }

        public List<zServer> GetZserverList()
        {
            return (this.zServerList);
        }

        public List<newZImage> GetZimageList()
        {
            return (this.zImageList);
        }

        public List<newZFlavor> GetZFlavorList()
        {
            return (this.zFlavorList);
        }


        public void DeletezServerMachine(string MachineID)
        {
            try
            {
                zServerList.Remove(zServerList.FirstOrDefault(zserver => zserver.ID == MachineID));
            }
            catch (Exception ex)
            {
                
                throw new Exception(" opration failed due to this error:" + ex.Message);
            }
                    
        }
    }
}