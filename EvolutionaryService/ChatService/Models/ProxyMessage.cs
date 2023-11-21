using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatService.Models
{
    public class ProxyMessage
    {
        public string DeptURL;
        public string DestinationURL;
        public string Method;
        public DateTime DateTime;
        public string Content;
    }
}