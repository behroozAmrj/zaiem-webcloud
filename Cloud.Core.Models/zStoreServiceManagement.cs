using Cloud.Foundation.Infrastructure;
using Cloud.IO.NetworkService;
using net.openstack.Core.Domain;
using net.openstack.Providers.Zstack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;


namespace Cloud.Core.Models
{
    public class zStoreServiceManagement 
    {
        private String userID = String.Empty;
        public List<CloudNetwork> cloudNetworks { get; set; }
        public zStoreServiceManagement(String UserID)
        {
            if (String.IsNullOrEmpty(UserID))
                throw new Exception("token is null or empty");
            this.userID = UserID;
        }
        public List<Appliance> RetrieveAppliances(String URL)
        {
            var repService = new RepositoryService.RepositoryClient();

            var userProSecurity = repService.getUser(userID) ;//STRepository.StrRepository.getUser(this.userID);

            String password = userProSecurity.Password;
            var cloudIndentity = new CloudIdentity();
            cloudIndentity.Username = userProSecurity.UserName;
            cloudIndentity.Password = userProSecurity.Password;
            cloudIndentity.TenantName = userProSecurity.UserTenant;
            var cloudNetworkProvider = new CloudNetworksProvider(cloudIndentity);
            this.cloudNetworks = new List<CloudNetwork>(cloudNetworkProvider.ListNetworks());

            if (!URL.EndsWith("/"))
                URL = String.Format("{0}/{1}",
                                    URL,
                                    userProSecurity.zStoreTenant);
            else
                URL = String.Format("{0}{1}",
                                    URL,
                                    userProSecurity.zStoreTenant);
            using (var zStoreService = new zStoreService(userProSecurity.zStoreToken))
            {
                var inptStream = zStoreService.displayContent(URL);
                using (var sReader = new StreamReader(inptStream))
                {
                    var aData = sReader.ReadToEnd();
                    var result = JObject.Parse(aData);
                    //List<Machine> myList = ((JArray)result).Select(x => new Machine { SessionID =  "item" }).ToList();
                    //var list = (JArray)aData[0];
                    var resultList = ((JArray)result["ServicesCatalog"]).Select(x => new Appliance
                    {
                        ID = (String)x["id"],
                        Name = x["name"].ToString(),
                        Owner = x["owner"].ToString(),
                        Description = x["description"].ToString(),
                        Tenant = x["tenant"].ToString(),
                        Public = (Boolean)x["public"],
                        Update = x["updated"].ToString(),
                        //Requirment = x["requirement"].ToObject<String[]>().ToArray()
                    }).ToList();

                    return (resultList);
                }
            }
        }

        public void createAnMachineWithAppliance(String URL, String applianceID, String networkID, String applianceName)
        {
            if ((String.IsNullOrEmpty(networkID)) ||
                (String.IsNullOrEmpty(URL)) ||
                (String.IsNullOrEmpty(applianceName)))
                throw new Exception("one or more parameres are null or empty");
            if (URL.Contains("http"))
                URL.Replace("http",
                            "ws");

            var repService = new RepositoryService.RepositoryClient();
            var user = repService.getUser(userID);// STRepository.StrRepository.getUser(this.userID);
            String token = user.zStoreToken;
            String tenant = user.zStoreTenant;
            String json = String.Format("{{\"Id\":\"{0}\",\"tenant_id\":\"{1}\",\"task\":\"run\",\"netId\": [\"{2}\"],\"name\":\"{3}\"}}",
                                        applianceID,
                                        tenant,
                                        networkID,
                                        applianceName);

            /*
            List<KeyValuePair<string, string>> header = new List<KeyValuePair<string, string>>();
            header.Add(new KeyValuePair<string, string>("X-Auth-Token", token));
            try
            {
                using (var wSocket = new WebSocket("ws://172.18.23.249:8888/install", null, null, header))
                {
                    wSocket.Open();
                    wSocket.Send(json);
                    wSocket.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            */
            //  # this class and method execute command
            var zStore = new zStoreService(new Func<String>(() => { return (token); })());
            INetworkServiceCommand zStoreCommans = new zStoreServiceCommand(URL,
                                                                           json,
                                                                           ServiceCommandName.zStoreNewAppliance,
                                                                           token);
            zStore.excuteCommand(zStoreCommans);
        }

        public void deleteAnAppliance(String URL, String applianceID)
        {


            if (String.IsNullOrEmpty(applianceID))
                throw new Exception("one or more parameres are null or empty");
            if (URL.Contains("http"))
                URL.Replace("http",
                            "ws");

            var repService = new RepositoryService.RepositoryClient();
            var user = repService.getUser(userID);// STRepository.StrRepository.getUser(this.userID);
            String token = user.zStoreToken;
            String tenant = user.zStoreTenant;
            String json = String.Format("{{\"Id\": \"{0}\",\"task\": \"Delete\"}}" ,
                                            applianceID );

            var zStore = new zStoreService(token);
            INetworkServiceCommand zStoreCommans = new zStoreServiceCommand(URL,
                                                                           json,
                                                                           ServiceCommandName.zStoreDeleteAppliance,
                                                                           token);
            zStore.excuteCommand(zStoreCommans);
        }

    }
}
