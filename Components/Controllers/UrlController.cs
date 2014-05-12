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
	public class UrlController
	{
		#region Public Methods

		public static string KeyFormat = "forDNN_UniversalAutosave_UrlID_{0}";

		public static int Url_Add(UrlInfo objUrl)
		{
			objUrl.UrlID = ((int)DataProvider.Instance().Url_Add(objUrl));
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objUrl.UrlID, objUrl, false);
			}
			return objUrl.UrlID;
		}

		public static void Url_Delete(UrlInfo objUrl)
		{
			UrlController.Url_Delete(objUrl.UrlID);
		}

		public static void Url_Delete(int UrlID)
		{
			DataProvider.Instance().Url_Delete(UrlID);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, UrlID, null, true);
			}
		}

		public static void Url_Update(UrlInfo objUrl)
		{
			DataProvider.Instance().Url_Update(objUrl);
			if (CommonController.UseCache)
			{
				CommonController.UpdateCache(KeyFormat, objUrl.UrlID, objUrl, false);
			}
		}

		public static UrlInfo Url_GetByPrimaryKey(int UrlID)
		{
			object objUrl = null;
			if (CommonController.UseCache)
			{
				objUrl = DotNetNuke.Common.Utilities.DataCache.GetCache(string.Format(KeyFormat, UrlID));
			}
			if (objUrl == null)
			{
				objUrl = ((UrlInfo)CBO.FillObject(DataProvider.Instance().Url_GetByPrimaryKey(UrlID), typeof(UrlInfo)));
				if (CommonController.UseCache)
				{
					DotNetNuke.Common.Utilities.DataCache.SetCache(string.Format(KeyFormat, UrlID), objUrl);
				}
			}
			return (UrlInfo)objUrl;
		}

		public static UrlInfo Url_GetByUrlOrCreate(string Url)
		{
			UrlInfo objUrl = Url_GetByUrl(Url);
			if (objUrl == null)
			{
				objUrl = new UrlInfo();
				objUrl.Url = Url;
				objUrl.UrlID = UrlController.Url_Add(objUrl);
			}
			return objUrl;
		}

		public static UrlInfo Url_GetByUrl(string Url)
		{
			return CBO.FillObject<UrlInfo>(DataProvider.Instance().Url_GetByUrl(Url));
		}

		public static List<UrlInfo> Url_GetAllItems(string SortBy)
		{
			return CBO.FillCollection<UrlInfo>(DataProvider.Instance().Url_GetAllItems(SortBy));
		}

		#endregion
	}
}
