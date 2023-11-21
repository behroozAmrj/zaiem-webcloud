

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RespositoryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Repository" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Repository.svc or Repository.svc.cs at the Solution Explorer and start debugging.
    public class Repository : IRepository
    {
        public string getUserToken(string userID)
        {
            string userToken = STRepository.StrRepository.getUserCredentialToken(userID);
            return (userToken);
        }

        public void setUserCredentialToken(String userID, String userName, String passWord, String tentant, String token, String zStoreToken, String zStoreTenant, String storageURL, DateTime expiredTime, String ConnectionID, String role = null)
        {

            STRepository.StrRepository.setUserCredentialToken(userID,
                                                                userName,
                                                                passWord,
                                                                tentant,
                                                                token,
                                                                zStoreToken,
                                                                zStoreTenant,
                                                                storageURL,
                                                                expiredTime,
                                                                ConnectionID,
                                                                role);

        }

        public void insertUserAndSession(string userID, string sessionID)
        {
            STRepository.StrRepository.insertUserAndSession(userID,
                                                                sessionID);
        }


        public RUserProSecurity getUser(string userID)
        {
            var user = STRepository.StrRepository.getUser(userID);//getUser(userID);
            return (user);
        }



        public string getUserBasedOnSessionID(string sessionID)
        {
            string userID = STRepository.StrRepository.getUserBasedOnSessionID(sessionID);
            return (userID);
        }


        public void updateConnectionID(string userID, string connectionID)
        {
            if (!string.IsNullOrEmpty(userID))
            {
                STRepository.StrRepository.updateUserConnection(userID,
                                                                connectionID);
            }
        }

        public List<RUserProSecurity> getOnlineUsers()
        {
            var list = STRepository.StrRepository.getOnlineUser();
            return (list.ToList());

        }


        public void removeUser(string userID)
        {
            if (!string.IsNullOrEmpty(userID))
                STRepository.StrRepository.removeUser(userID);
        }
    }
}
