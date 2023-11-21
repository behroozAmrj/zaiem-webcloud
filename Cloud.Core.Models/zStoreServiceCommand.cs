using Cloud.Foundation.Infrastructure;
using net.openstack.Core.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace Cloud.Core.Models
{
    public class zStoreServiceCommand : INetworkServiceCommand
    {
        private ServiceCommandName commandName;
        private String url;
        private String Json;
        private String token;
        public zStoreServiceCommand(String URL , String json ,ServiceCommandName commandName , String Token)
        {
            this.commandName = commandName;
            if ((String.IsNullOrEmpty(URL)) ||
                (String.IsNullOrEmpty(Token)) ||
                (String.IsNullOrEmpty(json)))
                throw new Exception("one or more parameres are null or empty");
            else
            {
                this.url = URL;
                this.Json = json;
                this.token = Token;
            }
        }

        private void deleteAppliance()
        {
            List<KeyValuePair<string, string>> header = new List<KeyValuePair<string, string>>();
            header.Add(new KeyValuePair<string, string>("X-Auth-Token", this.token));
            using (var wSocket = new WebSocket(this.url, null, null, header))
            {
                wSocket.Open();
                wSocket.Send(this.Json);
                System.Threading.Thread.Sleep(10000);
                wSocket.Close();
            }
        }

        private void newAppliance()
        {
            Action<Guid, string> recievedMessage = new Action<Guid, string>((id, msg) =>
            {
                // todo write your own codes
            });

            AutoResetEvent m_MessageReceiveEvent = new AutoResetEvent(false);
            AutoResetEvent m_OpenedEvent = new AutoResetEvent(false);
            AutoResetEvent m_CloseEvent = new AutoResetEvent(false);
            AutoResetEvent m_ResponseEvent = new AutoResetEvent(false);

            List<KeyValuePair<string, string>> header = new List<KeyValuePair<string, string>>();
            header.Add(new KeyValuePair<string, string>("X-Auth-Token", this.token));
            using (var wSocket = new WebSocket(this.url, null, null, header))
            {
                var socketGuid = Guid.NewGuid();
                if (socketGuid == Guid.Empty)
                {
                    socketGuid = Guid.NewGuid();
                }

                wSocket.Opened += (sender, e) =>
                {
                    sockets.Add(socketGuid, wSocket);
                    //(sender as WebSocket).Send(bodyStr);
                    m_OpenedEvent.Set();
                };

                wSocket.DataReceived += (sender, e) =>
                {

                };

                wSocket.MessageReceived += (sender, e) =>
                {
                    if (recievedMessage != null)
                    {
                        //recievedMessage.Invoke(string.Format("{0}: {1}", requestNumber, e.Message));
                        recievedMessage.Invoke(socketGuid, e.Message);
                    }
                    m_MessageReceiveEvent.Set();
                };

                wSocket.Closed += (sender, e) =>
                {
                    m_CloseEvent.Set();
                    m_MessageReceiveEvent.Set();
                };

                wSocket.Error += (sender, e) =>
                {
                    if (recievedMessage != null)
                    {
                        //recievedMessage.Invoke(string.Format("{0}: {1}", requestNumber, string.Format("Error on WebSocket: {0}", e.Exception.Message)));
                        net.openstack.Core.Domain.Appliance appliance = new net.openstack.Core.Domain.Appliance("", "ERROR", "", true, "", DateTime.Now,
                                0, e.Exception.Message, "WebSocket has an error", "");
                        var installApplicationResponse =   new InstallApplianceResponse(appliance);
                        string msg = JsonConvert.SerializeObject(installApplicationResponse);
                        recievedMessage.Invoke(socketGuid, msg);
                        //recievedMessage.Invoke(string.Format("Error on WebSocket: {0}", e.Exception.Message));
                        wSocket.Close();
                    }
                    //throw e.Exception;
                };

                wSocket.Open();

                if (!m_OpenedEvent.WaitOne(TimeSpan.FromSeconds(6000)))
                {
                    if (recievedMessage != null)
                    {
                        //recievedMessage.Invoke(string.Format("{0}: {1}", requestNumber, "Failed to open session ontime."));
                        var appliance = new  net.openstack.Core.Domain.Appliance("", "ERROR", "", true, "", DateTime.Now,
                                0, "Failed to open session ontime.", "WebSocket has an error", "");
                        InstallApplianceResponse installApplicationResponse = new InstallApplianceResponse(appliance);
                        string msg = JsonConvert.SerializeObject(installApplicationResponse);
                        recievedMessage.Invoke(socketGuid, msg);
                        //recievedMessage.Invoke("Failed to open session ontime.");
                    }
                    //throw new Exception("Failed to open session ontime.");
                }

                wSocket.Send(this.Json);
                int webSocketTimeout = 6000;

                while (true)
                {
                    if (!m_MessageReceiveEvent.WaitOne(TimeSpan.FromSeconds(webSocketTimeout)))
                    {
                        if (wSocket.State == WebSocketState.Closed || wSocket.State == WebSocketState.None)
                        {
                            break;
                        }
                        wSocket.Close();
                        if (recievedMessage != null)
                        {
                            //recievedMessage.Invoke(string.Format("{0}: {1}", requestNumber, "Failed to recieve message ontime. WebSocket Close request sent."));
                            net.openstack.Core.Domain.Appliance appliance = new net.openstack.Core.Domain.Appliance("", "ERROR", "", true, "", DateTime.Now,
                                0, "Failed to recieve message ontime. WebSocket Close request sent.", "WebSocket has an error", "");
                            InstallApplianceResponse installApplicationResponse = new InstallApplianceResponse(appliance);
                            string msg = JsonConvert.SerializeObject(installApplicationResponse);
                            recievedMessage.Invoke(socketGuid, msg);
                        }
                        break;
                        //throw new Exception(string.Format("{0}: {1}", requestNumber, "Failed to recieve message ontime."));
                    }
                    if (wSocket.State == WebSocketState.Closed)
                    {
                        break;
                    }
                }

                if (wSocket.State == WebSocketState.Open)
                    if (!m_CloseEvent.WaitOne(TimeSpan.FromSeconds(webSocketTimeout)))
                    {
                        if (recievedMessage != null)
                        {
                            //recievedMessage.Invoke(string.Format("{0}: {1}", requestNumber, "Failed to close session ontime."));
                            net.openstack.Core.Domain.Appliance appliance = new net.openstack.Core.Domain.Appliance("", "ERROR", "", true, "", DateTime.Now,
                                0, "Failed to close session ontime.", "WebSocket has an error", "");
                            InstallApplianceResponse installApplicationResponse = new InstallApplianceResponse(appliance);
                            string msg = JsonConvert.SerializeObject(installApplicationResponse);
                            recievedMessage.Invoke(socketGuid, msg);
                            //recievedMessage.Invoke("Failed to close session ontime.");
                        }
                        //throw new Exception(string.Format("{0}: {1}", requestNumber, "Failed to close session ontime."));
                    }
            }
        }

        public void executeCommand()
        {
            switch(commandName)
            {
                case ServiceCommandName.zStoreNewAppliance: { newAppliance(); break; }
                case ServiceCommandName.zStoreDeleteAppliance: { deleteAppliance(); break; }
            }


        }



        static int reqNumber = 0;
        Dictionary<Guid, WebSocket> sockets = new Dictionary<Guid, WebSocket>();

        protected void CloseSocket(Guid socketGuid, CloudIdentity identity, string region = null)
        {
            if (socketGuid != null)
            {
                WebSocket wSocket = null;
                if (sockets.TryGetValue(socketGuid, out wSocket))
                {
                    wSocket.Close();
                    sockets.Remove(socketGuid);
                }
            }
        }
    }
}
