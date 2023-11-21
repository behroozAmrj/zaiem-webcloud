using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MessageServer.Models
{
    public class Service 
    {
        public int ID { get; set; }
        public string serviceName { get; set; }
        public string URL { get; set; }
    }
    
}