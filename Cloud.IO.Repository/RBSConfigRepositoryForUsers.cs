using Cloud.Foundation.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Cloud.IO.Repository
{
    public class RBSConfigRepositoryForUsers :  RBSConfigRepository
    {
        
        public RBSConfigRepositoryForUsers(string Path)
            : base(Path)
        {

        }
        
        public override List<RBSConfigRepository> ReadOne(string XPath)
        {
            String userId = XPath;
            var xDoc = XDocument.Load(this.filepath);
            var nodeList = (from query in xDoc.Descendants("application")
                            where (query.Element("UserID").Value == userId || query.Element("UserID").Value == "*")
                            select query).ToList();
            

            var lcrlist = new List<RBSConfigRepository>();
            foreach (var item in nodeList)
            {
                var rbs = new RBSConfigRepository();
                rbs.Name = item.Attribute("name").Value;
                rbs.Directory = item.Attribute("directory").Value;
                rbs.type = ConfigType.RBS;
                lcrlist.Add(rbs);
            }
            return (lcrlist);
        }

        public override void Async_Read(object Parameters)
        {
             if (Parameters == null)
            {
                var msg = string.Format("[server].[RBSConfigRepository].[Async_Read] => parameters are null or empty ");
                throw new Exception(msg);
            }

            var parametersArray = Parameters as object[];
            if (parametersArray.Count() == 2)
            {
                string xPath = parametersArray[0].ToString();
                var tdata = parametersArray[1] as ITransparentData;
                
                var xDoc = new XmlDocument();
                xDoc.Load(filepath);
                var rbsListForThisUser = xDoc.SelectNodes(xPath);

                var rbsList = new List<RBSConfigRepository>();
                foreach (var item in rbsListForThisUser.Cast<XmlNode>())
                {
                    var rbs = new RBSConfigRepository();
                    rbs.Name = item.ParentNode.Attributes["name"].Value;
                    rbs.Directory = item.ParentNode.Attributes["directory"].Value;
                    rbs.type = ConfigType.RBS;
                    rbsList.Add(rbs);

                }
                tdata.DataValue =  rbsList;
            }
        }

        public Boolean addRBSinAppsConfig(String appName, String directory, String UserID)
        {
            var result = false;
            String xPathExist = String.Format("//root/applications/application[@name='{0}' and @directory='{1}']", 
                                            appName, 
                                            directory);
            var checkxDoc = new XmlDocument();
            checkxDoc.Load(this.filepath);
            var existList = checkxDoc.SelectSingleNode(xPathExist);
            if (existList == null)
            {
                String xpath = "//root/applications";
                var xDoc = new XmlDocument();
                xDoc.Load(this.filepath);
                var applicationNode = xDoc.SelectSingleNode(xpath);

                var applicationElem = xDoc.CreateElement("application");
                var appname = xDoc.CreateAttribute("name");
                var appDirectory = xDoc.CreateAttribute("directory");
                var appEnable = xDoc.CreateAttribute("enable");
                var appTime = xDoc.CreateAttribute("time");
                appname.Value = appName;
                appDirectory.Value = directory;
                appEnable.Value = "true";
                appTime.Value = DateTime.Now.ToShortTimeString();
                applicationElem.Attributes.Append(appname);
                applicationElem.Attributes.Append(appDirectory);
                applicationElem.Attributes.Append(appEnable);
                applicationElem.Attributes.Append(appTime);

                var user = xDoc.CreateElement("User");
                var id = xDoc.CreateAttribute("ID");
                id.Value = UserID;
                user.Attributes.Append(id);
                applicationElem.AppendChild(user);

                applicationNode.AppendChild(applicationElem);
                xDoc.Save(filepath);
                result = true;
            }
            return (result);
        }

        public Boolean assignRBSToUser(String appName , String UserID)
        {
            Boolean result = false;
            String xPathExist = String.Format("//root/applications/application[@name='{0}']/User[@ID='{1}']",
                                            appName,
                                            UserID);
            var ExistxDoc = new XmlDocument();
            ExistxDoc.Load(this.filepath);
            var applicationList = ExistxDoc.SelectNodes(xPathExist);
            if (applicationList.Count == 0)
            {
                String xpath = String.Format("//root/applications/application[@name='{0}']" , appName);
                var xDoc = new XmlDocument();
                xDoc.Load(this.filepath);
                var applicationNode = xDoc.SelectSingleNode(xpath);
                if (applicationNode != null)
                {
                    var user = xDoc.CreateElement("User");
                    var id = xDoc.CreateAttribute("ID");
                    id.Value = UserID;
                    user.Attributes.Append(id);
                    applicationNode.AppendChild(user);
                    xDoc.Save(filepath);
                    result = true;
                }
                    
            }
            return (result);
        }


        public Boolean removeOrUninstallRBSforUser(String appName , String UserID)
        {
            Boolean result = false;

            String xPathExist = String.Format("//root/applications/application[@name='{0}']/User[@ID='{1}']",
                                            appName,
                                            UserID);
            var ExistxDoc = new XmlDocument();
            ExistxDoc.Load(this.filepath);
            var applicationList = ExistxDoc.SelectNodes(xPathExist);
            if (applicationList.Count == 1)
            {
                String xpath = String.Format("//root/applications/application[@name='{0}']/User[@ID='{1}']",
                                            appName,
                                            UserID);
                var xDoc = new XmlDocument();
                xDoc.Load(this.filepath);
                var user = xDoc.SelectSingleNode(xpath);
                user.ParentNode.RemoveChild(user);
                xDoc.Save(filepath);
                result = true;
            }
            return (result);
        }

    }
}
