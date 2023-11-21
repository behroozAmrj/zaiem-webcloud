using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.IO.DataAccess.Transactions
{
    public class LogInfo
    {
        public string AppName { get; set; }
        public DateTime DateTime { get; set; }
        public string User_ID { get; set; }

        public string LogTrace { get; set; }
        public string Action { get; set; }
        public string ActionType { get; set; }
        public string Content { get; set; }
    }
}
