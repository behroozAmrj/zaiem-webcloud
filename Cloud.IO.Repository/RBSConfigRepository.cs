using Cloud.Foundation.Infrastructure;
//using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Cloud.IO.Repository
{
    public class RBSConfigRepository : ICustomViewRepository<RBSConfigRepository>, IDisposable
    {
        #region  Data
        string name;
        string directory;
        XmlDocument xdoc;
        //DeskTopBuilder caller;
        AppConfigType appconfigtype;
        string image;
        public ConfigType type;
        protected string filepath;
        protected RBSConfigRepository RBSconfg;
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

        public RBSConfigRepository()
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
        public virtual List<RBSConfigRepository> ReadOne(string XPath)
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


        public virtual void Async_Read(object Parameters)
        {

            if (Parameters == null)
            {
                var msg = string.Format("[server].[RBSConfigRepository].[Async_Read] => parameters are null or empty ");
                throw new Exception(msg);
            }

            var parametersArray = Parameters as object[];
            if (parametersArray.Count() == 2)
            {
                string xpath = parametersArray[0].ToString();
                var tdata = parametersArray[1] as ITransparentData;
                xdoc = new XmlDocument();
                xdoc.Load(this.filepath);
                var elements = xdoc.SelectNodes(xpath);
                var rbslist = ConvertXMLNodeToThis(elements);//.FirstOrDefault(app => app.Name == AppName); ;
                tdata.DataValue = rbslist;
            }
            else
            {
                var msg = string.Format("[server].[RBSConfigRepository].[Async_Read] => parameters count is {0} ",
                                            parametersArray.Count());
                throw new Exception(msg);
            }

        }


        public virtual void Async_Read(String xPath, ITransparentData tData)
        {

            if ((tData == null) ||
                (String.IsNullOrEmpty(xPath)))
            {
                var msg = string.Format("[server].[RBSConfigRepository].[Async_Read] => parameters are null or empty ");
                throw new Exception(msg);
            }

            xdoc = new XmlDocument();
            xdoc.Load(this.filepath);
            var elements = xdoc.SelectNodes(xPath);
            var rbslist = ConvertXMLNodeToThis(elements);//.FirstOrDefault(app => app.Name == AppName); ;
            tData.DataValue = rbslist;

        }
        
        //public RBSConfigRepository Read(AppConfigType AppType, string AppName)
        //{
        //    return (this.Read(AppType).FirstOrDefault(app => app.name == AppName));
        //}

        protected List<RBSConfigRepository> ConvertXMLNodeToThis(XmlNodeList NodeList)
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
            
/*
            NodeList.Cast<XmlNode>().Select((x) =>
            {
                var rbs = new RBSConfigRepository();
                rbs.Name = x.Attributes["name"].Value;
                rbs.Directory = x.Attributes["directory"].Value;
                rbs.type = ConfigType.RBS;
                lcrlist.Add(rbs);
                return (rbs);
            });
 */ 
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