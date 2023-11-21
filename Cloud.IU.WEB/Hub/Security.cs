using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using log4net;
using System.Reflection;
using Cloud.Core.Models;
using Cloud.Log.Tracking;
using net.openstack.Core.Domain;
using net.openstack.Providers.Zstack;
using Cloud.Foundation.Infrastructure;

namespace Cloud.IU.WEB.Hubs
{
    [HubName("Security")]
    public class Security : Hub
    {
        //ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string _connectionID;
        // zStack crccloud = new zStack();
        private string SessionID;
        string credentials;
        private ISecurity loginSecurity;
        [HubMethodName("Login")]
        public void Login(string UserName, string Password, string cloudUserName, string cloudPassword, string ConnectionID, string SessionID)
        {
            credentials = UserName;

            //logger.Info("login request");

            //Logging.INFOlogRegistrer(string.Format("Login Request:conID{0}", ConnectionID),
            //                                UserName,
            //                                MethodBase.GetCurrentMethod().DeclaringType);

            string userID = string.Empty;
            string action ="Security.Login"; 
            string actionType = "info"; 
            string content =string.Format("userName:{0}",
                                            UserName);

            try
            {
                if ((!string.IsNullOrEmpty(UserName)) &&
                    (!string.IsNullOrEmpty(Password)) &&
                    (!string.IsNullOrEmpty(cloudUserName)) &&
                    (!string.IsNullOrEmpty(cloudPassword)) &&
                    (!string.IsNullOrEmpty(ConnectionID)) &&
                    (!string.IsNullOrEmpty(SessionID)))
                {
                    this._connectionID = ConnectionID;
                    this.SessionID = SessionID;

                    String[] options = new String[] { cloudUserName, cloudPassword, SessionID, ConnectionID };
                    //var cloudIdentity = new CloudIdentity();

                    loginSecurity = new LoginSecurity_v1();
                    var permissionAnduserId = loginSecurity.Authenticate(UserName,
                                                                            Password,
                                                                            options)[0].ToString();
                    if (!String.IsNullOrWhiteSpace(permissionAnduserId))
                    {
                        Logging.INFOLogRegisterToDB(permissionAnduserId,
                                                    action,
                                                    actionType,
                                                    content);
                        SendBackResponse(_connectionID,
                                            permissionAnduserId);
                    }
                    else
                    {
                        Logging.INFOLogRegisterToDB(string.Empty,
                                                    action,
                                                    "fail",
                                                    content);
                        OnRaiseError(_connectionID,
                                        "login failed try again!");
                    }
                }
                else
                {
                    throw new Exception(" one or more parameters are empty");
                }
            }
            catch (Exception ex)
            {
                //logger.Error(string.Format("Login Request failed by this error {0}",
                //                            ex.Message));
                Logging.INFOLogRegisterToDB(string.Empty,
                                                   action,
                                                   "exception",
                                                   content + " : " + ex.Message);
                var msg = string.Format("invalid useName or password",
                                                ex.Message);
                //Logging.ErrorlogRegister(msg,
                //                            MethodBase.GetCurrentMethod().GetType(),
                //                            String.Format("{0} {1}", UserName, Password));
                OnRaiseError(ConnectionID,
                             msg);
            }
        }

        public void SendBackResponse(string ConnectionID, string UserID)
        {
            Clients.Client(ConnectionID).response(UserID);
        }

        public void OnRaiseError(string ConnectionID, string ErrorMessage)
        {
            Clients.Client(ConnectionID).onraiseError(ErrorMessage);
        }

    }
}