using Cloud.IU.WEB.InfraSructure;
using Cloud.IU.WEB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Cloud.IU.WEB.Repository
{
    public class CustomViewRepository : ICustomViewRepository
    {
        string file = @"App_Data\CustomViewMapper.xml";
        string parent_id;
        public CustomViewRepository(string Parent_ID)
        {
            if (!string.IsNullOrEmpty(Parent_ID))
            {
                parent_id = Parent_ID;
            }
        }
        public List<CustomView> RetrieveCustomView(string Parent_ID)
        {
            if ((!string.IsNullOrEmpty(Parent_ID)))
                return _retrieveCustomViews(Parent_ID);
            else
                throw new Exception(" Class name is null or empty ");
        }

        private List<CustomView> _retrieveCustomViews(string Name)
        {
            var customviewlist = new List<CustomView>();
            var path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath,
                                     file);
            var customdataset = new DataSet();

            if (!File.Exists(path))
                throw new Exception("Custom View mapper file is not exist . . ");

            customdataset.ReadXml(path);
            var objecttable = customdataset.Tables["objecttable"];
            var expression = "Name = '" + Name + "'";
            var list = objecttable.Select(expression);

            foreach (var row in list)
            {
                var cv = new CustomView();
                cv.ID = Convert.ToInt32(row["ID"].ToString());
                cv.Parent_ID = Convert.ToInt32(row["Parent_ID"].ToString());
                cv.ViewName = row["ViewName"].ToString();
                customviewlist.Add(cv);
            }

            return (customviewlist.ToList());
        }


        public List<CustomView> RetrieveCustomView()
        {
            if (!string.IsNullOrEmpty(this.parent_id))
                return (_retrieveCustomViews(this.parent_id));
            else
                throw new Exception("the class name is not set ");
        }


        public void InsetCustomView(CustomView CustomViewToInsert)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomView(CustomView CustomViewToDelete)
        {
            throw new NotImplementedException();
        }


        public string RetrieveCustomViewDesignedData(string FileName, string URL)
        {
            throw new NotImplementedException();
        }
    }
}