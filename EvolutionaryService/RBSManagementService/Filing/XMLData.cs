using RBSManagementService.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using RBSManagementService.Models;
using System.ComponentModel;

namespace RBSManagementService.Filing
{
    public class XMLData : CoreDataAccess
    {
        XmlDocument xDoc;
        protected string filePath = string.Empty;



        public XMLData(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = HttpContext.Current.Server.MapPath("/Data/") + "AppData.xml";

            xDoc = new XmlDocument();
            xDoc.Load(filePath);
        }

        //public RBS SelectAppByUserID(string appName, string userID)
        //{
        //            throw new NotImplementedException();
        //}

        //public List<RBS> SelectApplications(string UserID)
        //{
        //    if (string.IsNullOrEmpty(UserID))
        //        throw new Exception("user ID is null or empty");
        //    string xPath = String.Format("//applications/application/User[@ID='{0}' or @ID='*']", 
        //                                UserID);
        //    xDoc.Load(xPath);
        //    var nodeList = xDoc.SelectNodes(xPath);
        //    if ((nodeList == null) ||
        //        (nodeList.Count == 0))
        //        return (null);
        //    var RBSList = nodeList.Cast<XmlNode>().Select((x) => 
        //    {
        //        var rbs = new RBS();
        //        rbs.Name = x.Attributes["Name"].Value;
        //        rbs.source = x.Attributes["source"].Value;
        //        rbs.Enable = Convert.ToBoolean(x.Attributes["enable"].Value);
        //        return (rbs);
        //   });

        //    return (RBSList.ToList());
        //}
        /*
        public override T SelectAppByUserID<T>(string appName, string userID)
        {
            if (string.IsNullOrEmpty(userID))
                throw new Exception("user ID is null or empty");
            string xPath = String.Format("//applications/application/User[@ID='{0}' or @ID='*']",
                                        userID);
            xDoc.Load(xPath);
            var nodeList = xDoc.SelectNodes(xPath);
            if ((nodeList == null) ||
                (nodeList.Count == 0))
                return ((T)Convert.ChangeType(null , typeof(T)));
            var RBSList = nodeList.Cast<XmlNode>().Select((x) =>
            {
                var rbs = new RBS();
                rbs.Name = x.Attributes["Name"].Value;
                rbs.source = x.Attributes["source"].Value;
                rbs.Enable = Convert.ToBoolean(x.Attributes["enable"].Value);
                return (rbs);
            });

            return (T)(Convert.ChangeType(RBSList.ToList() ,
                                        typeof(T)));
        }

        public override T SelectApplications<T>(string UserID)
        {
            throw new NotImplementedException();
        }
        */
        public override List<object> GetData(string dataCondition)
        {
            if (string.IsNullOrEmpty(dataCondition))
                throw new Exception("data condition is null or emtpy!");
            string xPath = dataCondition;
            var  nodeList = xDoc.SelectNodes(xPath);
            if ((nodeList == null) ||
                (nodeList.Count == 0))
                return (null);
            var nodelistObj = nodeList.Cast<XmlNode>().Select((x) =>
                {
                    var obj = new Object();
                    obj = x;
                    return (obj);
            });

            return (nodelistObj.ToList());

        }

        public override void PostData(string dataCondition)
        {
            throw new NotImplementedException();
        }

        public override void updateData(params string[] param)
        {
            throw new NotImplementedException();
        }

        public override void deleteData(string conditions)
        {
            throw new NotImplementedException();
        }
    }
}