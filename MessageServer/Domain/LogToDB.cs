using MessageServer.DataAccess;
using MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageServer.Domain
{
    public class LogToDB : IDisposable , ILogRegister
    {
        public void RegisterLog (LogInfo log)
        {
            using(var logCnx = new DataManageDBDataContext())
            {
                logCnx.LogRegister(log.AppName,
                                    log.DateTime,
                                    log.User_ID,
                                    log.LogTrace,
                                    log.Action,
                                    log.ActionType,
                                    log.Content);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}