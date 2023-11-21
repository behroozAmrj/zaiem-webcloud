using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageServer.Models
{
    public interface IDataStore
    {
        List<Service> getAvailableServices();
        void RegisterService(Service service);

        void deleteService();
        Service selectService(string serviceName);
    }
}
