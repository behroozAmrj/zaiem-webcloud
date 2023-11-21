using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Cloud.IO.Repository
{
    public class LayoutConfigRepository : ICustomViewRepository<LayoutConfigRepository>, IDisposable
    {

        string filename;
        string name;
        string directory;
        ConfigType type;
        XmlDocument xdoc;
        //DeskTopBuilder caller;
        Cloud.Foundation.Infrastructure.AppConfigType appconfigtype;
        //public LayoutConfigRepository(DeskTopBuilder desktopbuilder, string FileName)
        //{
        //    if (!string.IsNullOrEmpty(FileName))
        //    {
        //        var directory = HttpContext.Current.Server.MapPath(FileName);
        //        var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
        //                                FileName);

        //        if (!File.Exists(path))
        //        {
        //            throw new Exception("Directory and File are not Exists . .. ");
        //        }
        //        else
        //        {
        //            filename = path;
        //            this.caller = desktopbuilder;


        //        }
        //    }
        //    else
        //    { throw new Exception("filename is null or empty"); }
        //}

        public LayoutConfigRepository(string FileName)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                var directory = HttpContext.Current.Server.MapPath(FileName);
                var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                                        FileName);

                if (!File.Exists(path))
                {
                    throw new Exception("Directory and File are not Exists . .. ");
                }
                else
                {
                    filename = path;

                }
            }
            else
            { throw new Exception("filename is null or empty"); }
        }
        private LayoutConfigRepository()
        {

        }

        public string Name { get { return (this.name); } set { this.name = value; } }

        public string Directory { get { return (this.directory); } set { this.directory = value; } }
        public ConfigType Type { get { return (this.type); } set { this.type = value; } }
        public string AbselutePath { get { return (filename); } }
        public List<LayoutConfigRepository> Read(string XPath)
        {
            if (string.IsNullOrEmpty(XPath))
                throw new Exception("[Server].[LayoutConfigRepository].[Read] => XPath is null or empty");
            xdoc = new XmlDocument();
            xdoc.Load(filename);
            string xpath = XPath;
            var list = xdoc.SelectNodes(xpath);
            return (ConvertXMLNodeToThis(list));

        }



        public LayoutConfigRepository ReadOne(string XPath)
        {
            if (string.IsNullOrEmpty(XPath))
                throw new Exception("[Server].[LayoutConfigRepository].[Read] => XPath is null or empty");
            xdoc = new XmlDocument();
            xdoc.Load(filename);
            string xpath = XPath;
            var elements = xdoc.SelectNodes(xpath);
            if( (elements == null) || 
                (elements.Count == 0))
                return (null);
            var lcr = ConvertXMLNodeToThis(elements).First();
            this.directory = lcr.directory;
            return (lcr);

        }




        public void Async_Read(object Parameters)
        {
            try
            {
                xdoc = new XmlDocument();
                xdoc.Load(filename);
                //var objectarray = CallerLayoutList as object[];
                //string xp = objectarray[1].ToString();
                var paramArray = Parameters as object[];
                var xpath = paramArray[0].ToString();
                var data = paramArray[1] as ITransparentData;
                var list = xdoc.SelectNodes(xpath);
                var lcrepository = new List<LayoutConfigRepository>(ConvertXMLNodeToThis(list));
                //ConfigRepository  lcrepository;
                data.DataValue = lcrepository;

            }
            catch (Exception ex)
            {

                throw new Exception("[layoutconfigrepository].[Async_Read]" + ex.Message);
            }



        }


        private List<LayoutConfigRepository> ConvertXMLNodeToThis(XmlNodeList NodeList)
        {
            var lcrlist = new List<LayoutConfigRepository>();
            foreach (var item in NodeList.Cast<XmlNode>())
            {
                var lcr = new LayoutConfigRepository();
                lcr.Name = item.Attributes["name"].Value;
                lcr.Directory = item.Attributes["directory"].Value;
                lcr.Type = ConfigType.layout;
                lcrlist.Add(lcr);
            }
            return (lcrlist);

        }
        public void Save()
        {
        }

        public void Dispose()
        {
            Save();
        }


        public XmlNode ChildNodeList(string XPath)
        {
            if (!string.IsNullOrEmpty(XPath))
            {
                xdoc = new XmlDocument();
                xdoc.Load(filename);
                var childnodes = xdoc.SelectSingleNode(XPath);
                return (childnodes);
            }
            else
            {
                return (null);
            }
        }





        List<LayoutConfigRepository> ICustomViewRepository<LayoutConfigRepository>.ReadOne(string XPath)
        {
            throw new NotImplementedException();
        }
    }
}
