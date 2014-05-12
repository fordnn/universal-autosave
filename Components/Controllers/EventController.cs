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
	public class EventController
	{
		#region Public Methods

		public static string KeyFormat = "forDNN_UniversalAutosave_EventID_{0}";

		public static int Event_Add(EventInfo objEvent)
		{
			objEvent.EventID = ((int)DataProvider.Instance().Event_Add(objEvent));
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objEvent.EventID, objEvent, false);
			}
			return objEvent.EventID;
		}

		public static void Event_Delete(EventInfo objEvent)
		{
			EventController.Event_Delete(objEvent.EventID);
		}

		public static void Event_Delete(int EventID)
		{
			DataProvider.Instance().Event_Delete(EventID);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, EventID, null, true);
			}
		}

		public static void Event_Update(EventInfo objEvent)
		{
			DataProvider.Instance().Event_Update(objEvent);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objEvent.EventID, objEvent, false);
			}
		}

		public static EventInfo Event_GetByPrimaryKey(int EventID)
		{
			object objEvent = null;
			if (CommonController.UseCache)
			{
				objEvent = DotNetNuke.Common.Utilities.DataCache.GetCache(string.Format(KeyFormat, EventID));
			}
			if (objEvent == null)
			{
				objEvent = ((EventInfo)CBO.FillObject(DataProvider.Instance().Event_GetByPrimaryKey(EventID), typeof(EventInfo)));
				if (CommonController.UseCache)
				{
					DotNetNuke.Common.Utilities.DataCache.SetCache(string.Format(KeyFormat, EventID), objEvent);
				}
			}
			return (EventInfo)objEvent;
		}

		public static List<EventInfo> Event_GetAllItems(string SortBy)
		{
			return CBO.FillCollection<EventInfo>(DataProvider.Instance().Event_GetAllItems(SortBy));
		}

		public static List<EventInfo> Event_GetByFilter(int ConfigurationID, string Selector)
		{
			return CBO.FillCollection<EventInfo>(DataProvider.Instance().Event_GetByFilter(ConfigurationID, Selector));
		}

		#endregion
	}
}
