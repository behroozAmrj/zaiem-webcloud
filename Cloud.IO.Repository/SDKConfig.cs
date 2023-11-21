using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Cloud.IO.Repository
{
    public class SDKConfig
    {
        string directory;
        string value;
        XmlDocument xdoc;
        string abspath;
        public string Value
        { get { return (value); } }

        public string Directory
        { get { return (directory); } }
        public SDKConfig(string Directory)
        {
            string filepath = string.Format("{0}{1}\\{2}\\{3}", AppDomain.CurrentDomain.BaseDirectory,
                                              "Application-Service",
                                              Directory,
                                              "config.xml");
            if (File.Exists(filepath))
            {
                directory = Directory;
                xdoc = new XmlDocument();
                xdoc.Load(filepath);
                this.abspath = string.Format("{0}{1}\\{2}", AppDomain.CurrentDomain.BaseDirectory,
                                              "Application-Service",
                                              Directory);
            }
            else
                throw new Exception("no file exist");
        }

        private SDKConfig()
        {

        }


        public List<SDKConfig> Reads(string XPath, string Attribute)
        {
            if ((string.IsNullOrEmpty(XPath)) &&
                (string.IsNullOrEmpty(Attribute)))
                return (null);
            var resultlist = new List<SDKConfig>();
            if (xdoc == null)
                return (null);
            string xpath = XPath;
            var items = xdoc.SelectNodes(xpath);
            var list = items.Cast<XmlNode>();
            foreach (var item in list)
            {
                var file = string.Format("{0}\\Bin\\{1}",
                                            abspath,
                                            item.Attributes[Attribute].Value);
                if (File.Exists(file))
                {
                    var note = new SDKConfig();
                    note.value = file;//item.Attributes[Attribute].Value;
                    resultlist.Add(note);
                }
            }
            return (resultlist);
        }

        public SDKConfig ReadOne(string XPath, string Attribute)
        {
            if ((string.IsNullOrEmpty(XPath)) &&
                (string.IsNullOrEmpty(Attribute)))
                return (null);
            var result = new SDKConfig();
            if (xdoc == null)
                return (null);
            string xpath = XPath;
            var items = xdoc.SelectNodes(xpath);
            if (items.Count > 0)
            {
                var item = items.Cast<XmlNode>().First();
                result.value = item.Attributes[Attribute].Value;
                return (result);
            }
            else
            {return(null);}
        }



    }
}