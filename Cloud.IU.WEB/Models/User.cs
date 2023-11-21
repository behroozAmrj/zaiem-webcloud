using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud.IU.WEB.Models
{
    public class User
    {
        private string username;
        private string password;
        private string groupname;
        private ICloudIP cloudip;
        
        public string UserName 
        {
            get { return (username); } 
            set 
            { 
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("نام کاربر وارد نشده");
                }
                else
                {
                    this.username = value;
                }
            } 
        }


        public string Password 
        {
            get { return (password); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("کلمه عبور وارد نشده");
                }
                else
                {
                    this.password = value;
                }
            }
        }


        public string GroupName 
        {
            get { return (groupname);}
            set 
            { 
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("نام گروه وارد نشده");
                }
                else
                {
                    this.groupname = value;
                }
            }
        }

        public ICloudIP CloudIP 
        {
            get 
            {
                return (cloudip);
            }
            set 
            {
                this.cloudip = value;
            }
        }


    }
}