using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBSManagementService.Models
{
    public class RBS
    {
        public string Name { get; set; }
        public string source { get; set; }
        public string version { get; set; }
        public Boolean Enable { get; set; }
        public string FirstPage { get; set; }
        public string  Title { get; set; }
        public string Image { get; set; }
        public int Type { get; set; }
    }
}