﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Foundation.Infrastructure
{
    public interface IWebService
    {
        object WS_ExecuteMethod(WebServiceConfigAndInfo  WebServicePropertyConfig);
    }
}
