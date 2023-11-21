using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.IO.DataAccess.Transactions
{
    public class TransactionRegister : IDisposable
    {
        public void LogRegister(LogInfo Log)
        {
            using (var transactionCnx = new TransactionDBDataContext())
            {
                transactionCnx.LogRegister(Log.AppName,
                                            Log.DateTime,
                                            Log.User_ID,
                                            Log.LogTrace,
                                            Log.Action,
                                            Log.ActionType,
                                            Log.Content);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
