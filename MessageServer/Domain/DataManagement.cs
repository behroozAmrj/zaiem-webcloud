using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace MessageServer.Domain
{
    public class DataManagement
    {
        string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;data source=";

        public DataManagement()
        {
            string path = string.Format("{0}\\RoatData\\RoatDB.accdb" , 
                                            AppDomain.CurrentDomain.BaseDirectory);
            if (!File.Exists(path))
                throw new Exception("roat table is not exist!");
            connStr = string.Format("{0}{1}",
                                    connStr,
                                    path);
        }

        public DataTable getAvalilableService()
        {
            using(var oleConnection = new OleDbConnection(connStr))
            {
                oleConnection.Open();
                var adapter = new OleDbDataAdapter("SELECT * FROM RoatTable" ,
                                                    oleConnection);
                var roatTable = new DataTable();
                adapter.Fill(roatTable);
                oleConnection.Close();
                return (roatTable );
                
            }
        }

        public DataTable selectServer(string ServiceName)
        {
            using (var oleConnection = new OleDbConnection(connStr))
            {
                oleConnection.Open();
                string query = string.Format("SELECT * FROM RoatTable WHERE ServiceName='{0}'" , 
                                                            ServiceName);
                var adapter = new OleDbDataAdapter(query,
                                                    oleConnection);
                var roatTable = new DataTable();
                adapter.Fill(roatTable);
                oleConnection.Close();
                return (roatTable);

            }
        }

        public void insertService(string service , string URL)
        {
            using (var oleConnection = new OleDbConnection(connStr))
            {
                oleConnection.Open();
                string query = string.Format("INSERT INTO RoatTable ServiceName,URL values ('{0}','{1}')");
                var command = new OleDbCommand(query,
                                                oleConnection);
                command.ExecuteNonQuery();
                oleConnection.Close();
            }
        }
    }
}