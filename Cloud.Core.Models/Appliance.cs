using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    public class Appliance
    {
        public String ID { get; set; }
        public String Name { get; set; }
        public String Owner { get; set; }
        public String Description { get; set; }
        public String Update { get; set; }
        public String Tenant { get; set; }
        public String[] Requirment { get; set; }
        public Boolean Public { get; set; }
    }
}
