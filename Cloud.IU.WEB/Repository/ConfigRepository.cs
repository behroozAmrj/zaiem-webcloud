using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Cloud.IU.WEB.Repository
{
    public class ConfigRepository : ICustomViewRepository<ConfigRepository>, IDisposable
    {

        #region Private Data
        string name;
        string directory;
        string image;
        XmlDocument xdoc;
        #endregion

        #region public property
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
                var crList = new List<ConfigRepository>();
                foreach (var config in list.Cast<XmlNode>())
                {
                    var cr = new ConfigRepository();
                    //cr.Name = img.Attributes["filename"].Value;
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

        public List<ConfigRepository> ReadrbsApplication_LayoutList(string XPath)
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
                foreach (var config in list.Cast<XmlNode>())
                {
                    var cr = new ConfigRepository();
                    cr.Name = config.Attributes["filename"].Value;
                    //cr.Name = config.InnerText;
                    crList.Add(cr);
                }
                return (crList.ToList());

            }
            catch (Exception ex)
            {
                throw new Exception("can not read file");
            }
        }


        public string ReadLayoutAddress(string XPath)
        {
            try
            {
                if (string.IsNullOrEmpty(XPath))
                    return(null);
                xdoc = new XmlDocument();
                xdoc.Load(directory);
                string xpath = XPath;
                var list = xdoc.SelectNodes(xpath);
                if (list.Count <= 0)
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
        public void Save()
        {

        }

        public void Dispose()
        {
            Save();
        }



    }
}