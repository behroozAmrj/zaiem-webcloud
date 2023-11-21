using MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageServer.Domain
{
    public interface ISender
    {
        object Send(Message message);
    }
}
