using Cloud.Foundation.Infrastructure;
using Cloud.IO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    public class ExistApp : IAppCatalog
    {
        private string _name;
        private string _destination;
        private string _departure;
        private string _scope;
        private string userID;
        private AppCatalogType appType = AppCatalogType.ExistApp;

        public ExistApp(String UserID = null)
        {
            if (!String.IsNullOrEmpty(UserID))
                this.userID = UserID;
            
        }
        public string Name
        {
            get
            {
                return (this._name);
            }
            set
            {
                this._name = value;
            }
        }

        public string Destination
        {
            get
            {
                return (this._destination);
            }
            set
            {
                this._destination = value;
            }
        }

        public string Departure
        {
            get
            {
                return (this._departure);
            }
            set
            {
                this._departure = value;
            }
        }

        public void Install(object destination)
        {
            if ((!String.IsNullOrEmpty(this.Name)) &&
                (!String.IsNullOrEmpty(this.userID)))
            {
                String path = String.Format("Application-service/appsConfig.xml",
                                            AppDomain.CurrentDomain.BaseDirectory);
                using (var existApp = new RBSConfigRepositoryForUsers(path))
                {
                    existApp.assignRBSToUser(this.Name,
                                            this.userID);
                }
            }
        }

        public void Remove(object destination)
        {
            if ((!String.IsNullOrEmpty(this.Name)) &&
                (!String.IsNullOrEmpty(this.userID)))
            {
                String path = String.Format("{0}//Application-service/appsConfig.xml",
                                            AppDomain.CurrentDomain.BaseDirectory);
                using (var existApp = new RBSConfigRepositoryForUsers(path))
                {
                    existApp.removeOrUninstallRBSforUser(this.Name,
                                                         this.userID);
                }
            }
        }




        public string Scope
        {
            get
            {
                return (this._scope);
            }
            set
            {
                this._scope = value;
            }
        }


        public AppCatalogType appCatalog
        {
            get { return (this.appType); }
        }
    }
}
