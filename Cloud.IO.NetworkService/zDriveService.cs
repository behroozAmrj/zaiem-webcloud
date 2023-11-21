
using Cloud.Foundation.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Cloud.IO.NetworkService
{
    public class zDriveService : INetworkService, IDisposable
    {
        private String token;
        //private String userID;
        public String FileName { get; set; }
        public String FileContent { get; set; }
        public HttpContextBase context { get; set; }
        public zDriveService(String token)
        {

            if ((!String.IsNullOrEmpty(token)))
            {
                this.token = token;
            }
            else
            {
                throw new Exception("token value is null or empty");
            }
        }

        public bool upload(string URL, byte[] fileContent)
        {
            try
            {
                var wRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
                wRequest.Method = "PUT";
                wRequest.ContentLength = fileContent.Length;
                wRequest.ContentType = "application/cdmi-object";
                wRequest.Headers.Add("X-Auth-Token",
                                        token);
                wRequest.ReadWriteTimeout = 15000;
                Console.WriteLine("requesting to server . . . ");
                using (var newStream = wRequest.GetRequestStream())
                {
                    newStream.Write(fileContent,
                                    0,
                                    fileContent.Length);
                    newStream.Close();

                    var wresp = (HttpWebResponse)wRequest.GetResponse();
                    while ((wresp.StatusCode != HttpStatusCode.NoContent) && (wresp.StatusCode != HttpStatusCode.OK))
                    { }

                }

                // writeByteToFile();
                return (true);
            }
            catch (WebException ex)
            {

                return (false);
            }
        }

        private void writeByteToFile()
        {
            try
            {
                using (var file = new FileStream(HttpContext.Current.Server.MapPath("/TempDownload/") + this.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    byte[] fContent = Convert.FromBase64String(this.FileContent);
                    file.Write(fContent,
                                0,
                                fContent.Length);
                    file.Flush();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Stream fileMetadata(String URL)
        {
            try
            {
                if (String.IsNullOrEmpty(URL))
                    throw new Exception("zcloud Service URL is not valid");
                HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(URL);
                wRequest.Method = "GET";
                wRequest.ContentLength = 0;
                wRequest.Timeout = 15000;

                wRequest.ContentType = "application/cdmi-object";
                wRequest.Headers.Add("X-Auth-Token",
                                        token);
                var response = (HttpWebResponse)wRequest.GetResponse();
                string responsevalue = string.Empty;
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("connecting to zDrive service failed");
                var streamReader = response.GetResponseStream();
                return (streamReader);

            }
            catch (WebException wx)
            {
                throw new Exception(String.Format("zDriveServics.displayContent error: {0}", wx.Message));
            }
        }

        public StreamReader download(string URL)
        {
            var wRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            wRequest.Method = "GET";
            wRequest.ContentType = "application/cdmi-object";
            wRequest.Headers.Add("X-Auth-Token",
                                    this.token);

            var response = (HttpWebResponse)wRequest.GetResponse();
            var fileStream = response.GetResponseStream();

            var streamReader = new StreamReader(fileStream);

            return (streamReader);


        }

        public Stream displayContent(string URL)
        {
            try
            {
                if (String.IsNullOrEmpty(URL))
                    throw new Exception("zcloud Service URL is not valid");
                HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(URL);
                wRequest.Method = "GET";
                wRequest.ContentLength = 0;
                wRequest.Timeout = 15000;

                wRequest.ContentType = "application/cdmi-container";
                wRequest.Headers.Add("X-Auth-Token",
                                        token);
                var response = (HttpWebResponse)wRequest.GetResponse();

                var streamReader = response.GetResponseStream();
                string responsevalue = string.Empty;
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception("connecting to zDrive service failed");
                return (streamReader);


            }
            catch (WebException wx)
            {
                throw new Exception(String.Format("zDriveServics.displayContent error: {0}", wx.Message));
            }

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
