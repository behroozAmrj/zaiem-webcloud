using Cloud.Foundation.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.IO.NetworkService
{
    public class CloudServers : IServer
    {
        private string token;
        
        public CloudServers(String Token = null)
        {
            if (!String.IsNullOrEmpty(Token))
                this.token = Token;
        }
        public bool Connect(string URL, object[] param = null)
        {
            throw new NotImplementedException();
        }


        public object RetrieveContent(string URL, object[] param = null)
        {
            if ((String.IsNullOrEmpty(URL)) ||
                (String.IsNullOrEmpty(token)))
                throw new Exception("url or token are null or empty");
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(URL);
            wRequest.Method = "GET";
            wRequest.ContentLength = 0;
            wRequest.Timeout = 15000;


            wRequest.Headers.Add("X-Auth-Token",
                                    token);
            var response = (HttpWebResponse)wRequest.GetResponse();
            var streamReader = response.GetResponseStream();
            string responsevalue = string.Empty;
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("connecting to cloud service failed");
            
            return (streamReader);
        }

        // # this method used whether we want to call webservice without header
        public object RetrieveContentNoneHeader(string URL, object[] param = null)
        {
            if (String.IsNullOrEmpty(URL))
                throw new Exception("url is null or empty");
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(URL);
            wRequest.Method = "GET";
            wRequest.ContentLength = 0;
            wRequest.Timeout = 15000;

            var response = (HttpWebResponse)wRequest.GetResponse();
            var streamReader = response.GetResponseStream();
            string responsevalue = string.Empty;
            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception("connecting to cloud service failed");
            return (streamReader);
        }

    }
}
