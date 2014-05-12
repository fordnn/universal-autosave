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

namespace forDNN.Modules.UniversalAutosave
{
	public class ConfigurationPermissionInfo : PermissionInfoBase
	{
		#region Private Members

		private int _ConfigurationPermissionID;
		private int _ConfigurationID;

		#endregion

		#region Constructors

		public ConfigurationPermissionInfo()
		{ }

		public ConfigurationPermissionInfo(PermissionInfo permission)
		{
			ModuleDefID = permission.ModuleDefID;
			PermissionCode = permission.PermissionCode;
			PermissionID = permission.PermissionID;
			PermissionKey = permission.PermissionKey;
			PermissionName = permission.PermissionName;
		}
		
		#endregion

		#region Public Properties


		public int ConfigurationPermissionID
		{
			get
			{
				return _ConfigurationPermissionID;
			}
			set
			{
				_ConfigurationPermissionID= value;
			}
		}

		public int ConfigurationID
		{
			get
			{
				return _ConfigurationID;
			}
			set
			{
				_ConfigurationID= value;
			}
		}

		#endregion
	}

	[Serializable]
	public class ConfigurationPermissionCollection : System.Collections.CollectionBase
	{
		#region Collection

		public ConfigurationPermissionCollection(List<ConfigurationPermissionInfo> uaPermissions)
		{
			foreach (ConfigurationPermissionInfo objInfo in uaPermissions)
			{
				Add(objInfo);
			}
		}

		public ConfigurationPermissionCollection(ArrayList uaPermissions)
		{
			AddRange(uaPermissions);
		}

		public ConfigurationPermissionCollection(ConfigurationPermissionCollection uaPermissions)
		{
			AddRange(uaPermissions);
		}

		public ConfigurationPermissionCollection()
		{
		}

		public ConfigurationPermissionInfo this[int index]
		{
			get
			{
				return (ConfigurationPermissionInfo)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		public int Add(ConfigurationPermissionInfo value)
		{
			return List.Add(value);
		}

		public int Add(ConfigurationPermissionInfo value, bool checkForDuplicates)
		{
			int id = Null.NullInteger;

			if (!checkForDuplicates)
			{
				id = Add(value);
			}
			else
			{
				bool isMatch = false;
				foreach (PermissionInfoBase permission in List)
				{
					if (permission.PermissionID == value.PermissionID && permission.UserID == value.UserID && permission.RoleID == value.RoleID)
					{
						isMatch = true;
						break;
					}
				}
				if (!isMatch)
				{
					id = Add(value);
				}
			}

			return id;
		}

		public void AddRange(ArrayList uaPermissions)
		{
			foreach (ConfigurationPermissionInfo permission in uaPermissions)
			{
				Add(permission);
			}
		}

		public void AddRange(IEnumerable<ConfigurationPermissionInfo> uaPermissions)
		{
			foreach (ConfigurationPermissionInfo permission in uaPermissions)
			{
				Add(permission);
			}
		}

		public void AddRange(ConfigurationPermissionCollection uaPermissions)
		{
			foreach (ConfigurationPermissionInfo permission in uaPermissions)
			{
				Add(permission);
			}
		}

		public bool Contains(ConfigurationPermissionInfo value)
		{
			return List.Contains(value);
		}

		public int IndexOf(ConfigurationPermissionInfo value)
		{
			return List.IndexOf(value);
		}

		public void Insert(int index, ConfigurationPermissionInfo value)
		{
			List.Insert(index, value);
		}

		public void Remove(ConfigurationPermissionInfo value)
		{
			List.Remove(value);
		}

		public void Remove(int permissionID, int roleID, int userID)
		{
			foreach (PermissionInfoBase permission in List)
			{
				if (permission.PermissionID == permissionID && permission.UserID == userID && permission.RoleID == roleID)
				{
					List.Remove(permission);
					break;
				}
			}
		}

		public List<PermissionInfoBase> ToList()
		{
			var list = new List<PermissionInfoBase>();
			foreach (PermissionInfoBase permission in List)
			{
				list.Add(permission);
			}
			return list;
		}

		public string ToString(string key)
		{
			return PermissionController.BuildPermissions(List, key);
		}

		#endregion
	}
}		
