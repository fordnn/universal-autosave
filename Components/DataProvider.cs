using System;
using System.Data;
using DotNetNuke;

namespace forDNN.Modules.UniversalAutosave
{
	public abstract class DataProvider
	{

		#region "Shared/Static Methods"

		/// <summary>
		/// singleton reference to the instantiated object 
		/// </summary>
		private static DataProvider objProvider = null;

		/// <summary>
		/// constructor
		/// </summary>
		static DataProvider()
		{
			CreateProvider();
		}

		/// <summary>
		/// dynamically create provider 
		/// </summary>
		private static void CreateProvider()
		{
			objProvider = (DataProvider)DotNetNuke.Framework.Reflection.CreateObject("data", "forDNN.Modules.UniversalAutosave", "");
		}

		/// <summary>
		/// return the provider 
		/// </summary>
		/// <returns></returns>
		public static DataProvider Instance()
		{
			return objProvider;
		}

		#endregion

		#region Abstract Methods Configuration

		public abstract int Configuration_Add(ConfigurationInfo objInfo);
		public abstract void Configuration_Delete(int ConfigurationID);
		public abstract void Configuration_Update(ConfigurationInfo objInfo);
		public abstract IDataReader Configuration_GetByPrimaryKey(int ConfigurationID);
		public abstract IDataReader Configuration_GetAllItems(string SortBy);
		public abstract IDataReader Configuration_GetByTabID(int TabID);
		public abstract IDataReader Configuration_GetDistinctValues();

		#endregion

        #region Abstract Methods TabsInGlobalConfiguration

        public abstract void TabsInGlobalConfiguration_Add(int ConfigurationID, int TabID);

        #endregion

		#region Abstract Methods ConfigurationPermission

		public abstract int ConfigurationPermission_Add(ConfigurationPermissionInfo objInfo);
		public abstract void ConfigurationPermission_DeleteByConfigurationID(int ConfigurationID);
		public abstract void ConfigurationPermission_Delete(int ConfigurationPermissionID);
		public abstract void ConfigurationPermission_Update(ConfigurationPermissionInfo objInfo);
		public abstract IDataReader ConfigurationPermission_GetByPrimaryKey(int ConfigurationPermissionID);
		public abstract IDataReader ConfigurationPermission_GetAllItems(string SortBy);
		public abstract IDataReader ConfigurationPermission_GetByConfigurationID(int ConfigurationID);

		#endregion

		#region Abstract Methods Control

		public abstract int Control_Add(ControlInfo objInfo);
		public abstract void Control_Delete(int ControlID);
		public abstract void Control_Update(ControlInfo objInfo);
		public abstract IDataReader Control_GetByPrimaryKey(int ControlID);
		public abstract IDataReader Control_GetAllItems(string SortBy);
		public abstract IDataReader Control_GetByFilter(int ConfigurationID, string Selector);
		public abstract IDataReader Control_GetDistinctValues(int ConfigurationID);

		#endregion

		#region Abstract Methods Value

		public abstract int Value_Add(ValueInfo objInfo);
		public abstract void Value_Delete(int ValueID);
		public abstract void Value_Update(ValueInfo objInfo);
		public abstract IDataReader Value_GetByPrimaryKey(int ValueID);
		public abstract IDataReader Value_GetAllItems(string SortBy);
		public abstract IDataReader Value_GetByFilter(int ControlID, int UrlID, int UserID, bool Anonymous, object dtFrom, object dtTo, bool CannedOnly);
		public abstract IDataReader Value_GetLastValue(int ControlID, int UrlID, int UserID, bool Anonymous);
		public abstract IDataReader Value_UpdateClosed(int ControlID, int UrlID, int UserID, bool Anonymous, int HistoryLength, int HistoryExpiry);
		public abstract void Value_CloseSession(int ConfigurationID, int UrlID, int UserID, bool Anonymous);

		#endregion

		#region Abstract Methods Anonymous

		public abstract int Anonymous_Add(AnonymousInfo objInfo);
		public abstract void Anonymous_Delete(int AnonymousID);
		public abstract void Anonymous_Update(AnonymousInfo objInfo);
		public abstract IDataReader Anonymous_GetByPrimaryKey(int AnonymousID);
		public abstract IDataReader Anonymous_GetByUserGUID(System.Guid UserGUID);
		public abstract IDataReader Anonymous_GetAllItems(string SortBy);
		public abstract IDataReader Anonymous_GetDistinctValues();

		#endregion

		#region Abstract Methods Url

		public abstract int Url_Add(UrlInfo objInfo);
		public abstract void Url_Delete(int UrlID);
		public abstract void Url_Update(UrlInfo objInfo);
		public abstract IDataReader Url_GetByPrimaryKey(int UrlID);
		public abstract IDataReader Url_GetByUrl(string Url);
		public abstract IDataReader Url_GetAllItems(string SortBy);

		#endregion

		#region Abstract Methods Event

		public abstract int Event_Add(EventInfo objInfo);
		public abstract void Event_Delete(int EventID);
		public abstract void Event_Update(EventInfo objInfo);
		public abstract IDataReader Event_GetByPrimaryKey(int EventID);
		public abstract IDataReader Event_GetAllItems(string SortBy);
		public abstract IDataReader Event_GetByFilter(int ConfigurationID, string Selector);

		#endregion

	}
}