using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Foundation.Infrastructure
{
    public enum ServiceCommandName
    {
        CreateContainer = 0,
        RenameContainer =1,
        RenameFile = 2,
        DeleteContainer = 3,
        DeleteFile = 4,
        //zStore method name
        zStoreNewAppliance = 5,
        zStoreDeleteAppliance = 6,
    }
}
