using Cloud.IO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Cloud.Core.Models
{
    public class FormsConfiguration
    {
        private string formName;
        private XmlNodeList xmlNodelist;
        private Dictionary<string , string> configuration ;
        private string xpath;
        public FormsConfiguration(string FormName)
        {
            if (!string.IsNullOrEmpty(FormName))
                formName = FormName.Trim();
            else
                throw new Exception("[Server].[FormsConfiguration].[ctor] => form name is null or empty ");
        }

        public Dictionary<string, string> GetFormConfigurationSetting(string XPath)
        {
            if (!string.IsNullOrEmpty(XPath))
                xpath = XPath;
            else
                return (null);
            configuration = new Dictionary<string, string>();
            var appconfig = new LayoutConfigRepository("Application-Service\\AppsConfig.xml");
            var xmlnodes = appconfig.ChildNodeList(XPath);
            foreach (var xnode in xmlnodes.Cast<XmlNode>())
            {
                string name = xnode.Name;
                string value = xnode.Attributes[0].Value;
                configuration.Add(name,
                                  value);
            }
            return (configuration);
        }

    }
}
