using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Cloud.IU.WEB.Models
{
    public class CloudIPV4 : ICloudIP, Cloud.IU.WEB.Models.ICloudIPV4
    {
        private string pattern = @"^"; 
        private string ip;
         public CloudIPV4(string IP)
         {
             if (string.IsNullOrEmpty(IP))
             {
                 this.ip = "127.0.0.1";
             }
             else
             {
                 this.ip = IP;
             }
         }
        public string IP
        {
            get
            {
                return(this.ip);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception ("مقدار  آدرس ابر وارد نشده");
                }
                else
                {
                    this.ip = value;
                }
            }
        }

        public ICloudIP ICloudIP
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        private void IPValidateReg(string IP)
        {
           
        }
    }
}