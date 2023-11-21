using Cloud.Foundation.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cloud.IO.NetworkService
{
    public class ProxyService
    {
        private string URL;
        public ProxyService(string serviceURL)
        {
            if (string.IsNullOrEmpty(serviceURL))
                throw new Exception("servive url is null or empty!");
            string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";

            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!reg.IsMatch(serviceURL))
                throw new Exception("service url is not valid URL");
            this.URL = serviceURL;

        }

        public void Send(ProxyMessage message)
        {
            if (message == null)
                throw new Exception("proxy message is null or emtpy!");
            var request = (HttpWebRequest)WebRequest.Create(this.URL);
            request.Method = "POST";
            request.ContentType = "application/json";
            string body = JsonConvert.SerializeObject(message);
            byte[] bodyByte = Encoding.UTF8.GetBytes(body);
            using (var reqStream = request.GetRequestStream())
            {
                reqStream.Write(bodyByte,
                                0,
                                bodyByte.Length);
            }
            var response = request.GetResponse() as HttpWebResponse;
        }

        public Stream SendBack(ProxyMessage message)
        {
            if (message == null)
                throw new Exception("proxy message is null or emtpy!");
            var request = (HttpWebRequest)WebRequest.Create(this.URL);
            request.Method = "Post";
            request.ContentType = "application/json";
            string body = JsonConvert.SerializeObject(message);
            byte[] bodyByte = Encoding.UTF8.GetBytes(body);
            using (var reqStream = request.GetRequestStream())
            {
                reqStream.Write(bodyByte,
                                0,
                                bodyByte.Length);
            }
            try
            {
                var response = request.GetResponse() as HttpWebResponse;
                if (!(response.StatusCode == HttpStatusCode.OK))
                    return (null);
                var responseStream = response.GetResponseStream();
                return (responseStream);
            }
            catch (Exception ex)
            {
                return (null);
            }
            
        }
    }
}
