
using MessageServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MessageServer.Domain
{


    public static class Logging 
    {
        //static ILog log;
        static private XmlDocument xDoc;
        static private String filePath;
        private static XmlDocument xTranDoc;
        private static string tFilePath;
        public static void SimpleINOFlogRegister(string Event)
        {

        }

       
        public static void INFOlogRegistrer(string method, string extra, string type, string cont)
        {
            chekExistFile();

            String xPath = "//Logging";
            lock (xDoc)
            {
                var loggingNode = xDoc.SelectSingleNode(xPath);
                var eventElem = xDoc.CreateElement("event");
                //var userID = xDoc.CreateAttribute("UserID");
                var dateTime = xDoc.CreateAttribute("dateTime");
                var eventType = xDoc.CreateAttribute("type");
                var content = xDoc.CreateElement("content");
                //var source = xDoc.CreateElement("source");
                var extr = xDoc.CreateAttribute("extra");
                extr.Value = extra;
                content.Attributes.Append(extr);
                ///userID.Value = UserID;
                dateTime.Value = DateTime.Now.ToString();
                content.InnerText = cont;
                eventType.Value = type;

                //eventElem.Attributes.Append(userID);
                eventElem.Attributes.Append(dateTime);
                eventElem.AppendChild(content);
                //eventElem.AppendChild(source);
                eventElem.Attributes.Append(eventType);

                loggingNode.AppendChild(eventElem);
                xDoc.Save(filePath);
            }



        }

        public static void registerTransaction(Message msg, string functionMethod)
        {

            checkTransactionLogFileExist();
            String xPath = "//transactions";
            lock (xTranDoc)
            {
                //XmlSerializer xsSubmit = new XmlSerializer(typeof(Message));

                //using (StringWriter sww = new StringWriter())
                //using (XmlWriter writer = XmlWriter.Create(sww))
                //{
                //    xsSubmit.Serialize(writer, msg);
                //    var xml = sww.ToString(); // Your XML
                //    var loggingNode = xDoc.SelectSingleNode(xPath);
                //    xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?><Message xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" ,
                //                                                        string.Empty);
                //    loggingNode.InnerText = xml;
                //    xTranDoc.Save(tFilePath);
                //}

                var transactions = xTranDoc.SelectSingleNode(xPath);
                var transaction = xTranDoc.CreateElement("transaction");
                var derparture = xTranDoc.CreateAttribute("depar");
                var destination = xTranDoc.CreateAttribute("desti");
                var method = xTranDoc.CreateAttribute("method");
                var dateTime = xTranDoc.CreateAttribute("dateTime");
                var content = xTranDoc.CreateElement("content");

                derparture.Value = msg.DeptURL;
                destination.Value = msg.DestinationURL;
                method.Value = msg.Method;
                dateTime.Value = msg.DateTime.ToShortDateString();
                content.InnerText = msg.Content;

                transaction.Attributes.Append(derparture);
                transaction.Attributes.Append(destination);
                transaction.Attributes.Append(method);
                transaction.Attributes.Append(dateTime);
                transaction.AppendChild(content);

                transactions.AppendChild(transaction);
                xTranDoc.Save(tFilePath);
            }


        }

        private static void chekExistFile()
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

        private static void checkTransactionLogFileExist()
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/LogStore/")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/LogStore"));
            tFilePath = HttpContext.Current.Server.MapPath("/LogStore/") + "transactionLog.xml";
            if (!File.Exists(tFilePath))
            {
                XmlWriter xmlWriter = XmlWriter.Create(tFilePath);
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("transactions");

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();

            }
            else
            {
                if (xTranDoc == null)
                {
                    xTranDoc = new XmlDocument();
                    xTranDoc.Load(tFilePath);
                }
                String xPath = "//transactions";
                var loggingNode = xTranDoc.SelectSingleNode(xPath);
                if (loggingNode == null)
                {
                    var loggingDom = xTranDoc.CreateElement("transactions");
                    xTranDoc.AppendChild(loggingDom);
                    xTranDoc.Save(tFilePath);

                }

            }
        }







    }
}
