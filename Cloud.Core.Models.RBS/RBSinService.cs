using Cloud.Foundation.Infrastructure;
using Cloud.IO.NetworkService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models.RBS
{
    public class RBSinService
    {
        string appname;
        string classname;
        string method;
        List<object> constructorparams;
        List<object> param;
        string directory;
        string applicationpath;
        String userID;

        public string ApplicationPath
        {
            get { return applicationpath; }
            set { applicationpath = value; }
        }

        public string Directory
        {
            get { return directory; }
        }
        public string AppName
        {
            get { return appname; }
        }


        public string Method
        {
            get { return method; }

        }
        public string ClassName
        {
            get { return classname; }

        }

        public List<object> Param
        {
            get { return param; }
            set { this.param = value; }
        }

        public List<object> ConstructorParams
        { get { return (this.constructorparams); } set { this.constructorparams = value; } }
        public RBSinService(string AppName, string ClassName, string Method , String UserID )
        {
            if ((string.IsNullOrEmpty(AppName)) &&
                (string.IsNullOrEmpty(ClassName)) &&
                (string.IsNullOrEmpty(Method)) &&
                (!String.IsNullOrEmpty(UserID)))
                throw new Exception("[Server].[SDKManager].[SDKManager] => input value are null or empty ");
            
                this.appname = AppName;
                this.classname = ClassName;
                this.method = Method;
                this.userID = UserID;

        }

        public object ExecuteMethod()
        {
            var proxMessage = new ProxyMessage();
            proxMessage.DeptURL = "webcloud";
            proxMessage.DestinationURL = "RBSManagement";
            proxMessage.Method = string.Format("SDK/ExecuteRBS/{0}",
                                                this.userID);
            string structJson;
            if ((this.constructorparams == null) ||
                (this.constructorparams.Count == 0))
                structJson = JsonConvert.SerializeObject(new List<object>());
            else
                structJson = JsonConvert.SerializeObject(this.constructorparams);
            string pararmsJson = JsonConvert.SerializeObject(this.param);
            string content = string.Format("{{\"AppName\":\"{0}\",\"ClassName\":\"{1}\",\"MethodName\":\"{2}\",\"contructParams\":{3},\"Param\":{4}}}",
                                                this.appname,
                                                this.classname,
                                                this.method,
                                                structJson,
                                                pararmsJson);
            proxMessage.Content = content;
            string proxyURL = string.Format("{0}/PostMsgCallBack/{1}", 
                                             ConfigurationManager.AppSettings["proxyService"],
                                             this.userID);
            var proxService = new ProxyService(proxyURL);
            using(var stream = proxService.SendBack(proxMessage))
            {
                if (stream == null)
                    return (string.Empty);
                var sReader = new StreamReader(stream);
                var result = sReader.ReadToEnd();
                stream.Close();
                var obje = JsonConvert.DeserializeObject(result);
                return (obje);
            }
        }
        
    }
}
