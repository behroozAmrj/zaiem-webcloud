using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RespositoryService
{
    [DataContract]
    public class RUserProSecurity  : IDisposable
    {
        private String _role = "user";
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public String Token { get; set; }
        [DataMember]
        public String StorageURL { get; set; }
        [DataMember]
        public String UserName { get; set; }
        [DataMember]
        public String Password { get; set; }
        [DataMember]
        public String UserTenant { get; set; }
        [DataMember]
        public String zStoreToken { get; set; }
        [DataMember]
        public String zStoreTenant { get; set; }
        [DataMember]
        public String ConnectionID { get; set; }
        [DataMember]
        public String Role
        {
            get
            { return (_role); }
            set
            {
                if ((!String.IsNullOrEmpty(value)) && (!String.IsNullOrWhiteSpace(value)))
                    this._role = value;
            }
        }

        public DateTime LoginTime { get { return (loginTime); } }
        private DateTime loginTime;




         public RUserProSecurity(DateTime? exTime = null)
        {
            /*
            timeoutChecker = new Timer(timeout);
            timeoutChecker.Elapsed += timeoutChecker_Elapsed;
            timeoutChecker.Enabled = true;
            timeoutChecker.Start();
            */
            if (exTime != null)
                this.loginTime = DateTime.Now;
        }

        
     

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}