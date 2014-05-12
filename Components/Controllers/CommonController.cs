using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.UI.WebControls;

using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Localization;

namespace forDNN.Modules.UniversalAutosave
{
	public class CommonController
	{
		public static bool UseCache = false;

		#region Cache

		public static void UpdateCache(string KeyFormat, object KeyID, object objCache, bool Remove)
		{
			if (!UseCache)
			{
				return;
			}
			string CacheKey = string.Format(KeyFormat, KeyID);
			if (Remove)
			{
				DotNetNuke.Common.Utilities.DataCache.RemoveCache(CacheKey);
			}
			else
			{
				DotNetNuke.Common.Utilities.DataCache.SetCache(CacheKey, objCache);
			}
		}

		#endregion

		#region Common

		public static void PopulateDropDown(DropDownList ddlSource, object lstItems, string TextField, string ValueField, bool IncludeAll)
		{
			ddlSource.DataSource = lstItems;
			ddlSource.DataTextField = TextField;
			ddlSource.DataValueField = ValueField;
			ddlSource.DataBind();

			if (IncludeAll)
			{
				ddlSource.Items.Insert(0, new ListItem(Localization.GetString("All", CommonController.GetCommonResourcesPath()), "0"));
			}
		}

		public static bool IsAdmin()
		{
			DotNetNuke.Entities.Users.UserInfo objUser = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo();
			if (objUser.UserID > 0)
			{
				if (objUser.IsSuperUser)
				{
					return true;
				}
				return objUser.IsInRole(DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings().AdministratorRoleName);
			}
			return false;
		}

		public static string ResolveUrl(string FileName, bool VirtualPathOnly)
		{
			System.Web.HttpRequest objRequest = System.Web.HttpContext.Current.Request;
			string Secure = "";
			string Port = "";

			if (objRequest.IsSecureConnection)
			{
				Secure = "s";
			}
			if (!objRequest.Url.IsDefaultPort)
			{
				Port = ":" + objRequest.Url.Port.ToString();
			}

			string FullRoot = "";
			if (!VirtualPathOnly)
			{
				FullRoot = string.Format("http{0}://{1}{2}",
					Secure,
					objRequest.Url.Host,
					Port);
			}

			string Result = string.Format("{0}{1}DesktopModules/forDNN.UniversalAutosave/{2}",
				FullRoot,
				(objRequest.ApplicationPath == "") ? "" : string.Format("{0}/", objRequest.ApplicationPath),
				FileName);

			return Result;
		}

		public static int GetCurrentUrlID()
		{
			//we have to sort query params, so UrlID will be independent from the order of query params
			NameValueCollection nvQueryString = System.Web.HttpContext.Current.Request.QueryString;
			List<string> lstKeys = new List<string>();
			foreach (string Key in nvQueryString.Keys)
			{
				lstKeys.Add(Key);
			}
			lstKeys.Sort();

			StringBuilder sbUrl = new StringBuilder();
			foreach (string Key in lstKeys)
			{
				sbUrl.AppendFormat("{2}{0}={1}",
					Key,
					nvQueryString[Key],
					(sbUrl.Length == 0) ? "" : "&");
			}

			return UrlController.Url_GetByUrlOrCreate(sbUrl.ToString()).UrlID;
		}

		public static string GetCommonResourcesPath()
		{
			return ResolveUrl("App_LocalResources/Common.ascx.resx", true);
		}

		public static void AddConfirmation(WebControl objControl)
		{
			objControl.Attributes.Add("onclick",
				string.Format("javascript: return confirm('{0}');", Localization.GetString("AreYouSure", GetCommonResourcesPath())));
		}

		public static bool ConfigAllowed(UserInfo objUser, ConfigurationInfo objConfig)
		{
			List<ConfigurationPermissionInfo> lstPermissions =
				ConfigurationPermissionController.ConfigurationPermission_GetByConfigurationID(objConfig.ConfigurationID);
			foreach (ConfigurationPermissionInfo objPermission in lstPermissions)
			{
				if (objPermission.RoleID < 0)
				{
					objPermission.RoleName = string.Format("[{0}]", objPermission.RoleID);
				}
				if ((objPermission.AllowAccess) && (!objUser.IsInRole(objPermission.RoleName)))
				{
					return false;
				}

				if ((!objPermission.AllowAccess) && (objUser.IsInRole(objPermission.RoleName)))
				{
					return false;
				}
			}
			return true;
		}

		#endregion

	}
}
