
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RespositoryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRepository" in both code and config file together.
    [ServiceContract]
    public interface IRepository
    {
        [OperationContract]
        string getUserToken(string userID);

        [OperationContract]
        void setUserCredentialToken(String userID, String userName, String passWord, String tentant, String token, String zStoreToken, String zStoreTenant, String storageURL, DateTime expiredTime,  String ConnectionID ,String role = null );
      
        [OperationContract]
        void insertUserAndSession(string userID, string sessionID);

        [OperationContract]
        RUserProSecurity getUser(String userID);
       
        [OperationContract]
        String getUserBasedOnSessionID(String sessionID);
        
        [OperationContract]
        void updateConnectionID(String userID, String connectionID);

        [OperationContract]
        List<RUserProSecurity> getOnlineUsers();

        [OperationContract]
        void removeUser(string userID);


    }
}
