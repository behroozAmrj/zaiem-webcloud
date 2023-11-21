using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloud.Core.GitLabDomain
{
    public class GIssue
    {
        public int ID { get; set; }
        public int IID { get; set; }
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string State { get; set; }
        public string Created_at { get; set; }
        public string Update_at { get; set; }
    }
}
