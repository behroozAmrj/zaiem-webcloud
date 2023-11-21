using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBSManagementService.Models
{
    public class RbsRequestInfo
    {
        public string  AppName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public List<object> contructParams { get; set; }
        public List<object> param { get; set; }
    }
}