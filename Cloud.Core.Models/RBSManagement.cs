using Cloud.Foundation.Infrastructure;
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
    public class RBSManagement : IDisposable
    {
        public RBSManagement()
        {

        }


        public void createFileAndRegister(String appName, String userId, String fileName, String filePath, String fileContent, String status)
        {
            var fileByte = Convert.FromBase64String(fileContent);
            using (var file = new FileStream(filePath +"\\"+ fileName, FileMode.Append, FileAccess.Write))
            {
                file.Write(fileByte,
                            0,
                            fileByte.Length);
                file.Flush();
            }
            switch (status)
            {
                case "load":
                    {

                        break;
                    }
                case "end":
                    {
                        try
                        {
                            String absoluteFileNamePath = String.Format("{0}\\{1}",
                                                                    filePath,
                                                                    fileName);
                            String newFileName = String.Format("{0}",
                                                                     appName);
                            String directory = String.Format("{0}\\{1}",filePath,newFileName);
                            ZipFile.ExtractToDirectory(absoluteFileNamePath,
                                                       directory);
                            var rbsConfig = new RBSConfigRepositoryForUsers("Application-Service\\AppsConfig.xml");
                            rbsConfig.addRBSinAppsConfig(appName,
                                                            newFileName,
                                                            userId);
                            File.Delete(absoluteFileNamePath);

                        }
                        catch (Exception)
                        {
                            
                            throw;
                        }
                        break;
                    }
            }

        }

        public List<IAppCatalog> RetrievCloudApp(String URL , String Token)
        {
            return (null);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
