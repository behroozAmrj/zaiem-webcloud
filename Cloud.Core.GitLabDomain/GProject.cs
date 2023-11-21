using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloud.Core.GitLabDomain
{
    public class GProject
    {
        public int ID { get; set; }
        public string Desc { get; set; }
        public Boolean Public { get; set; }

        public string Name { get; set; }
        public string HTTP_URL { get; set; }
        public string Web_URL { get; set; }
        public string Name_With_NameSpace { get; set; }
        public string Created_at { get; set; }
        public string Last_activity { get; set; }
    }
}
