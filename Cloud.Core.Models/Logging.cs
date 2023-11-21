using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Cloud.Core.Models
{
    public static class Logging
    {
        static ILog log;
        public static void SimpleINOFlogRegister(string Event)
        {
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Info(Event);
        }

        public static void INFOlogRegistrer(string Event, string UserID  , Type Type)
        {
            if ((string.IsNullOrEmpty(UserID)) ||
                (Type == null))
                SimpleINOFlogRegister(Event);
            else
            {
                string statement = string.Format("[{0}] {1}",
                                                    UserID,
                                                    Event);
                log = LogManager.GetLogger(Type);
                log.Info(statement);
            }
        }


        public static void SimpleErrorlogRegister(string Event)
        {
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Error(Event);
        }

        public static void ErrorlogRegistrer(string Event, string UserID, Type Type)
        {
            if ((string.IsNullOrEmpty(UserID)) ||
                (Type == null))
                SimpleErrorlogRegister(Event);
            else
            {
                string statement = string.Format("[{0}] {1}",
                                                    UserID,
                                                    Event);
                log = LogManager.GetLogger(Type);
                log.Error(statement);
            }
        }


        public static void SimpleWarringRegister(string Event)
        {
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            log.Warn(Event);
        }

    }
}