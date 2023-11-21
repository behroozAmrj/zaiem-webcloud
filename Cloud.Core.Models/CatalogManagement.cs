using Cloud.Foundation.Infrastructure;
using Cloud.IO.NetworkService;
using Cloud.IO.Repository;
using Cloud.Log.Tracking;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    public class CatalogManagement
    {
        List<String> urlList;
        String token;
        List<IAppCatalog> appCatalog = new List<IAppCatalog>();
        private string userID;
        public CatalogManagement(String UserID)
        {

            if (String.IsNullOrEmpty(UserID))
                throw new Exception("one or more parameters are null");
            string repositryServiceURL = ConfigurationManager.AppSettings["repository"].ToString();
            var basicBinding = new BasicHttpBinding();
            var repService = new RepositoryService.RepositoryClient(basicBinding,
                                                                        new EndpointAddress(repositryServiceURL));
            this.token = repService.getUser(userID).Token;// STRepository.StrRepository.getUser(UserID).Token;
            this.userID = UserID;

        }


        public List<IAppCatalog> getApplicationList()
        {
            Logging.INFOlogRegistrer("request:getApplicationList",
                                        this.userID,
                                        MethodBase.GetCurrentMethod().GetType());
            var cloudServer = new CloudServers(this.token);
            var ExistAppServer = new ExistApps();
            String xPath = String.Format("//root/applications/application/User[not(contains(@ID, '{0}')) and not(contains(@ID , '*'))]",
                                                                           userID);
            // var rStream = cloudServer.RetrieveContent(urlList[0]);
            var appResult = ExistAppServer.RetrieveContent(userID,
                                                               new object[]{ AppDomain.CurrentDomain.BaseDirectory + "\\Application-Service\\AppsConfig.xml", 
                                                                            xPath}) as List<RBSConfigRepository>;
            // # in this gap we get list of application whichi is in cloud and we retrieve from it and combine together

            return (convertRBSToAppList(appResult));
        }


        private List<IAppCatalog> convertRBSToAppList(List<RBSConfigRepository> rbsList)
        {
            var existApp = rbsList.Select(x => new ExistApp()
            {
                Name = x.Name,
                Scope = x.Directory
            }).ToList<IAppCatalog>();

            return (existApp);
        }

        private List<IAppCatalog> convertCatalogAppToList(Stream stream, String address)
        {
            using (var sReader = new StreamReader(stream))
            {
                // = new List<String>();
                var content = sReader.ReadToEnd();
                var jo = JObject.Parse(content);
                //var children = jo["children"].ToObject<List<String>>();
                var retList = jo["children"].ToObject<List<String>>();
                //foreach (string fileandDirectory in children)
                //  retList.Add(fileandDirectory);
                sReader.Close();
                //return (retList);
            }
            return (null);
        }


        public IList<IAppCatalog> InstallAppforUser(IAppCatalog appCatalog)
        {
            Logging.INFOlogRegistrer("request:InstallAppforUser",
                                        this.userID,
                                        MethodBase.GetCurrentMethod().GetType());
            if (appCatalog != null)
            {
                appCatalog.Install(new object());
            }
            return (getApplicationList());
        }

        public void UnInstallCatalog(string catalogID)
        {
            Logging.INFOlogRegistrer("delete a catalog",
                                        this.userID,
                                        MethodBase.GetCurrentMethod().GetType());
            if (string.IsNullOrEmpty(catalogID))
                throw new Exception("catalogID is null or empty");

            var existApp = new ExistApp(this.userID);
            existApp.Remove(null);
        }

    }
}
