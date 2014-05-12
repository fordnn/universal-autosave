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
	public class ValueController
	{
		#region Public Methods

		public static string KeyFormat = "forDNN_UniversalAutosave_ValueID_{0}";

		public static int Value_Add(ValueInfo objValue)
		{
			objValue.ValueID = ((int)DataProvider.Instance().Value_Add(objValue));
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objValue.ValueID, objValue, false);
			}
			return objValue.ValueID;
		}

		public static void Value_Delete(ValueInfo objValue)
		{
			ValueController.Value_Delete(objValue.ValueID);
		}

		public static void Value_Delete(int ValueID)
		{
			DataProvider.Instance().Value_Delete(ValueID);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, ValueID, null, true);
			}
		}

		public static void Value_Update(ValueInfo objValue)
		{
			DataProvider.Instance().Value_Update(objValue);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objValue.ValueID, objValue, false);
			}
		}

		public static ValueInfo Value_GetByPrimaryKey(int ValueID)
		{
			object objValue = null;
			if (CommonController.UseCache)
			{
				objValue = DotNetNuke.Common.Utilities.DataCache.GetCache(string.Format(KeyFormat, ValueID));
			}
			if (objValue == null)
			{
				objValue = ((ValueInfo)CBO.FillObject(DataProvider.Instance().Value_GetByPrimaryKey(ValueID), typeof(ValueInfo)));
				if (CommonController.UseCache)
				{
					DotNetNuke.Common.Utilities.DataCache.SetCache(string.Format(KeyFormat, ValueID), objValue);
				}
			}
			return (ValueInfo)objValue;
		}

		public static List<ValueInfo> Value_GetAllItems(string SortBy)
		{
			return CBO.FillCollection<ValueInfo>(DataProvider.Instance().Value_GetAllItems(SortBy));
		}

		public static List<ValueInfo> Value_GetByFilter(int ControlID, int UrlID, int UserID, bool Anonymous, object dtFrom, object dtTo, bool CannedOnly)
		{
			return CBO.FillCollection<ValueInfo>(DataProvider.Instance().Value_GetByFilter(ControlID, UrlID, UserID, Anonymous, dtFrom, dtTo, CannedOnly));
		}

		public static string Value_GetLastValue(int ControlID, int UrlID, int UserID, bool Anonymous)
		{
			ValueInfo objValue = CBO.FillObject<ValueInfo>(DataProvider.Instance().Value_GetLastValue(ControlID, UrlID, UserID, Anonymous));
			return (objValue == null) ? "" : objValue.Value;
		}

		public static List<ValueInfo> Value_UpdateClosed(int ControlID, int UrlID, int UserID, bool Anonymous, int HistoryLength, int HistoryExpiry)
		{
			return CBO.FillCollection<ValueInfo>(DataProvider.Instance().Value_UpdateClosed(ControlID, UrlID, UserID, Anonymous, HistoryLength, HistoryExpiry));
		}

		public static void Value_CloseSession(int ConfigurationID, int UrlID, int UserID, bool Anonymous)
		{
			DataProvider.Instance().Value_CloseSession(ConfigurationID, UrlID, UserID, Anonymous);
		}

		#endregion
	}
}
