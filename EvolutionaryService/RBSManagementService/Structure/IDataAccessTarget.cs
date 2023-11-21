using RBSManagementService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RBSManagementService.Structure
{
    public interface IDataAccessTarget
    {
        List<RBS> SelectApplications(string UserID);
        RBS SelectAppByUserID(string appName, string userID);
    }
}