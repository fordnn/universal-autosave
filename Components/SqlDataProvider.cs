using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;

namespace forDNN.Modules.UniversalAutosave
{
	public class SqlDataProvider : DataProvider
	{

		#region "Private Members"

		private const string ProviderType = "data";
		private const string ModuleQualifier = "forDNN_UniversalAutosave_";

		private ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
		private string _connectionString;
		private string _providerPath;
		private string _objectQualifier;
		private string _databaseOwner;

		#endregion

		#region "Constructors"

		public SqlDataProvider()
		{

			// Read the configuration specific information for this provider 
			Provider objProvider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

			// Read the attributes for this provider 

			//Get Connection string from web.config 
			_connectionString = Config.GetConnectionString();

			if (_connectionString == "")
			{
				// Use connection string specified in provider 
				_connectionString = objProvider.Attributes["connectionString"];
			}

			_providerPath = objProvider.Attributes["providerPath"];

			_objectQualifier = objProvider.Attributes["objectQualifier"];
			if (_objectQualifier != "" & _objectQualifier.EndsWith("_") == false)
			{
				_objectQualifier += "_";
			}

			_databaseOwner = objProvider.Attributes["databaseOwner"];
			if (_databaseOwner != "" & _databaseOwner.EndsWith(".") == false)
			{
				_databaseOwner += ".";
			}

		}

		#endregion

		#region "Properties"

		public string ConnectionString
		{
			get { return _connectionString; }
		}

		public string ProviderPath
		{
			get { return _providerPath; }
		}

		public string ObjectQualifier
		{
			get { return _objectQualifier; }
		}

		public string DatabaseOwner
		{
			get { return _databaseOwner; }
		}

		#endregion

		#region "Private Methods"

		private string GetFullyQualifiedName(string name)
		{
			return DatabaseOwner + ObjectQualifier + ModuleQualifier + name;
		}

		private object GetNull(object Field)
		{
			return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value);
		}

		#endregion

		#region Configuration Methods

