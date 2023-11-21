using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Log.Tracking
{
    public class LogRecord
    {

        public String userID { get; set; }

        public String  type { get; set; }
        public String dateTime { get; set; }
        public String message { get; set; }
    }
}
