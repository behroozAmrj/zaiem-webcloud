using Cloud.Foundation.Infrastructure;
using Cloud.IO.NetworkService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    public class APIServiceManagement : IDisposable
    {
        private ProxyMessage msg;
        private string proxyURL = string.Empty;
        private string userID = string.Empty;
        public APIServiceManagement(ProxyMessage message)
        {
            if (message == null)
                throw new Exception("message is null or emtpy!");
            this.msg = message;
            this.proxyURL = ConfigurationManager.AppSettings["proxyService"];
        }

        public APIServiceManagement(ProxyMessage message  , string userID)
        {
            if (message == null)
                throw new Exception("message is null or emtpy!");
            this.msg = message;
            this.proxyURL = ConfigurationManager.AppSettings["proxyService"];
            this.userID = userID;
        }
        public void PostData()
        {
            if (string.IsNullOrEmpty(this.userID))
                proxyURL = string.Format("{0}/Postmsg",
                                        proxyURL);
            else
                proxyURL = string.Format("{0}/Postmsg/{1}",
                                        proxyURL,
                                        this.userID);

            var service = new ProxyService(proxyURL);
            service.Send(msg);
        }

        public string PostBackData()
        {
            proxyURL = string.Format("{0}/PostMsgCallBack",
                                    proxyURL);
            var service = new ProxyService(proxyURL);
            using ( var stream =  service.SendBack(msg))
            {
                if (stream == null)
                    return (string.Empty);
                var sReader = new StreamReader(stream);
                var result = sReader.ReadToEnd();
                stream.Close();
                return (result);
            }

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
