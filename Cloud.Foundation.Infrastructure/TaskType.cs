using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud.IU.WEB.InfraSructure
{
    public enum TaskType
    {
        Create = 0,
        Delete = 1,
        PowerOn = 2,
        ShutDown = 3,
        Restart = 4,
        Pause = 5
    }
}