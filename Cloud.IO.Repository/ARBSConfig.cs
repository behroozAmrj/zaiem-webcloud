using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Cloud.IO.Repository
{
    public class ARBSConfig : ApplicationAndViewFiles<ARBSConfig>
    {
        string name;
        string directory;
        string handler;
        string titel;
        string image;

        public string Image
        {
            get { return image; }
            set { image = value; }
        }
        public override string Name
        {
            set { name = value; }
            get { return (name); }
        }

        public override string Directory
        {
            set { directory = value; }
            get { return (directory); }
        }

        public string Handler { get { return (handler); } set { handler = value; } }
        public string Titel { get { return (titel); } set { titel = value; } }
        public ARBSConfig(string FileName, string Path)
            : base(FileName)
        {
            directory = Path;
        }

        private ARBSConfig()
            : base()
        {

        }



        public override ARBSConfig Read()
        {
            var rbs = new ARBSConfig();
            var xdoc = new XmlDocument();
            xdoc.Load(base.filename);
            
            var image = xdoc.SelectNodes("//applicationConfig//image");
            var config = image.Cast<XmlNode>().First();
            rbs.image = config.Attributes["src"].Value;
            var handler = xdoc.SelectNodes("//applicationConfig//appconfig");
            var iconfig = handler.Cast<XmlNode>().First();
            string absolutepath = "Application-Service/" + directory + "/layouts/";
            rbs.handler = absolutepath + iconfig.Attributes["startpage"].Value;

            rbs.Titel = iconfig.Attributes["title"].Value;

            return (rbs);
        }


        //public List<object> APIsList()
        //{
        //    var xdoc = new XmlDocument();
        //    xdoc.Load(base.filename);
        //    //var apilist = xdoc.SelectNodes("//applicationConfig[@xmlns='appconfig']//APIS");
        //    //var list = new List<object>(apilist.Cast<XmlNode>() as List<object>);
        //    var apilist = xdoc.GetElementsByTagName("lib",
        //                                            "appconfig");
                                                    
        //    var list = new List<object>(apilist.Cast<XmlNode>() as List<object>);
        //    return (list);
        //}



        public string path { get; set; }
    }
}