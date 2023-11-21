using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using CRC_SDK.Classes;
using net.openstack.Core.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Cloud.Core.Models
{
    public class STRepository
    {
        public static readonly STRepository StrRepository = new STRepository();
        List<Server> Zserver;
        List<ServerImage> Zimage;
        List<Flavor> ZFlavor;

        //  List<newZImage> nZImageList = new List<newZImage>();
        // List<newZFlavor> nZFlavorList = new List<newZFlavor>();

        Dictionary<string, zStack> crcCloudStore = new Dictionary<string, zStack>();

        Dictionary<string, List<string>> crcCloudLogs = new Dictionary<string, List<string>>();
        Dictionary<string, UserProSecurity> UserProperyStore = new Dictionary<string, UserProSecurity>();
        Dictionary<string, List<IEntity>> DeskTopItems = new Dictionary<string, List<IEntity>>();
        Dictionary<String, String> UserSessionMap = new Dictionary<string, string>();
        private int userTimeout = 30;
        #region user specific servers


        //  #   zDrive ///////////////////     zStore    /////////////////////////////////////////////////////////////////                                                                       
        public void setUserCredentialToken(String userID, String userName, String passWord, String tentant, String token,  String zStoreToken , String zStoreTenant, String storageURL, DateTime expiredTime, String role = null)
        {
            if (UserProperyStore.ContainsKey(userID))
            {
                lock (UserProperyStore[userID])
                {
                    var userProper = UserProperyStore[userID] as UserProSecurity;
                    userProper.Token = token;
                    userProper.StorageURL = storageURL;
                    userProper.UserTenant = tentant;
                    userProper.Role = role;
                    userProper.zStoreTenant = zStoreTenant;
                    userProper.zStoreToken = zStoreToken;
                }
            }
            else
            {
                var userProperty = new UserProSecurity();
                userProperty.UserID = userID;
                userProperty.Token = token;
                userProperty.StorageURL = storageURL;
                userProperty.UserName = userName;
                userProperty.Password = passWord;
                userProperty.UserTenant = tentant;
                userProperty.Role = role;
                userProperty.zStoreToken = zStoreToken;
                userProperty.zStoreTenant = zStoreTenant;
               // userProperty.onUserTimeout += userProperty_onUserTimeout;
                UserProperyStore.Add(userID,
                                        userProperty);
            }
        }

        public UserProSecurity getUser(String userID)
        {
            if (UserProperyStore.ContainsKey(userID))
            {
                return (UserProperyStore[userID] as UserProSecurity);
            }
            else
                return (null);
        }

        
        public String getUserStorageURL(String UserID)
        {
            String url = String.Empty;
            if (UserProperyStore.ContainsKey(UserID))
            {
                lock (UserProperyStore[UserID])
                {
                    var userProper = UserProperyStore[UserID] as UserProSecurity;
                    url = userProper.StorageURL;
                }
            }
            else
            {
                url = String.Empty;
            }

            return (url);

        }

        public void removeUser(String UserID)
        {
            if (UserProperyStore.ContainsKey(UserID))
            {
                UserProperyStore.Remove(UserID);
            }

        }
        public String getUserCredentialToken(String UserID)
        {
            String token = String.Empty;
            if (UserProperyStore.ContainsKey(UserID))
            {
                lock (UserProperyStore[UserID])
                {
                    var userProper = UserProperyStore[UserID] as UserProSecurity;
                    var loginDatetime = userProper.LoginTime;
                    if (DateTime.Compare(DateTime.Now, loginDatetime) > 0)
                        token = userProper.Token;
                    else
                    {
                        UserProperyStore.Remove(UserID);
                        token = string.Empty;
                    }
                }
            }
            else
            {
                token = String.Empty;
            }

            return (token);
        }

        public UserProSecurity getUserCredential(String UserID)
        {
            UserProSecurity userCredential;
            if (UserProperyStore.ContainsKey(UserID))
            {
                lock (UserProperyStore[UserID])
                {
                    var userProper = UserProperyStore[UserID] as UserProSecurity;
                    var loginDatetime = userProper.LoginTime;
                    if (DateTime.Compare(DateTime.Now, loginDatetime) > 0)
                        userCredential = userProper;
                    else
                    {
                        UserProperyStore.Remove(UserID);
                        userCredential = null;
                    }
                }
            }
            else
            {
                userCredential = null;
            }

            return (userCredential);
        }

        public void FillupServer(List<Server> PZSrever, string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
                if (UserProperyStore.ContainsKey(UserID))
                {
                    var uzseverlist = UserProperyStore[UserID] as UserProSecurity;
                    lock (UserProperyStore[UserID])
                    {
                        uzseverlist.FillupServer(PZSrever);
                    }
                }
                else
                {
                    var userproperty = new UserProSecurity();
                    userproperty.FillupServer(PZSrever);
                    userproperty.UserID = UserID;
                    UserProperyStore.Add(UserID,
                                        userproperty);
                }

        }

        public void FillupServer(Server zServer, string UserID)
        {

            if (!string.IsNullOrEmpty(UserID))
                if (UserProperyStore.ContainsKey(UserID))
                {
                    lock (UserProperyStore[UserID])
                    {
                        var uzimagelist = UserProperyStore[UserID] as UserProSecurity;
                        uzimagelist.FillupServer(zServer);
                    }
                }
                else
                {
                    var userproperty = new UserProSecurity();
                    userproperty.FillupServer(zServer);
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

        public Server RetrievezServer(string MachineID, string UserID)
        {
            if ((!string.IsNullOrEmpty(MachineID)) &&
                (!string.IsNullOrEmpty(UserID)))
            {
                if (UserProperyStore.ContainsKey(UserID))
                {
                    var user = UserProperyStore[UserID] as UserProSecurity;
                    var zserver = user.SelectServer(MachineID);
                    return (zserver);
                }
                else
                { throw new Exception("user dose not exist "); }
            }
            else
            { throw new Exception(" one or more parameters are null. . ."); }
        }


        public void FillUpZImage(List<ServerImage> ZImage, string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
                if (UserProperyStore.ContainsKey(UserID))
                {
                    lock (UserProperyStore[UserID])
                    {
                        var uzimagelist = UserProperyStore[UserID] as UserProSecurity;
                        uzimagelist.FillUpImage(ZImage);
                    }
                }
                else
                {
                    var user = new UserProSecurity();
                    user.UserID = UserID;
                    user.FillUpImage(ZImage);
                    this.UserProperyStore.Add(UserID,
                                                user);
                }

        }

        public void FillUp_ZFlavor(List<FlavorDetails> ZFlavor, string UserID)
        {

            if (!string.IsNullOrEmpty(UserID))
                if (UserProperyStore.ContainsKey(UserID))
                {
                    lock (UserProperyStore[UserID])
                    {
                        var uzimagelist = UserProperyStore[UserID] as UserProSecurity;
                        uzimagelist.FillUp_Flavor(ZFlavor);
                    }
                }
                else
                {
                    var user = new UserProSecurity();
                    user.UserID = UserID;
                    user.FillUp_Flavor(ZFlavor);
                    this.UserProperyStore.Add(UserID,
                                                user);

                }
        }

        public List<Server> GetZserverList(string UserID)
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                if (UserProperyStore.ContainsKey(UserID))
                {
                    var userproperty = UserProperyStore[UserID] as UserProSecurity;
                    var zserverlist = userproperty.GetServerList();
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
                    var zimagelist = userproperty.GetImageList();
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
                    var zflavorlist = user.GetFlavorList();
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
                { return (null); }
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
                    var zserverlist = userproperty.GetServerList();
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
                        var machine = new Machine(UserID,
                                                    zserv.Name,
                                                   "/images/Play.png",
                                                   zserv,
                                                   EntityType.Machine);

                        machinelist.Add(machine);
                    }
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

           // var desktoplist = list[1] as List<IEntity>;

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


        #region user and mapped session

        public void insertUserAndSession(String userId, String sessionID)
        {
            if ((!String.IsNullOrWhiteSpace(userId)) ||
                (!String.IsNullOrWhiteSpace(sessionID)))
            {
                if (!UserSessionMap.ContainsKey(sessionID))
                {

                    UserSessionMap.Add(sessionID,
                                        userId);
                }
            }
        }

        public String getUserBasedOnSessionID(String sessionID)
        {
            String userID = "";
            if (!String.IsNullOrWhiteSpace(sessionID))
                if (UserSessionMap.ContainsKey(sessionID))
                    userID = UserSessionMap[sessionID];
            return (userID);
        }
        #endregion

    }
}
