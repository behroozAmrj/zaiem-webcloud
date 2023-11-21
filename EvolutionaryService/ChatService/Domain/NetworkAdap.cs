using ChatService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ChatService.Domain
{
    public class NetworkAdap : IDisposable
    {
        private string proxyServiceURL = "http://localhost:6729/router/Postmsg";
        private string destinationURL;
        private string deptURL;
        private DateTime datetime;
        private string[] target;
        private string content;

        public NetworkAdap(string destinationURL , string deptURL , string[] target )
        {
            proxyServiceURL = ConfigurationManager.AppSettings["proxyService"];
            if ((!string.IsNullOrEmpty(destinationURL)) ||
                (!string.IsNullOrEmpty(deptURL)))
            {
                this.destinationURL = "http://localhost:6729/router/Postmsg";
                this.deptURL = deptURL;
                this.datetime = DateTime.Now;
                this.target = target;
            }
            else
                throw new Exception("one or more parameters are null or empty");
        }

        public NetworkAdap(string proxyserviceURL , string destinationURL, string deptURL, string[] target)
        {
            if ((!string.IsNullOrEmpty(proxyserviceURL)) ||
                (!string.IsNullOrEmpty(destinationURL)) ||
                (!string.IsNullOrEmpty(deptURL)))
            {
                this.proxyServiceURL = proxyserviceURL;
                this.destinationURL = destinationURL;
                this.deptURL = deptURL;
                this.datetime = DateTime.Now;
                this.target = target;
            }
            else
                throw new Exception("one or more parameters are null or empty");
        }

        public void SendMessage(Message message)
        {
            string objStr = JsonConvert.SerializeObject(message);
            this.content = objStr;
            var hRequest = HttpWebRequest.Create(this.proxyServiceURL);
            hRequest.Method = "POST";
            hRequest.ContentType = "application/json";
            var masterMsg = new ProxyMessage();
            masterMsg.DeptURL = deptURL;
            masterMsg.DestinationURL = message.DestinationURL;
            masterMsg.Method = "get";
            masterMsg.DateTime = this.datetime;
            masterMsg.Content = content;
            /*

            string body = string.Format("{{\"DeptURL\":\"{0}\",\"DestinationURL\":\"{1}\",\"DateTime\":\"{2}\",\"Content\":\"{3}\" }}",
                                            deptURL,
                                            this.destinationURL,
                                            this.datetime.ToShortDateString(),
                                            content);
             
            */

            string body = JsonConvert.SerializeObject(masterMsg);
            byte[] bodybyte = Encoding.UTF8.GetBytes(body);
            using (var request = hRequest.GetRequestStream())
            {
                request.Write(bodybyte,
                            0,
                            bodybyte.Length);
            }
            try
            {
                var response =  hRequest.GetResponse() as HttpWebResponse;
                
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}