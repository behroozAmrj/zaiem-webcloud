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
        public string AbselutePath { get { return (filename); }  }
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

        // This method oprate old approach to read xml files in apps.xml
        //public LayoutConfigRepository Read_(AppConfigType AppType, Type EntityType)
        //{
        //    xdoc = new XmlDocument();
        //    xdoc.Load(filename);

        //    var elements = xdoc.GetElementsByTagName(AppType.ToString(),
        //                                                AppType.ToString());
        //    var lcr = ConvertXMLNodeToThis(elements).First(x => x.Name == EntityType.Name);
        //    return (lcr);

        //}

        public LayoutConfigRepository ReadOne(string XPath)
        {
            if (string.IsNullOrEmpty(XPath))
                throw new Exception("[Server].[LayoutConfigRepository].[Read] => XPath is null or empty");
            xdoc = new XmlDocument();
            xdoc.Load(filename);
            string xpath = XPath ;
            var elements = xdoc.SelectNodes(xpath);
            if (elements.Count <= 0)
                return (null);
            var lcr = ConvertXMLNodeToThis(elements).First();
            this.directory = lcr.directory;
            return (lcr);

        }

        // read layout destop config from config.xml file 
        //public void Async_Read_(object CallerLayoutList)
        //{
        //    try
        //    {
        //        xdoc = new XmlDocument();
        //        xdoc.Load(filename);
        //        var objectarray = CallerLayoutList as object[];
        //        string xmlns = objectarray[1].ToString();
        //        var list = xdoc.GetElementsByTagName(xmlns,
        //                                              xmlns);
        //        this.caller.lcr_List = new List<LayoutConfigRepository>(ConvertXMLNodeToThis(list));
        //    }
        //    catch (Exception ex)
        //    {
                
        //        throw new Exception("[layoutconfigrepository].[Async_Read]" + ex.Message);
        //    }
            


        //}


        //public void Async_Read(object CallerLayoutList)
        //{
        //    try
        //    {
        //        xdoc = new XmlDocument();
        //        xdoc.Load(filename);
        //        var objectarray = CallerLayoutList as object[];
        //        string xp = objectarray[1].ToString();
        //        string xpath = xp ;

        //        var list = xdoc.SelectNodes(xpath);
        //        this.caller.lcr_List = new List<LayoutConfigRepository>(ConvertXMLNodeToThis(list));
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception("[layoutconfigrepository].[Async_Read]" + ex.Message);
        //    }



        //}


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








        List<LayoutConfigRepository> ICustomViewRepository<LayoutConfigRepository>.ReadOne(string XPath)
        {
            throw new NotImplementedException();
        }
    }
}