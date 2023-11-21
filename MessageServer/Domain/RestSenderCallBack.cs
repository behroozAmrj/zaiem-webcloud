using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MessageServer.Domain
{
    public class RestSenderCallBack : ISender
    {
        public object Send(Models.Message message)
        {
            if (string.IsNullOrEmpty(message.DestinationURL))
                throw new Exception("destination URL is null or empty!");
            Logging.INFOlogRegistrer("RestSender.Send",
                                           "RestSender.Send",
                                            "info",
                                            message.Content);

            var address = selectServiceURL(message.DestinationURL);
            if (address == string.Empty)
                throw new Exception("service not found");
            if (!address.EndsWith("/"))
                address += "/";
            string url = string.Format("{0}{1}",
                                        address,
                                        message.Method);
            var validation = new Validation();
            if (!validation.validURL(url))
                throw new Exception("url is not valid!");
            var req = HttpWebRequest.Create(url) as HttpWebRequest;
            if ((string.IsNullOrEmpty(message.Content)) ||
                  (string.IsNullOrWhiteSpace(message.Content)))
            {
                req.Method = "Get";
                using (var response = (HttpWebResponse)req.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    var sReader = new StreamReader(stream);
                    string resultBack = sReader.ReadToEnd();
                    return (resultBack);
                }
            }
            else
            {
                req.Method = "POST";
                req.ContentType = "application/json";
                string strBody = String.Format("{{\"content\":{0}}}",
                                                message.Content);
                byte[] bodyBytes = Encoding.UTF8.GetBytes(strBody);
                string str = bodyBytes.ToString();
                using (var request = req.GetRequestStream())
                {
                    request.Write(bodyBytes,
                                    0,
                                    bodyBytes.Length);
                }
                using (var response = (HttpWebResponse)req.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    var sReader = new StreamReader(stream);
                    string resultBack = sReader.ReadToEnd();
                    return (resultBack);
                }
            }
        }
        private string selectServiceURL(string serviceName)
        {
            var xmlData = ServiceManagementCenter.ServiceCenterMgr.getStorage();
            var service = xmlData.selectService(serviceName);
            if (service == null)
                return (string.Empty);
            else
                return (service.URL);
        }

    }
}