using MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageServer.Domain
{
    public enum StorageType
    {
        XML = 0 ,
        SqlServer = 1
    }
    public class ServiceManagementCenter 
    {
        internal static readonly ServiceManagementCenter ServiceCenterMgr = new ServiceManagementCenter();
        internal IDataStore StoreService;
        internal ILogRegister logRegister;
        
        internal void SetStorage(StorageType type)
        {
            switch (type)
            {
                case StorageType.XML :
                    {
                        this.StoreService = new XmlServiceDataStore();
                        this.logRegister = new LogToXML();
                        break;
                    }
                case StorageType.SqlServer :
                    {
                        this.StoreService = new SqlDataStore();
                        this.logRegister = new LogToDB();
                        break;
                    }
                default :
                    {
                        this.StoreService = new XmlServiceDataStore();
                        this.logRegister = new LogToXML();
                        break;
                    }
            }
        }

        internal IDataStore getStorage()
        {
            if (StoreService == null)
                this.StoreService = new XmlServiceDataStore();
            return (this.StoreService);
        }
        internal ILogRegister getLogStorage()
        {
            if (logRegister == null)
                this.logRegister = new LogToXML();
            return (this.logRegister);
        }

    }
}