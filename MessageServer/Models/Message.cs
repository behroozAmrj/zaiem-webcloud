using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MessageServer.Models
{
    [Serializable]
    public class Message
    {
        public string DeptURL;
        public string DestinationURL;
        public string Method;
        public DateTime DateTime;
        public string Content;
        
    }
}