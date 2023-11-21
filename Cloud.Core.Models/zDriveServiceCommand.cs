using Cloud.Foundation.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    public class zDriveServiceCommand : INetworkServiceCommand
    {
        private String token;
        private ServiceCommandName commandName;
        private String URL;
        private String destinationURL;
        public zDriveServiceCommand(String url, String destinationURL, String token, ServiceCommandName cmdName)
        {
            this.URL = url;
            this.token = token;
            this.destinationURL = destinationURL;
            this.commandName = cmdName;
        }

        public void createContainer(string URL)
        {
            try
            {
                if (String.IsNullOrEmpty(URL))
                    throw new Exception("zDriveServiceCommand.createContainer:url is null or empty");
                HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(URL);
                wRequest.Method = "PUT";
                wRequest.ContentLength = 0;
                wRequest.Timeout = 15000;

                wRequest.ContentType = "application/cdmi-container";
                wRequest.Headers.Add("X-Auth-Token",
                                        token);
                String bodyText = "{\"metadata\":{}}";

                byte[] body = Encoding.ASCII.GetBytes(bodyText);
                wRequest.ContentLength = body.Length;
                var reqStream = wRequest.GetRequestStream();
                reqStream.Write(body,
                                    0,
                                    body.Length);
                reqStream.Close();

                using (var response = (HttpWebResponse)wRequest.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception("network Error!");
                }


                /*using (var stream = wRequest.GetRequestStream())
                {
                    stream.Write(body,
                                0,
                                body.Length);
                    stream.Close();

                    using(var response = (HttpWebResponse)wRequest.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.NoContent)
                            throw new Exception("network error");
                    }
                }
                */



            }
            catch (WebException e)
            {

                throw;
            }

        }
        public void deleteContainer(string URL)
        {
            try
            {
                if (String.IsNullOrEmpty(URL))
                    throw new Exception("zDriveServiceCommand.delete:url is null or empty");
                HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(URL);
                wRequest.Method = "DELETE";
                wRequest.ContentLength = 0;
                wRequest.Timeout = 15000;

                wRequest.ContentType = "application/cdmi-container";
                wRequest.Headers.Add("X-Auth-Token",
                                        token);
                var response = (HttpWebResponse)wRequest.GetResponse();
                string responsevalue = string.Empty;
                if (response.StatusCode != HttpStatusCode.NoContent)
                    throw new Exception("network error connetcting");
            }
            catch (WebException e)
            {
                throw;
            }
        }


        public void deleteFile(String URL)
        {
            try
            {
                if (String.IsNullOrEmpty(URL))
                    throw new Exception("zDriveServiceCommand.delete:url is null or empty");
                HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(URL);
                wRequest.Method = "DELETE";
                wRequest.ContentLength = 0;
                wRequest.Timeout = 15000;

                wRequest.ContentType = "application/cdmi-object";
                wRequest.Headers.Add("X-Auth-Token",
                                        token);
                var response = (HttpWebResponse)wRequest.GetResponse();
                string responsevalue = string.Empty;
                if (response.StatusCode != HttpStatusCode.NoContent)
                    throw new Exception("network error connetcting");
            }
            catch (WebException e)
            {
                throw;
            }
        }
        public void RenameContainer(string sourceURL, string destinationURL)
        {
            throw new NotImplementedException();
        }



        public void executeCommand()
        {
            switch (commandName)
            {
                case ServiceCommandName.CreateContainer: { createContainer(this.URL); break; }
                case ServiceCommandName.DeleteContainer: { deleteContainer(this.URL); break; }
                case ServiceCommandName.RenameContainer: { RenameContainer(this.URL, this.destinationURL); break; }
                case ServiceCommandName.DeleteFile: { deleteFile(this.URL); break; }
            }
        }
    }
}
