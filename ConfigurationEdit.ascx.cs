using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;

namespace forDNN.Modules.UniversalAutosave
{

	partial class UniversalAutosave : PortalModuleBase, IActionable
	{

		#region Private Members

		private int itemId;

		#endregion

		#region "Event Handlers"

		protected void Page_Init(object sender, EventArgs e)
		{
			grdPermissions.LocalResourceFile = LocalResourceFile;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (!(Request.Params["ConfigurationID"] == null))
				{
					itemId = Int32.Parse(Request.Params["ConfigurationID"]);
					cmdDelete.Visible = true;
				}
				else
				{
					itemId = 0;
				}

				RegisterJavaScript();

				if (!Page.IsPostBack)
				{
					cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
					DrawConfigurationEdit();
				}
			}

			catch (Exception exc)
			{
				//Module failed to load 
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private bool DoUpdate(bool UpdateOnly)
		{
			try
			{
				int SelectedTabID = -1;
				Int32.TryParse(lstPages.SelectedValue, out SelectedTabID);
				if (SelectedTabID <= 0)
				{
					DotNetNuke.UI.Skins.Skin.AddModuleMessage(this,
						Localization.GetString("RequiredPage", this.LocalResourceFile),
						DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning);
					return false;
				}

				if (Page.IsValid == true)
				{
					ConfigurationInfo objConfiguration = new ConfigurationInfo();
					if (itemId != 0)
					{
						objConfiguration =
							ConfigurationController.Configuration_GetByPrimaryKey(itemId);
					}
					objConfiguration.Title = tbTitle.Text;
					objConfiguration.Description = tbDescription.Text;
					objConfiguration.TabID = SelectedTabID;
					objConfiguration.AutosaveEnabled = cbEnableAutosave.Checked;
					objConfiguration.AutosaveIcon = cbAutosaveIcon.Checked;
					objConfiguration.AutosaveOnBlur = cbBlurAutosave.Checked;
					objConfiguration.AutosavePeriod = Convert.ToInt32(tbAutosavePeriod.Text);
					objConfiguration.HistoryLength = Convert.ToInt32(tbHistoryLength.Text);
					objConfiguration.HistoryExpiry = Convert.ToInt32(tbHistoryExpiry.Text);
					objConfiguration.AutosaveLocation = Convert.ToInt32(ddlAutosaveLocation.SelectedValue);
					objConfiguration.UrlIndependent = cbUrlIndependent.Checked;

					if (itemId == 0)
					{
						objConfiguration.ConfigurationID = ConfigurationController.Configuration_Add(objConfiguration);
					}
					else
					{
						ConfigurationController.Configuration_Update(objConfiguration);
					}

					SavePermissions(objConfiguration.ConfigurationID);

					if (UpdateOnly)
					{
						if (itemId == 0)
						{
							string NewURL = EditUrl("ConfigurationID", objConfiguration.ConfigurationID.ToString(), "ConfigurationEdit");
							Response.Redirect(NewURL, false);
						}
					}
					else
					{
						DoCancel();
					}
				}
			}
			catch (Exception exc)
			{
				Exceptions.ProcessModuleLoadException(this, exc);
				return false;
			}
			return true;
		}

		protected void cmdUpdate_Click(object sender, EventArgs e)
		{
			DoUpdate(true);
		}

		protected void cmdUpdateClose_Click(object sender, EventArgs e)
		{
			DoUpdate(false);
		}

		private void SavePermissions(int ConfigurationID)
		{
			ConfigurationPermissionController.ConfigurationPermission_DeleteByConfigurationID(ConfigurationID);
			foreach (ConfigurationPermissionInfo objInfo in grdPermissions.Permissions)
			{
				if ((objInfo.RoleID == 0) && (objInfo.UserID == 0))
				{
					continue;
				}
				objInfo.ConfigurationID = ConfigurationID;
				ConfigurationPermissionController.ConfigurationPermission_Add(objInfo);
			}
		}

		protected void cmdCancel_Click(object sender, EventArgs e)
		{
			try
			{
				DoCancel();
			}
			catch (Exception exc)
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		protected void cmdDelete_Click(object sender, EventArgs e)
		{
			try
			{
				ConfigurationController.Configuration_Delete(itemId);
				DoCancel();
			}
			catch (Exception exc)
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private void DoCancel()
		{
			Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(this.TabId), true);
		}

		#endregion

		#region "Optional Interfaces"

		public ModuleActionCollection ModuleActions
		{
			get
			{
				ModuleActionCollection Actions = new ModuleActionCollection();
				Actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
				   ModuleActionType.AddContent, "", "add.gif", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
					true, false);
				return Actions;
			}
		}

		#endregion

		#region Draw

		private void RegisterJavaScript()
		{
			string Script = @"
<script type=""text/javascript"">
	jQuery(function($)
	{
		$('#tabs-demo').dnnTabs({{0}});
		$('#controls').dnnPanels({});
		$('#events').dnnPanels({});
	});
</script>
"
				.Replace("{0}", (itemId > 0) ? "" : " selected:0, active: 0, disabled: [3, 4, 5] ")
			;
			if (!Page.ClientScript.IsStartupScriptRegistered("ConfigurationEdit"))
			{
				Page.ClientScript.RegisterStartupScript(this.GetType(), "ConfigurationEdit", Script);
				Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "uaConfiguration.js",
					string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", ResolveUrl("js/uaConfiguration.js")));
			}
		}

		private void DrawConfigurationEdit()
		{
			tbPeriodFrom.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
			tbPeriodTo.Text = DateTime.Now.ToShortDateString();
			lnkCalendarFrom.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(tbPeriodFrom);
			lnkCalendarTo.NavigateUrl = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(tbPeriodTo);

			CommonController.PopulateDropDown(ddlFilterByControl, ControlController.Control_GetDistinctValues(itemId), "Selector", "ControlID", true);
			CommonController.PopulateDropDown(ddlFilterByUser, AnonymousController.Anonymous_GetDistinctValues(), "UserName", "UserID", true);

			grdPermissions.ConfigurationID = itemId;
			grdPermissions.Permissions =
				ConfigurationPermissionController.ConfigurationPermission_GetByConfigurationID(itemId);
			grdPermissions.DataBind();

			int SelectedTabID = -1;

			if (itemId > 0)
			{
				ConfigurationInfo objConfiguration =
					ConfigurationController.Configuration_GetByPrimaryKey(itemId);
				if (objConfiguration == null)
				{
					DoCancel();
					return;
				}

				tbTitle.Text = objConfiguration.Title;
				tbDescription.Text = objConfiguration.Description;
				SelectedTabID = objConfiguration.TabID;
				cbEnableAutosave.Checked = objConfiguration.AutosaveEnabled;
				cbAutosaveIcon.Checked = objConfiguration.AutosaveIcon;
				cbBlurAutosave.Checked = objConfiguration.AutosaveOnBlur;
				tbAutosavePeriod.Text = objConfiguration.AutosavePeriod.ToString();
				tbHistoryLength.Text = objConfiguration.HistoryLength.ToString();
				tbHistoryExpiry.Text = objConfiguration.HistoryExpiry.ToString();
				ddlAutosaveLocation.SelectedValue = objConfiguration.AutosaveLocation.ToString();
				cbUrlIndependent.Checked = objConfiguration.UrlIndependent;

				CommonController.AddConfirmation(cmdDelete);

				lnkStartWizard.Attributes.Add("onclick", string.Format("javascript:uaWizard({0});", itemId));
				lnkStartEventWizard.Attributes.Add("onclick", string.Format("javascript:uaWizard({0});", itemId));
			}
			else
			{
				cmdDelete.Visible = false;
				DotNetNuke.Skin.AddModuleMessage(this, Localization.GetString("SaveFirst", this.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning);
			}

			DrawPagesTree(this.PortalId, SelectedTabID);
			DrawControls(itemId);
			DrawEvents(itemId);
		}

		#region Pages

		private string TrimPagesName(string PageName)
		{
			if (PageName.Length > 50)
			{
				return PageName.Substring(0, 50) + "...";
			}
			return PageName;
		}

		private void AddChildPages(Telerik.Web.UI.RadTreeNode objParentNode, int ParentID, int PortalID, int SelectedTabID)
		{
			foreach (DotNetNuke.Entities.Tabs.TabInfo objTab in
				DotNetNuke.Entities.Tabs.TabController.GetTabsByParent(ParentID, PortalID))
			{
				if (objTab.IsDeleted)
				{
					continue;
				}
				Telerik.Web.UI.RadTreeNode objNode =
					new Telerik.Web.UI.RadTreeNode(TrimPagesName(objTab.TabName), objTab.TabID.ToString());
				objParentNode.Nodes.Add(objNode);
				if (objTab.TabID == SelectedTabID)
				{
					objNode.Selected = true;
					Telerik.Web.UI.RadTreeNode objTempParent = objNode.ParentNode;
					while (objTempParent != null)
					{
						objTempParent.Expanded = true;
						objTempParent = objTempParent.ParentNode;
					}
				}
				AddChildPages(objNode, objTab.TabID, PortalID, SelectedTabID);
			}
		}

		private void DrawPagesTree(int PortalID, int SelectedTabID)
		{
			DotNetNuke.Entities.Portals.PortalController objPortalController =
				new DotNetNuke.Entities.Portals.PortalController();
			DotNetNuke.Entities.Portals.PortalInfo objPortal =
				objPortalController.GetPortal(PortalID);
			lstPages.Nodes.Clear();
			Telerik.Web.UI.RadTreeNode objRoot = new Telerik.Web.UI.RadTreeNode(TrimPagesName(objPortal.PortalName), "-1");
			objRoot.Expanded = true;

			AddChildPages(objRoot, Null.NullInteger, PortalID, SelectedTabID);

			lstPages.Nodes.Add(objRoot);
		}

		#endregion

		#region Controls

		private void DrawControls(int ConfigurationID)
		{
			Localization.LocalizeDataGrid(ref grdControls, this.LocalResourceFile);
			List<ControlInfo> lstControls = ControlController.Control_GetByFilter(ConfigurationID, "");
			grdControls.DataSource = lstControls;
			grdControls.DataBind();
		}

		protected void grdControls_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			var item = e.Item;
			if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.SelectedItem)
			{
				ControlInfo objControl = (ControlInfo)item.DataItem;
				string Path = CommonController.ResolveUrl("", false);

				CheckBox cbEnabled = (CheckBox)item.FindControl("cbEnabled");
				CheckBox cbRestoreOnLoad = (CheckBox)item.FindControl("cbRestoreOnLoad");
				CheckBox cbRestoreIfEmpty = (CheckBox)item.FindControl("cbRestoreIfEmpty");
				CheckBox cbShowCannedOnly = (CheckBox)item.FindControl("cbShowCannedOnly");
				if (cbEnabled != null)
				{
					cbEnabled.Attributes.Add("onchange", string.Format("javascript:uaCheckedChanged(this, \"{0}\", {1}, \"changeControlEnabled\");",
						Path,
						objControl.ControlID));
					cbRestoreOnLoad.Attributes.Add("onchange", string.Format("javascript:uaCheckedChanged(this, \"{0}\", {1}, \"changeControlRestoreOnLoad\");",
						Path,
						objControl.ControlID));
					cbRestoreIfEmpty.Attributes.Add("onchange", string.Format("javascript:uaCheckedChanged(this, \"{0}\", {1}, \"changeControlRestoreIfEmpty\");",
						Path,
						objControl.ControlID));
					cbShowCannedOnly.Attributes.Add("onchange", string.Format("javascript:uaCheckedChanged(this, \"{0}\", {1}, \"changeControlShowCannedOnly\");",
						Path,
						objControl.ControlID));
				}

				//Edit link
				var imgColumnControl = item.Controls[0].Controls[0];
				if (imgColumnControl is HyperLink)
				{
					var editLink = (HyperLink)imgColumnControl;
					editLink.NavigateUrl = "#top";
					editLink.Attributes.Add("onclick", "javascript:return uaEditItem(this);");

					ImageButton btnSelectorSave = (ImageButton)item.FindControl("btnSelectorSave");
					btnSelectorSave.ImageUrl = "~/icons/sigma/Save_16X16_Standard.png";
					btnSelectorSave.Attributes.Add("onclick",
						string.Format("javascript:return uaSaveItem(this, \"{0}\", {1}, \"updateControlSelector\");",
							Path,
							objControl.ControlID));

					ImageButton btnSelectorCancel = (ImageButton)item.FindControl("btnSelectorCancel");
					btnSelectorCancel.ImageUrl = "~/icons/sigma/Cancel_16X16_Standard.png";
					btnSelectorCancel.Attributes.Add("onclick", "javascript:return uaCancelItem(this);");
				}

				//Delete link
				imgColumnControl = item.Controls[1].Controls[0];
				if (imgColumnControl is ImageButton)
				{
					var delImage = (ImageButton)imgColumnControl;
					delImage.CommandName = "delete";
					delImage.CommandArgument = objControl.ControlID.ToString();
					CommonController.AddConfirmation(delImage);
				}
			}
		}

