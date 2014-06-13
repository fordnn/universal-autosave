using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace forDNN.Modules.UniversalAutosave
{
	public partial class UAHandler : System.Web.UI.Page
	{
		#region Private Properties

		private string LocalResourceFile { get; set; }
		private NameValueCollection htParams { get; set; }

		#endregion

		private void FillProperties()
		{
			LocalResourceFile = CommonController.GetCommonResourcesPath();
			string Income = Request.ContentEncoding.GetString(Request.BinaryRead(Request.ContentLength));
			htParams = System.Web.HttpUtility.ParseQueryString(Income);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				FillProperties();

				if (Request.QueryString["ConfigurationID"] == null)
				{
					//work mode
					DoWork();
				}
				else
				{
					//config mode
					DoConfig();
				}
			}
			catch (Exception Exc)
			{
				DotNetNuke.Services.Exceptions.Exceptions.LogException(Exc);
				Response.Write(Localization.GetString("ExceptionContactAdmin", this.LocalResourceFile));
			}
		}

		#region Work

		private void DoWork()
		{
			switch (htParams["action"].ToLower())
			{
				case "trackchanges":
					TrackChanges(htParams);
					break;
				case "gethistory":
					GetHistory(htParams);
					break;
				case "closesession":
					CloseSession(htParams);
					break;
			}
		}

		private ValueInfo ParseValue(NameValueCollection htParams, ConfigurationInfo objConfiguration)
		{
			ValueInfo objValue = new ValueInfo();

			string Value = (htParams["value"] == null) ? "" : htParams["value"];
			string Selector = (htParams["selector"] == null) ? "" : htParams["selector"];
			string AnonymousGUID = (htParams["anonymousGUID"] == null) ? "" : htParams["anonymousGUID"];

			List<ControlInfo> lstControls = ControlController.Control_GetByFilter(objConfiguration.ConfigurationID, Selector);
			if (lstControls.Count == 0)
			{
				Response.Write(Localization.GetString("NoControlsInDB", this.LocalResourceFile));
				return null;
			}
			objValue.ControlID = lstControls[0].ControlID;

			objValue.UrlID = Convert.ToInt32(htParams["urlID"]);
			objValue.UserID = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo().UserID;
			if (objValue.UserID == -1)
			{
				objValue.Anonymous = true;
				try
				{
					objValue.UserID = AnonymousController.Anonymous_GetByUserGUIDorCreate(new System.Guid(AnonymousGUID)).AnonymousID;
				}
				catch
				{
					Response.Write(Localization.GetString("AnonymousUserError", this.LocalResourceFile));
					return null;
				}
			}
			else
			{
				objValue.Anonymous = false;
			}
			objValue.Canned = false;
			objValue.Closed = false;
			objValue.Value = Value;

			return objValue;
		}

		private void GetHistory(NameValueCollection htParams)
		{
			int ConfigurationID = Convert.ToInt32(htParams["configurationID"]);
			ConfigurationInfo objConfiguration = ConfigurationController.Configuration_GetByPrimaryKey(ConfigurationID);

			ValueInfo objValue = ParseValue(htParams, objConfiguration);
			objValue.UrlID = objConfiguration.UrlIndependent ? -1 : objValue.UrlID;

			ControlInfo objControl = ControlController.Control_GetByPrimaryKey(objValue.ControlID);

			List<ValueInfo> lstValues =
				ValueController.Value_GetByFilter(
					objValue.ControlID,
					objValue.UrlID,
					objValue.UserID,
					objValue.Anonymous,
					null,
					null,
					objControl.ShowCannedOnly
			);

			if (lstValues.Count == 0)
			{
				Response.Write(Localization.GetString("HistoryAbsent", CommonController.GetCommonResourcesPath()));
				return;
			}

			string SubDate = "";
			StringBuilder sb = new StringBuilder("<table class=\"uaHistory\" border=\"0\" width=\"100%\">");
			foreach (ValueInfo objTemp in lstValues)
			{
				if (SubDate != objTemp.CreatedAt.ToShortDateString())
				{
					SubDate = objTemp.CreatedAt.ToShortDateString();
					sb.AppendFormat("<tr><td class=\"uaCenter\" colspan=\"2\">{0}</td></tr>", SubDate);
				}
                ///TrimHTML(objTemp.Value)
				sb.AppendFormat("<tr ua=\"{2}\"><td class=\"value\">{0}</td><td>{1}</td></tr>",
					objTemp.Value,
					objTemp.CreatedAt.ToShortTimeString(),
					htParams["selector"]);
			}
			sb.Append("</table>");

			Response.Write(sb.ToString());
		}

		private void TrackChanges(NameValueCollection htParams)
		{
			int ConfigurationID = Convert.ToInt32(htParams["configurationID"]);
			ConfigurationInfo objConfiguration = ConfigurationController.Configuration_GetByPrimaryKey(ConfigurationID);

			ValueInfo objValue = ParseValue(htParams, objConfiguration);

			ValueController.Value_UpdateClosed(
				objValue.ControlID,
				objValue.UrlID,
				objValue.UserID,
				objValue.Anonymous,
				objConfiguration.HistoryLength,
				objConfiguration.HistoryExpiry
			);

			objValue.ValueID = ValueController.Value_Add(objValue);

			WriteSuccess();
		}

		private void CloseSession(NameValueCollection htParams)
		{
			int ConfigurationID = Convert.ToInt32(htParams["configurationID"]);
			ConfigurationInfo objConfiguration = ConfigurationController.Configuration_GetByPrimaryKey(ConfigurationID);

			ValueInfo objValue = ParseValue(htParams, objConfiguration);

			ValueController.Value_CloseSession(
				ConfigurationID,
				objValue.UrlID,
				objValue.UserID,
				objValue.Anonymous
			);

			WriteSuccess();
		}

		#endregion

		#region Config

		private void DoConfig()
		{
			if (!CommonController.IsAdmin())
			{
				return;
			}

			switch (Request.QueryString["action"])
			{
				case "addControl":
					AddControl();
					break;
				case "removeControl":
					RemoveControl();
					break;
				case "updateControlSelector":
					UpdateControlSelector();
					break;
				case "changeCanned":
					ChangeCanned();
					break;
				case "changeControlEnabled":
					ChangeControlEnabled();
					break;
				case "changeControlRestoreOnLoad":
					ChangeControlRestoreOnLoad();
					break;
				case "changeControlRestoreIfEmpty":
					ChangeControlRestoreIfEmpty();
					break;
				case "changeControlShowCannedOnly":
					ChangeControlShowCannedOnly();
					break;
				case "updateValue":
					UpdateValue();
					break;
				case "changeEventEnabled":
					ChangeEventEnabled();
					break;
				case "updateEventSelector":
					UpdateEventSelector();
					break;
			}
		}

		#region Controls Grid

		private void UpdateControlSelector()
		{
			ControlInfo objControl =
				ControlController.Control_GetByPrimaryKey(Convert.ToInt32(htParams["itemID"]));
			if (objControl != null)
			{
				objControl.Selector = htParams["value"];
				ControlController.Control_Update(objControl);
				WriteSuccess();
			}
			else
			{
				Response.Write(Localization.GetString("ControlDoesNotExistsInDB", this.LocalResourceFile));
			}
		}

		private void RemoveControl()
		{
			int ConfigurationID = Convert.ToInt32(Request.QueryString["configurationID"]);
			string Selector = Request.QueryString["selector"];
			//string objType = Request.QueryString["type"].ToLower();
			//bool IsEvent = ((objType == "a") || (objType == "input[type=\"submit\"]"));
            bool IsEvent = bool.Parse(Request.QueryString["IsEvent"]);
			if (IsEvent)
			{
				List<EventInfo> lstEvents = EventController.Event_GetByFilter(ConfigurationID, Selector);
				if (lstEvents.Count > 0)
				{
					foreach (EventInfo objEvent in lstEvents)
					{
						objEvent.Enabled = false;
						EventController.Event_Update(objEvent);
					}
					WriteSuccess();
				}
				else
				{
					Response.Write(Localization.GetString("EventDoesNotExistsInDB", this.LocalResourceFile));
				}
			}
			else
			{
				List<ControlInfo> lstControls = ControlController.Control_GetByFilter(ConfigurationID, Selector);
				if (lstControls.Count > 0)
				{
					foreach (ControlInfo objControl in lstControls)
					{
						objControl.Enabled = false;
						ControlController.Control_Update(objControl);
					}
					WriteSuccess();
				}
				else
				{
					Response.Write(Localization.GetString("ControlDoesNotExistsInDB", this.LocalResourceFile));
				}
			}
		}

		private void AddControl()
		{
			int ConfigurationID = Convert.ToInt32(Request.QueryString["configurationID"]);
			string Selector = Request.QueryString["selector"];
			//string objType = Request.QueryString["type"].ToLower();
            //bool IsEvent = ((objType == "a") || (objType == "input[type=\"submit\"]"));
            string _RTFType = Request.QueryString["RTFEditor"].ToLower();
            bool IsEvent = bool.Parse(Request.QueryString["IsEvent"]);

			if (IsEvent)
			{
				List<EventInfo> lstEvents = EventController.Event_GetByFilter(ConfigurationID, Selector);
				if (lstEvents.Count == 0)
				{
					EventInfo objEvent = new EventInfo();
					objEvent.ConfigurationID = ConfigurationID;
					objEvent.Selector = Selector;
					objEvent.Enabled = true;
					objEvent.EventID = EventController.Event_Add(objEvent);
				}
				else
				{
					lstEvents[0].Enabled = true;
					EventController.Event_Update(lstEvents[0]);
				}
			}
			else
			{
				List<ControlInfo> lstControls = ControlController.Control_GetByFilter(ConfigurationID, Selector);
				if (lstControls.Count == 0)
				{
					ControlInfo objControl = new ControlInfo();
					objControl.ConfigurationID = ConfigurationID;
					objControl.Selector = Selector;
					objControl.Enabled = true;
                    objControl.RTFType = _RTFType;
					objControl.ControlID = ControlController.Control_Add(objControl);
				}
				else
				{
					lstControls[0].Enabled = true;
					ControlController.Control_Update(lstControls[0]);
				}
			}

			WriteSuccess();
		}

		private void ChangeControlEnabled()
		{
			int ControlID = Convert.ToInt32(Request.QueryString["itemID"]);
			bool Checked = (Request.QueryString["checked"].ToLower() == "true");
			ControlInfo objControl = ControlController.Control_GetByPrimaryKey(ControlID);
			objControl.Enabled = Checked;
			ControlController.Control_Update(objControl);
			WriteSuccess();
		}

		private void ChangeControlRestoreOnLoad()
		{
			int ControlID = Convert.ToInt32(Request.QueryString["itemID"]);
			bool Checked = (Request.QueryString["checked"].ToLower() == "true");
			ControlInfo objControl = ControlController.Control_GetByPrimaryKey(ControlID);
			objControl.RestoreOnLoad = Checked;
			ControlController.Control_Update(objControl);
			WriteSuccess();
		}

		private void ChangeControlRestoreIfEmpty()
		{
			int ControlID = Convert.ToInt32(Request.QueryString["itemID"]);
			bool Checked = (Request.QueryString["checked"].ToLower() == "true");
			ControlInfo objControl = ControlController.Control_GetByPrimaryKey(ControlID);
			objControl.RestoreIfEmpty = Checked;
			ControlController.Control_Update(objControl);
			WriteSuccess();
		}

		private void ChangeControlShowCannedOnly()
		{
			int ControlID = Convert.ToInt32(Request.QueryString["itemID"]);
			bool Checked = (Request.QueryString["checked"].ToLower() == "true");
			ControlInfo objControl = ControlController.Control_GetByPrimaryKey(ControlID);
			objControl.ShowCannedOnly = Checked;
			ControlController.Control_Update(objControl);
			WriteSuccess();
		}

		#endregion

		#region Events Grid

		private void UpdateEventSelector()
		{
			EventInfo objEvent =
				EventController.Event_GetByPrimaryKey(Convert.ToInt32(htParams["itemID"]));
			if (objEvent != null)
			{
				objEvent.Selector = htParams["value"];
				EventController.Event_Update(objEvent);
				WriteSuccess();
			}
			else
			{
				Response.Write(Localization.GetString("EventDoesNotExistsInDB", this.LocalResourceFile));
			}
		}

		private void ChangeEventEnabled()
		{
			int EventID = Convert.ToInt32(Request.QueryString["itemID"]);
			bool Checked = (Request.QueryString["checked"].ToLower() == "true");
			EventInfo objEvent = EventController.Event_GetByPrimaryKey(EventID);
			objEvent.Enabled = Checked;
			EventController.Event_Update(objEvent);
			WriteSuccess();
		}

		#endregion

		#region Values Grid

		private void ChangeCanned()
		{
			int ValueID = Convert.ToInt32(Request.QueryString["itemID"]);
			bool Checked = (Request.QueryString["checked"].ToLower() == "true");
			ValueInfo objValue = ValueController.Value_GetByPrimaryKey(ValueID);
			objValue.Canned = Checked;
			ValueController.Value_Update(objValue);
			WriteSuccess();
		}

		private void UpdateValue()
		{
			ValueInfo objValue =
				ValueController.Value_GetByPrimaryKey(Convert.ToInt32(htParams["itemID"]));
			if (objValue != null)
			{
				objValue.Value = htParams["value"];
				ValueController.Value_Update(objValue);
				WriteSuccess();
			}
			else
			{
				Response.Write(Localization.GetString("ValueDoesNotExistsInDB", this.LocalResourceFile));
			}
		}

		#endregion

		#endregion

		private void WriteSuccess()
		{
			Response.Write("1");
		}
	}
}
