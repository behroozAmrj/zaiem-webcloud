using Cloud.Foundation.Infrastructure;
//using Cloud.IU.WEB.InfraSructure;
using Cloud.IU.WEB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Cloud.IU.WEB.Repository
{
    public class RBSConfigRepository : ICustomViewRepository<RBSConfigRepository>, IDisposable
    {
        #region  Data
        string name;
        string directory;
        ConfigType type;
        XmlDocument xdoc;
        //DeskTopBuilder caller;
        AppConfigType appconfigtype;
        string image;
        string filepath;
        #endregion

        #region public data

        public string Name
        {
            get { return (this.name); }
            set { this.name = value; }
        }

        public string Directory
        {
            get { return (this.directory); }
            set { this.directory = value; }
        }

        public string Image { get { return (this.image); } set { this.image = value; } }

        public Cloud.Foundation.Infrastructure.ConfigType Type { get { return (this.type); } set { this.type = value; } }

        #endregion

        private RBSConfigRepository()
        {

        }

        //public RBSConfigRepository(string Path)
        //{
        //    if (!string.IsNullOrEmpty(Path))
        //    {
        //        Path = AppDomain.CurrentDomain.BaseDirectory + Path;
        //        if (File.Exists(Path))
        //        {
        //            this.filepath = Path;
        //            //this.caller = desktopbuilder;
        //        }
        //        else
        //        { throw new Exception(" file not found in path"); }

        //    }
        //    else
        //    { throw new Exception("[RBSConfigRepository].[cunstruction] one or more entrance parameters are null "); }
        //}
        public RBSConfigRepository(string Path)
        {
            if (!string.IsNullOrEmpty(Path))
            {
                Path = AppDomain.CurrentDomain.BaseDirectory + Path;
                if (File.Exists(Path))
                {
                    this.filepath = Path;
                }
                else
                { throw new Exception(" file not found in path"); }

            }
            else
            { throw new Exception("[RBSConfigRepository].[cunstruction] one or more entrance parameters are null "); }
        }



         // This section is used in ancient approach of reading xml file 
        //public List<RBSConfigRepository> Read_(AppConfigType AppType)
        //{
        //    xdoc = new XmlDocument();
        //    xdoc.Load(this.filepath);

        //    var elements = xdoc.GetElementsByTagName(AppType.ToString(),
        //                                                AppType.ToString());
        //    var rbslist = ConvertXMLNodeToThis(elements);//.FirstOrDefault(app => app.Name == AppName); ;
        //    return (rbslist);
        //}

        public List<RBSConfigRepository> Read(AppConfigType AppType)
        {
            xdoc = new XmlDocument();
            xdoc.Load(this.filepath);
            string xpath = "//root//applications//application";
            var elements = xdoc.SelectNodes(xpath);
            var rbslist = ConvertXMLNodeToThis(elements);//.FirstOrDefault(app => app.Name == AppName); ;
            return (rbslist);
        }
        public List<RBSConfigRepository> ReadOne(string XPath)
        {
            if (string.IsNullOrEmpty(XPath))
                throw new Exception("Xpath is null or empty");
            xdoc = new XmlDocument();
            xdoc.Load(this.filepath);
            string xpath = "//root//applications//application";
            var elements = xdoc.SelectNodes(xpath);
            var rbslist = ConvertXMLNodeToThis(elements);//.FirstOrDefault(app => app.Name == AppName); ;
            return (rbslist);
        }


        public void Async_Read(object AppType)
        { 
        }
        
        //public RBSConfigRepository Read(AppConfigType AppType, string AppName)
        //{
        //    return (this.Read(AppType).FirstOrDefault(app => app.name == AppName));
        //}

        private List<RBSConfigRepository> ConvertXMLNodeToThis(XmlNodeList NodeList)
        {
            var lcrlist = new List<RBSConfigRepository>();
            foreach (var item in NodeList.Cast<XmlNode>())
            {
                var rbs = new RBSConfigRepository();
                rbs.Name = item.Attributes["name"].Value;
                rbs.Directory = item.Attributes["directory"].Value;
                rbs.type = ConfigType.RBS;
                lcrlist.Add(rbs);
            }
            return (lcrlist);

        }

        public void Async_Read()
        {
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