		protected void grdControls_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			ControlController.Control_Delete(Convert.ToInt32(e.CommandArgument));
			DrawControls(itemId);
		}

		protected void btnQuickAddSelector_Click(object sender, EventArgs e)
		{
			ControlInfo objControl = new ControlInfo();
			objControl.ConfigurationID = itemId;
			objControl.Enabled = true;
			objControl.Selector = tbQuickAddSelector.Text;
			ControlController.Control_Add(objControl);
			tbQuickAddSelector.Text = "";
			DrawControls(itemId);
		}

		protected void btnStartWizard_Click(object sender, EventArgs e)
		{
			Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(Convert.ToInt32(lstPages.SelectedNode.Value)), true);
		}

		#endregion

		#region Events

		private void DrawEvents(int ConfigurationID)
		{
			Localization.LocalizeDataGrid(ref grdEvents, this.LocalResourceFile);
			List<EventInfo> lstEvents = EventController.Event_GetByFilter(ConfigurationID, "");
			grdEvents.DataSource = lstEvents;
			grdEvents.DataBind();
		}

		protected void grdEvents_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			var item = e.Item;
			if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.SelectedItem)
			{
				EventInfo objEvent = (EventInfo)item.DataItem;
				string Path = CommonController.ResolveUrl("", false);

				CheckBox cbEnabled = (CheckBox)item.FindControl("cbEnabled");
				if (cbEnabled != null)
				{
					cbEnabled.Attributes.Add("onchange", string.Format("javascript:uaCheckedChanged(this, \"{0}\", {1}, \"changeEventEnabled\");",
						Path,
						objEvent.EventID));
				}

				//Edit link
				var imgColumnEvent = item.Controls[0].Controls[0];
				if (imgColumnEvent is HyperLink)
				{
					var editLink = (HyperLink)imgColumnEvent;
					editLink.NavigateUrl = "#top";
					editLink.Attributes.Add("onclick", "javascript:return uaEditItem(this);");

					ImageButton btnSelectorSave = (ImageButton)item.FindControl("btnSelectorSave");
					btnSelectorSave.ImageUrl = "~/icons/sigma/Save_16X16_Standard.png";
					btnSelectorSave.Attributes.Add("onclick",
						string.Format("javascript:return uaSaveItem(this, \"{0}\", {1}, \"updateEventSelector\");",
							Path,
							objEvent.EventID));

					ImageButton btnSelectorCancel = (ImageButton)item.FindControl("btnSelectorCancel");
					btnSelectorCancel.ImageUrl = "~/icons/sigma/Cancel_16X16_Standard.png";
					btnSelectorCancel.Attributes.Add("onclick", "javascript:return uaCancelItem(this);");
				}

				//Delete link
				imgColumnEvent = item.Controls[1].Controls[0];
				if (imgColumnEvent is ImageButton)
				{
					var delImage = (ImageButton)imgColumnEvent;
					delImage.CommandName = "delete";
					delImage.CommandArgument = objEvent.EventID.ToString();
					CommonController.AddConfirmation(delImage);
				}
			}
		}

		protected void grdEvents_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			EventController.Event_Delete(Convert.ToInt32(e.CommandArgument));
			DrawEvents(itemId);
		}

		protected void btnQuickAddEventSelector_Click(object sender, EventArgs e)
		{
			EventInfo objEvent = new EventInfo();
			objEvent.ConfigurationID = itemId;
			objEvent.Enabled = true;
			objEvent.Selector = tbQuickAddEventSelector.Text;
			EventController.Event_Add(objEvent);
			tbQuickAddEventSelector.Text = "";
			DrawEvents(itemId);
		}


		#endregion

		#region Values

		private void DrawValues()
		{
			int ControlID = Convert.ToInt32(ddlFilterByControl.SelectedValue);
			int UserID = Convert.ToInt32(ddlFilterByUser.SelectedValue);
			bool IsAnonymous = (UserId < 0);
			DateTime dtFrom = Convert.ToDateTime(tbPeriodFrom.Text);
			DateTime dtTo = Convert.ToDateTime(tbPeriodTo.Text).AddDays(1);

			List<ValueInfo> lstValues =
				ValueController.Value_GetByFilter((ControlID == 0) ? -1 : ControlID, -1, UserId, IsAnonymous, dtFrom, dtTo, false);

			if (lstValues.Count == 0)
			{
				lblNoValuesFound.Visible = true;
				grdValues.Visible = false;
			}
			else
			{
				lblNoValuesFound.Visible = false;
				grdValues.Visible = true;

				Localization.LocalizeDataGrid(ref grdValues, this.LocalResourceFile);
				grdValues.DataSource = lstValues;
				grdValues.DataBind();
			}
		}

		protected void grdValues_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			var item = e.Item;
			if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.SelectedItem)
			{
				ValueInfo objValue = (ValueInfo)item.DataItem;
				jsInfo objjsInfo = new jsInfo(itemId, new List<ControlInfo>());
				System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
				string Path = CommonController.ResolveUrl("", false);

				CheckBox cbCanned = (CheckBox)item.FindControl("cbCanned");
				if (cbCanned != null)
				{
					cbCanned.Checked = objValue.Canned;
					cbCanned.Attributes.Add("onchange",
						string.Format("javascript:uaCheckedChanged(this, \"{0}\", {1}, \"changeCanned\");",
							Path,
							objValue.ValueID));
				}

				//Edit link
				var imgColumnControl = item.Controls[0].Controls[0];
				if (imgColumnControl is HyperLink)
				{
					var editLink = (HyperLink)imgColumnControl;
					editLink.NavigateUrl = "#top";
					editLink.Attributes.Add("onclick", "javascript:return uaEditItem(this);");

					ImageButton btnValueSave = (ImageButton)item.FindControl("btnValueSave");
					btnValueSave.ImageUrl = "~/icons/sigma/Save_16X16_Standard.png";
					btnValueSave.Attributes.Add("onclick",
						string.Format("javascript:return uaSaveItem(this, \"{0}\", {1}, \"updateValue\");", Path, objValue.ValueID));

					ImageButton btnValueCancel = (ImageButton)item.FindControl("btnValueCancel");
					btnValueCancel.ImageUrl = "~/icons/sigma/Cancel_16X16_Standard.png";
					btnValueCancel.Attributes.Add("onclick", "javascript:return uaCancelItem(this);");
				}

				//Delete link
				imgColumnControl = item.Controls[1].Controls[0];
				if (imgColumnControl is ImageButton)
				{
					var delImage = (ImageButton)imgColumnControl;
					delImage.CommandName = "delete";
					delImage.CommandArgument = objValue.ValueID.ToString();
					CommonController.AddConfirmation(delImage);
				}
			}
		}

		protected void grdValues_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			ValueController.Value_Delete(Convert.ToInt32(e.CommandArgument));
			DrawValues();
		}

		protected void btnRefreshValues_Click(object sender, EventArgs e)
		{
			DrawValues();
		}

		#endregion

		#endregion

	}

}