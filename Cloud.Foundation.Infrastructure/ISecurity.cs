using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Foundation.Infrastructure
{
    public interface ISecurity
    {
        object[] Authenticate(string userName, string passWord, string[] options);
        void Register(String userName, String password , String[] options);
    }
}
