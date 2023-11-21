using MessageServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace MessageServer.Domain
{
    public class XmlServiceDataStore : IDataStore
    {
        private XmlDocument xDoc;
        private string filePath;
        public XmlServiceDataStore()
        {
            filePath = HttpContext.Current.Server.MapPath("/RoatData/RoatDB.xml");
            if (!File.Exists(filePath))
                throw new Exception("no roat table is exist!");
            xDoc = new XmlDocument();
            xDoc.Load(filePath);
        }

        public List<Service> getAvailableServices()
        {
            lock (xDoc)
            {
                xDoc.Load(filePath);
                string xPath = "//Services/service";
                var list = xDoc.SelectNodes(xPath);
                var serviceList = new List<Service>();
                foreach (var server in list.Cast<XmlNode>())
                {
                    var service = new Service();
                    service.serviceName = server.Attributes["serviceName"].Value;
                    service.URL = server.Attributes["URL"].Value;
                    serviceList.Add(service);
                }
                return (serviceList);
            }
        }

        public void RegisterService(Service service)
        {
            string tXpath = string.Format("//Services/service[@serviceName='{0}' and @URL='{1}']",
                                        service.serviceName,
                                        service.URL);

            var checkXdoc = new XmlDocument();
            checkXdoc.Load(filePath);
            var node = checkXdoc.SelectSingleNode(tXpath);
            if (node != null)
                return;
            lock (xDoc)
            {

                string xPath = "//Services";
                var servicesNode = xDoc.SelectSingleNode(xPath);
                var newService = xDoc.CreateElement("service");
                var serviceName = xDoc.CreateAttribute("serviceName");
                var url = xDoc.CreateAttribute("URL");
                serviceName.Value = service.serviceName;
                url.Value = service.URL;

                newService.Attributes.Append(serviceName);
                newService.Attributes.Append(url);


                servicesNode.AppendChild(newService);
                xDoc.Save(filePath);
            }
           
        }
        private void chekExistFile()
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/RoatData/")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/RoatData"));
            filePath = HttpContext.Current.Server.MapPath("/LogStore/") + "mLogStore.xml";
            /* ####     this section is for whether to create dataFile
                XmlWriter xmlWriter = XmlWriter.Create(filePath);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Services");

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            */

            if (xDoc == null)
            {

            }
            String xPath = "//Services";
            var loggingNode = xDoc.SelectSingleNode(xPath);
            if (loggingNode == null)
            {
                var loggingDom = xDoc.CreateElement("Logging");
                xDoc.AppendChild(loggingDom);
                xDoc.Save(filePath);

            }
        }

        public void deleteService()
        {
            /*
           string tXpath = string.Format("//Services/service[@serviceName='{0}' and @URL='{1}']",
                                       service.serviceName,
                                       service.URL);

           var checkXdoc = new XmlDocument();
           checkXdoc.Load(filePath);
           var node = checkXdoc.SelectSingleNode(tXpath);
           node.ParentNode.RemoveChild(node);
           checkXdoc.Save(filePath);
           */
        }

        public Service selectService(string serviceName)
        {
            string xPath = string.Format("//Services/service[@serviceName='{0}']",
                                       serviceName);
            lock(xDoc)
            {
                var serviceNode = xDoc.SelectSingleNode(xPath);
                if ( serviceNode == null)
                    return (null);
                var service = new Service();
                service.serviceName = serviceNode.Attributes["serviceName"].Value;
                service.URL = serviceNode.Attributes["URL"].Value;
                return (service);
            }
                                       
        }
    }
}