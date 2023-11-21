using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Foundation.Infrastructure
{
    // the subclasses will declare in models layer and then we can access to UserID from SessionID
    // and on developing must inherit from Exception class to catch in try catch block
    public abstract class CustomException : Exception
    {
        protected String msg;
        protected String userId;
        public CustomException(String UserId, String message)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                this.msg = message;
                this.msg = String.Format("userID[{0}] {1}",
                                            userId,
                                            this.msg);
            }
            else
            {
                this.msg = string.Empty;
            }
        }
    }

}
