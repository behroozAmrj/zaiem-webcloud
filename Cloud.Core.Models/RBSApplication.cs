using Cloud.Foundation.Infrastructure;
using Cloud.IO.Repository;
using Cloud.Log.Tracking;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;

namespace Cloud.Core.Models
{
    public class RBSApplication : IEntity 
    {
        string handler;
        string absolutepath;
        List<string> layoutlist;
        string directory = string.Empty;
        string file;
        string appfolder;
        string skin;
        string height;
        String _userID;

        public string Height
        {
            get { return height; }
        }
        string width;

        public string Width
        {
            get { return width; }
        }
        public RBSApplication(String UserID , string Handler, [CallerMemberName] string calller = null)
        {
            if ((string.IsNullOrEmpty(Handler)) || (string.IsNullOrEmpty(UserID)))
                throw new Exception("[Server].[RBSApplication].[Ctor] => Application handler is null");
            //Logging.INFOlogRegistrer("resuest:RBSApplication",
            //                            UserID,
            //                            MethodBase.GetCurrentMethod().GetType());
            this._userID = UserID;
            string[] handarray = Handler.Split('/');
            string laststr = handarray[handarray.Length - 1];
            string layout = handarray[handarray.Length - 2];
            string path = Handler.Replace("/" + laststr,
                                            string.Empty);
            directory = path.Replace("/" + layout,
                                            string.Empty);
            appfolder = directory.Split('/')[1];
            this.absolutepath = AppDomain.CurrentDomain.BaseDirectory + directory.Replace('/',
                                                                                        '\\');
            string tfile = string.Format("{0}\\{1}",
                                          absolutepath,
                                          "config.xml");

            if (File.Exists(tfile))
            {
                this.file = tfile;

            }
            else
            {
                throw new Exception("[Server].[RBSApplication].[Ctor] => config file is not exist! ");
            }

            this.handler = Handler;

        }


        public RBSApplication(String UserID , string Handler, int Type)
        {
            if ((string.IsNullOrEmpty(Handler) )|| (string.IsNullOrEmpty(UserID)))
            {
                throw new Exception("[Server].[RBSApplication].[ctor] => Handler and Type is null or empty");
            }

            this._userID = UserID;
            var harray = Handler.Split('\\');
            if (harray[0] != null)
                this.appfolder = harray[0];
            if (harray[1] != null)
                this.skin = harray[1];
            else
                throw new Exception("[Server].[RBSApplication].[ctor] => skin name is null or empty ");
            this.directory = string.Format("{0}\\{1}",
                                           "Application-Service\\",
                                            harray[0]);
            this.absolutepath = string.Format("{0}{1}",
                                                AppDomain.CurrentDomain.BaseDirectory,
                                                this.directory);
            var tfile = string.Format("{0}\\{1}",
                                        absolutepath,
                                        "config.xml");
            if (File.Exists(tfile))
            { this.file = tfile; }
            else
            { throw new Exception("[Server].[RBSApplication].[ctor] =>  config file is not exist !"); }
        }
        public List<string> ReadApplicationLayoutlist()
        {
            if (!string.IsNullOrEmpty(this.directory))
            {
                //Logging.INFOlogRegistrer("resuest:ReadApplicationLayoutlist",
                //                       this._userID,
                //                       MethodBase.GetCurrentMethod().GetType());
                var config = new ConfigRepository(this.directory);
                var laylist = config.Read_LayoutList("//applicationConfig//layouts//layout");
                var list = new List<string>(laylist.ConfigRepositoryListToStringList());
                if ((!string.IsNullOrEmpty(config.Height)) && 
                    (!string.IsNullOrEmpty(config.Width)))
                {
                    this.height = config.Height;
                    this.width  = config.Width;
                }
                else
                {
                    this.height = string.Empty;
                    this.width = string.Empty;
                }
                this.layoutlist = new List<string>();
                foreach (var layout in list)
                {
                    string path = string.Format("{0}\\{1}",
                                                 this.appfolder,
                                                 layout);
                    layoutlist.Add(path);
                }
                return (this.layoutlist);
            }
            else
            { throw new Exception("[Server].[RBSApplication].[ReadApplicationLayoutList] => Directory is not declared"); }
        }


        public string RBSapplicaitonLoadSkin()
        {
            var config = new ConfigRepository("Application-Service\\" + this.appfolder);
            var layoutfilename = config.ReadLayoutAddress("//applicationConfig//layouts//layout[text()='" + this.skin + "']");
           
            if (!string.IsNullOrEmpty(layoutfilename))
            {
                string file = string.Format("{0}\\Layouts\\{1}",
                                            this.absolutepath,
                                            layoutfilename);
                if (config.IsFileExist(file))
                {
                    string address = string.Format("{0}/{1}/{2}/{3}",
                                                        "Application-Service\\",
                                                        this.appfolder,
                                                        "Layouts",
                                                        layoutfilename);
                    return (address);
                }
                else
                { return (null); }

            }
            else
            { return (null); }
        }


        public string Titel
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

        public string Image
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

        public string Handler
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

        public EntityType Type
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

        public object Result
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

        public List<Foundation.Infrastructure.CustomView> CustomViews
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

        public List<string> LayoutFileList
        {
             set;
             get;

        }

        string IEntity.Height
        {
            get
            {
                return(this.height);
            }
            set
            {
                this.height = value;
            }
        }

        string IEntity.Width
        {
            get
            {
                return (this.width);
            }
            set
            {
                this.width = value;
            }
        }

        public object DoWork()
        {
            if (!string.IsNullOrEmpty(this.directory))
            {
                //Log.Tracking.Logging.INFOlogRegistrer("run rbs" ,
                //                                      this._userID,
                //                                      MethodBase.GetCurrentMethod().GetType());
                var config = new ConfigRepository(this.directory);
                var laylist = config.Read_LayoutList("//applicationConfig//layouts//layout");
                var list = new List<string>(laylist.ConfigRepositoryListToStringList());
                if ((!string.IsNullOrEmpty(config.Height)) &&
                    (!string.IsNullOrEmpty(config.Width)))
                {
                    this.height = config.Height;
                    this.width = config.Width;
                }
                else
                {
                    this.height = string.Empty;
                    this.width = string.Empty;
                }
                this.layoutlist = new List<string>();
                foreach (var layout in list)
                {
                    string path = string.Format("{0}\\{1}",
                                                 this.appfolder,
                                                 layout);
                    layoutlist.Add(path);
                }
                this.LayoutFileList = layoutlist;
                return ("/"+handler);
            }
            else
            {
                //Log.Tracking.Logging.ErrorlogRegister("run RBS Failed",
                //                                      MethodBase.GetCurrentMethod().GetType(),
                //                                      this._userID);

                throw new Exception("[Server].[RBSApplication].[ReadApplicationLayoutList] => Directory is not declared"); }
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public string LoadTemplate(string TemplateName)
        {
            throw new NotImplementedException();
        }


        public void uploadFileAndAddtoConfigFile(String UserID, String status, String fileName, String fileContent)
        { }

        public void insetRBSinConfigFile(String appName , String directory , String UserID)
        {
            var userAppConfig = new RBSConfigRepositoryForUsers("Application-Service\\AppsConfig.xml");
            var result =userAppConfig.addRBSinAppsConfig(appName,
                                                         directory,
                                                         UserID);
            if (!result)
                throw new Exception("register app to file failde");
        }


    }
}