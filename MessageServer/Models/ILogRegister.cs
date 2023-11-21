using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageServer.Models
{
    internal interface ILogRegister
    {
        void RegisterLog(LogInfo log);
    }
}
