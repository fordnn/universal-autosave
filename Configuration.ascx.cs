using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;

namespace forDNN.Modules.UniversalAutosave
{
	public partial class Configuration : PortalModuleBase, IActionable
	{
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		#region Private Properties

		private int CurrentPage
		{
			get
			{
				return (Request.QueryString["currentpage"] == null) ? 1 : Convert.ToInt32(Request.QueryString["currentpage"]);
			}
		}

		private int PageSize
		{
			get
			{
				return (Request.QueryString["pagesize"] == null) ? Convert.ToInt32(ddlConfigurationPageSize.SelectedValue) : Convert.ToInt32(Request.QueryString["pagesize"]);
			}
		}

		#endregion

		#region Event Handlers

		private bool LoadSettings()
		{
			lnkAddConfiguration.NavigateUrl = EditUrl("ConfigurationEdit");
			Session["ConfigurationSort"] = "Title";
			return true;
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				if (!LoadSettings())
				{
					return;
				}
				if (!IsPostBack)
				{
					DrawConfigurationList();
				}
			}
			catch (Exception exc)
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

		#region Optional Interfaces

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
		{
			get
			{
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				Actions.Add(GetNextActionID(),
					Localization.GetString("AddConfiguration", this.LocalResourceFile),
					DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent,
					"", "", EditUrl("ConfigurationEdit"), false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
				return Actions;
			}
		}

		#endregion

		#region List

		private void DrawConfigurationList()
		{
			List<ConfigurationInfo> lstConfigurations = ConfigurationController.Configuration_GetAllItems((string)Session["ConfigurationSort"]);

			if (Request.QueryString["PageSize"] != null)
			{
				ddlConfigurationPageSize.SelectedValue = PageSize.ToString();
			}

			ctlPagingControl.TotalRecords = lstConfigurations.Count;
			ctlPagingControl.PageSize = PageSize;
			ctlPagingControl.CurrentPage = CurrentPage;
			ctlPagingControl.TabID = this.TabId;
			ctlPagingControl.QuerystringParams = string.Format("PageSize={0}", PageSize);

			Localization.LocalizeDataGrid(ref grdConfig, this.LocalResourceFile);
			grdConfig.PageSize = ctlPagingControl.PageSize;
			grdConfig.CurrentPageIndex = ctlPagingControl.CurrentPage - 1;
			grdConfig.DataSource = lstConfigurations;
			grdConfig.DataBind();
		}

		private void DeleteConfiguration(int ConfigurationID)
		{
			ConfigurationController.Configuration_Delete(ConfigurationID);
			DrawConfigurationList();
		}

		protected void ddlConfigurationPageSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			string URL =
				DotNetNuke.Common.Globals.NavigateURL(
					this.TabId, "",
					new string[]
					{ 
						string.Format("currentpage={0}", 1),
						string.Format("pagesize={0}", Convert.ToInt32(ddlConfigurationPageSize.SelectedValue))
					});
			Response.Redirect(URL, true);
		}

		protected void grdConfig_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			var item = e.Item;
			if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.SelectedItem)
			{
				ConfigurationInfo objConfig = (ConfigurationInfo)item.DataItem;
				string EditItemURL = EditUrl("ConfigurationID", objConfig.ConfigurationID.ToString(), "ConfigurationEdit");

				//Edit link
				var imgColumnControl = item.Controls[0].Controls[0];
				if (imgColumnControl is HyperLink)
				{
					var editLink = (HyperLink)imgColumnControl;
					editLink.NavigateUrl = EditItemURL;
				}

				//Delete link
				imgColumnControl = item.Controls[1].Controls[0];
				if (imgColumnControl is ImageButton)
				{
					var delImage = (ImageButton)imgColumnControl;
					delImage.CommandName = "delete";
					delImage.CommandArgument = objConfig.ConfigurationID.ToString();
					CommonController.AddConfirmation(delImage);
				}

				Literal litTitle = (Literal)item.FindControl("litTitle");
				litTitle.Text = string.Format("<a href=\"{0}\">{1}</a>", EditItemURL, objConfig.Title);
			}
		}

		protected void grdConfig_DeleteCommand(object source, DataGridCommandEventArgs e)
		{
			DeleteConfiguration(Convert.ToInt32(e.CommandArgument));
		}

		protected void grdConfig_SortCommand(object source, DataGridSortCommandEventArgs e)
		{
			Session["ConfigurationSort"] = e.SortExpression;
			DrawConfigurationList();
		}

		#endregion

	}
}

