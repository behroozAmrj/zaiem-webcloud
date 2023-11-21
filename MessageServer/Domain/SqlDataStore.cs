using MessageServer.DataAccess;
using MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Util;

namespace MessageServer.Domain
{
    public class SqlDataStore : IDataStore , IDisposable
    {
        public SqlDataStore()
        {
        }
        public List<Service> getAvailableServices()
        {
            List<Service> serviceList;
            using (var cnx = new DataManageDBDataContext())
            {
                serviceList = cnx.getLServiceList().Select((x) =>
                                {
                                    var service = new Service();
                                    service.ID = x.Service_ID;
                                    service.serviceName = x.ServiceName;
                                    service.URL = x.URL;
                                    return (service);
                                }).ToList();
                return (serviceList);
            }
        }

        public void RegisterService(Service service)
        {
            using (var cnx = new DataManageDBDataContext())
            {
                cnx.RegisterNewService(service.serviceName,
                                            service.URL);
            }
        }

        public void deleteService()
        {
            throw new NotImplementedException();
        }

        public Service selectService(string serviceName)
        {
            using (var cnx = new DataManageDBDataContext())
            {
                var availableService = cnx.GetServiceByName(serviceName).First();
                var service = new Service();
                service.ID = availableService.Service_ID;
                service.serviceName =availableService.ServiceName;
                service.URL = availableService.URL;

                return (service);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}