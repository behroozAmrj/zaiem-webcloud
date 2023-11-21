using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Cloud.IU.WEB.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGateWayService" in both code and config file together.
    [ServiceContract]
    public interface IGateWayService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        object Execute_Method(string AppName , string ClassName , string MethodName , List<object> Params);




        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        List<object> CloudService_CallMethod(string AppName, string ServiceName, string ClassName, string MethodName, List<object> Params);

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        object Ping();

          [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        object Call(string AppName, string ServiceName, string ClassName, string MethodName, List<object> Params);

    }
}
