using RBSManagementService.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RBSManagementService.Models;
using System.Xml;
using System.Collections;
using System.IO;
using RBSManagementService.Filing;

namespace RBSManagementService.LojicModel
{
    public class XMLStorage : XMLData, IDataAccessTarget
    {
        public XMLStorage(string filePath) : base(filePath)
        {

        }
        public RBS SelectAppByUserID(string appName, string userID)
        {
            if ((string.IsNullOrEmpty(appName)) ||
                    (string.IsNullOrEmpty(userID)))
                throw new Exception("application name or userID is null or empty!");
            string xPath = String.Format("//applications/application[@name='{0}']/User[@ID='{1}']",
                                            appName,
                                            userID);
            var nodeListobj = GetData(xPath);

            if ((nodeListobj == null) || ((nodeListobj as List<object>).Count == 0))
                return (null);
            var RBS = nodeListobj.Select((x) =>
           {
               var rbs = new RBS();
               rbs.Name = (x as XmlNode).ParentNode.Attributes["name"].Value;
               rbs.source = (x as XmlNode).ParentNode.Attributes["source"].Value;
              // rbs.version = (x as XmlNode).ParentNode.Attributes["version"].Value;
               return (rbs);
           }).First();

            return (RBS);
           
        }

        public List<RBS> SelectApplications(string UserID)
        {
            if (string.IsNullOrEmpty(UserID))
                throw new Exception("application name or userID is null or empty!");
            string xPath = String.Format("//applications/application/User[@ID='{0}' or @ID='*']",
                                            UserID);
            var nodeListobj = GetData(xPath);

            if ((nodeListobj == null) || ((nodeListobj as List<object>).Count == 0))
                return (null);
            var RBSList = new List<RBS>();
             foreach(var x in nodeListobj)
            {
                string source = (x as XmlNode).ParentNode.Attributes["source"].Value;
                if (source.StartsWith("http://"))
                {
                    var rbs = new RBS();
                    rbs.Name = (x as XmlNode).ParentNode.Attributes["name"].Value;
                    rbs.source = source;
                    rbs.version = (x as XmlNode).ParentNode.Attributes["version"].Value;
                    RBSList.Add(rbs);
                }
                else
                {
                    string configPath = string.Format("{0}\\AppStore\\{1}\\config.xml" ,
                                                        AppDomain.CurrentDomain.BaseDirectory,
                                                        source);
                    if (File.Exists(configPath))
                    {
                        var rbs = new RBS();
                        rbs.Name = (x as XmlNode).ParentNode.Attributes["name"].Value;
                        rbs.source = source;
                        rbs.version = (x as XmlNode).ParentNode.Attributes["version"].Value;
                        getThisRBSFromConfigFile(configPath  , 
                                                rbs);
                        RBSList.Add(rbs);
                    }
                }
            }
            return (RBSList.ToList());
        }

        private void getThisRBSFromConfigFile(string configPath, RBS rbs)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(configPath);
            string image = xDoc.SelectNodes("//applicationConfig//image").Cast<XmlNode>().First().Attributes["src"].Value;
            string page = xDoc.SelectNodes("//applicationConfig//appconfig").Cast<XmlNode>().First().Attributes["startpage"].Value;
            string title = xDoc.SelectNodes("//applicationConfig//appconfig").Cast<XmlNode>().First().Attributes["title"].Value;
            page = string.Format("Application-Service/{0}/layouts/{1}",
                                    rbs.source,
                                    page);
            rbs.Image = image;
            rbs.FirstPage = page;
            rbs.Title = title;

        }
    }
}