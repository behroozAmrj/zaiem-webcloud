using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using CRC_SDK.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud.IU.WEB.Models
{
    public class STRepository
    {
        public static readonly STRepository StrRepository = new STRepository();
        List<zServer> Zserver;
        List<zImage> Zimage;
        List<zFlavor> ZFlavor;

        List<newZImage> nZImageList = new List<newZImage>();
        List<newZFlavor> nZFlavorList = new List<newZFlavor>();

        Dictionary<string, zStack> crcCloudStore = new Dictionary<string, zStack>();

        Dictionary<string, List<string>> crcCloudLogs = new Dictionary<string, List<string>>();
        Dictionary<string, UserProSecurity> UserProperyStore = new Dictionary<string, UserProSecurity>();
        Dictionary<string, List<IEntity>> DeskTopItems = new Dictionary<string, List<IEntity>>();
        #region user specific servers
        public void FillupZSrever(List<zServer> PZSrever, string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
                if (UserProperyStore.ContainsKey(UserID))
                {
                    var uzseverlist = UserProperyStore[UserID] as UserProSecurity;
                    lock (UserProperyStore[UserID])
                    {
                        uzseverlist.FillupZSrever(PZSrever);
                    }
                }
                else
                {
                    var userproperty = new UserProSecurity();
                    userproperty.FillupZSrever(PZSrever);
                    userproperty.UserID = UserID;
                    UserProperyStore.Add(UserID,
                                        userproperty);
                }

        }

        public void FillupZSrever(zServer zServer, string UserID)
        {

            if (!string.IsNullOrEmpty(UserID))
                if (UserProperyStore.ContainsKey(UserID))
                {
                    lock (UserProperyStore[UserID])
                    {
                        var uzimagelist = UserProperyStore[UserID] as UserProSecurity;
                        uzimagelist.FillupZSrever(zServer);
                    }
                }
                else
                {
                    var userproperty = new UserProSecurity();
                    userproperty.FillupZSrever(zServer);
                    userproperty.UserID = UserID;
                    UserProperyStore.Add(UserID,
                                        userproperty);
                }
        }

        public void DeletezServerMachine(string MachineID, string UserID)
        {

            if ((!string.IsNullOrEmpty(MachineID)) &&
                  (!string.IsNullOrEmpty(UserID)))
            {
                if (UserProperyStore.ContainsKey(UserID))
                {
                    lock (UserProperyStore[UserID])
                    {
                        var user = UserProperyStore[UserID] as UserProSecurity;
                        user.DeletezServerMachine(MachineID);
                    }

                }
                else
                { throw new Exception("user dose not exist "); }
            }
            else
            { throw new Exception("one or more parameters are null . . ."); }
        }

        public zServer RetrievezServer(string MachineID, string UserID)
        {
            if ((!string.IsNullOrEmpty(MachineID)) &&
                (!string.IsNullOrEmpty(UserID)))
            {
                if (UserProperyStore.ContainsKey(UserID))
                {
                    var user = UserProperyStore[UserID] as UserProSecurity;
                    var zserver = user.SelectzServer(MachineID);
                    return (zserver);
                }
                else
                { throw new Exception("user dose not exist "); }
            }
            else
            { throw new Exception(" one or more parameters are null. . ."); }
        }


        public void FillUpZImage(List<zImage> ZImage, string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
                if (UserProperyStore.ContainsKey(UserID))
                {
                    lock (UserProperyStore[UserID])
                    {
                        var uzimagelist = UserProperyStore[UserID] as UserProSecurity;
                        uzimagelist.FillUpZImage(ZImage);
                    }
                }
                else
                {
                    var user = new UserProSecurity();
                    user.UserID = UserID;
                    user.FillUpZImage(ZImage);
                    this.UserProperyStore.Add(UserID,
                                                user);
                }

        }

        public void FillUp_ZFlavor(List<zFlavor> ZFlavor, string UserID)
        {

            if (!string.IsNullOrEmpty(UserID))
                if (UserProperyStore.ContainsKey(UserID))
                {
                    lock (UserProperyStore[UserID])
                    {
                        var uzimagelist = UserProperyStore[UserID] as UserProSecurity;
                        uzimagelist.FillUp_ZFlavor(ZFlavor);
                    }
                }
                else
                {
                    var user = new UserProSecurity();
                    user.UserID = UserID;
                    user.FillUp_ZFlavor(ZFlavor);
                    this.UserProperyStore.Add(UserID,
                                                user);

                }
        }

        public List<zServer> GetZserverList(string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                if (UserProperyStore.ContainsKey(UserID))
                {
                    var userproperty = UserProperyStore[UserID] as UserProSecurity;
                    var zserverlist = userproperty.GetZserverList();
                    return (zserverlist);

                }
                else
                { throw new Exception("use dose not exist . . . "); }
            }
            else
            { throw new Exception("null user ID"); }

        }

        public List<newZImage> GetZimageList(string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                if (UserProperyStore.ContainsKey(UserID))
                {
                    var userproperty = UserProperyStore[UserID] as UserProSecurity;
                    var zimagelist = userproperty.GetZimageList();
                    return (zimagelist);
                }
                else
                { throw new Exception("user dose not exist  . . ."); }
            }
            else
            { throw new Exception(" null user ID"); }
        }

        public List<newZFlavor> GetZFlavorList(string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                if (UserProperyStore.ContainsKey(UserID))
                {
                    var user = UserProperyStore[UserID] as UserProSecurity;
                    var zflavorlist = user.GetZFlavorList();
                    return (zflavorlist);
                }
                else
                { throw new Exception("user dose not exist "); }
            }
            else
            { throw new Exception("null User ID"); }

        }

        #endregion

        #region  zstack zone
        public void AddzStackToRepository(string SessionID, zStack CrcCloud)
        {
            if (!string.IsNullOrEmpty(SessionID))
                if ((!crcCloudStore.ContainsKey(SessionID)) &&
                    (CrcCloud != null))
                {
                    crcCloudStore.Add(SessionID,
                                        CrcCloud);
                }
        }


        public zStack RetrievezStack(string SessionID)
        {
            if (!string.IsNullOrEmpty(SessionID))
            {
                if (crcCloudStore.ContainsKey(SessionID))
                {
                    var crccloud = crcCloudStore[SessionID] as zStack;
                    return (crccloud);
                }
                else
                { return (null);}
            }
            else
            {
                throw new Exception("[STRpository] SessionID is Null");
            }
        }


        #endregion



        public void Logs(string UserID, string Description)
        {
            if ((!string.IsNullOrEmpty(UserID))
                && (!string.IsNullOrEmpty(Description)))
            {
                if (crcCloudLogs.ContainsKey(UserID))
                {
                    var loglist = crcCloudLogs[UserID] as List<string>;
                    loglist.Add(Description);

                }
                else
                {
                    var loglist = new List<string>();
                    loglist.Add(Description);
                    crcCloudLogs.Add(UserID,
                                        loglist);
                }
            }
            else
            { throw new Exception("one or more parameters are empty"); }
        }

        public List<string> Retrieve_LogsList(string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                if (crcCloudLogs.ContainsKey(UserID))
                {
                    lock (crcCloudLogs[UserID])
                    {
                        var loglist = crcCloudLogs[UserID] as List<string>;
                        return (loglist);
                    }
                }
                else
                { throw new Exception("user dose not exists . . . "); }
            }
            else
            { throw new Exception("one or more parameters are empty"); }
        }



        #region  DeskTop

        public void Convert_zServerToMachine(string UserID)
        {

            if (!string.IsNullOrEmpty(UserID))
            {
                if (UserProperyStore.ContainsKey(UserID))
                {
                    var userproperty = UserProperyStore[UserID] as UserProSecurity;
                    var zserverlist = userproperty.GetZserverList();
                    List<IEntity> machinelist;// = new List<IEntity>();
                    if (DeskTopItems.ContainsKey(UserID))
                    {
                        machinelist = DeskTopItems[UserID] as List<IEntity>;
                        machinelist.Clear();
                    }
                    else
                    {
                        machinelist = new List<IEntity>();
                        DeskTopItems.Add(UserID,
                                        machinelist);
                    }
                    foreach (var zserv in zserverlist)
                    {
                        var machine = new Machine(zserv.Name,
                                                   "/images/Play.png",
                                                   zserv,
                                                   EntityType.Machine);
                        machinelist.Add(machine);
                    }
                    //var temp = new TItem("test");
                    //temp.Titel = "test";
                    //temp.Type = EntityType.Text;
                    //temp.Handler = "hello-behrooz";
                    //temp.Image = "/images/reload.png";
                    //var titemlist = DeskTopItems[UserID] as List<IEntity>;
                    //titemlist.Add(temp);
                }
            }
        }



        public List<IEntity> RetrieveDesktopItem(string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                if (DeskTopItems.ContainsKey(UserID))
                {
                    var desktopitems = DeskTopItems[UserID] as List<IEntity>;
                    return (desktopitems);
                }
                else
                { return (null); }
            }
            else
            { throw new Exception(" User ID in null or empty "); };
        }

        public void Async_RetrieveDesktopItem(object List)
        {
            var list = List as object[];
            var caller = list[0] as DeskTopBuilder;
            string userid = (string)list[1];

            var desktoplist = list[1] as List<IEntity>;

            if (!string.IsNullOrEmpty(userid))
            {
                if (DeskTopItems.ContainsKey(userid))
                {
                     caller.dtitems = DeskTopItems[userid] as List<IEntity>;

                    //DeskTopList = desktopitems;
                    //return (desktopitems);
                }
                else
                { //return (null); 
                }
            }
            else
            { throw new Exception(" User ID in null or empty "); };

        }

        #endregion

    }
}
