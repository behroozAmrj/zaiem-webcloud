using Cloud.Core.Models;
using Cloud.IU.WEB.InfraSructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cloud.IU.WEB.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAPIManagementService" in both code and config file together.
    [ServiceContract]
    public interface IAPIManagementService
    {
       
        //[OperationContract]
        //object ExecuteMethod();
        [OperationContract]
        object Execute_Method(SDKProperty Property);

    }
}
