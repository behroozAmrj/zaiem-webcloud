using Cloud.Foundation.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Cloud.IO.Repository
{
    public class CloudServicesConfig
    {
        XmlDocument xdoc;
        private string directory;
        private string abspath;
        private string url;
        private ServiceType serviceType;

        public ServiceType ServiceType
        {
            get { return serviceType; }
        }
        public string Url
        {
            get { return url; }
        }

        public CloudServicesConfig(string Directory)
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

        private CloudServicesConfig()
        {

        }
        private List<CloudServicesConfig> Reads()
        { return (null); }

        public CloudServicesConfig ReadOne(string XPath, string AttributeName , string AttributeValue)
        {
            if ((!string.IsNullOrEmpty(XPath)) ||
                (!string.IsNullOrEmpty(AttributeName)) ||
                (!string.IsNullOrEmpty(AttributeValue)))
            {
                var cloudserice = xdoc.SelectNodes(XPath);
                if (cloudserice.Count == 0)
                    return (null);
                var serviceconfig = cloudserice.Cast<XmlNode>().First(x => x.Attributes[AttributeName].Value == AttributeValue);
                if (cloudserice == null)
                    return (null);
                var serviceobject = new CloudServicesConfig();
                try
                {
                    var stype = (ServiceType)Enum.Parse(typeof(ServiceType),
                                                        serviceconfig.Attributes["type"].Value.ToString());
                    serviceobject.serviceType = stype;
                    serviceobject.url = serviceconfig.Attributes["url"].Value;
                    return (serviceobject);
                }
                catch (Exception)
                {

                    return (null);
                }


            }
            else
            { return (null); }

        }
    }
}
