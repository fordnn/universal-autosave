using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Security.Permissions;
using DotNetNuke.Security.Permissions.Controls;

namespace forDNN.Modules.UniversalAutosave.Controls
{
	public class UaPermissionsGrid : PermissionsGrid
	{
		#region "Private Members"

		private List<ConfigurationPermissionInfo> _UaPermissions;
		private List<PermissionInfoBase> _PermissionsList;

		#endregion

		#region "Properties"

		public string LocalResourceFile
		{
			get
			{
				return (string)ViewState["LocalResourceFile"];
			}
			set
			{
				ViewState["LocalResourceFile"] = value;
				ResourceFile = value;
			}
		}

		public int ConfigurationID
		{
			get;
			set;
		}

		protected override List<PermissionInfoBase> PermissionsList
		{
			get
			{
				return _PermissionsList;
			}
		}

		private void UpdatePermissionsList()
		{
			_PermissionsList = new List<PermissionInfoBase>();
			if (_UaPermissions != null)
			{
				foreach (ConfigurationPermissionInfo objInfo in _UaPermissions.FindAll(
					delegate(ConfigurationPermissionInfo objCPInfo)
					{
						return (objCPInfo.UserID > 0);
					}
					))
				{
					_PermissionsList.Add(objInfo);
				}
			}
		}

		public List<ConfigurationPermissionInfo> Permissions
		{
			get
			{
				//First Update Permissions in case they have been changed
				UpdatePermissions();

				//Return the Permissions
				return _UaPermissions;
			}
			set
			{
				_UaPermissions = value;

				UpdatePermissionsList();
			}
		}

		private void GetUaPermissions()
		{
			if (_UaPermissions == null)
			{
				_UaPermissions = new List<ConfigurationPermissionInfo>();
			}
		}

		public override void DataBind()
		{
			GetUaPermissions();
			base.DataBind();
		}

		private ConfigurationPermissionInfo ParseKeys(string[] Settings)
		{
			var objPermission = new ConfigurationPermissionInfo();

			base.ParsePermissionKeys(objPermission, Settings);

			return objPermission;
		}

		protected override void AddPermission(PermissionInfo permission, int roleId, string roleName, int userId, string displayName, bool allowAccess)
		{
			var objPermission = new ConfigurationPermissionInfo(permission);
			objPermission.RoleID = roleId;
			objPermission.RoleName = roleName;
			objPermission.AllowAccess = allowAccess;
			objPermission.UserID = userId;
			objPermission.DisplayName = displayName;
			_UaPermissions.Add(objPermission);
			UpdatePermissionsList();
		}

		protected override void AddPermission(ArrayList permissions, UserInfo user)
		{
			bool isMatch = false;
			foreach (ConfigurationPermissionInfo objPermission in _UaPermissions)
			{
				if (objPermission.UserID == user.UserID)
				{
					isMatch = true;
					break;
				}
			}

			//user not found so add new
			if (!isMatch)
			{
				foreach (PermissionInfo objPermission in permissions)
				{
					AddPermission(objPermission, int.Parse(Globals.glbRoleNothing), Null.NullString, user.UserID, user.DisplayName, true);
				}
			}
		}

		protected override bool GetEnabled(PermissionInfo objPerm, RoleInfo role, int column)
		{
			return true;
			//return (role.RoleID != AdministratorRoleId);
		}

		protected override string GetPermission(PermissionInfo objPerm, UserInfo user, int column, string defaultState)
		{
			var stateKey = defaultState;
			if (PermissionsList != null)
			{
				ConfigurationPermissionInfo objCPInfo =
					_UaPermissions.Find(
						delegate(ConfigurationPermissionInfo objTempInfo)
						{
							return (objTempInfo.UserID == user.UserID);
						}
					);
				if (objCPInfo != null)
				{
					stateKey = objCPInfo.AllowAccess ? PermissionTypeGrant : PermissionTypeDeny;
				}
			}
			return stateKey;
		}

