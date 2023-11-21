using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Foundation.Infrastructure
{
    public interface IServer
    {
        Boolean Connect(String URL ,object[] param = null);
        object  RetrieveContent(String Address, object[] param = null);
    }
}
