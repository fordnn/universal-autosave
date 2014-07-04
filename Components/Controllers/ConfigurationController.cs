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
	public class ConfigurationController
	{

		public ConfigurationInfo GlobalConfiguration()
		{
			ConfigurationInfo objGlobalConfiguration = new ConfigurationInfo();
			objGlobalConfiguration.AutosaveEnabled = true;
			objGlobalConfiguration.AutosaveIcon = true;
			objGlobalConfiguration.AutosaveLocation = 0;
			objGlobalConfiguration.AutosaveOnBlur = true;
			objGlobalConfiguration.AutosavePeriod = 30;
			objGlobalConfiguration.HistoryExpiry = 0;
			objGlobalConfiguration.HistoryLength = 0;
			objGlobalConfiguration.TabID = -1;
			objGlobalConfiguration.UrlIndependent = true;
			return objGlobalConfiguration;
		}
	

		#region Public Methods
		
		public static string KeyFormat = "forDNN_UniversalAutosave_ConfigurationID_{0}";

		public static ConfigurationInfoLimited ConfigurationInfoToLimited(ConfigurationInfo objSource)
		{
			return new ConfigurationInfoLimited(objSource);
		}

		public static int Configuration_Add(ConfigurationInfo objConfiguration)
		{
			objConfiguration.ConfigurationID = ((int)DataProvider.Instance().Configuration_Add(objConfiguration));
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objConfiguration.ConfigurationID, objConfiguration, false);
			}
			return objConfiguration.ConfigurationID;
		}

		public static void Configuration_Delete(ConfigurationInfo objConfiguration)
		{
			ConfigurationController.Configuration_Delete(objConfiguration.ConfigurationID);
		}

		public static void Configuration_Delete(int ConfigurationID)
		{
			DataProvider.Instance().Configuration_Delete(ConfigurationID);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, ConfigurationID, null, true);
			}
		}

		public static void Configuration_Update(ConfigurationInfo objConfiguration)
		{
			DataProvider.Instance().Configuration_Update(objConfiguration);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objConfiguration.ConfigurationID, objConfiguration, false);
			}
		}

		public static ConfigurationInfo Configuration_GetByPrimaryKey(int ConfigurationID)
		{
			object objConfiguration = null;
			if (CommonController.UseCache)
			{
				objConfiguration = DotNetNuke.Common.Utilities.DataCache.GetCache(string.Format(KeyFormat, ConfigurationID));
			}
			if (objConfiguration == null)
			{
				objConfiguration = ((ConfigurationInfo)CBO.FillObject(DataProvider.Instance().Configuration_GetByPrimaryKey(ConfigurationID), typeof(ConfigurationInfo)));
				if (CommonController.UseCache)
				{
					DotNetNuke.Common.Utilities.DataCache.SetCache(string.Format(KeyFormat, ConfigurationID), objConfiguration);
				}
			}
			return (ConfigurationInfo)objConfiguration;
		}

		public static List<ConfigurationInfo> Configuration_GetAllItems(string SortBy)
		{
			return CBO.FillCollection<ConfigurationInfo>(DataProvider.Instance().Configuration_GetAllItems(SortBy));
		}

		public static List<ConfigurationInfo> Configuration_GetByTabID(int TabID)
		{
			return CBO.FillCollection<ConfigurationInfo>(DataProvider.Instance().Configuration_GetByTabID(TabID));
		}

		public static List<ConfigurationInfo> Configuration_GetDistinctValues()
		{
			return CBO.FillCollection<ConfigurationInfo>(DataProvider.Instance().Configuration_GetDistinctValues());
		}

		#endregion
	}
}
