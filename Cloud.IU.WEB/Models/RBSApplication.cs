using Cloud.IO.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloud.IU.WEB.Models
{
    public class RBSApplication
    {
        string handler;
        string absolutepath;
        List<string> layoutlist;
        string directory = string.Empty;
        string file;
        string appfolder;
        string skin;
        public RBSApplication(string Handler)
        {
            if (string.IsNullOrEmpty(Handler))
                throw new Exception("[Server].[RBSApplication].[Ctor] => Application handler is null");
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


        }


        public RBSApplication(string Handler, int Type)
        {
            if (string.IsNullOrEmpty(Handler))
            {
                throw new Exception("[Server].[RBSApplication].[ctor] => Handler and Type is null or empty");
            }

            var harray = Handler.Split('\\');
            if (harray[0] != null)
                this.appfolder = harray[0];
            if (harray[1] != null)
                this.skin = harray[1];
            else
                throw new Exception("[Server].[RBSApplication].[ctor] => skin name is null or empty ");
            this.directory = string.Format("{0}\\{1}",
                                            ConfigurationManager.AppSettings.Get("appservice"),
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
                var config = new ConfigRepository(this.directory);
                var laylist = config.Read_LayoutList("//applicationConfig//layouts//layout");
                var list = new List<string>(laylist.ConfigRepositoryListToStringList());
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
            var config = new ConfigRepository(ConfigurationManager.AppSettings.Get("appservice") + this.appfolder);
            var layoutfilename = config.ReadLayoutAddress("//applicationConfig//layouts//layout[text()='" + this.skin + "']");
            if (!string.IsNullOrEmpty(layoutfilename))
            {
                string file = string.Format("{0}\\Layouts\\{1}",
                                            this.absolutepath,
                                            layoutfilename);
                if (config.IsFileExist(file))
                {
                    string address = string.Format("{0}/{1}/{2}/{3}",
                                                        ConfigurationManager.AppSettings.Get("appservice"),
                                                        this.appfolder,
                                                        "Layouts",
                                                        layoutfilename);
                    return (address);
                }
                else
                { return (null); }

            }
            else
            {return (null); }
        }

    }
}