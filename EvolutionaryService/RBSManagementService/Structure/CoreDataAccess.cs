using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBSManagementService.Structure
{
    public abstract class CoreDataAccess
    {
        public abstract List<object> GetData(string dataCondition);
        public abstract void PostData(string dataCondition);

        public abstract void updateData(params string[] param);

        public abstract void deleteData(string conditions);

    }
}