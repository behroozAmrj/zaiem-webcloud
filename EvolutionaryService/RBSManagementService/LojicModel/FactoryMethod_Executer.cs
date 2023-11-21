using RBSManagementService.Models;
using RBSManagementService.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBSManagementService.LojicModel
{
    public static class FactoryMethod_Executer
    {
        public static IExecuter FactoryMethod(RBS rbs)
        {
            if (rbs.source == "http://")
            {
                var serviceExecuter = new ServiceExecuter();
                serviceExecuter.Name = rbs.Name;
                serviceExecuter.source = rbs.source;
                serviceExecuter.version = rbs.version;
                serviceExecuter.Enable = rbs.Enable;
                return (serviceExecuter);
            }
            else
            {
                var DNexecuter = new DotNetExecuter();
                DNexecuter.Name = rbs.Name;
                DNexecuter.source = rbs.source;
                DNexecuter.version = rbs.version;
                DNexecuter.Enable = rbs.Enable;
                return (DNexecuter);
            }
        }
    }
}