		public override int Configuration_Add(ConfigurationInfo objConfiguration)
		{
			int intResID = -1;
			intResID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
				GetFullyQualifiedName("Configuration_Add"),
				objConfiguration.Title,
				objConfiguration.Description,
				objConfiguration.TabID,
				objConfiguration.AutosaveEnabled,
				objConfiguration.AutosaveIcon,
				objConfiguration.AutosaveOnBlur,
				objConfiguration.AutosavePeriod,
				objConfiguration.HistoryLength,
				objConfiguration.HistoryExpiry,
				objConfiguration.AutosaveLocation,
				objConfiguration.UrlIndependent
				));
			return intResID;
		}

		public override void Configuration_Update(ConfigurationInfo objConfiguration)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Configuration_Update"),
				objConfiguration.ConfigurationID,
				objConfiguration.Title,
				objConfiguration.Description,
				objConfiguration.TabID,
				objConfiguration.AutosaveEnabled,
				objConfiguration.AutosaveIcon,
				objConfiguration.AutosaveOnBlur,
				objConfiguration.AutosavePeriod,
				objConfiguration.HistoryLength,
				objConfiguration.HistoryExpiry,
				objConfiguration.AutosaveLocation,
				objConfiguration.UrlIndependent
			);
		}

		public override void Configuration_Delete(int ConfigurationID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Configuration_Delete"),
				ConfigurationID);
		}

		public override IDataReader Configuration_GetByPrimaryKey(int ConfigurationID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Configuration_GetByPrimaryKey"),
				ConfigurationID);
		}

		public override IDataReader Configuration_GetAllItems(string SortBy)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Configuration_GetAllItems"), SortBy);
		}

		public override IDataReader Configuration_GetByTabID(int TabID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Configuration_GetByTabID"), TabID);
		}

		public override IDataReader Configuration_GetDistinctValues()
		{//todo проверить выборку
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Configuration_GetDistinctValues"));
		}

		#endregion

        //#region TabsInGlobalConfiguration Methods

        //public override void TabsInGlobalConfiguration_Add(int ConfigurationID, int TabID)
        //{
        //    SqlHelper.ExecuteScalar(ConnectionString,
        //        GetFullyQualifiedName("TabsInGlobalConfiguration_Add"),
        //        ConfigurationID,
        //        TabID);            
        //}

        //#endregion TabsInGlobalConfiguration Methods

        #region ConfigurationPermission Methods

        public override int ConfigurationPermission_Add(ConfigurationPermissionInfo objConfigurationPermission)
		{
			int intResID = -1;
			intResID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
				GetFullyQualifiedName("ConfigurationPermission_Add"),
				objConfigurationPermission.ConfigurationID,
				objConfigurationPermission.RoleID,
				objConfigurationPermission.UserID,
				objConfigurationPermission.AllowAccess));
			return intResID;
		}
		public override void ConfigurationPermission_Update(ConfigurationPermissionInfo objConfigurationPermission)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("ConfigurationPermission_Update"),
				objConfigurationPermission.ConfigurationPermissionID,
				objConfigurationPermission.ConfigurationID,
				objConfigurationPermission.RoleID,
				objConfigurationPermission.UserID,
				objConfigurationPermission.AllowAccess);
		}
		public override void ConfigurationPermission_Delete(int ConfigurationPermissionID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("ConfigurationPermission_Delete"),
				ConfigurationPermissionID);
		}
		public override void ConfigurationPermission_DeleteByConfigurationID(int ConfigurationID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("ConfigurationPermission_DeleteByConfigurationID"),
					ConfigurationID);
		}
		public override IDataReader ConfigurationPermission_GetByPrimaryKey(int ConfigurationPermissionID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("ConfigurationPermission_GetByPrimaryKey"),
				ConfigurationPermissionID);
		}
		public override IDataReader ConfigurationPermission_GetAllItems(string SortBy)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("ConfigurationPermission_GetAllItems"), SortBy);
		}
		public override IDataReader ConfigurationPermission_GetByConfigurationID(int ConfigurationID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("ConfigurationPermission_GetByConfigurationID"),
					ConfigurationID);
		}
		#endregion

		#region Control Methods

		public override int Control_Add(ControlInfo objControl)
		{
			int intResID = -1;
			intResID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
				GetFullyQualifiedName("Control_Add"),
				objControl.ConfigurationID,
				objControl.Selector,
				objControl.Enabled,
				objControl.RestoreOnLoad,
				objControl.RestoreIfEmpty,
				objControl.ShowCannedOnly,
                objControl.RTFType));
			return intResID;
		}

		public override void Control_Update(ControlInfo objControl)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Control_Update"),
				objControl.ControlID,
				objControl.ConfigurationID,
				objControl.Selector,
				objControl.Enabled,
				objControl.RestoreOnLoad,
				objControl.RestoreIfEmpty,
				objControl.ShowCannedOnly,
                objControl.RTFType);
		}

		public override void Control_Delete(int ControlID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Control_Delete"),
				ControlID);
		}

		public override IDataReader Control_GetByPrimaryKey(int ControlID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Control_GetByPrimaryKey"),
				ControlID);
		}

		public override IDataReader Control_GetAllItems(string SortBy)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Control_GetAllItems"), SortBy);
		}

		public override IDataReader Control_GetByFilter(int ConfigurationID, string Selector)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Control_GetByFilter"),
					ConfigurationID,
					Selector);
		}

		public override IDataReader Control_GetDistinctValues(int ConfigurationID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Control_GetDistinctValues"),
				ConfigurationID);
		}
	
		#endregion

		#region Value Methods

		public override int Value_Add(ValueInfo objValue)
		{
			int intResID = -1;
			intResID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
				GetFullyQualifiedName("Value_Add"),
				objValue.ControlID,
				objValue.UrlID,
				objValue.CreatedAt,
				objValue.UserID,
				objValue.Anonymous,
				objValue.Canned,
				objValue.Closed,
				objValue.Value));
			return intResID;
		}

		public override void Value_Update(ValueInfo objValue)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Value_Update"),
				objValue.ValueID,
				objValue.ControlID,
				objValue.UrlID,
				objValue.CreatedAt,
				objValue.UserID,
				objValue.Anonymous,
				objValue.Canned,
				objValue.Closed,
				objValue.Value);
		}

		public override void Value_Delete(int ValueID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Value_Delete"),
				ValueID);
		}

		public override IDataReader Value_GetByPrimaryKey(int ValueID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Value_GetByPrimaryKey"),
				ValueID);
		}

		public override IDataReader Value_GetAllItems(string SortBy)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Value_GetAllItems"), SortBy);
		}

		public override IDataReader Value_GetByFilter(int ControlID, int UrlID, int UserID, bool Anonymous, object dtFrom, object dtTo, bool CannedOnly)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Value_GetByFilter"),
					ControlID,
					UrlID,
					UserID,
					Anonymous,
					(dtFrom == null) ? DBNull.Value : dtFrom,
					(dtTo == null) ? DBNull.Value : dtTo,
					CannedOnly);
		}

		public override IDataReader Value_GetLastValue(int ControlID, int UrlID, int UserID, bool Anonymous)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Value_GetLastValue"),
					ControlID,
					UrlID,
					UserID,
					Anonymous);
		}

		public override IDataReader Value_UpdateClosed(int ControlID, int UrlID, int UserID, bool Anonymous, int HistoryLength, int HistoryExpiry)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Value_UpdateTrackChanges"),
					ControlID,
					UrlID,
					UserID,
					Anonymous,
					HistoryLength,
					HistoryExpiry);
		}

		public override void Value_CloseSession(int ConfigurationID, int UrlID, int UserID, bool Anonymous)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Value_CloseSession"),
					ConfigurationID,
					UrlID,
					UserID,
					Anonymous);
		}

		#endregion

		#region Anonymous Methods

		public override int Anonymous_Add(AnonymousInfo objAnonymous)
		{
			int intResID = -1;
			intResID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
				GetFullyQualifiedName("Anonymous_Add"),
				objAnonymous.UserGUID,
				objAnonymous.CreatedAt,
				objAnonymous.IP));
			return intResID;
		}

		public override void Anonymous_Update(AnonymousInfo objAnonymous)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Anonymous_Update"),
				objAnonymous.AnonymousID,
				objAnonymous.UserGUID,
				objAnonymous.CreatedAt,
				objAnonymous.IP);
		}

		public override void Anonymous_Delete(int AnonymousID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Anonymous_Delete"),
				AnonymousID);
		}

		public override IDataReader Anonymous_GetByPrimaryKey(int AnonymousID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Anonymous_GetByPrimaryKey"),
				AnonymousID);
		}

		public override IDataReader Anonymous_GetByUserGUID(System.Guid UserGUID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Anonymous_GetByUserGUID"),
				UserGUID);
		}

		public override IDataReader Anonymous_GetAllItems(string SortBy)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Anonymous_GetAllItems"), SortBy);
		}

		public override IDataReader Anonymous_GetDistinctValues()
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Anonymous_GetDistinctValues"));
		}

		#endregion

		#region Url Methods

		public override int Url_Add(UrlInfo objUrl)
		{
			int intResID = -1;
			intResID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
				GetFullyQualifiedName("Url_Add"),
				objUrl.Url));
			return intResID;
		}

		public override void Url_Update(UrlInfo objUrl)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Url_Update"),
				objUrl.UrlID,
				objUrl.Url);
		}

		public override void Url_Delete(int UrlID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Url_Delete"),
				UrlID);
		}

		public override IDataReader Url_GetByPrimaryKey(int UrlID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Url_GetByPrimaryKey"),
				UrlID);
		}

		public override IDataReader Url_GetByUrl(string Url)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Url_GetByUrl"),
					Url);
		}

		public override IDataReader Url_GetAllItems(string SortBy)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Url_GetAllItems"), SortBy);
		}

		#endregion

		#region Event Methods

		public override int Event_Add(EventInfo objEvent)
		{
			int intResID = -1;
			intResID = Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString,
				GetFullyQualifiedName("Event_Add"),
				objEvent.ConfigurationID,
				objEvent.Selector,
				objEvent.EventName,
				objEvent.Enabled));
			return intResID;
		}

		public override void Event_Update(EventInfo objEvent)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Event_Update"),
				objEvent.EventID,
				objEvent.ConfigurationID,
				objEvent.Selector,
				objEvent.EventName,
				objEvent.Enabled);
		}

		public override void Event_Delete(int EventID)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString,
				GetFullyQualifiedName("Event_Delete"),
				EventID);
		}

		public override IDataReader Event_GetByPrimaryKey(int EventID)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Event_GetByPrimaryKey"),
				EventID);
		}

		public override IDataReader Event_GetAllItems(string SortBy)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Event_GetAllItems"), SortBy);
		}

		public override IDataReader Event_GetByFilter(int ConfigurationID, string Selector)
		{
			return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
				GetFullyQualifiedName("Event_GetByFilter"),
					ConfigurationID,
					Selector);
		}

		#endregion
	}
}