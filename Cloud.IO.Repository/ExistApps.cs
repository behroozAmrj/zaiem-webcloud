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
    public class ExistApps : IServer , IDisposable
    {
        public bool Connect(string URL, object[] param = null)
        {
            throw new NotImplementedException();
        }

        public object RetrieveContent(string Address, object[] param = null)
        {
            if ((param == null) || (param.Count() == 0))
                return (null);
            String userId = Address;
            String filePath = param[0] as String;



            var parametersArray = param as object[];
            if (parametersArray.Count() == 2)
            {
                string xPath = parametersArray[1].ToString();
                var tdata = parametersArray[1] as ITransparentData;

                var xDoc = new XmlDocument();
                xDoc.Load(filePath);
                var rbsListForThisUser = xDoc.SelectNodes(xPath);

                var rbsList = new List<RBSConfigRepository>();
                foreach (var item in rbsListForThisUser.Cast<XmlNode>())
                {
                    var rbs = new RBSConfigRepository();

                    var allChiledList = item.ParentNode.ChildNodes.Cast<XmlNode>();

                    try
                    {
                        int notUserIDNode = (from query in allChiledList
                                             where query.Attributes["ID"].Value == userId
                                             select query).Count();

                        if (notUserIDNode == 0)
                        {
                            if (rbsList.Count == 0)
                            {
                                rbs.Name = item.ParentNode.Attributes["name"].Value;
                                rbs.Directory = item.ParentNode.Attributes["directory"].Value;
                                rbs.type = ConfigType.RBS;
                                rbsList.Add(rbs);
                            }
                            else
                            {
                                String parentName = item.ParentNode.Attributes["name"].Value;
                                int check =  (from query in  rbsList
                                             where query.Name == parentName
                                             select query).Count();
                                if (check == 0)
                                {
                                    rbs.Name = item.ParentNode.Attributes["name"].Value;
                                    rbs.Directory = item.ParentNode.Attributes["directory"].Value;
                                    rbs.type = ConfigType.RBS;
                                    rbsList.Add(rbs);
                                }
                            }
                        }
                    }
                    catch
                    {


                    }

                }
                return (rbsList);

            }
            else
                return (null);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
