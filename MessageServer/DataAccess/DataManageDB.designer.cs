﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MessageServer.DataAccess
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="TransactionDB")]
	public partial class DataManageDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public DataManageDBDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["TransactionDBConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataManageDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataManageDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataManageDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataManageDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.getLServiceList")]
		public ISingleResult<getLServiceListResult> getLServiceList()
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())));
			return ((ISingleResult<getLServiceListResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetServiceByName")]
		public ISingleResult<GetServiceByNameResult> GetServiceByName([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(70)")] string serviceName)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), serviceName);
			return ((ISingleResult<GetServiceByNameResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.GetTransactionBasedOnUserID")]
		public ISingleResult<GetTransactionBasedOnUserIDResult> GetTransactionBasedOnUserID([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(50)")] string userID)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userID);
			return ((ISingleResult<GetTransactionBasedOnUserIDResult>)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.RegisterNewService")]
		public int RegisterNewService([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(70)")] string serviceName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(200)")] string url)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), serviceName, url);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.TransactionAdd")]
		public int TransactionAdd([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(50)")] string userID, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(50)")] string userName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(200)")] string departure, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(200)")] string destination, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(200)")] string method, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(50)")] string dateTime, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(1000)")] string content)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), userID, userName, departure, destination, method, dateTime, content);
			return ((int)(result.ReturnValue));
		}
		
		[global::System.Data.Linq.Mapping.FunctionAttribute(Name="dbo.LogRegister")]
		public int LogRegister([global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(70)")] string appName, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="DateTime")] System.Nullable<System.DateTime> dateTime, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(50)")] string userID, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(70)")] string logTracce, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="VarChar(70)")] string action, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="Char(20)")] string actionType, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType="NVarChar(200)")] string content)
		{
			IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), appName, dateTime, userID, logTracce, action, actionType, content);
			return ((int)(result.ReturnValue));
		}
	}
	
	public partial class getLServiceListResult
	{
		
		private int _Service_ID;
		
		private string _ServiceName;
		
		private string _URL;
		
		public getLServiceListResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Service_ID", DbType="Int NOT NULL")]
		public int Service_ID
		{
			get
			{
				return this._Service_ID;
			}
			set
			{
				if ((this._Service_ID != value))
				{
					this._Service_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServiceName", DbType="VarChar(70) NOT NULL", CanBeNull=false)]
		public string ServiceName
		{
			get
			{
				return this._ServiceName;
			}
			set
			{
				if ((this._ServiceName != value))
				{
					this._ServiceName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL", DbType="NVarChar(200) NOT NULL", CanBeNull=false)]
		public string URL
		{
			get
			{
				return this._URL;
			}
			set
			{
				if ((this._URL != value))
				{
					this._URL = value;
				}
			}
		}
	}
	
	public partial class GetServiceByNameResult
	{
		
		private int _Service_ID;
		
		private string _ServiceName;
		
		private string _URL;
		
		public GetServiceByNameResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Service_ID", DbType="Int NOT NULL")]
		public int Service_ID
		{
			get
			{
				return this._Service_ID;
			}
			set
			{
				if ((this._Service_ID != value))
				{
					this._Service_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ServiceName", DbType="VarChar(70) NOT NULL", CanBeNull=false)]
		public string ServiceName
		{
			get
			{
				return this._ServiceName;
			}
			set
			{
				if ((this._ServiceName != value))
				{
					this._ServiceName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_URL", DbType="NVarChar(200) NOT NULL", CanBeNull=false)]
		public string URL
		{
			get
			{
				return this._URL;
			}
			set
			{
				if ((this._URL != value))
				{
					this._URL = value;
				}
			}
		}
	}
	
	public partial class GetTransactionBasedOnUserIDResult
	{
		
		private int _User_ID;
		
		private string _UserID;
		
		private string _DESC;
		
		public GetTransactionBasedOnUserIDResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_User_ID", DbType="Int NOT NULL")]
		public int User_ID
		{
			get
			{
				return this._User_ID;
			}
			set
			{
				if ((this._User_ID != value))
				{
					this._User_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this._UserID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="[DESC]", Storage="_DESC", DbType="NVarChar(50)")]
		public string DESC
		{
			get
			{
				return this._DESC;
			}
			set
			{
				if ((this._DESC != value))
				{
					this._DESC = value;
				}
			}
		}
	}
}
#pragma warning restore 1591