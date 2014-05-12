using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace forDNN.Modules.UniversalAutosave
{
	public class ConfigurationPermissionController
	{
		#region Public Methods

		public static string KeyFormat = "forDNN_UniversalAutosave_ConfigurationPermissionID_{0}";

		public static int ConfigurationPermission_Add(ConfigurationPermissionInfo objConfigurationPermission)
		{
			objConfigurationPermission.ConfigurationPermissionID = ((int)DataProvider.Instance().ConfigurationPermission_Add(objConfigurationPermission));
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objConfigurationPermission.ConfigurationPermissionID, objConfigurationPermission, false);
			}
			return objConfigurationPermission.ConfigurationPermissionID;
		}
		
		public static void ConfigurationPermission_Delete(ConfigurationPermissionInfo objConfigurationPermission)
		{
			ConfigurationPermissionController.ConfigurationPermission_Delete(objConfigurationPermission.ConfigurationPermissionID);
		}
		
		public static void ConfigurationPermission_Delete(int ConfigurationPermissionID)
		{
			DataProvider.Instance().ConfigurationPermission_Delete(ConfigurationPermissionID);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, ConfigurationPermissionID, null, true);
			}
		}

		public static void ConfigurationPermission_DeleteByConfigurationID(int ConfigurationID)
		{
			DataProvider.Instance().ConfigurationPermission_DeleteByConfigurationID(ConfigurationID);
		}

		public static void ConfigurationPermission_Update(ConfigurationPermissionInfo objConfigurationPermission)
		{
			DataProvider.Instance().ConfigurationPermission_Update(objConfigurationPermission);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objConfigurationPermission.ConfigurationPermissionID, objConfigurationPermission, false);
			}
		}
		
		public static ConfigurationPermissionInfo ConfigurationPermission_GetByPrimaryKey(int ConfigurationPermissionID)
		{
			object objConfigurationPermission = null;
			if (CommonController.UseCache)
			{
				objConfigurationPermission = DotNetNuke.Common.Utilities.DataCache.GetCache(string.Format(KeyFormat, ConfigurationPermissionID));
			}
			if (objConfigurationPermission == null)
			{
				objConfigurationPermission = ((ConfigurationPermissionInfo)CBO.FillObject(DataProvider.Instance().ConfigurationPermission_GetByPrimaryKey(ConfigurationPermissionID), typeof(ConfigurationPermissionInfo)));
				if (CommonController.UseCache)
				{
					DotNetNuke.Common.Utilities.DataCache.SetCache(string.Format(KeyFormat, ConfigurationPermissionID), objConfigurationPermission);
				}
			}
			return (ConfigurationPermissionInfo)objConfigurationPermission;
		}
		
		public static List<ConfigurationPermissionInfo> ConfigurationPermission_GetAllItems(string SortBy)
		{
			return CBO.FillCollection<ConfigurationPermissionInfo>(DataProvider.Instance().ConfigurationPermission_GetAllItems(SortBy));
		}
		
		public static List<ConfigurationPermissionInfo> ConfigurationPermission_GetByConfigurationID(int ConfigurationID)
		{
			return CBO.FillCollection<ConfigurationPermissionInfo>(DataProvider.Instance().ConfigurationPermission_GetByConfigurationID(ConfigurationID));
		}

		#endregion
	}
}
