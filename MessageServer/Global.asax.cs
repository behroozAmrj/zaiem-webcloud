using MessageServer.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace MessageServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            string connStr = ConfigurationManager.AppSettings["transactionDB"].ToString();
            chooseDataStore(connStr);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private void chooseDataStore(string connStr)
        {
            if (!string.IsNullOrEmpty(connStr))
            {
                using(var sqlConn = new SqlConnection(connStr))
                {
                    if (sqlConn.State == System.Data.ConnectionState.Closed)
                    {
                        try
                        {
                            sqlConn.Open();
                            
                            sqlConn.Close();
                            ServiceManagementCenter.ServiceCenterMgr.SetStorage(StorageType.SqlServer);
                            Logging.INFOlogRegistrer("router/postMsg",
                                             "no target",
                                             "info",
                                             "SQL choosed for data");
                        }
                        catch (Exception)
                        {
                            ServiceManagementCenter.ServiceCenterMgr.SetStorage(StorageType.XML);
                             Logging.INFOlogRegistrer("router/postMsg",
                                              "no target",
                                              "Exception",
                                              "XML choosed for data");
                        }
                    }
                }
            }
            else
            {
                ServiceManagementCenter.ServiceCenterMgr.SetStorage(StorageType.XML);
            }
        }

    }
}
