using Cloud.Foundation.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Core.Models
{
    public class TransparentData : ITransparentData
    {

        object _datavalue;
        public object DataValue
        {
            get { return (this._datavalue); }
            set { this._datavalue = value; }
        }
    }
}