		protected override string GetPermission(PermissionInfo objPerm, RoleInfo role, int column, string defaultState)
		{
			string permission;

			//if (role.RoleID == AdministratorRoleId)
			//{
			//    permission = PermissionTypeGrant;
			//}
			//else
			//{
			permission = defaultState;
			ConfigurationPermissionInfo objCPInfo =
				_UaPermissions.Find(
					delegate(ConfigurationPermissionInfo objTempInfo)
					{
						return (objTempInfo.RoleID == role.RoleID);
					}
				);
			if (objCPInfo != null)
			{
				permission = objCPInfo.AllowAccess ? PermissionTypeGrant : PermissionTypeDeny;
			}
			//}
			return permission;
		}

		protected override ArrayList GetPermissions()
		{
			PermissionInfo objPermission = new PermissionInfo();
			ConfigurationPermissionInfo objUaPermission = new ConfigurationPermissionInfo();
			objUaPermission.PermissionName = "AllowUsage";
			objUaPermission.PermissionKey = "AllowUsage";
			objUaPermission.PermissionCode = "AllowUsage";
			objUaPermission.PermissionID = 1;
			objUaPermission.ModuleDefID = 1;
			ArrayList lstPermissions = new ArrayList();
			lstPermissions.Add(objUaPermission);

			return lstPermissions;
		}

		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				//Load State from the array of objects that was saved with SaveViewState.
				var myState = (object[])savedState;

				//Load Base Controls ViewState
				if (myState[0] != null)
				{
					base.LoadViewState(myState[0]);
				}

				if (myState[1] != null)
				{
					ConfigurationID = Convert.ToInt32(myState[1]);
				}

				//Load Permissions
				if (myState[2] != null)
				{
					_UaPermissions = new List<ConfigurationPermissionInfo>();
					string state = Convert.ToString(myState[2]);
					if (!String.IsNullOrEmpty(state))
					{
						//First Break the String into individual Keys
						string[] permissionKeys = state.Split(new[] { "##" }, StringSplitOptions.None);
						foreach (string key in permissionKeys)
						{
							string[] Settings = key.Split('|');
							_UaPermissions.Add(ParseKeys(Settings));
						}
					}
				}
			}
		}

		protected override void RemovePermission(int permissionID, int roleID, int userID)
		{
			ConfigurationPermissionInfo objInfoToDelete =
				_UaPermissions.Find(
				delegate(ConfigurationPermissionInfo objCPInfo)
				{
					return ((objCPInfo.RoleID == roleID) && (objCPInfo.UserID == userID));
				}
			);
			_UaPermissions.Remove(objInfoToDelete);

			UpdatePermissionsList();
		}

		protected override object SaveViewState()
		{
			var allStates = new object[3];

			//Save the Base Controls ViewState
			allStates[0] = base.SaveViewState();

			//Save the Id
			allStates[1] = ConfigurationID;

			//Persist the Permisisons
			var sb = new StringBuilder();
			if (_UaPermissions != null)
			{
				bool addDelimiter = false;
				foreach (ConfigurationPermissionInfo objUaPermission in _UaPermissions)
				{
					if ((objUaPermission.RoleID == 0) || (objUaPermission.UserID == 0))
					{
						continue;
					}

					if (addDelimiter)
					{
						sb.Append("##");
					}
					else
					{
						addDelimiter = true;
					}
					sb.Append(BuildKey(objUaPermission.AllowAccess,
									   objUaPermission.PermissionID,
									   ConfigurationID,
									   objUaPermission.RoleID,
									   objUaPermission.RoleName,
									   objUaPermission.UserID,
									   objUaPermission.DisplayName));
				}
			}
			allStates[2] = sb.ToString();
			return allStates;
		}

		protected override bool SupportsDenyPermissions(PermissionInfo permissionInfo)
		{
			return true;
		}

		#endregion

		#region "Public Methods"

		public override void GenerateDataGrid()
		{
		}

		#endregion
	}
}
