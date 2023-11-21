using RBSManagementService.Filing;
using RBSManagementService.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;

namespace RBSManagementService.LojicModel
{
    public class SDKExecuter
    {
        private string appname;
        private string classname;
        private string method;
        private string source;
        private string userID;
        private IExecuter executer;
        List<object> constructorparams;
        public List<object> param;
        public List<object> ConstructorParams
        { get { return (this.constructorparams); } set { this.constructorparams = value; } }

        public SDKExecuter(string AppName, string ClassName, string Method, String UserID )
        {
            if (!String.IsNullOrEmpty(UserID))
                this.userID = UserID;
            if ((string.IsNullOrEmpty(AppName)) &&
                (string.IsNullOrEmpty(ClassName)) &&
                (string.IsNullOrEmpty(Method)))

                throw new Exception("[Server].[SDKManager].[SDKManager] => input value are null or empty ");

            this.appname = AppName;
            this.classname = ClassName;
            this.method = Method;
            this.source = source;

            //var appcon = appconfig.ReadOne("//root//applications//application[@name='" + appname + "']");

            //if (appcon == null)
            //    throw new Exception("[Server].[SDKManager].[SDKManager] => application not found in AppsConfig ");

            //this.source = appcon.Directory;
            //this.applicationpath = string.Format("{0}{1}\\{2}",
            //                                      AppDomain.CurrentDomain.BaseDirectory,
            //                                      "Application-Service",
            //                                      this.directory);
            string path = HttpContext.Current.Server.MapPath("/Data/") + "AppData.xml";
            var xmlStorage = new XMLStorage(path);
            var rbs = xmlStorage.SelectAppByUserID(this.appname,
                                                    this.userID);
            this.source = rbs.source;
            if (rbs == null)
                throw new Exception("RBS not found in storage");
            executer = FactoryMethod_Executer.FactoryMethod(rbs);

        }


        public object MethodExecute()
        {

            try
            {

                //var sdkconfig = new SDKConfig(this.source);

                var sdkconfig = new SDKConfig(this.source);
                string xpath = "//applicationConfig//APIS//lib";
                var libList = sdkconfig.Reads(xpath,
                                              "filename");

                if (libList == null)
                    return (null);
                var instance = SelectClassInAssembly(libList,
                                                    this.classname);
                if (instance == null)
                    throw new Exception("[Server].[GateWayService].[ExuteMethod] => Class was not found in dlls ");

                var consinfo = instance.GetConstructor(System.Type.EmptyTypes).GetParameters();

                object obj;
                if ((consinfo != null) &&
                    (consinfo.Count() > 0))
                    obj = Activator.CreateInstance(instance,
                                                        new object[] { this.constructorparams });
                else
                    obj = Activator.CreateInstance(instance,
                                                    new object[] { });
                object result;
                try
                {
                    var re = obj.GetType().GetMethod(this.method).GetParameters();
                    var methodinfo = SelectMethodinClass(obj.GetType(),
                                                            this.method);
                    if (methodinfo == null)
                        throw new Exception("[Server].[GateWayService].[ExecuteMethod] => method is not exist in class ");
                    if ((re != null) &&
                            (re.Count() > 0))

                        result = methodinfo.Invoke(obj,
                                                    new object[] { this.param });

                    else
                        result = methodinfo.Invoke(obj,
                                                    new object[] { });

                    return (result);
                }
                catch (Exception ex)
                {
                    string msg = string.Format("[Server].[SDKManager].[ExecuteMethod] => execution has been faild on Invoke Method by this error : {0} ",
                                                    ex.Message);
                   
                    //throw new Exception(msg);
                    return (null);
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("[Server].[SDKManager].[ExecuteMethod] => execution has been stop by this error {0} ",
                                                    ex.Message);
               
                //throw new Exception(msg);
                return (null);
            }

        }


        private Type SelectClassInAssembly(List<SDKConfig> LibList, string ClassName)
        {
            foreach (var item in LibList)
            {
                try
                {
                    var plugin = Assembly.LoadFile(item.Value);
                    var instance = plugin.GetTypes().Where(x => x.Name == this.classname).First();
                    return (instance);
                }
                catch
                { }

            }
            return (null);
        }

        private MethodInfo SelectMethodinClass(Type types, string Method)
        {
            try
            {
                return (types.GetMethods().Where(x => x.Name == Method).First());
            }
            catch (Exception ex)
            {

                return (null);
            }
        }

       
       
    }
}