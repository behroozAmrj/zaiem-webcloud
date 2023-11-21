using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;
using CRC_SDK.Classes;
using net.openstack.Core.Domain;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Configuration;
using Cloud.Log.Tracking;
using System.Reflection;

namespace Cloud.IU.WEB.Hubs
{
    [HubName("Notifications")]
    public class Notifications : Hub
    {
        [HubMethodName("Start")]
        public void Start()
        {
            /*
            Task.Factory.StartNew(() => { 
                while(true)
                {
                    Clients.Caller.time(DateTime.Now.ToShortTimeString());
                    Thread.Sleep(1000);
                }
            
            });
            */
        }

        [HubMethodName("InitNotification")]
        public void InitNotification(string UserID , string connectionID)
        {
            try
            {

                string repositryServiceURL = ConfigurationManager.AppSettings["repository"].ToString();
                var basicBinding = new BasicHttpBinding();
                var repService = new RepositoryService.RepositoryClient(basicBinding,
                                                                        new EndpointAddress(repositryServiceURL));
                repService.updateConnectionID(UserID,
                                                connectionID);
            }
            catch (Exception ex)
            {

                //Logging.INFOlogRegistrer(ex.Message,
                //                         UserID,
                //                         MethodBase.GetCurrentMethod().DeclaringType);
                Logging.INFOLogRegisterToDB(UserID,
                                            "Notification.InitNotification",
                                            "execption",
                                            ex.Message);
            }
        }
    }
}