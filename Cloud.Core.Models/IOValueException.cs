using Cloud.Foundation.Infrastructure;
using Cloud.Log.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    public class IOValueException : CustomException 
    {
        public IOValueException(String sessionId , String message) 
            :base(getUserID(sessionId), message)
        {
            Logging.ErrorlogRegister( this.msg , MethodBase.GetCurrentMethod().GetType() , getUserID(sessionId));
        }

        private static string getUserID(string sessionId)
        {
            var repService = new RepositoryService.RepositoryClient();

            String resuly = string.Empty;
            String userID = repService.getUserBasedOnSessionID(sessionId);// STRepository.StrRepository.getUserBasedOnSessionID(sessionId);
            if (!String.IsNullOrWhiteSpace(userID))
                resuly = userID;
            return (resuly);
        }
    }
}
