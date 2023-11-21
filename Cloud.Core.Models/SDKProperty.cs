using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Cloud.Core.Models
{
    [DataContract]
    public class SDKProperty : ISDKProperties
    {
        string application;
        string classname;
        string method;
        List<object> param;
        [DataMember]
        public string ApplicationName
        {
            get { return (application); }
            set { this.application = value; }
        }
        [DataMember]
        public string Class
        {
            get { return (this.classname); }
            set { this.classname = value; }
        }
        [DataMember]

        public string Method
        {
            get { return (method); }
            set { this.method = value; }
        }
        [DataMember]
        public List<object> Parameters
        {
            get { return (this.param); }
            set { this.param = value; }
        }
    }
}