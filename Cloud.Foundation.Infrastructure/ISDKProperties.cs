using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.IU.WEB.InfraSructure
{
    public interface ISDKProperties
    {
        string ApplicationName { get; set; }
        string Class { get; set; }
        string Method { get; set; }
        List<object> Parameters { get; set; }
    }
}
