using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud.Core.Models
{
    public class CloudIP : ICloudIP
    {
        private string ip;
        public CloudIP(string IP)
        {
            if (string.IsNullOrEmpty(IP))
            {
                throw new Exception("مقدار  آدرس ابر وارد نشده");
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
    }
}