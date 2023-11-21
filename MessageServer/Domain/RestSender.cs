using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MessageServer.Domain
{
    public class RestSender : ISender
    {
        public object Send(Models.Message message)
        {
            try
            {
                if (string.IsNullOrEmpty(message.DestinationURL))
                    throw new Exception("destination URL is null or empty!");
                Logging.INFOlogRegistrer("RestSender.Send",
                                               "no target",
                                                "info",
                                                message.Content);

                var address = selectServiceURL(message.DestinationURL);
                if(address == string.Empty)
                    throw new Exception("service not found");
                if (!address.EndsWith("/"))
                    address += "/";
                string url = string.Format("{0}{1}" , 
                                            address,
                                            message.Method);
                var validation = new Validation();
                if (!validation.validURL(url))
                    throw new Exception("url is not valid!");
                var req = HttpWebRequest.Create(url) as HttpWebRequest;
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
                req.GetResponse();

                return (null);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} to:{1}",
                                                    ex.Message,
                                                    message.DestinationURL));
            }
            // # if any response required, code must be added after this line
        }

        private string selectServiceURL(string serviceName)
        {
            var xmlData = ServiceManagementCenter.ServiceCenterMgr.getStorage();
            var service =  xmlData.selectService(serviceName);
            if (service == null)
                return (string.Empty);
            else
                return (service.URL);
        }
    }
}