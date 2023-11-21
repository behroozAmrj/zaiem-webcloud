using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using OODBMS.OODBMSDataLib;
using OODBMS.OODBMSConnectionLib;

namespace OODBMS_PowerDB
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
        
    public class Service : System.Web.Services.WebService
    {
        OODBMSObject currentObj;
        OODBMSClass currentClass;

        public Service()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        public double GetVersion()
        {
            return 1.05;
        }

        [WebMethod]
        public bool CreateClass(string classname)
        {
            try
            {
                string path = Server.MapPath("App_Data/"+classname);
                currentClass = OODBMSClass.CreateClass(path);
                return true;

            }
            catch
            {
                return false;
            }
        }

        [WebMethod]
        public List<string> GetClassFields(string classname)
        {
            CreateClass(classname);
            List<string> list = new List<string>();
            foreach (OODBMSField field in currentClass.Fields)
            {
                list.Add(field.Name);
            }
            return list;
        }

        [WebMethod]
        public bool AddMainFiled(string classname, string filed)
        {
            CreateClass(classname);

            if (currentClass != null)
            {
                if (!currentClass.FieldList.ContainsKey(filed))
                {
                    currentClass.AddField(filed, FieldTypes.string_, FieldKind.Main);
                    currentClass.Save();
                    return true;

                }
            }
            return false;
        }

        [WebMethod]
        public bool AddExtraFiled(string classname, string filed)
        {
            CreateClass(classname);

            if (currentClass != null)
            {
                if (!currentClass.FieldList.ContainsKey(filed))
                {
                    currentClass.AddField(filed, FieldTypes.string_, FieldKind.Extra);
                    currentClass.Save();
                    return true;
                }
            }
            return false;
        }

        [WebMethod]
        public bool DeleteField(string classname, string filed)
        {
            CreateClass(classname);

            if (currentClass != null)
            {
                if (currentClass.FieldList.ContainsKey(filed))
                {
                    currentClass.DeleteField(filed);
                    currentClass.Save();
                    return true;

                }
            }
            return false;
        }

        [WebMethod]
        public int CountObjects(string classname)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                return currentClass.CountObjects();
            }
            return -1;
        }

        [WebMethod]
        public List<string> GetObjectFields(string classname, int objectindex)
        {
            List<string> list = new List<string>();
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects("[@ID=" + objectindex + "]");
                foreach (OODBMSField field in ((OODBMSObject)currentClass.Objects[0]).Fields)
                {
                    list.Add(field.Name);
                }
            }

            return list;
        }

        [WebMethod]
        public List<string> GetObjectFieldsbyQuery(string classname, string query, int objectindex)
        {
            List<string> list = new List<string>();
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects(query);
                currentClass.GetObjects("[@ID=" + objectindex + "]");
                OODBMSObject currentObj = ((OODBMSObject)currentClass.Objects[0]);

                foreach (OODBMSField field in currentObj.Fields)
                {
                    list.Add(field.Name);
                }
            }

            return list;
        }


        [WebMethod]
        public string GetObjectValue(string classname, int objectindex, string field)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects("[@ID=" + objectindex + "]");
                OODBMSObject currentObj = ((OODBMSObject)currentClass.Objects[0]);
                //currentClass.GetObjects();
                //currentObj = ((OODBMSObject)currentClass.Objects[objectindex]);
                return currentObj[field].ToString();
            }
            return null;
        }

        [WebMethod]
        public string GetClassPath(string classname)
        {

            CreateClass(classname);
            if (currentClass != null)
            {
                return currentClass.DataPath;
            }
            return null;
        }

        [WebMethod]
        public string GetObjectValuebyQuery(string classname, string query, int objectindex, string field)
        {
            
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects(query); // I THINK THIS LINE MUST BE DELETED
                currentClass.GetObjects("[@ID=" + objectindex + "]");
                OODBMSObject currentObj = ((OODBMSObject)currentClass.Objects[0]);

                //currentObj = ((OODBMSObject)currentClass.Objects[objectindex]);
                return currentObj[field].ToString();
            }
            return null;
        }

        [WebMethod]
        public long CreateObject(string classname)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                //currentClass.GetObjects();
                currentObj = currentClass.CreateObject(true);
                currentClass.Save();
                return currentObj.ID;
            }
            return -1;
        }

        [WebMethod]
        public bool UpdateObject(string classname, int objectindex, string field, string value)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects("[@ID=" + objectindex + "]");
                OODBMSObject obj = ((OODBMSObject)currentClass.Objects[0]);
                if (!obj.HasField(field))
                    obj.AddField(field, FieldTypes.string_, FieldKind.Extra, value);
                else
                    obj[field] = value;
                //currentObj = ((OODBMSObject)currentClass.Objects[objectindex]);
                //currentObj[field] = value;
                currentClass.Save();
                return true;

            }
            return false;
        }

        [WebMethod]
        public bool DeleteObject(string classname, int objectindex)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects("[@ID=" + objectindex + "]");
                ((OODBMSObject)currentClass.Objects[0]).Delete();
                //currentObj = ((OODBMSObject)currentClass.Objects[objectindex]);
                //currentObj.Delete();
                currentClass.Save();
                return true;

            }
            return false;
        }

        [WebMethod]
        public bool DeleteObjectField(string classname, int objectindex, string field)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects("[@ID=" + objectindex + "]");
                ((OODBMSObject)currentClass.Objects[0]).DeleteField(field);

                //currentClass.GetObjects();
                //currentObj = ((OODBMSObject)currentClass.Objects[objectindex]);
                //currentObj.DeleteField(field);
                currentClass.Save();
                return true;

            }
            return false;
        }

        [WebMethod]
        public long AddFullObject(string classname, string[] mainfields, string[] mainvalues, string[] extrafields, string[] extravalues)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentObj = currentClass.CreateObject(true);

                for (int i = 0; i < mainfields.Length; i++ )
                    currentObj.AddField(mainfields[i], FieldTypes.string_, FieldKind.Main, mainvalues[i]);
                
                for (int i = 0; i < extrafields.Length; i++)
                    currentObj.AddField(extrafields[i], FieldTypes.string_, FieldKind.Extra, extravalues[i]);
                
                currentClass.Save();
                return currentObj.ID;
            }
            return -1;
        }

        [WebMethod]
        public bool AddObjectMainField(string classname, int objectindex, string field, string value)
        {
            CreateClass(classname);
            if (currentClass != null)
            {

                currentClass.GetObjects("[@ID=" + objectindex + "]");
                //currentClass.GetObjects();
                //currentObj = ((OODBMSObject)currentClass.Objects[objectindex]);
                ((OODBMSObject)currentClass.Objects[0]).AddField(field, FieldTypes.string_, FieldKind.Main, value);
                currentClass.Save();
                return true;

            }
            return false;
        }
        [WebMethod]
        public bool AddObjectExtraField(string classname, int objectindex, string field, string value)
        {
            CreateClass(classname);
            if (currentClass != null)
            {

                currentClass.GetObjects("[@ID=" + objectindex + "]");
                //currentClass.GetObjects();
                //currentObj = ((OODBMSObject)currentClass.Objects[objectindex]);
                ((OODBMSObject)currentClass.Objects[0]).AddField(field, FieldTypes.string_, FieldKind.Extra, value);
                currentClass.Save();
                return true;
            }
            return false;
        }
        [WebMethod]
        public bool AddBulkToDictionary(string classname, List<string> lines)
        {
            try
            {

                CreateClass(classname);
                currentClass.AddField("En", FieldTypes.string_, FieldKind.Main);
                currentClass.AddField("Fa", FieldTypes.string_, FieldKind.Main);

                foreach (string line in lines)
                {
                    string en = line.Substring(0, 50).Trim();
                    string[] fa = line.Substring(50).Trim().TrimEnd('.').Split('،');

                    currentObj = currentClass.CreateObject(true);

                    currentObj["En"] = en;
                    currentObj["Fa"] = fa[0];

                    if (fa.Length > 1)
                    {
                        for (int j = 1; j < fa.Length; j++)
                            currentObj.AddField("Fa" + j, FieldTypes.string_, FieldKind.Extra, fa[j]);
                    }
                }
                currentClass.Save();

                return true;

            }
            catch
            {
                return false;
            }
        }
        [WebMethod]
        public List<int> Search(string classname, string query)
        {

            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects(query);
                List<int> list = new List<int>();
                foreach (OODBMSObject obj in currentClass.Objects)
                {
                    list.Add((int)obj.ID);
                }
                return list;
            }
            return null;
        }
        [WebMethod]
        public List<string> Search_ReturnRows(string classname, string query, string field)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects(query);
                List<string> list = new List<string>();
                foreach (OODBMSObject obj in currentClass.Objects)
                {
                    list.Add(obj[field].ToString());
                }
                return list;
            }
            return null;
        }

        [WebMethod]
        public List<string> Search_ReturnValues(string classname, string query, string[] fields, string seperator)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects(query);
                List<string> list = new List<string>();
                foreach (OODBMSObject obj in currentClass.Objects)
                {
                    string s = "";
                    foreach (string f in fields)
                    {
                        s += obj[f].ToString() +seperator;
                    }
                    list.Add(s);
                }
                return list;
            }
            return null;
        }

        [WebMethod]
        public List<string> Search_ReturnValues_id(string classname, string query, string[] fields, string seperator)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects(query);
                List<string> list = new List<string>();
                foreach (OODBMSObject obj in currentClass.Objects)
                {
                    string s = obj.ID + seperator;
                    foreach (string f in fields)
                    {
                        s += obj[f].ToString() + seperator;
                    }
                    list.Add(s);
                }
                return list;
            }
            return null;
        }

        [WebMethod]
        public List<string> Search_Filter_ReturnRows(string classname, string query, string field, string filter, string[] returns, string seperator)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects(query);
                List<string> list = new List<string>();
                foreach (OODBMSObject obj in currentClass.Objects)
                {
                   
                    string r = obj[field].ToString();
                    if (r.Contains(filter))
                    {
                        string s = "";
                        foreach (string f in returns)
                        {
                            s += obj[f].ToString() + seperator;
                        }
                        list.Add(s);
                    }
                }
                return list;
            }
            return null;
        }

        [WebMethod]
        public List<string> Search_Filter_ReturnRows_id(string classname, string query, string field, string filter, string[] returns, string seperator)
        {
            CreateClass(classname);
            if (currentClass != null)
            {
                currentClass.GetObjects(query);
                List<string> list = new List<string>();
                foreach (OODBMSObject obj in currentClass.Objects)
                {

                    string r = obj[field].ToString();
                    if (r.Contains(filter))
                    {
                        string s = obj.ID + seperator;
                        foreach (string f in returns)
                        {
                            s += obj[f].ToString() + seperator;
                        }
                        list.Add(s);
                    }
                }
                return list;
            }
            return null;
        }

        [WebMethod]
        public List<string> SearchWordInDictionarybyEn(string word)
        {
            CreateClass("Dictionary");
            string query = "[En='" + word + "']";
            List<string> list = new List<string>();

            if (currentClass != null)
            {
                currentClass.GetObjects(query);
                foreach (OODBMSObject obj in currentClass.Objects)
                {
                    foreach (OODBMSField field in obj.Fields)
                    {
                        if (field.Name != "En")
                            list.Add(obj[field.Name].ToString());
                    }
                }

            }
            return list;
        }
        [WebMethod]
        public List<string> SearchWordInDictionarybyFa(string word)
        {
            CreateClass("Dictionary");
            string query = "[Fa='" + word + "']";
            List<string> list = new List<string>();

            if (currentClass != null)
            {
                currentClass.GetObjects(query);
                foreach (OODBMSObject obj in currentClass.Objects)
                {
                    string en = obj["En"].ToString();
                    list.Add(en);
                }

            }
            return list;
        }
    }
}