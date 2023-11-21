using Cloud.IO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Cloud.IU.WEB.Models
{
    public class SDKManager
    {
        string appname;
        string classname;
        string method;
        List<object> constructorparams;
        List<object> param;
        string directory;
        string applicationpath;

        public string ApplicationPath
        {
            get { return applicationpath; }
            set { applicationpath = value; }
        }

        public string Directory
        {
            get { return directory; }
        }
        public string AppName
        {
            get { return appname; }
        }


        public string Method
        {
            get { return method; }

        }
        public string ClassName
        {
            get { return classname; }

        }

        public List<object> Param
        {
            get { return param; }
            set { this.param = value; }
        }

        public List<object> ConstructorParams
        { get { return (this.constructorparams); } set { this.constructorparams = value; } }


        public SDKManager(string AppName, string ClassName, string Method)
        {
            if ((string.IsNullOrEmpty(AppName)) &&
                (string.IsNullOrEmpty(ClassName)) &&
                (string.IsNullOrEmpty(Method)))

                throw new Exception("[Server].[SDKManager].[SDKManager] => input value are null or empty ");

            try
            {
                this.appname = AppName;
                this.classname = ClassName;
                this.method = Method;

                var appconfig = new LayoutConfigRepository("Application-Service\\AppsConfig.xml");
                var appcon = appconfig.ReadOne("//root//applications//application[@name='" + appname + "']");

                if (appcon == null)
                    throw new Exception("[Server].[SDKManager].[SDKManager] => application not found in AppsConfig ");

                this.directory = appcon.Directory;
                this.applicationpath = string.Format("{0}{1}\\{2}",
                                                      AppDomain.CurrentDomain.BaseDirectory,
                                                      "Application-Service",
                                                      this.directory);
            }
            catch (Exception ex)
            {

                throw new Exception(string.Format("[Server].[SDKManager].[Cunstructor] => execution has been stop by this error {0} ",
                                                    ex.Message));
            }

        }




        public object MethodExecute()
        {
            try
            {
                var sdkconfig = new SDKConfig(this.directory);
                string xpath = "//applicationConfig//APIS//lib";
                var liblist = sdkconfig.Reads(xpath,
                                                "filename");

                var instance = SelectClassInAssembly(liblist,
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
                    Logging.SimpleErrorlogRegister(msg);
                    //throw new Exception(msg);
                    return (null);
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format("[Server].[SDKManager].[ExecuteMethod] => execution has been stop by this error {0} ",
                                                    ex.Message);
                Logging.SimpleErrorlogRegister(msg);
                //throw new Exception(msg);
                return(null);
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

                foreach (var method in types.GetMethods())
                {
                    if (method.Name == Method)
                    {
                        return (method);
                    }
                }
                return (null);
            }
            catch (Exception ex)
            {

                return (null);
            }
        }



    }


}

#region commen Area

//     //SECTION #1 being Used as default                
//     var plugin = Assembly.LoadFile(path);
//     var instance = plugin.GetTypes().Where(x => x.Name == ClassName).First();
//     var obj = Activator.CreateInstance(instance,
//                                         new object[] { });
//     var result = obj.GetType().GetMethod(MethodName).Invoke(obj,
//                                                           new object[] { Params });

// This MethodExecution was previous responsible for execution of SDK
//public object MethodExecute()
//{
//    try
//    {
//        var sdkconfig = new SDKConfig(this.directory);
//        string xpath = "//applicationConfig//APIS//lib";
//        var liblist = sdkconfig.Reads(xpath,
//                                        "filename").First();

//        string path = string.Format("{0}\\Bin\\{1}",
//                                      this.applicationpath,
//                                      liblist.Value);
//        var plugin = Assembly.LoadFile(path);
//        var instance = plugin.GetTypes().Where(x => x.Name == this.classname).First();

//        var consinfo = instance.GetConstructor(System.Type.EmptyTypes).GetParameters();

//        if (instance == null)
//            throw new Exception("[Server].[SDKManager].[MethdExecute] => specific class not found !");

//        object obj ;
//        if ( (this.constructorparams != null) && 
//            (this.constructorparams.Count > 0))
//        obj = Activator.CreateInstance(instance,
//                                            new object[] { this.constructorparams });
//        else
//            obj = Activator.CreateInstance(instance,
//                                            new object[] {});
//        object result;
//        try
//        {
//            var re = obj.GetType().GetMethod(this.method).GetParameters();
//            if ((this.param != null) &&
//            (this.param.Count > 0))
//                result = obj.GetType().GetMethod(this.method,
//                                                   BindingFlags.Instance | BindingFlags.Public).Invoke(obj,
//                                                                                                        new object[] { this.param });
//            else
//                result = obj.GetType().GetMethod(this.method,
//                                                   BindingFlags.Instance | BindingFlags.Public).Invoke(obj,
//                                                                                                        new object[] { });
//            return (result);
//        }
//        catch (Exception ex)
//        {

//            throw new Exception(string.Format("[Server].[SDKManager].[ExecuteMethod] => execution has been faild on Invoke Method by this error : {0} ",
//                                            ex.Message));
//        }

//    }
//    catch (Exception ex)
//    {

//        throw new Exception(string.Format("[Server].[SDKManager].[ExecuteMethod] => execution has been stop by this error {0} ",
//                                            ex.Message));
//    }

//}
#endregion