using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Cloud.IO.Repository
{
    public class ConfigRepository : ICustomViewRepository<ConfigRepository>, IDisposable
    {

        #region Private Data
        string name;
        string directory;
        string image;
        XmlDocument xdoc;
        string height;
        string width;


        #endregion

        #region public property
        public string Width
        {
            get { return width; }
            set { width = value; }
        }
        public string Height
        {
            get { return height; }
            set { height = value; }
        }
        public string Name { get { return (name); } set { name = value; } }
        public string Directory { get { return (directory); } set { directory = value; } }

        public string Image { get { return (image); } set { image = value; } }

        #endregion
        public ConfigRepository(string Directory)
        {
            Directory += "\\config.xml";
            Directory = System.AppDomain.CurrentDomain.BaseDirectory + Directory;

            if (File.Exists(Directory))
            {
                this.directory = Directory;
            }
            else
            { throw new Exception("file  not found in directory"); }
        }

        private ConfigRepository()
        {

        }

        public List<ConfigRepository> Read(AppConfigType AppConfig)
        {
            try
            {
                xdoc = new XmlDocument();
                xdoc.Load(directory);
                var list = xdoc.GetElementsByTagName("image",
                                                     "appconfig");
                var crList = new List<ConfigRepository>();
                foreach (var img in list.Cast<XmlNode>())
                {
                    var cr = new ConfigRepository();
                    cr.Image = img.Attributes["src"].Value;
                    crList.Add(cr);
                }
                return (crList.ToList());

            }
            catch (Exception ex)
            {
                throw new Exception("can not read file");
            }
        }

        public List<ConfigRepository> Read(string XPath)
        {
            try
            {
                if (string.IsNullOrEmpty(XPath))
                    throw new Exception("[Server].[ConfigRepository].[Read] => xpath is null or empty");
                xdoc = new XmlDocument();
                xdoc.Load(directory);
                string xpath = XPath;
                var list = xdoc.SelectNodes(xpath);
                var crList = new List<ConfigRepository>();
                foreach (var img in list.Cast<XmlNode>())
                {
                    var cr = new ConfigRepository();
                    cr.Image = img.Attributes["src"].Value;
                    crList.Add(cr);
                }
                return (crList.ToList());
            }
            catch (Exception ex)
            {

                throw new Exception("[Server].[ConfigRepository].[Read] => can not read files ");
            }
        }

        public List<ConfigRepository> ReadOne(string XPath)
        {
            try
            {
                if (string.IsNullOrEmpty(XPath))
                    throw new Exception("XPath is null or empty");
                xdoc = new XmlDocument();
                xdoc.Load(directory);
                string xpath = XPath;
                var list = xdoc.SelectNodes(xpath);
                var crList = new List<ConfigRepository>();
                foreach (var img in list.Cast<XmlNode>())
                {
                    var cr = new ConfigRepository();
                    cr.Image = img.Attributes["src"].Value;
                    crList.Add(cr);
                }
                return (crList.ToList());

            }
            catch (Exception ex)
            {
                throw new Exception("can not read file");
            }
        }

        public List<ConfigRepository> Read_LayoutList(string XPath)
        {
            try
            {
                if (string.IsNullOrEmpty(XPath))
                    throw new Exception("XPath is null or empty");
                xdoc = new XmlDocument();
                xdoc.Load(directory);
                string xpath = XPath;
                var list = xdoc.SelectNodes(xpath);
                CheckForLayoutAttribute(xpath);
                var crList = new List<ConfigRepository>();
                foreach (var config in list.Cast<XmlNode>())
                {
                    var cr = new ConfigRepository();
                    cr.Name = config.InnerText;
                    crList.Add(cr);
                }
                return (crList.ToList());

            }
            catch (Exception ex)
            {
                throw new Exception("can not read file");
            }
        }

        //public List<ConfigRepository> ReadrbsApplication_LayoutList(string XPath)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(XPath))
        //            throw new Exception("XPath is null or empty");
        //        xdoc = new XmlDocument();
        //        xdoc.Load(directory);
        //        string xpath = XPath;
        //        var list = xdoc.SelectNodes(xpath);
        //        var crList = new List<ConfigRepository>();
        //        foreach (var config in list.Cast<XmlNode>())
        //        {
        //            var cr = new ConfigRepository();
        //            cr.Name = config.Attributes["filename"].Value;
        //            //cr.Name = config.InnerText;
        //            crList.Add(cr);
        //        }
        //        return (crList.ToList());

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("can not read file");
        //    }
        //}


        public string ReadLayoutAddress(string XPath)
        {
            try
            {
                if (string.IsNullOrEmpty(XPath))
                    return (null);
                xdoc = new XmlDocument();
                xdoc.Load(directory);
                string xpath = XPath;
                var list = xdoc.SelectNodes(xpath);
                if (list == null)
                    return (null);
                var value = list.Cast<XmlNode>().First();
                return (value.Attributes["filename"].Value);

            }
            catch (Exception ex)
            {
                throw new Exception("can not read file");
            }

        }

        public Boolean IsFileExist(string FileandPath)
        {
            if (File.Exists(FileandPath))
            { return (true); }
            else
            { return (false); }
        }
        private void CheckForLayoutAttribute(string XPath)
        {
            string xpath = XPath.Replace("//",
                                        "\\");
            string[] steps = xpath.Split('\\');
            string nxpath = string.Format("{0}//{1}",
                                            steps[1],
                                            steps[2]);
            var xdoc = new XmlDocument();
            xdoc.Load(this.directory);
            var parentnode = xdoc.SelectSingleNode(nxpath);
            if (parentnode != null)
            {
                if ((parentnode.Attributes["height"] != null) &&
                        (parentnode.Attributes["width"] != null))
                {
                    this.height = parentnode.Attributes["height"].Value;
                    this.width = parentnode.Attributes["width"].Value;
                }
                else
                {
                    this.height = "400px";
                    this.width = "500px";
                }
            }
            else
            {
                xdoc.Load(this.directory);
                string xp = ConfigurationManager.AppSettings["RBSpagesize"];
                var pnode = xdoc.SelectSingleNode(xp);
                if (pnode != null)
                {
                    if ((pnode.Attributes["height"] != null)&&
                        (pnode.Attributes["width"] != null))
                    {
                        this.height = pnode.Attributes["height"].Value;
                        this.width = pnode.Attributes["width"].Value;
                    }
                    else
                    {
                        this.height = "400px";
                        this.width = "500px";
                    }
                }
                else
                {
                    this.height = "400px";
                    this.width = "500px";
                }
            }

        }
        public void Save()
        {

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }



    }
}