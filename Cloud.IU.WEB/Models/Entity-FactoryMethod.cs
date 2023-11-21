using Cloud.Foundation.Infrastructure;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloud.IU.WEB.Models
{
    public class Entity_FactoryMethod
    {
        public static IEntity Method(EntityType Type, string Handler, string UserID)
        {
            switch (Type)
            {
                case EntityType.Machine:
                    {
                        return (new Machine(Handler,
                                             UserID)); break;
                    }
                case EntityType.Text: { return (new TItem()); break; }
                default:
                    {
                        return (new Machine(Handler,
                                              UserID));
                    }
            }
        }




        public static IEntity Method(object[] Parameters)
        {

            var type = (EntityType)Parameters[0];
            if (type == null)
                throw new Exception("type is null");
            switch (type)
            {
                case EntityType.Machine:
                    {


                        if (Parameters.Count() > 1)
                        {
                            var handler = (string)Parameters[1];
                            var userid = (string)Parameters[2];
                            var sessionid = (string)Parameters[3];
                            var machine = new Machine(handler,
                                                        userid);
                            machine.SessionID = sessionid;
                            return (machine); break;
                        }
                        else
                        {
                            var machine = new Machine();
                            return (machine); break;

                        }
                    }
                case EntityType.Text: { return (new TItem()); break; }
                default:
                    {
                        return (null);
                    }
            }
        }
    }
}