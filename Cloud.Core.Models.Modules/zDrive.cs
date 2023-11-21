using Cloud.Foundation.Infrastructure;
using Cloud.IO.NetworkService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Cloud.Core.Models.Modules
{
    public class zDrive : ISecurityModule
    {
        public object Autenticate(string userName, string password, string[] optionalParams)
        {
            if ((optionalParams == null) ||
                 (optionalParams.Count() == 0))
                throw new Exception("optional parameters null or empty");
            string URL = optionalParams[0];
            String url = String.Format("{0}:35357/v2.0/tokens",
                                                 URL);
            var result = new String[3];// String.Empty;
            String resultValue = String.Empty;
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(url);
            wRequest.Timeout = 15000;
            wRequest.Method = "POST";
            wRequest.ContentType = "application/json";
            //wRequest.Headers.Add("X-Auth-Token", "_token");
            //wRequest.Accept = "application/json";
            //wRequest.AllowAutoRedirect = true;

            /*
                        String username = String.Format("\"username:\"{0}\"", userName);
                        String password = String.Format("\"password\":\"{0}\"", passWord);
                        String tenant = String.Format("\"tenantName\":\"{0}\"", "public");
                        String requestBody = String.Format("auth{{ passwordCredentials {{ {0},{1} }} {2} }}" , 
                                                    username,
                                                    password,
                                                    tenant);
                        */
            try
            {
                String tempURL = String.Format("{{\"auth\":{{ \"passwordCredentials\": {{\"username\":\"{0}\" , \"password\" : \"{1}\"}},\"tenanName\":\"public\"}}}}",
                                                                                                                                                                 userName,
                                                                                                                                                                 password);
                //String tempURL = String.Format("{'auth':{ 'passwordCredentials': {'username:{0}' , 'password' : '{1}'},'tenanName':'public'}}", userName , password );
                var encoding = new ASCIIEncoding();
                byte[] byteStream = encoding.GetBytes(tempURL);// (requestBody);
                wRequest.ContentLength = byteStream.Length;
                var rStream = wRequest.GetRequestStream();
                rStream.Write(byteStream,
                                0,
                                byteStream.Length);
                rStream.Close();

                using (var response = (HttpWebResponse)wRequest.GetResponse())
                {
                    if (!(response.StatusCode == HttpStatusCode.OK))
                        return (null);


                    using (var reponseStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(reponseStream))
                        {
                            resultValue = reader.ReadToEnd();
                        }
                    }
                    var parseTree = JsonConvert.DeserializeObject<JObject>(resultValue);
                    var jo = JObject.Parse(resultValue);
                    var eDate = Convert.ToDateTime((String)jo["access"]["token"]["expires"]);
                    var sDate = Convert.ToDateTime((String)jo["access"]["token"]["issued_at"]);
                    var data = eDate - sDate;
                    var token = (String)jo["access"]["token"]["id"];// ["token"];
                    response.Close();
                    if (!String.IsNullOrWhiteSpace(token))
                    {
                        result[0] = token;
                        result[1] = (String)jo["access"]["token"]["expires"];
                        result[2] = (String)jo["access"]["token"]["role"] == null ? "admin" : null;
                    }

                }
                return (result);

            }
            catch (WebException ex)
            {
                Console.WriteLine("no connection to Server please Try later");
                return (null);
            }
        }

        public void SaveCredentials(string userID, params string[] additionalParams)
        {
            throw new NotImplementedException();
        }

        public object RetrieveCredentials(string userID)
        {
            throw new NotImplementedException();
        }


        private String userID;
        public zDrive(String UserId)
        {
            if (!String.IsNullOrEmpty(UserId))
            {
                this.userID = UserId;
            }
            else
            {
                if (HttpContext.Current.Session["userID"] != null)
                    this.userID = HttpContext.Current.Session["userID"].ToString();


            }
        }

        public List<String> getContentListFromzDriveService(String URL, String token)
        {

            var zdriveService = new zDriveService(token);
            using (var responseStream = zdriveService.displayContent(URL))
            {
                using (var sReader = new StreamReader(responseStream))
                {
                    // = new List<String>();
                    var content = sReader.ReadToEnd();
                    var jo = JObject.Parse(content);
                    //var children = jo["children"].ToObject<List<String>>();
                    var retList = jo["children"].ToObject<List<String>>();
                    //foreach (string fileandDirectory in children)
                    //  retList.Add(fileandDirectory);
                    sReader.Close();
                    return (retList);
                }
            }
            /*
            try
            {
            }
            catch (Exception)
            {
                try
                {
                    Thread.Sleep(2000);
                    var zdriveService = new zDriveService(token);
                    using (var responseStream = zdriveService.displayContent(URL))
                    {
                        using (var sReader = new StreamReader(responseStream))
                        {
                            var retList = new List<String>();
                            var content = sReader.ReadToEnd();
                            var jo = JObject.Parse(content);
                            var children = (JArray)jo["children"];
                            foreach (string fileandDirectory in children)
                                retList.Add(fileandDirectory);
                            sReader.Close();
                            return (retList);
                        }
                    }

                }
                catch (Exception e)
                {

                    throw;
                }

            }
            */
        }

        public void createContainer(String URL, String token)
        {
            INetworkServiceCommand zdriveCommand = new zDriveServiceCommand(URL,
                                                                            null,
                                                                            token,
                                                                            ServiceCommandName.CreateContainer);
            var zDriveService = new zDriveService(token);
            zDriveService.excuteCommand(zdriveCommand);
        }


        public Boolean uploadFileStreamTozDrive(String fileContent, String fileName, String fileSize, String status, String startByte, String endByte, String MUrl, String token)
        {
            String url = MUrl;
            Boolean result = false;
            string dataString = "{\"metadata\":{},\"valuetransferencoding\":\"base64\",\"value\":\" " + fileContent + " \"}";
            var encoding = new ASCIIEncoding();
            byte[] body = encoding.GetBytes(dataString);
            //writeByteToFile(fileName,
            //                fileContent,
            //                startByte,
            //                endByte);
            switch (status)
            {
                case "start":
                    {
                        if (!url.EndsWith("/"))
                            url = String.Format("{0}/{1}", url,
                                                            fileName);
                        else
                            url = string.Format("{0}{1}",
                                                    url,
                                                    fileName);
                        using (var zDriService = new zDriveService(token))
                        {
                            zDriService.FileName = fileName;
                            zDriService.FileContent = fileContent;
                            result = zDriService.upload(url,
                                                         body);

                        }
                        break;
                    }
                case "progress":
                    {
                        if (!url.EndsWith("/"))
                            url = String.Format("{0}/{1}", url,
                                                            fileName);
                        else
                            url = string.Format("{0}{1}",
                                                    url,
                                                    fileName);
                        string updateURL = String.Format("{0}?value:{1}-{2}",
                                                                url,
                                                                startByte,
                                                                endByte);
                        using (var zDService = new zDriveService(token))
                        {
                            zDService.FileName = fileName;
                            zDService.FileContent = fileContent;
                            result = zDService.upload(updateURL,
                                                        body);
                        }
                        break;
                    }

            }
            /*
            //Thread.Sleep(3000);
            #region check uploaded file size
            String contentDescripttion = String.Format("attachment ; filename=\"{0}\"", fileName);
            using (var zDService = new zDriveService(token))
            {
                string metaUrl = string.Empty;
                if (!MUrl.EndsWith("/"))
                    metaUrl = String.Format("{0}/{1}", MUrl,
                                                    fileName);
                else
                    metaUrl = string.Format("{0}{1}",
                                            MUrl,
                                            fileName);

                metaUrl = String.Format("{0}?metadata",
                                             metaUrl);
                var responseStream = zDService.fileMetadata(metaUrl);
                var strReader = new StreamReader(responseStream);
                String streamValue = strReader.ReadToEnd();
                strReader.Close();

                var JSONparser = JObject.Parse(streamValue);
                long zFileSize = Convert.ToInt64((String)JSONparser["metadata"]["cdmi_size"].ToString());
                byte[] fileContents = Convert.FromBase64String(fileContent);
                long sendFileSize = Convert.ToInt64(startByte) + fileContents.Length;

                while(zFileSize != sendFileSize)
                {
                    
                    responseStream = zDService.fileMetadata(metaUrl);
                    strReader = new StreamReader(responseStream);
                    streamValue = strReader.ReadToEnd();
                    strReader.Close();

                    JSONparser = JObject.Parse(streamValue);
                    zFileSize = Convert.ToInt64((String)JSONparser["metadata"]["cdmi_size"].ToString());
                    fileContents = Convert.FromBase64String(fileContent);
                    sendFileSize = Convert.ToInt64(startByte) + fileContents.Length;
                }

               
            }
            
            #endregion
            */

            return (result);
        }

        private void writeByteToFile(String fileName, String fileContent, String startBytes, String endBytes)
        {
            try
            {
                using (var fileStream = new FileStream(HttpContext.Current.Server.MapPath("/TempDownload/") + fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
                using (var binaryWriter = new BinaryWriter(fileStream))
                //using (var file = new StreamWriter(HttpContext.Current.Server.MapPath("/TempDownload/") + "fileSize.txt"))
                {

                    byte[] fContent = Convert.FromBase64String(fileContent);
                    int startByte = Convert.ToInt32(startBytes);
                    int endbyte = Convert.ToInt32(endBytes);
                    /*
                    binaryWriter.BaseStream.Write(fContent,
                                                  (int)binaryWriter.Seek(startByte, SeekOrigin.End) ,
                                                  fContent.Length);
                    //file.Flush();                    

                    /*
                    if (HttpContext.Current.Session["fileSize"] != null)
                    {
                        var fileSize = Convert.ToInt64(HttpContext.Current.Session["fileSize"].ToString());
                        var contentSize = fContent.Length;
                        fileSize = fileSize + contentSize;
                        HttpContext.Current.Session["fileSize"] = fileSize.ToString();
                        file.Write(fileSize.ToString());
                    }
                    else
                    {
                        HttpContext.Current.Session["fileSize"] = fContent.Length.ToString();
                        file.Write(fContent.Length.ToString());
                    }
                    
                    binaryWriter.Flush();
                    binaryWriter.Close();
                    */


                    fileStream.Seek(startByte, SeekOrigin.Begin);
                    fileStream.Write(fContent,
                                        0,
                                        fContent.Length);
                    fileStream.Flush();
                    fileStream.Close();

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public String downloadFile(String URL, String token, String userID)
        {
            try
            {
                var zdriveService = new zDriveService(token);
                var streamReader = zdriveService.download(URL);
                String streamValue = streamReader.ReadToEnd();

                var JSONparser = JObject.Parse(streamValue);
                String fileContent = (String)JSONparser["value"];
                String fileName = (String)JSONparser["objectName"];
                if ((!String.IsNullOrEmpty(fileContent)) &&
                    (!String.IsNullOrEmpty(fileName)))
                {
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Aplication-Service")))
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Application-Service"));
                    streamReader.Close();
                    String tempFileName = String.Format("{0}\\{2}",
                                                        HttpContext.Current.Server.MapPath("~/Application-Service"),
                                                        fileName);

                    using (Stream memStream = new FileStream(tempFileName, FileMode.Create, FileAccess.Write))
                    using (var cs = new CryptoStream(memStream, new FromBase64Transform(), CryptoStreamMode.Write))
                    using (var streamWriter = new StreamWriter(cs))
                    {

                        streamWriter.Write(fileContent);
                        streamWriter.Flush();
                        streamWriter.Close();

                        return (tempFileName);
                    }
                }
                else
                { return (string.Empty); }



            }
            catch (Exception ex)
            {
                return (string.Empty);
            }
        }

        public Boolean downloadFileStream(HttpContextBase context, String URL, String token, String userID)
        {
            Boolean result = false;

            if (!URL.EndsWith("?value"))
                URL = URL + "?value";
            String[] splitedURL = URL.Split('/');
            String fileName = splitedURL[splitedURL.Length - 1];
            fileName = fileName.Replace("?value",
                                        "");

            String contentDescripttion = String.Format("attechment ; filename=\"{0}\"", fileName);
            int buffLength = 1024;
            int byteRead;
            byte[] byteBuffer = new byte[buffLength];
            var zdriveService = new zDriveService(token);


            var responseStream = zdriveService.fileMetadata(URL.Replace("value", "metadata"));
            var strReader = new StreamReader(responseStream);
            String streamValue = strReader.ReadToEnd();
            strReader.Close();

            var JSONparser = JObject.Parse(streamValue);
            String fileSize = (String)JSONparser["metadata"]["cdmi_size"];
            context.Response.ClearContent();
            context.Response.ClearHeaders();
            context.Response.ContentType = "application/octet-stream";
            context.Response.AddHeader("Content-Disposition", contentDescripttion);
            context.Response.AddHeader("Content-Description", "File Transfer");
            context.Response.AddHeader("Content-Transfer-Encoding", "binary");
            context.Response.AddHeader("Content-Length", fileSize);


            using (var streamReader = zdriveService.download(URL))
            {
                char[] tempChar = new char[10];
                char[] readCharByte = new char[buffLength];
                streamReader.ReadBlock(tempChar, 0, tempChar.Length);
                while ((byteRead = streamReader.ReadBlock(readCharByte, 0, readCharByte.Length)) > 0)
                {
                    String decodeStr = new String(readCharByte);
                    try
                    {
                        char[] c = { '\0', '"', '}' };
                        String fileContent = decodeStr.Trim(c);
                        var base64EncodedBytes = System.Convert.FromBase64String(fileContent);
                        context.Response.BinaryWrite(base64EncodedBytes);
                        context.Response.Flush();

                        readCharByte = new char[buffLength];

                    }
                    catch (Exception ex)
                    {
                        byteBuffer = Convert.FromBase64CharArray(readCharByte,
                                                                    0,
                                                                    byteRead - 1);
                        context.Response.Flush();
                    }
                    result = true;
                }
                context.Response.Close();
            }
            //}
            return (result);
        }

        public void deleteContainer(String URL, String token, String userID)
        {
            INetworkServiceCommand zdriveCommand = new zDriveServiceCommand(URL,
                                                                            null,
                                                                            token,
                                                                            ServiceCommandName.DeleteContainer);
            var zDriveService = new zDriveService(token);
            zDriveService.excuteCommand(zdriveCommand);
        }

        public void deleteFile(String URL, String token, String userID)
        {
            INetworkServiceCommand zdriveCommand = new zDriveServiceCommand(URL,
                                                                            null,
                                                                            token,
                                                                            ServiceCommandName.DeleteFile);
            var zDriveService = new zDriveService(token);
            zDriveService.excuteCommand(zdriveCommand);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
