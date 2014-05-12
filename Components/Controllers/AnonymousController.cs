using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Users;

namespace forDNN.Modules.UniversalAutosave
{
	public class AnonymousController
	{
		#region Public Methods

		public static string KeyFormat = "forDNN_UniversalAutosave_AnonymousID_{0}";

		public static int Anonymous_Add(AnonymousInfo objAnonymous)
		{
			objAnonymous.AnonymousID = ((int)DataProvider.Instance().Anonymous_Add(objAnonymous));
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objAnonymous.AnonymousID, objAnonymous, false);
			}
			return objAnonymous.AnonymousID;
		}

		public static void Anonymous_Delete(AnonymousInfo objAnonymous)
		{
			AnonymousController.Anonymous_Delete(objAnonymous.AnonymousID);
		}

		public static void Anonymous_Delete(int AnonymousID)
		{
			DataProvider.Instance().Anonymous_Delete(AnonymousID);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, AnonymousID, null, true);
			}
		}
		
		public static void Anonymous_Update(AnonymousInfo objAnonymous)
		{
			DataProvider.Instance().Anonymous_Update(objAnonymous);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objAnonymous.AnonymousID, objAnonymous, false);
			}
		}

		public static AnonymousInfo Anonymous_GetByPrimaryKey(int AnonymousID)
		{
			object objAnonymous = null;
			if (CommonController.UseCache)
			{
				objAnonymous = DotNetNuke.Common.Utilities.DataCache.GetCache(string.Format(KeyFormat, AnonymousID));
			}
			if (objAnonymous == null)
			{
				objAnonymous = ((AnonymousInfo)CBO.FillObject(DataProvider.Instance().Anonymous_GetByPrimaryKey(AnonymousID), typeof(AnonymousInfo)));
				if (CommonController.UseCache)
				{
					DotNetNuke.Common.Utilities.DataCache.SetCache(string.Format(KeyFormat, AnonymousID), objAnonymous);
				}
			}
			return (AnonymousInfo)objAnonymous;
		}

		public static AnonymousInfo Anonymous_GetByUserGUID(System.Guid UserGUID)
		{
			return ((AnonymousInfo)CBO.FillObject(DataProvider.Instance().Anonymous_GetByUserGUID(UserGUID), typeof(AnonymousInfo)));
		}

		public static AnonymousInfo Anonymous_GetByUserGUIDorCreate(System.Guid UserGUID)
		{
			AnonymousInfo objAnonymous = Anonymous_GetByUserGUID(UserGUID);
			if(objAnonymous==null)
			{
				objAnonymous = new AnonymousInfo();
				objAnonymous.UserGUID = UserGUID;
				objAnonymous.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
				objAnonymous.AnonymousID = AnonymousController.Anonymous_Add(objAnonymous);
			}
			return objAnonymous;
		}

		public static List<AnonymousInfo> Anonymous_GetAllItems(string SortBy)
		{
			return CBO.FillCollection<AnonymousInfo>(DataProvider.Instance().Anonymous_GetAllItems(SortBy));
		}

		public static List<TempUserInfo> Anonymous_GetDistinctValues()
		{
			IDataReader idr = DataProvider.Instance().Anonymous_GetDistinctValues();
			return CBO.FillCollection <TempUserInfo>(idr);
		}

		#endregion
	}
}
