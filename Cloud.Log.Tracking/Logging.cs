using Cloud.IO.DataAccess.Transactions;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Cloud.Log.Tracking
{


    public static class Logging
    {
        static ILog log;
        static private XmlDocument xDoc;
        static private String filePath;
        public static void SimpleINOFlogRegister(string Event)
        {
            log4net.GlobalContext.Properties["UserID"] = "MyUserID";
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Info(Event);

        }

        public static void INFOLogRegisterToDB(string userID  , string action , string actionType , string content)
        {
            using (var logInDB = new TransactionRegister())
            {
                var logInfo = new LogInfo();
                logInfo.AppName = "webcloud";
                logInfo.DateTime = DateTime.Now;
                logInfo.User_ID = userID;
                logInfo.Action = action;
                logInfo.ActionType = actionType;
                logInfo.Content = content;
                logInDB.LogRegister(logInfo);
            }
        }
        public static void INFOlogRegistrer(string Event, string UserID, Type Type)
        {
            using (var logInDB = new TransactionRegister())
            {
                var logInfo = new LogInfo();
                logInfo.AppName = "webcloud";
                logInfo.DateTime = DateTime.Now;
                logInfo.User_ID = UserID;
                logInfo.Action = Type.ToString();
                logInfo.ActionType = "info";
                logInfo.Content = Event;
                logInDB.LogRegister(logInfo);
            }
            chekExistFile();
            /*
            if ((string.IsNullOrEmpty(UserID)) ||
                (Type == null))
                SimpleINOFlogRegister(Event);
            else
            {
                string statement = string.Format("[{0}] {1}",
                                                    UserID,
                                                    Event);
                log4net.GlobalContext.Properties["UserID"] = "MyUserID";
                log = LogManager.GetLogger(Type);
                log.Info(statement);
            }
             */

            String xPath = "//Logging";
            lock (xDoc)
            {
                var loggingNode = xDoc.SelectSingleNode(xPath);
                var eventElem = xDoc.CreateElement("event");
                var userID = xDoc.CreateAttribute("UserID");
                var dateTime = xDoc.CreateAttribute("dateTime");
                var eventType = xDoc.CreateAttribute("type");
                var msg = xDoc.CreateElement("message");
                var source = xDoc.CreateElement("source");

                userID.Value = UserID;
                dateTime.Value = DateTime.Now.ToString();
                msg.InnerText = Event;
                eventType.Value = "info";
                source.InnerText = Type.ToString();

                eventElem.Attributes.Append(userID);
                eventElem.Attributes.Append(dateTime);
                eventElem.AppendChild(msg);
                eventElem.AppendChild(source);
                eventElem.Attributes.Append(eventType);

                loggingNode.AppendChild(eventElem);
                xDoc.Save(filePath);
            }


        }

        private static void chekExistFile()
        {
            filePath = HttpContext.Current.Server.MapPath("/LogStore/") + "xLogEvent.xml";
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


        public static void ErrorlogRegister(string Event, Type type, String UserID = null)
        {
            /*
            log4net.GlobalContext.Properties["UserID"] = "MyUserID";
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(Event);
            */

            chekExistFile();
            lock (xDoc)
            {
                String xPath = "//Logging";
                var loggingNode = xDoc.SelectSingleNode(xPath);
                var eventElem = xDoc.CreateElement("event");
                var userID = xDoc.CreateAttribute("UserID");
                var dateTime = xDoc.CreateAttribute("dateTime");
                var eventType = xDoc.CreateAttribute("info");
                var msg = xDoc.CreateElement("message");
                var source = xDoc.CreateElement("source");

                userID.Value = UserID == null ? "no User" : UserID;
                dateTime.Value = DateTime.Now.ToString();
                msg.InnerText = Event;
                eventType.Value = "error";
                source.InnerText = type.ToString();

                eventElem.Attributes.Append(userID);
                eventElem.Attributes.Append(dateTime);
                eventElem.Attributes.Append(eventType);

                eventElem.AppendChild(msg);
                eventElem.AppendChild(source);

                loggingNode.AppendChild(eventElem);
                xDoc.Save(filePath);
            }
        }

        /*
        public static void SimpleErrorlogRegister(String UserID ,string Event)
        {
           

            chekExistFile();
            lock (xDoc)
            {
                String xPath = "//Logging";
                var loggingNode = xDoc.SelectSingleNode(xPath);
                var eventElem = xDoc.CreateElement("event");
                var userID = xDoc.CreateAttribute("UserID");
                var dateTime = xDoc.CreateAttribute("dateTime");
                var msg = xDoc.CreateAttribute("message");
                var eventType = xDoc.CreateAttribute("type");

                userID.Value = UserID;
                dateTime.Value = DateTime.Now.ToString();
                msg.Value = Event;
                eventType.Value = "error";

                eventElem.Attributes.Append(userID);
                eventElem.Attributes.Append(dateTime);
                eventElem.Attributes.Append(msg);
                eventElem.Attributes.Append(eventType);

                loggingNode.AppendChild(eventElem);
                xDoc.Save(filePath);
            }
        }
        */
        public static void ErrorlogRegistrer(string Event, string UserID)
        {
            /*
            if ((string.IsNullOrEmpty(UserID)) ||
                (Type == null))
            {
                //SimpleErrorlogRegister(Event);
            }
            else
            {
                string statement = string.Format("[{0}] {1}",
                                                    UserID,
                                                    Event);
                log4net.GlobalContext.Properties["UserID"] = "MyUserID";
                log = LogManager.GetLogger(Type);
                log.Error(statement);
            }
             * */
            
        }



        public static void SimpleWarringRegister(string Event)
        {
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Warn(Event);
        }


        public static List<LogRecord> ShowLogs()
        {
            var logList = new List<LogRecord>();
            chekExistFile();
            lock (xDoc)
            {
                String xPath = "//Logging/event";
                var eventList = xDoc.SelectNodes(xPath);
                foreach (var logEvent in eventList.Cast<XmlNode>())
                {
                    var logRecord = new LogRecord();
                    logRecord.userID = logEvent.Attributes["UserID"].Value;
                    logRecord.dateTime = logEvent.Attributes["dateTime"].Value;
                    logRecord.message = logEvent.ChildNodes.Item(0).InnerText;
                    logList.Add(logRecord);
                }
            }
            return (logList);
        }

        public static List<LogRecord> ShowLogs(String UserID)
        {
            if ((String.IsNullOrEmpty(UserID)) || (String.IsNullOrWhiteSpace(UserID)))
                return (ShowLogs());
            var logList = new List<LogRecord>();
            chekExistFile();
            lock (xDoc)
            {
                String xPath = String.Format("//Logging/event[@UserID=\"{0}\"]", UserID);
                var eventList = xDoc.SelectNodes(xPath);
                foreach (var logEvent in eventList.Cast<XmlNode>())
                {
                    var logRecord = new LogRecord();
                    logRecord.userID = logEvent.Attributes["UserID"].Value;
                    logRecord.dateTime = logEvent.Attributes["dateTime"].Value;
                    logRecord.type = logEvent.Attributes["type"].Value;
                    logRecord.message = logEvent.ChildNodes.Item(0).InnerText;
                    logList.Add(logRecord);
                }
            }
            return (logList);
        }
    }
}
