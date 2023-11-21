using Cloud.Foundation.Infrastructure;
using Cloud.Log.Tracking;
using net.openstack.Core.Domain;
using net.openstack.Providers.Zstack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Cloud.Core.Models
{
    public class LoginSecurity_v1 : ISecurity
    {
        object[] permission;
        public String Token { get; private set; }
        public LoginSecurity_v1()
        {
            this.permission = null;
        }

        public String Authenticates(string userName, string passWord, string[] options)
        {
            string permission = string.Empty;

            if ((String.IsNullOrWhiteSpace(userName)) ||
                (String.IsNullOrWhiteSpace(passWord)))
                throw new IOValueException("", "security.authenticate => paramers are null or empty");
            string userId = String.Empty;
            var cloudIdentity = new CloudIdentity();
            cloudIdentity.Username = userName;
            cloudIdentity.Password = passWord;
            String clouduserName = options[0];
            String cloudPassword = options[1];
            String sessionId = options[2];
            try
            {
                //Configuration configWeb = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                //configWeb.AppSettings.Settings.Remove("defaultIP");
                //configWeb.AppSettings.Settings.Add("defaultIP", "172.18.23.248");
                //configWeb.Save();

                //string confPath = System.AppDomain.CurrentDomain.RelativeSearchPath + /*Assembly.GetExecutingAssembly().FullName*/
                //    @"\" + HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name
                //    + @".dll.config";
                //Configuration config = ConfigurationManager.OpenExeConfiguration(confPath);//ConfigurationUserLevel.None);

                //ConfigurationManager.RefreshSection("appSettings");

                //config.AppSettings.Settings["defaultIP"].Value = "172.18.23.249";

                //config.Save(ConfigurationSaveMode.Modified, true);
                //ConfigurationManager.RefreshSection("appSettings");

                var identityProvider = new CloudIdentityProvider();
                var userAccess = identityProvider.Authenticate(cloudIdentity);

                IEnumerable<Tenant> tmpTenant = identityProvider.ListTenants(cloudIdentity);
                if (tmpTenant.Count() > 0)
                {
                    cloudIdentity.TenantName = tmpTenant.First().Name;
                    userAccess = identityProvider.Authenticate(cloudIdentity);
                    userId = userAccess.User.Id;

                    var serverProvider = new CloudServersProvider();

                    //var networkProvider = new CloudNetworksProvider(cloudIdentity);
                    //var list = networkProvider.ListNetworks();

                    var serversList = serverProvider.ListServersWithDetails(cloudIdentity).ToList();
                    var flavorsList = serverProvider.ListFlavorsWithDetails(null, null, null, null, null, cloudIdentity).ToList();
                    var imagesList = serverProvider.ListImagesWithDetails(null, null, null, null, null, null, null, null, cloudIdentity).ToList();

                    String url = "http://172.18.23.218";

                    String[] resultCredential = zDriveAuthenticateAndGetToken(url,
                                                                clouduserName,
                                                                cloudPassword);
                    if ((resultCredential == null) ||
                        (resultCredential.Length != 3))
                        throw new Exception("the user credential is invalid ");
                    String token = resultCredential[0];
                    String endTime = resultCredential[1];
                    String role = resultCredential[2];


                    Cloud.Core.Models.STRepository.StrRepository.setUserCredentialToken(userId,
                                                                                        userName,
                                                                                        passWord,
                                                                                        tmpTenant.First().Name,
                                                                                        token,
                                                                                        userAccess.Token.Id,
                                                                                        userAccess.Token.Tenant.Id,
                                                                                        String.Format("{0}:8080/WebTest3/storage/{1}/", url, clouduserName),
                                                                                        Convert.ToDateTime(endTime),
                                                                                        role);

                    Cloud.Core.Models.STRepository.StrRepository.FillupServer(serversList,
                                                                                  userId);
                    Cloud.Core.Models.STRepository.StrRepository.FillUpZImage(imagesList,
                                                                                userId);
                    Cloud.Core.Models.STRepository.StrRepository.FillUp_ZFlavor(flavorsList,
                                                                                userId);
                    //Cloud.Core.Models.STRepository.StrRepository.insertUserAndSession(userId,
                    //                                                                   sessionId);


                    var pointer = Cloud.Core.Models.STRepository.StrRepository;

                    //Logging.INFOlogRegistrer(string.Format("login accepted [{0}] ",
                    //                           cloudIdentity.Username),
                    //                           userId,
                    //                           MethodBase.GetCurrentMethod().DeclaringType);
                    permission = userId;
                }
            }
            catch (Exception e)
            {

                permission = string.Empty;
                throw new IOValueException(sessionId,
                                            String.Format("LoginSecurity.Authenticate failed due this error:{0},{1}",
                                                            e.Message,
                                                            e.StackTrace));
            }
            return (permission);
        }


        //public object[] Authenticate(string userName, string passWord, string[] options)
        //{
        //   AutoResetEvent permission = new AutoResetEvent(false);

        //    if ((String.IsNullOrWhiteSpace(userName)) ||
        //        (String.IsNullOrWhiteSpace(passWord)))
        //        throw new IOValueException("", "security.authenticate => paramers are null or empty");
        //    string userId = String.Empty;
        //    var cloudIdentity = new CloudIdentity();
        //    cloudIdentity.Username = userName;
        //    cloudIdentity.Password = passWord;
        //    String clouduserName = options[0];
        //    String cloudPassword = options[1];
        //    String sessionId = options[2];
        //    try
        //    {
        //        //Configuration configWeb = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
        //        //configWeb.AppSettings.Settings.Remove("defaultIP");
        //        //configWeb.AppSettings.Settings.Add("defaultIP", "172.18.23.248");
        //        //configWeb.Save();

        //        //string confPath = System.AppDomain.CurrentDomain.RelativeSearchPath + /*Assembly.GetExecutingAssembly().FullName*/
        //        //    @"\" + HttpContext.Current.ApplicationInstance.GetType().BaseType.Assembly.GetName().Name
        //        //    + @".dll.config";
        //        //Configuration config = ConfigurationManager.OpenExeConfiguration(confPath);//ConfigurationUserLevel.None);

        //        //ConfigurationManager.RefreshSection("appSettings");

        //        //config.AppSettings.Settings["defaultIP"].Value = "172.18.23.249";

        //        //config.Save(ConfigurationSaveMode.Modified, true);
        //        //ConfigurationManager.RefreshSection("appSettings");

        //        Task<string> userID = Task.Factory.StartNew(() => 
        //        {
        //            var identityProvider = new CloudIdentityProvider();
        //                var userAccess = identityProvider.Authenticate(cloudIdentity);

        //                IEnumerable<Tenant> tmpTenant = identityProvider.ListTenants(cloudIdentity);
        //            if (tmpTenant.Count() > 0)
        //            {
        //                cloudIdentity.TenantName = tmpTenant.First().Name;
        //                userAccess = identityProvider.Authenticate(cloudIdentity);
        //                userId = userAccess.User.Id;
        //                permission.Set();

        //                var serverProvider = new CloudServersProvider();

        //                //var networkProvider = new CloudNetworksProvider(cloudIdentity);
        //                //var list = networkProvider.ListNetworks();

        //                var serversList = serverProvider.ListServersWithDetails(cloudIdentity).ToList();
        //                var flavorsList = serverProvider.ListFlavorsWithDetails(null, null, null, null, null, cloudIdentity).ToList();
        //                var imagesList = serverProvider.ListImagesWithDetails(null, null, null, null, null, null, null, null, cloudIdentity).ToList();
        //                permission.WaitOne();
        //            }//end of IF
        //            return ("hello");
        //        });

        //        Task.Factory.StartNew(() => 
        //        {
        //            String URL = "http://172.18.23.218";

        //            String url = String.Format("{0}:35357/v2.0/tokens",
        //                                        URL);
        //            var result = new String[3];// String.Empty;
        //            String resultValue = String.Empty;
        //            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(url);
        //            wRequest.Timeout = 15000;
        //            wRequest.Method = "POST";
        //            wRequest.ContentType = "application/json";
        //            try
        //    {
        //        String tempURL = String.Format("{{\"auth\":{{ \"passwordCredentials\": {{\"username\":\"{0}\" , \"password\" : \"{1}\"}},\"tenanName\":\"public\"}}}}",
        //                                                                                                                                                         userName,
        //                                                                                                                                                         password);
        //        //String tempURL = String.Format("{'auth':{ 'passwordCredentials': {'username:{0}' , 'password' : '{1}'},'tenanName':'public'}}", userName , password );
        //        var encoding = new ASCIIEncoding();
        //        byte[] byteStream = encoding.GetBytes(tempURL);// (requestBody);
        //        wRequest.ContentLength = byteStream.Length;
        //        var rStream = wRequest.GetRequestStream();
        //        rStream.Write(byteStream,
        //                        0,
        //                        byteStream.Length);
        //        rStream.Close();

        //        using (var response = (HttpWebResponse)wRequest.GetResponse())
        //        {
        //            //if (!(response.StatusCode == HttpStatusCode.OK))
        //              //  return (null);


        //            using (var reponseStream = response.GetResponseStream())
        //            {
        //                using (var reader = new StreamReader(reponseStream))
        //                {
        //                    resultValue = reader.ReadToEnd();
        //                }
        //            }
        //            var parseTree = JsonConvert.DeserializeObject<JObject>(resultValue);
        //            var jo = JObject.Parse(resultValue);
        //            var eDate = Convert.ToDateTime((String)jo["access"]["token"]["expires"]);
        //            var sDate = Convert.ToDateTime((String)jo["access"]["token"]["issued_at"]);
        //            var data = eDate - sDate;
        //            var token = (String)jo["access"]["token"]["id"];// ["token"];
        //            response.Close();
        //            if (!String.IsNullOrWhiteSpace(token))
        //            {
        //                result[0] = token;
        //                result[1] = (String)jo["access"]["token"]["expires"];
        //                result[2] = (String)jo["access"]["token"]["role"] == null ? "admin" : null;
        //            }

        //        }


        //        }
        //        catch (WebException ex)
        //        {
        //            Console.WriteLine("no connection to Server please Try later");

        //        }


        //        });


        //            String[] resultCredential = zDriveAuthenticateAndGetToken(url,
        //                                                        clouduserName,
        //                                                        cloudPassword);
        //            if ((resultCredential == null) ||
        //                (resultCredential.Length != 3))
        //                throw new Exception("the user credential is invalid ");
        //            String token = resultCredential[0];
        //            String endTime = resultCredential[1];
        //            String role = resultCredential[2];


        //            Cloud.Core.Models.STRepository.StrRepository.setUserCredentialToken(userId,
        //                                                                                userName,
        //                                                                                passWord,
        //                //tmpTenant.First().Name,
        //                //token,
        //                //userAccess.Token.Id,
        //                //userAccess.Token.Tenant.Id,
        //                //String.Format("{0}:8080/WebTest3/storage/{1}/", url, clouduserName),
        //                //Convert.ToDateTime(endTime),
        //                                                                                role);

        //            permission = new object[] { userId, tmpTenant.First().Name, token, userAccess.Token.Id, userAccess.Token.Tenant.Id };
        //            Cloud.Core.Models.STRepository.StrRepository.FillupServer(serversList,
        //                                                                          userId);
        //            Cloud.Core.Models.STRepository.StrRepository.FillUpZImage(imagesList,
        //                                                                        userId);
        //            Cloud.Core.Models.STRepository.StrRepository.FillUp_ZFlavor(flavorsList,
        //                                                                        userId);
        //            Cloud.Core.Models.STRepository.StrRepository.insertUserAndSession(userId,
        //                                                                               sessionId);


        //            var pointer = Cloud.Core.Models.STRepository.StrRepository;

        //            Logging.INFOlogRegistrer(string.Format("login accepted [{0}] ",
        //                                       cloudIdentity.Username),
        //                                       userId,
        //                                       MethodBase.GetCurrentMethod().DeclaringType);
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        permission = null;
        //        throw new IOValueException(sessionId,
        //                                    String.Format("LoginSecurity.Authenticate failed due this error:{0},{1}",
        //                                                    e.Message,
        //                                                    e.StackTrace));
        //    }
        //    return (permission);
        //}


        public object[] Authenticate(string userName, string passWord, string[] options)
        {
            var mutualExclusive = new AutoResetEvent(false);
            var exitExclusive = new ManualResetEvent(false);

            string permission = string.Empty;

            if ((String.IsNullOrWhiteSpace(userName)) ||
                (String.IsNullOrWhiteSpace(passWord)))
                throw new IOValueException("", "security.authenticate => paramers are null or empty");
            string userId = String.Empty;
            var cloudIdentity = new CloudIdentity();
            cloudIdentity.Username = userName;
            cloudIdentity.Password = passWord;
            String clouduserName = options[0];
            String cloudPassword = options[1];
            String sessionId = options[2];
            string connectionID = options[3];

            String token = string.Empty;
            String endTime = string.Empty;
            String role = string.Empty;
            String URL = "http://172.18.23.218";


            // # in this task authenticate from os_sdk
            Task<string> userPermissionID = Task.Factory.StartNew(() =>
            {
                try
                {
                    var identityProvider = new CloudIdentityProvider();
                    var userAccess = identityProvider.Authenticate(cloudIdentity);

                    IEnumerable<Tenant> tmpTenant = identityProvider.ListTenants(cloudIdentity);
                    if (tmpTenant.Count() > 0)
                    {
                        cloudIdentity.TenantName = tmpTenant.First().Name;
                        userAccess = identityProvider.Authenticate(cloudIdentity);
                        userId = userAccess.User.Id;

                        var serverProvider = new CloudServersProvider();

                        //var networkProvider = new CloudNetworksProvider(cloudIdentity);
                        //var list = networkProvider.ListNetworks();

                        var serversList = serverProvider.ListServersWithDetails(cloudIdentity).ToList();
                        var flavorsList = serverProvider.ListFlavorsWithDetails(null, null, null, null, null, cloudIdentity).ToList();
                        var imagesList = serverProvider.ListImagesWithDetails(null, null, null, null, null, null, null, null, cloudIdentity).ToList();
                        mutualExclusive.WaitOne();//# this method waits until other thread finish their jobs

                        Task.Factory.StartNew(() =>
                        {
                            var repService = new RepositoryService.RepositoryClient();

                            repService.setUserCredentialToken(userId,
                                                              userName,
                                                              passWord,
                                                              tmpTenant.First().Name,
                                                              token,
                                                              userAccess.Token.Id,
                                                              userAccess.Token.Tenant.Id,
                                                              String.Format("{0}:8080/WebTest3/storage/{1}/", URL, clouduserName),
                                                              Convert.ToDateTime(endTime),
                                                              connectionID,
                                                              role);
                            repService.insertUserAndSession(userId,
                                                             sessionId);

                        });


                        Cloud.Core.Models.STRepository.StrRepository.setUserCredentialToken(userId,
                                                                                            userName,
                                                                                            passWord,
                                                                                            tmpTenant.First().Name,
                                                                                            token,
                                                                                            userAccess.Token.Id,
                                                                                            userAccess.Token.Tenant.Id,
                                                                                            String.Format("{0}:8080/WebTest3/storage/{1}/", URL, clouduserName),
                                                                                            Convert.ToDateTime(endTime),
                                                                                            role);



                        Cloud.Core.Models.STRepository.StrRepository.FillupServer(serversList,
                                                                                      userId);
                        Cloud.Core.Models.STRepository.StrRepository.FillUpZImage(imagesList,
                                                                                    userId);
                        Cloud.Core.Models.STRepository.StrRepository.FillUp_ZFlavor(flavorsList,
                                                                                    userId);
                        //Cloud.Core.Models.STRepository.StrRepository.insertUserAndSession(userId,
                        //                                                                   sessionId);
                        //Logging.INFOlogRegistrer(string.Format("login accepted [{0}] ",
                        //                           cloudIdentity.Username),
                        //                           userId,
                        //                           MethodBase.GetCurrentMethod().DeclaringType);

                        //exitExclusive.Set();
                        return (userId);

                    }
                    else
                        return (string.Empty);
                }
                catch (Exception ex)
                {
                    return (string.Empty);
                }
            });
            // # in this task authenticates to zDrive 
            Task.Factory.StartNew(() =>
            {

                String url = String.Format("{0}:35357/v2.0/tokens",
                                                URL);
                var result = new String[3];// String.Empty;
                String resultValue = String.Empty;
                HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(url);
                wRequest.Timeout = 15000;
                wRequest.Method = "POST";
                wRequest.ContentType = "application/json";

                try
                {
                    String tempURL = String.Format("{{\"auth\":{{ \"passwordCredentials\": {{\"username\":\"{0}\" , \"password\" : \"{1}\"}},\"tenanName\":\"public\"}}}}",
                                                                                                                                                                     clouduserName,
                                                                                                                                                                     cloudPassword);
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
                        if ((response.StatusCode == HttpStatusCode.OK))
                        {

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
                            var tokens = (String)jo["access"]["token"]["id"];// ["token"];
                            response.Close();
                            if (!String.IsNullOrWhiteSpace(tokens))
                            {
                                token = tokens;
                                endTime = (String)jo["access"]["token"]["expires"];
                                role = (String)jo["access"]["token"]["role"] == null ? "admin" : null;
                            }
                            mutualExclusive.Set();
                        }

                    }

                }
                catch (WebException ex)
                {
                    Console.WriteLine("no connection to Server please Try later");

                }
            });



            return (new object[] { userPermissionID.Result });
        }
        private String[] zDriveAuthenticateAndGetToken(String URL, String userName, String password)
        {

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

        public void LogOut(String userID)
        {
            //Cloud.Core.Models.STRepository.StrRepository.removeUser(userID);
            if (!string.IsNullOrEmpty(userID))
            {
                var repService = new RepositoryService.RepositoryClient();
                repService.removeUser(userID);
            }
        }

        public Boolean IsAlive(String userID)
        {
            Boolean result = false;
            var repService = new RepositoryService.RepositoryClient();
            var user = repService.getUser(userID);
            String token = user.Token; //Cloud.Core.Models.STRepository.StrRepository.getUserCredentialToken(userID);
            if (String.IsNullOrEmpty(token))
                result = false;
            else
            {
                result = true;
                this.Token = token;
            }
            return (result);
        }


        public void Register(string userName, string password, string[] options)
        {
            if (options.Length < 2)
                throw new Exception("one or more data are not completed");
            try
            {

                String email = options[0];
                String userID = options[1];
                var repSerivce = new RepositoryService.RepositoryClient();
                var userPro = repSerivce.getUser(userID);// Cloud.Core.Models.STRepository.StrRepository.getUserCredential(userID);
                var identity = new CloudIdentity();
                identity.Username = userPro.UserName;
                identity.Password = userPro.Password;
                identity.TenantName = userPro.UserTenant;

                var identityProvider = new CloudIdentityProvider();
                var newUser = new NewUser(userName,
                                            email,
                                            password,
                                            true);
                identityProvider.AddUser(newUser,
                                            identity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String getUserRole(String userID)
        {
            if (String.IsNullOrEmpty(userID))
                return ("user");
            var repService = new RepositoryService.RepositoryClient();
            var userPro = repService.getUser(userID);//STRepository.StrRepository.getUser(userID);
            if (userPro != null)
            {
                String role = userPro.Role;
                return (role);
            }
            else
            {
                return ("user");
            }
        }
    }
}
