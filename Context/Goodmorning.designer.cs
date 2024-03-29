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

namespace ECP_WEBAPI.Context
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="GoogMorning")]
	public partial class GoodmorningDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void Inserttblconfig(tblconfig instance);
    partial void Updatetblconfig(tblconfig instance);
    partial void Deletetblconfig(tblconfig instance);
    partial void Inserttbluser(tbluser instance);
    partial void Updatetbluser(tbluser instance);
    partial void Deletetbluser(tbluser instance);
    partial void InserttblTask(tblTask instance);
    partial void UpdatetblTask(tblTask instance);
    partial void DeletetblTask(tblTask instance);
    #endregion
		
		public GoodmorningDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["GoogMorningConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public GoodmorningDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GoodmorningDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GoodmorningDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GoodmorningDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<tblconfig> tblconfigs
		{
			get
			{
				return this.GetTable<tblconfig>();
			}
		}
		
		public System.Data.Linq.Table<tbluser> tblusers
		{
			get
			{
				return this.GetTable<tbluser>();
			}
		}
		
		public System.Data.Linq.Table<tblTask> tblTasks
		{
			get
			{
				return this.GetTable<tblTask>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.tblconfig")]
	public partial class tblconfig : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private System.Nullable<int> _Loai;
		
		private System.Nullable<System.DateTime> _CrateDate;
		
		private System.Nullable<System.DateTime> _Time;
		
		private System.Nullable<int> _Active;
		
		private System.Nullable<int> _UserId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnLoaiChanging(System.Nullable<int> value);
    partial void OnLoaiChanged();
    partial void OnCrateDateChanging(System.Nullable<System.DateTime> value);
    partial void OnCrateDateChanged();
    partial void OnTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnTimeChanged();
    partial void OnActiveChanging(System.Nullable<int> value);
    partial void OnActiveChanged();
    partial void OnUserIdChanging(System.Nullable<int> value);
    partial void OnUserIdChanged();
    #endregion
		
		public tblconfig()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Loai", DbType="Int")]
		public System.Nullable<int> Loai
		{
			get
			{
				return this._Loai;
			}
			set
			{
				if ((this._Loai != value))
				{
					this.OnLoaiChanging(value);
					this.SendPropertyChanging();
					this._Loai = value;
					this.SendPropertyChanged("Loai");
					this.OnLoaiChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CrateDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> CrateDate
		{
			get
			{
				return this._CrateDate;
			}
			set
			{
				if ((this._CrateDate != value))
				{
					this.OnCrateDateChanging(value);
					this.SendPropertyChanging();
					this._CrateDate = value;
					this.SendPropertyChanged("CrateDate");
					this.OnCrateDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Time", DbType="DateTime")]
		public System.Nullable<System.DateTime> Time
		{
			get
			{
				return this._Time;
			}
			set
			{
				if ((this._Time != value))
				{
					this.OnTimeChanging(value);
					this.SendPropertyChanging();
					this._Time = value;
					this.SendPropertyChanged("Time");
					this.OnTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Active", DbType="Int")]
		public System.Nullable<int> Active
		{
			get
			{
				return this._Active;
			}
			set
			{
				if ((this._Active != value))
				{
					this.OnActiveChanging(value);
					this.SendPropertyChanging();
					this._Active = value;
					this.SendPropertyChanged("Active");
					this.OnActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="Int")]
		public System.Nullable<int> UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.tbluser")]
	public partial class tbluser : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _Id;
		
		private string _UserName;
		
		private string _Password;
		
		private System.Nullable<int> _Active;
		
		private string _Phone;
		
		private System.Nullable<System.DateTime> _NgaySinh;
		
		private System.Nullable<long> _Blance;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdChanging(long value);
    partial void OnIdChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnActiveChanging(System.Nullable<int> value);
    partial void OnActiveChanged();
    partial void OnPhoneChanging(string value);
    partial void OnPhoneChanged();
    partial void OnNgaySinhChanging(System.Nullable<System.DateTime> value);
    partial void OnNgaySinhChanged();
    partial void OnBlanceChanging(System.Nullable<long> value);
    partial void OnBlanceChanged();
    #endregion
		
		public tbluser()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				if ((this._Id != value))
				{
					this.OnIdChanging(value);
					this.SendPropertyChanging();
					this._Id = value;
					this.SendPropertyChanged("Id");
					this.OnIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(50)")]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Password", DbType="NVarChar(50)")]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Active", DbType="Int")]
		public System.Nullable<int> Active
		{
			get
			{
				return this._Active;
			}
			set
			{
				if ((this._Active != value))
				{
					this.OnActiveChanging(value);
					this.SendPropertyChanging();
					this._Active = value;
					this.SendPropertyChanged("Active");
					this.OnActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Phone", DbType="NVarChar(50)")]
		public string Phone
		{
			get
			{
				return this._Phone;
			}
			set
			{
				if ((this._Phone != value))
				{
					this.OnPhoneChanging(value);
					this.SendPropertyChanging();
					this._Phone = value;
					this.SendPropertyChanged("Phone");
					this.OnPhoneChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NgaySinh", DbType="DateTime")]
		public System.Nullable<System.DateTime> NgaySinh
		{
			get
			{
				return this._NgaySinh;
			}
			set
			{
				if ((this._NgaySinh != value))
				{
					this.OnNgaySinhChanging(value);
					this.SendPropertyChanging();
					this._NgaySinh = value;
					this.SendPropertyChanged("NgaySinh");
					this.OnNgaySinhChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Blance", DbType="BigInt")]
		public System.Nullable<long> Blance
		{
			get
			{
				return this._Blance;
			}
			set
			{
				if ((this._Blance != value))
				{
					this.OnBlanceChanging(value);
					this.SendPropertyChanging();
					this._Blance = value;
					this.SendPropertyChanged("Blance");
					this.OnBlanceChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.tblTask")]
	public partial class tblTask : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _IdTask;
		
		private string _ContentTask;
		
		private string _TitleTask;
		
		private System.Nullable<System.DateTime> _CrateDate;
		
		private System.Nullable<System.DateTime> _Time;
		
		private System.Nullable<int> _Active;
		
		private System.Nullable<int> _UserId;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIdTaskChanging(long value);
    partial void OnIdTaskChanged();
    partial void OnContentTaskChanging(string value);
    partial void OnContentTaskChanged();
    partial void OnTitleTaskChanging(string value);
    partial void OnTitleTaskChanged();
    partial void OnCrateDateChanging(System.Nullable<System.DateTime> value);
    partial void OnCrateDateChanged();
    partial void OnTimeChanging(System.Nullable<System.DateTime> value);
    partial void OnTimeChanged();
    partial void OnActiveChanging(System.Nullable<int> value);
    partial void OnActiveChanged();
    partial void OnUserIdChanging(System.Nullable<int> value);
    partial void OnUserIdChanged();
    #endregion
		
		public tblTask()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IdTask", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long IdTask
		{
			get
			{
				return this._IdTask;
			}
			set
			{
				if ((this._IdTask != value))
				{
					this.OnIdTaskChanging(value);
					this.SendPropertyChanging();
					this._IdTask = value;
					this.SendPropertyChanged("IdTask");
					this.OnIdTaskChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ContentTask", DbType="NVarChar(4000)")]
		public string ContentTask
		{
			get
			{
				return this._ContentTask;
			}
			set
			{
				if ((this._ContentTask != value))
				{
					this.OnContentTaskChanging(value);
					this.SendPropertyChanging();
					this._ContentTask = value;
					this.SendPropertyChanged("ContentTask");
					this.OnContentTaskChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TitleTask", DbType="NVarChar(4000)")]
		public string TitleTask
		{
			get
			{
				return this._TitleTask;
			}
			set
			{
				if ((this._TitleTask != value))
				{
					this.OnTitleTaskChanging(value);
					this.SendPropertyChanging();
					this._TitleTask = value;
					this.SendPropertyChanged("TitleTask");
					this.OnTitleTaskChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CrateDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> CrateDate
		{
			get
			{
				return this._CrateDate;
			}
			set
			{
				if ((this._CrateDate != value))
				{
					this.OnCrateDateChanging(value);
					this.SendPropertyChanging();
					this._CrateDate = value;
					this.SendPropertyChanged("CrateDate");
					this.OnCrateDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Time", DbType="DateTime")]
		public System.Nullable<System.DateTime> Time
		{
			get
			{
				return this._Time;
			}
			set
			{
				if ((this._Time != value))
				{
					this.OnTimeChanging(value);
					this.SendPropertyChanging();
					this._Time = value;
					this.SendPropertyChanged("Time");
					this.OnTimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Active", DbType="Int")]
		public System.Nullable<int> Active
		{
			get
			{
				return this._Active;
			}
			set
			{
				if ((this._Active != value))
				{
					this.OnActiveChanging(value);
					this.SendPropertyChanging();
					this._Active = value;
					this.SendPropertyChanged("Active");
					this.OnActiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", DbType="Int")]
		public System.Nullable<int> UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
