
using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using CRC_SDK.Classes;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace RespositoryService
{
    public class STRepository
    {
        internal static readonly STRepository StrRepository = new STRepository();


        //  List<newZImage> nZImageList = new List<newZImage>();
        // List<newZFlavor> nZFlavorList = new List<newZFlavor>();

        //Dictionary<string, zStack> crcCloudStore = new Dictionary<string, zStack>();


        Dictionary<string, RUserProSecurity> UserProperyStore = new Dictionary<string, RUserProSecurity>();
        Dictionary<string, List<IEntity>> DeskTopItems = new Dictionary<string, List<IEntity>>();
        Dictionary<String, String> UserSessionMap = new Dictionary<string, string>();
        private int userTimeout = 30;
        #region user specific servers


        //  #   zDrive ///////////////////     zStore    /////////////////////////////////////////////////////////////////                                                                       
        internal void setUserCredentialToken(String userID, String userName, String passWord, String tentant, String token, String zStoreToken, String zStoreTenant, String storageURL, DateTime expiredTime, String ConnectionID, String role = null)
        {
            if (UserProperyStore.ContainsKey(userID))
            {
                lock (UserProperyStore[userID])
                {
                    var userProper = UserProperyStore[userID] as RUserProSecurity;
                    userProper.Token = token;
                    userProper.StorageURL = storageURL;
                    userProper.UserTenant = tentant;
                    userProper.Role = role;
                    userProper.zStoreTenant = zStoreTenant;
                    userProper.zStoreToken = zStoreToken;
                    userProper.ConnectionID = ConnectionID;
                }
            }
            else
            {
                var userProperty = new RUserProSecurity();
                userProperty.UserID = userID;
                userProperty.Token = token;
                userProperty.StorageURL = storageURL;
                userProperty.UserName = userName;
                userProperty.Password = passWord;
                userProperty.UserTenant = tentant;
                userProperty.Role = role;
                userProperty.zStoreToken = zStoreToken;
                userProperty.zStoreTenant = zStoreTenant;
                userProperty.ConnectionID = ConnectionID;
                UserProperyStore.Add(userID,
                                        userProperty);
            }
        }

        internal RUserProSecurity getUser(String userID)
        {
            if (UserProperyStore.ContainsKey(userID))
            {
                return (UserProperyStore[userID] as RUserProSecurity);
            }
            else
                return (null);
        }


        internal String getUserStorageURL(String UserID)
        {
            String url = String.Empty;
            if (UserProperyStore.ContainsKey(UserID))
            {
                lock (UserProperyStore[UserID])
                {
                    var userProper = UserProperyStore[UserID] as RUserProSecurity;
                    url = userProper.StorageURL;
                }
            }
            else
            {
                url = String.Empty;
            }

            return (url);

        }

        internal void removeUser(String UserID)
        {
            if (UserProperyStore.ContainsKey(UserID))
            {
                UserProperyStore.Remove(UserID);
            }

        }
        internal String getUserCredentialToken(String UserID)
        {
            String token = String.Empty;
            if (UserProperyStore.ContainsKey(UserID))
            {
                lock (UserProperyStore[UserID])
                {
                    var userProper = UserProperyStore[UserID] as RUserProSecurity;
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

        internal RUserProSecurity getUserCredential(String UserID)
        {
            RUserProSecurity userCredential;
            if (UserProperyStore.ContainsKey(UserID))
            {
                lock (UserProperyStore[UserID])
                {
                    var userProper = UserProperyStore[UserID] as RUserProSecurity;
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

        internal List<RUserProSecurity> getOnlineUser()
        {
            var userList = new List<RUserProSecurity>();
            foreach (var user in UserProperyStore.Values)
            {
                userList.Add(user);
            }
            return (userList);
        }







        #endregion

        #region  zstack zone
        /*
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
        */

        #endregion

       


        #region  DeskTop

        internal List<IEntity> RetrieveDesktopItem(string UserID)
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

        //# this method is used to get destktop items in built-in way and in service-deriven it is not used
        //public void Async_RetrieveDesktopItem(object List)
        //{
        //    var list = List as object[];
        //    var caller = list[0] as DeskTopBuilder;
        //    string userid = (string)list[1];

        //    var desktoplist = list[1] as List<IEntity>;

        //    if (!string.IsNullOrEmpty(userid))
        //    {
        //        if (DeskTopItems.ContainsKey(userid))
        //        {
        //            caller.dtitems = DeskTopItems[userid] as List<IEntity>;

        //            //DeskTopList = desktopitems;
        //            //return (desktopitems);
        //        }
        //        else
        //        { //return (null); 
        //        }
        //    }
        //    else
        //    { throw new Exception(" User ID in null or empty "); };

        //}

        #endregion


        #region user and mapped session

        internal void insertUserAndSession(String userId, String sessionID)
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

        internal String getUserBasedOnSessionID(String sessionID)
        {
            String userID = "";
            if (!String.IsNullOrWhiteSpace(sessionID))
                if (UserSessionMap.ContainsKey(sessionID))
                    userID = UserSessionMap[sessionID];
            return (userID);
        }
        #endregion


        internal void updateUserConnection(string userID, string connectionID)
        {
            if (UserProperyStore.ContainsKey(userID))
            {
                lock (UserProperyStore[userID])
                {
                    var userProper = UserProperyStore[userID] as RUserProSecurity;
                    userProper.ConnectionID = connectionID;
                }
            }
        }
    }
}
