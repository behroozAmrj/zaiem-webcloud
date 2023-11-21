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
    public class zStoreService : INetworkService , IDisposable
    {
        private String token = string.Empty;

        public zStoreService(String Token)
        {
            if (String.IsNullOrEmpty(Token))
                throw new Exception("token is null or empty");
            this.token = Token;
        }
        public bool upload(string URL, byte[] fileContent)
        {
            throw new NotImplementedException();
        }

        public StreamReader download(string URL)
        {
            throw new NotImplementedException();
        }

        public Stream displayContent(string URL)
        {
                if (String.IsNullOrEmpty(URL))
                    throw new Exception("zStore Service URL is not valid");
                HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(URL);
                wRequest.Method = "GET";
                wRequest.ContentLength = 0;
                wRequest.Timeout = 15000;

                //wRequest.ContentType = "application/cdmi-container";
                wRequest.Headers.Add("X-Auth-Token",
                                        token);
                var response = (HttpWebResponse)wRequest.GetResponse();
                var streamReader = response.GetResponseStream();
                string responsevalue = string.Empty;
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("connecting to zStore service failed");
                return (streamReader);

            
        }

        public void excuteCommand(INetworkServiceCommand command)
        {
            command.executeCommand();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
