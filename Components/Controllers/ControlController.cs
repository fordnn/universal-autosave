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
	public class ControlController
	{
		#region Public Methods

		public static string KeyFormat = "forDNN_UniversalAutosave_ControlID_{0}";

		public static int Control_Add(ControlInfo objControl)
		{
			objControl.ControlID = ((int)DataProvider.Instance().Control_Add(objControl));
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objControl.ControlID, objControl, false);
			}
			return objControl.ControlID;
		}
		public static void Control_Delete(ControlInfo objControl)
		{
			ControlController.Control_Delete(objControl.ControlID);
		}
		public static void Control_Delete(int ControlID)
		{
			DataProvider.Instance().Control_Delete(ControlID);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, ControlID, null, true);
			}
		}
		public static void Control_Update(ControlInfo objControl)
		{
			DataProvider.Instance().Control_Update(objControl);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objControl.ControlID, objControl, false);
			}
		}
		public static ControlInfo Control_GetByPrimaryKey(int ControlID)
		{
			object objControl = null;
			if (CommonController.UseCache)
			{
				objControl = DotNetNuke.Common.Utilities.DataCache.GetCache(string.Format(KeyFormat, ControlID));
			}
			if (objControl == null)
			{
				objControl = ((ControlInfo)CBO.FillObject(DataProvider.Instance().Control_GetByPrimaryKey(ControlID), typeof(ControlInfo)));
				if (CommonController.UseCache)
				{
					DotNetNuke.Common.Utilities.DataCache.SetCache(string.Format(KeyFormat, ControlID), objControl);
				}
			}
			return (ControlInfo)objControl;
		}
		public static List<ControlInfo> Control_GetAllItems(string SortBy)
		{
			return CBO.FillCollection<ControlInfo>(DataProvider.Instance().Control_GetAllItems(SortBy));
		}
		public static List<ControlInfo> Control_GetByFilter(int ConfigurationID, string Selector)
		{
			return CBO.FillCollection<ControlInfo>(DataProvider.Instance().Control_GetByFilter(ConfigurationID, Selector));
		}
		public static List<ControlInfo> Control_GetDistinctValues(int ConfigurationID)
		{
			return CBO.FillCollection<ControlInfo>(DataProvider.Instance().Control_GetDistinctValues(ConfigurationID));
		}

		#endregion
	}
}
