using net.openstack.Core.Domain;
//using CRC_SDK.Classes;
//using CRC_SDK.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.IU.WEB.InfraSructure
{
    public interface IMachine
    {
        Server zServerMachine { get; set; }
    }
}
