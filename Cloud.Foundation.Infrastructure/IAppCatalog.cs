using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Foundation.Infrastructure
{
    public interface IAppCatalog
    {
        String Name{get;set;}
        String Destination { get; set; }
        String Departure { get; set; }

        String Scope { get; set; }
        void Install(object destination);
        void Remove(object destination);

        AppCatalogType appCatalog{get;}
    }
}
