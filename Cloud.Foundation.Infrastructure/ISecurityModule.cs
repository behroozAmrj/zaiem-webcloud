using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Foundation.Infrastructure
{
    public interface ISecurityModule
    {


        object Autenticate(string userName, string password, string[] optionalParams);
        void SaveCredentials(string userID, params string[] additionalParams);

        object RetrieveCredentials(string userID);
    }
}
