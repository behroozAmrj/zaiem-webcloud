
using Cloud.Core.Models;
using Cloud.Log.Tracking;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cloud.IU.WEB.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GateWayService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GateWayService.svc or GateWayService.svc.cs at the Solution Explorer and start debugging.
    public class GateWayService : IGateWayService
    {
        string callerApp;
        string requestApp;
        private Boolean IsAPIRequest(string AppName)
        {
            var result = false;
            var strarray = AppName.Split('/');
            if (strarray.Length > 1)
            {
                callerApp = strarray[0];
                requestApp = strarray[1];
                result = true;
            }
            return (result);
        }

        public object Execute_Method(string AppName, string ClassName, string MethodName, List<object> Params)
        {
            if ((string.IsNullOrEmpty(AppName)) &&
                 (string.IsNullOrEmpty(ClassName)) &&
                 (string.IsNullOrEmpty(MethodName)))
            {
                return (DateTime.Now.ToShortTimeString());
            }

            if (IsAPIRequest(AppName))
            {
                var sdkproperty = new SDKProperty();
                sdkproperty.ApplicationName = this.requestApp;
                sdkproperty.Class = ClassName;
                sdkproperty.Method = MethodName;
                sdkproperty.Parameters = Params;
                var apiservice = new APIManagementService();
                apiservice.ClientApp = callerApp;
                apiservice.Property = sdkproperty;
                var result = apiservice.Execute_Method(sdkproperty);
                return(result);
            }

            else
            {
                var type = MethodBase.GetCurrentMethod().DeclaringType;
                string msg = string.Empty;
                string logmethod = MethodName;
                if (Params != null)
                if (Params.Count > 0)
                {
                    string resultformat = GenerateFormat(Params);
                    var arr = Params.ToArray();
                    string res = string.Format(resultformat,
                                                arr);
                    logmethod = string.Format("{0}  {1}",
                                                  logmethod,
                                                  res);
                }
                msg = string.Format("[Execute_Method] {0} {1} {2} ",
                                           AppName,
                                           ClassName,
                                           logmethod);

                Logging.SimpleINOFlogRegister(msg);
                var sdkmgr = new SDKManager(AppName,
                                            ClassName,
                                            MethodName);
                sdkmgr.Param = Params;
                var result = sdkmgr.MethodExecute();
                if (result != null)
                    return (result);
                return (DateTime.Now.ToShortTimeString());
            }
            
        }

        public object CallMethod(string AppName, string ServiceName, string ClassName, string MethodName, List<object> Params)
        {

            if ((string.IsNullOrEmpty(AppName)) &&
               (string.IsNullOrEmpty(ClassName)) &&
               (string.IsNullOrEmpty(MethodName)))
            {
                Logging.ErrorlogRegister("parametrs are null or empty" ,
                                            MethodBase.GetCurrentMethod().GetType(),
                                            null);
                return (DateTime.Now.ToShortTimeString());

            }

            var serviceing = new CloudServiceRequestCenter(AppName);
            serviceing.ClassName = ClassName;
            serviceing.Method = MethodName;
            serviceing.Param = Params;
            serviceing.Servicename = ServiceName;
            var result = serviceing.CallServiceMethod();
            return (result);
        }


        public object Ping()
        {

            var dt = DateTime.Now.ToShortTimeString();
            return (dt);
        }

        private string GenerateFormat(List<object> Params)
        {
            var lenght = Params.Count;
            string result = string.Empty;
            string tresult = string.Empty;
            for (int i = 0; i < lenght; i++)
            {
                tresult += " [{" + i.ToString() + "}] ";
            }
            result = tresult;
            return (result);
        }

        public object Call(string AppName, string ServiceName, string ClassName, string MethodName, List<object> Params)
        {
            
            return (DateTime.Now.ToShortTimeString());
        }


        public List<object> CloudService_CallMethod(string AppName, string ServiceName, string ClassName, string MethodName, List<object> Params)
        {
            if ((string.IsNullOrEmpty(AppName)) &&
                (string.IsNullOrEmpty(ClassName)) &&
                (string.IsNullOrEmpty(MethodName)))
            {
                Logging.ErrorlogRegister("parametrs are null or empty",
                                            MethodBase.GetCurrentMethod().GetType(),
                                            null);
                object obj = new object();
                obj = DateTime.Now.ToShortTimeString(); 
                var olist  = new List<object>();
                olist.Add(obj);
                return (olist);

            }

            var serviceing = new CloudServiceRequestCenter(AppName);
            serviceing.ClassName = ClassName;
            serviceing.Method = MethodName;
            serviceing.Param = Params;
            serviceing.Servicename = ServiceName;
            var outputResult =  serviceing.CallServiceMethod();
            var result = new List<object>( outputResult as IList<object>);
            return (result);
        }
    }

}


//// BEGIN : HardCode Section    
// var path = AppDomain.CurrentDomain.BaseDirectory + "Application-Service\\NotePad\\Bin\\NotePads.dll";
// if (File.Exists(path))
// {
//     //SECTION #1 being Used as default                
//     var plugin = Assembly.LoadFile(path);
//     var instance = plugin.GetTypes().Where(x => x.Name == ClassName).First();
//     var obj = Activator.CreateInstance(instance,
//                                         new object[] { });
//     var result = obj.GetType().GetMethod(MethodName).Invoke(obj,
//                                                           new object[] { Params });

//     //SECTION #2 Unload dlls after used but it just works at the same framework
//     //var dom = AppDomain.CreateDomain("rundll");
//     //var assemname = AssemblyName.GetAssemblyName(path) ;
//     ////assemname.CodeBase = path;
//     //var plugin = dom.Load(assemname);
//     //var instance = plugin.GetTypes().Where(x => x.Name == ClassName).First();
//     //var obj = Activator.CreateInstance(instance,
//     //                                new object[]{});
//     //var result = obj.GetType().GetMethod(MethodName).Invoke(obj,
//     //                                                        new object[] {Params });
//     //AppDomain.Unload(dom);

// }
////END 