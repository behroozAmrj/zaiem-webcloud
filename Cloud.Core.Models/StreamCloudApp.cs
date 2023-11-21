using Cloud.Foundation.Infrastructure;
using Cloud.IO.NetworkService;
using Cloud.IO.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    public class StreamCloudApp : IAppCatalog
    {
        private string _name;
        private string _destination;
        private string _departure;
        private AppCatalogType appType = AppCatalogType.CloudApp;
        public string Name
        {
            get
            {
                return (this._name);
            }
            set
            {
                this._name = value;
            }
        }

        public string Destination
        {
            get
            {
                return (this._destination);
            }
            set
            {
                this._destination = value;
            }
        }

        public string Departure
        {
            get
            {
                return (this._departure);
            }
            set
            {
                this._departure = value;
            }
        }

        public void Install(object destination)
        {
            if ((destination == null) ||
                ((destination as object[]).Length < 4))
                throw new Exception("object parameters are null or empty");

            object[] paramsArray = destination as object[];
            string userID = paramsArray[0] as String;
            string token = paramsArray[1] as string;
            string url = paramsArray[2] as string;
            string appName = paramsArray[3] as string;
            //  # at first download app from cloud to application-service folder
            var zdriveService = new StorageServiceManagement(userID);
            string zipDownloadedFilePath =  zdriveService.downloadFile(url,
                                                                    token,
                                                                    userID);
           string[] fNameArray = zipDownloadedFilePath.Split('\\');
           string filename = fNameArray[fNameArray.Length - 1].Split('.')[0];
           string destinationAppDir = String.Format("{0}\\{1}\\{2}",
                                                        AppDomain.CurrentDomain.BaseDirectory,
                                                        filename,
                                                        filename);
            //  # unzip box at same folder (application-service)
            ZipFile.ExtractToDirectory(zipDownloadedFilePath,
                                       destinationAppDir);
            //  # add app to appsConfig.xml file and refresh application store in STRepository
            var rbsConfig = new RBSConfigRepositoryForUsers("Application-Service\\AppsConfig.xml");
            rbsConfig.addRBSinAppsConfig(filename,
                                            filename,
                                            userID);


            File.Delete(zipDownloadedFilePath);


        }

        public void Remove(object destination)
        {
            if ((destination == null) ||
                ((destination as object[]).Length < 2))
                throw new Exception("object parameters are null or empty");

            string Name = (destination as object[])[0].ToString();
            string userID = (destination as object[])[1].ToString();

            if ((!String.IsNullOrEmpty(Name)) &&
                (!String.IsNullOrEmpty(userID)))
            {
                String path = String.Format("{0}//Application-service/appsConfig.xml",
                                            AppDomain.CurrentDomain.BaseDirectory);
                using (var existApp = new RBSConfigRepositoryForUsers(path))
                {
                    existApp.removeOrUninstallRBSforUser(Name,
                                                         userID);
                }
            }
            else
                throw new Exception("appName or userID is null or empty");
        }


        public string Scope
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public AppCatalogType appCatalog
        {
            get
            {
                return (this.appType);
            }

        }
    }
}
