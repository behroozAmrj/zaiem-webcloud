using RBSManagementService.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RBSManagementService.LojicModel
{
    public class SDKManager
    {
        public List<RBS> ListAllApplication(string userID)
        {
            string path = HttpContext.Current.Server.MapPath("/Data/") + "AppData.xml";
            if (!File.Exists(path))
                throw new System.Exception("file not exist!");
            var xmlExcuter = new XMLStorage(path);
            return (xmlExcuter.SelectApplications(userID));
        }



    }
}