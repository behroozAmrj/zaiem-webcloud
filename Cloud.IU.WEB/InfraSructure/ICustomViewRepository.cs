using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.IU.WEB.InfraSructure
{
    public interface ICustomViewRepository<T>
                     where T : class
    {
        string Name { get; set; }
        string Directory { get; set; }
        //ConfigType Type;
        List<T> ReadOne(string XPath);
        void Save();
    }
}
