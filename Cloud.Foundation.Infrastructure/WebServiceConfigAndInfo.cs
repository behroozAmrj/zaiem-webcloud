using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cloud.Foundation.Infrastructure
{
    public abstract class WebServiceConfigAndInfo
    {
        string url;
        string className;
        string method;
        object[] param;
        string assemblyReference;
        string _urlAdress;

        public string Url
        {
            get { return _urlAdress; }
        }
        
        private Dictionary<string, IEnumerable<ServiceEndpoint>> endpointsforContracts;
        public Dictionary<string, IEnumerable<ServiceEndpoint>> EndpointsforContracts
        {
            get { return endpointsforContracts; }
            set { endpointsforContracts = value; }
        }
        public string ClassName
        {
            get { return className; }
            set { className = value; }
        }


        public string Method
        {
            get { return method; }
            set { method = value; }
        }


        public object[] Param
        {
            get { return param; }
            set { param = value; }
        }


        public string AssemblyReference
        {
            get { return assemblyReference; }
            set { assemblyReference = value; }
        }

        public WebServiceConfigAndInfo(string ServiceURLAddress)
        {
            if (!string.IsNullOrEmpty(ServiceURLAddress))
            {
                var regx = new Regex(@"^(((ht|f)tp(s?))\://)?((([a-zA-Z0-9_\-]{2,}\.)+[a-zA-Z]{2,})|((?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(\.?\d)\.)){4}))(:[a-zA-Z0-9]+)?(/[a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~]*)?$");
                if (!regx.IsMatch(ServiceURLAddress))
                    throw new Exception(" the Service URL Address is not valid or not in well format of URL Pattern ");
                else
                {
                    _urlAdress = ServiceURLAddress;
                }
            }
            else
            { throw new Exception("Service URL Address is null or empty"); }
        }
    }
}
