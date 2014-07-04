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
using System.Text.RegularExpressions;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

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
                case "getdiff":
                    GetDiff(htParams);
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

            if  (lstValues.Count == 0)
			{
				Response.Write(Localization.GetString("HistoryAbsent", CommonController.GetCommonResourcesPath()));
				return;
			}

			string SubDate = "";
			StringBuilder sb = new StringBuilder();
            //sb.Append("<a href=\"\" class=\"uaButtonShowDiff\" onclick=\"onClickButtonShowDiff(this, event); return false;\">Show difference</a>");
            sb.Append("<a href=\"\" class=\"uaButtonShowDiff\">Show difference</a>");
           
            sb.Append("<table class=\"uaHistory\" border=\"0\" width=\"100%\">");
            sb.Append("<tbody>");

            string innerText;

            foreach (ValueInfo objTemp in lstValues)
			{
                if (SubDate != objTemp.CreatedAt.ToShortDateString())
                {
                    SubDate = objTemp.CreatedAt.ToShortDateString();
                    sb.AppendFormat("<tr><td class=\"uaCenter\" colspan=\"3\">{0}</td></tr>", SubDate);
                }
                if ( ! string.IsNullOrEmpty(objControl.RTFType))
                {
                    innerText = Regex.Replace(Regex.Replace(objTemp.Value, @"\s*<\/?[^\>]+\/?>\s*", " "), @"\s\s+", " ");
                    if (innerText.Length > 200)
                    {
                        innerText =
                            string.Format(
                            "{0}<a href=\"\" onclick=\"onClickShowAllText(this, event ); return false;\">...</a>"
                            + "<span class=\"uanextText\" style=\"display:none;\">{1}</span>"
                            + "<div class=\"fullTextHtml\" style=\"display:none;\">{2}</div>"
                            , innerText.Substring(0, 200)
                            , innerText.Substring(200)
                            , objTemp.Value);
                    }
                    else
                    {
                        innerText =
                           string.Format("{0}<div class=\"fullTextHtml\" style=\"display:none;\">{1}</div>"
                                           , innerText
                                           , objTemp.Value);
                    }
                        
                    objTemp.Value = innerText;
                    innerText = string.Empty;
                }

                    
                sb.AppendFormat("<tr ua=\"{2}\">" + 
                    "<td class=\"uaSelectItemHistory\"><input valueid=\"{3}\" type=\"checkbox\" onchange=\"handleDiffCheckbox(this, event);\"/></td>" +
                    /*"<td class=\"value\" onclick=\"onClickUaRestoreValue(this, event ); return false;\">{0}</td>" +*/
                    "<td class=\"value\">{0}</td>" +
                    "<td class=\"timeStamp\">{1}</td>"+
                    "</tr>",
                        
                    objTemp.Value,
                    objTemp.CreatedAt.ToShortTimeString(),
                    htParams["selector"],
                    objTemp.ValueID);
			}
            
            sb.Append("</tbody>");
			sb.Append("</table>");

			Response.Write(sb.ToString());
		}

        private void GetDiff(NameValueCollection htParams)
        {
            int val1 = int.Parse(htParams["idValue1"]);
            int val2 = int.Parse(htParams["idValue2"]);
            List<ValueInfo> values = new List<ValueInfo>(2) { ValueController.Value_GetByPrimaryKey(val1), ValueController.Value_GetByPrimaryKey(val2) };

            ValueInfo valueOld, valueNew;
            if (values[0].CreatedAt < values[1].CreatedAt)
            { 
                valueOld = values[0];
                valueNew = values[1];
            }
            else
            {
                valueOld = values[1];
                valueNew = values[0];  
            }
            ISideBySideDiffBuilder diffBuilder;
            diffBuilder = new SideBySideDiffBuilder(new Differ());
            var model = diffBuilder.BuildDiffModel(valueOld.Value, valueNew.Value);

            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine("<div id=\"diffBox\">"); //main div
            BuildPaneDiff(sb, model.OldText, "leftPane", Localization.GetString("OldText", LocalResourceFile));
            BuildPaneDiff(sb, model.NewText, "rightPane", Localization.GetString("NewText", LocalResourceFile));
            sb.AppendLine("</div>");//main div

            Response.Write(sb.ToString());
        }

        private void BuildPaneDiff(StringBuilder inputText, DiffPaneModel model, params string[] param)
        {
            inputText.AppendLine("<div id=\"" + param[0] + "\">");
            inputText.AppendLine("<div class=\"diffHeader\">" + param[1] + "</div>");
            inputText.AppendLine("<div class=\"diffPane\">");

            inputText.AppendLine("<table cellpadding=\"0\" cellspacing=\"0\" class=\"diffTable\">");

            foreach (var diffLine in model.Lines)
            {
                inputText.AppendLine("<tr>");
                inputText.Append("<td class=\"lineNumber\">");
                inputText.Append(diffLine.Position.HasValue ? diffLine.Position.ToString() : "&nbsp;");
                inputText.Append("</td>");
                inputText.Append("<td class=\"line " + diffLine.Type.ToString() + "Line\">");
                inputText.Append("<span>");

                if (!string.IsNullOrEmpty(diffLine.Text))
                {
                    string spaceValue = "\u00B7";
                    string tabValue = "\u00B7\u00B7";
                    if (diffLine.Type == ChangeType.Deleted || diffLine.Type == ChangeType.Inserted || diffLine.Type == ChangeType.Unchanged)
                    {
                        inputText.Append(Server.HtmlEncode(diffLine.Text).Replace(" ", spaceValue).Replace("\t", tabValue));
                    }
                    else if (diffLine.Type == ChangeType.Modified)
                    {
                        foreach (var character in diffLine.SubPieces)
                        {
                            if (character.Type == ChangeType.Imaginary) continue;
                            inputText.Append("<span class=\"" + character.Type.ToString() + "Character\">" + Server.HtmlEncode(character.Text.Replace(" ", spaceValue.ToString())) + "</span>");
                        }
                    }
                }

                inputText.Append("</span>");
                inputText.Append("</td>");
                inputText.AppendLine("</tr>");
            }

            inputText.AppendLine("</table>");
            inputText.AppendLine("</div>"); // diffPane
            inputText.AppendLine("</div>"); //Pane
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
