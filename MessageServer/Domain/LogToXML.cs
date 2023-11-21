using MessageServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace MessageServer.Domain
{
    public class LogToXML : ILogRegister
    {
        private XmlDocument xDoc;
        private String filePath;
        private XmlDocument xTranDoc;
        private string tFilePath;
        public void RegisterLog(LogInfo logInfo)
        {
            chekExistFile();
            String xPath = "//Logging";
            lock (xDoc)
            {
                var loggingNode = xDoc.SelectSingleNode(xPath);
                var eventElem = xDoc.CreateElement("event");
                var appName = xDoc.CreateAttribute("appName");
                var userID = xDoc.CreateAttribute("UserID");
                var dateTime = xDoc.CreateAttribute("dateTime");
                var action = xDoc.CreateAttribute("action");
                var actionType = xDoc.CreateAttribute("actionType");
                var content = xDoc.CreateElement("content");

                appName.Value = logInfo.AppName;
                userID.Value = logInfo.User_ID;
                dateTime.Value = DateTime.Now.ToString();
                actionType.Value = logInfo.ActionType;
                action.Value = logInfo.Action;
                content.InnerText = logInfo.Content;

                eventElem.Attributes.Append(appName);
                eventElem.Attributes.Append(userID);
                eventElem.Attributes.Append(dateTime);
                eventElem.Attributes.Append(action);
                eventElem.Attributes.Append(actionType);

                eventElem.AppendChild(content);
                //eventElem.AppendChild(source);
                eventElem.Attributes.Append(action);

                loggingNode.AppendChild(eventElem);
                xDoc.Save(filePath);
            }
        }

        private void chekExistFile()
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/LogStore/")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/LogStore"));
            filePath = HttpContext.Current.Server.MapPath("/LogStore/") + "mLogStore.xml";
            if (!File.Exists(filePath))
            {
                XmlWriter xmlWriter = XmlWriter.Create(filePath);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Logging");

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();

            }
            else
            {
                if (xDoc == null)
                {
                    xDoc = new XmlDocument();
                    xDoc.Load(filePath);
                }
                String xPath = "//Logging";
                var loggingNode = xDoc.SelectSingleNode(xPath);
                if (loggingNode == null)
                {
                    var loggingDom = xDoc.CreateElement("Logging");
                    xDoc.AppendChild(loggingDom);
                    xDoc.Save(filePath);
                }
            }
        }
    }
}