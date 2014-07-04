using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Users;

namespace forDNN.Modules.UniversalAutosave
{

	public class UAHtml : System.IO.Stream
	{
		System.IO.Stream _HTML;
		private string _InnerHTML;

		#region Constructor
		public UAHtml(System.IO.Stream HTML)
		{
			_InnerHTML = "";
			_HTML = HTML;
		}
		#endregion

		#region Public Properties
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}
		public override long Length
		{
			get
			{
				return _HTML.Length;
			}
		}
		public override long Position
		{
			get
			{
				return _HTML.Position;
			}
			set
			{
				_HTML.Position = value;
			}
		}
		#endregion

		#region Public Methods

		public override long Seek(long offset, System.IO.SeekOrigin origin)
		{
			return _HTML.Seek(offset, origin);
		}
		public override void SetLength(long value)
		{
			_HTML.SetLength(value);
		}

		private ConfigurationInfo GetConfiguration(System.Web.HttpContext objContext)
		{
			ConfigurationInfo objResult = null;

			int TabID = -1;
			Int32.TryParse(objContext.Request.QueryString["TabID"], out TabID);
			List<ConfigurationInfo> lstConfigurations = ConfigurationController.Configuration_GetByTabID(TabID);
			UserInfo objUser = UserController.GetCurrentUserInfo();

			foreach (ConfigurationInfo objConfig in lstConfigurations)
			{
				if ((CommonController.ConfigAllowed(objUser, objConfig)) && (objConfig.AutosaveEnabled))
				{
					objResult = objConfig;
					break;
				}
			}

			return objResult;
		}

		private string GetWizardHelp(System.Web.HttpContext objContext, string Path, string LocalResource)
		{
			if (objContext.Request.Cookies["uaWizardHelp"] != null)
			{
				if (objContext.Request.Cookies["uaWizardHelp"].Value == "-1")
				{
					return "";
				}
			}

			string Result = string.Format(Localization.GetString("WizardHelp", LocalResource),
				Path,
				Localization.GetString("WizardHelpHide", LocalResource),
				@"<script type=""text/javascript"">$( document ).ready(function() {$(""div.uaWizardHelp"").dialog(
	{
	    modal: true,
		width: ""50%"",
	    closeOnEscape: true,
		buttons: [{ text: """ + Localization.GetString("WizardClose", LocalResource) + @""", click: function () { $(this).dialog(""close""); } }],
	    title: """ + Localization.GetString("WizardHelpTitle", LocalResource) + @""",
	});});</script>"
				);

			return Result;
		}

		public override void Flush()
		{
			System.Web.HttpContext objContext = System.Web.HttpContext.Current;
			byte[] outdata = new byte[1];

			try
			{
				int HeadPosition = _InnerHTML.ToLower().IndexOf("</head>");
				int BodyPosition = _InnerHTML.ToLower().IndexOf("</body>", HeadPosition);

				if ((BodyPosition != -1) && (HeadPosition != -1))
				{
					int ConfigID = 0;
					try
					{
						ConfigID = (objContext.Request.Cookies["uaWizard"] == null) ? 0 : Convert.ToInt32(objContext.Request.Cookies["uaWizard"].Value);
					}
					catch
					{ }
					ConfigID = (CommonController.IsAdmin() && (ConfigID != 0)) ? ConfigID : 0;
					bool ConfigurationMode = (ConfigID != 0);

					ConfigurationInfo objConfig = null;

					if (ConfigurationMode)
					{
						objConfig = ConfigurationController.Configuration_GetByPrimaryKey(ConfigID);
						if (objConfig.TabID.ToString() != objContext.Request.QueryString["TabID"])
						{
							//configuration not for this page
							objConfig = null;
							ConfigID = 0;
						}
					}

					if (!ConfigurationMode)
					{
						//lets try to check if we have work configuration for current TabID
						objConfig = GetConfiguration(objContext);
					}

					if ((objConfig != null) && ((objConfig.AutosaveEnabled && (!ConfigurationMode)) || (ConfigurationMode)))
					{
						objConfig.ConfigurationMode = ConfigurationMode;

						int UserID = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo().UserID;
						bool IsAnonymous = (UserID == -1);
						if (IsAnonymous)
						{
							if (objContext.Request.Cookies["uaAnonymousGUID"] != null)
							{
								UserID =
									AnonymousController.Anonymous_GetByUserGUIDorCreate(new System.Guid(objContext.Request.Cookies["uaAnonymousGUID"].Value)).AnonymousID;
							}
							else
							{
								UserID = -1;
							}
						}

						//get controls and last values for them
						List<ControlInfo> lstFullControls =
								ControlController.Control_GetByFilter(objConfig.ConfigurationID, "");
						foreach (ControlInfo objCtl in lstFullControls)
						{
							objCtl.Value =
								ValueController.Value_GetLastValue(
									objCtl.ControlID,
									(objConfig.UrlIndependent) ? -1 : CommonController.GetCurrentUrlID(),
									UserID,
									IsAnonymous);
						}

						jsInfo objjsInfo = new jsInfo(ConfigurationController.ConfigurationInfoToLimited(objConfig), lstFullControls);
						objjsInfo.events = EventController.Event_GetByFilter(objConfig.ConfigurationID, "");
						objjsInfo.anonymousGUID = (IsAnonymous) ? System.Guid.NewGuid().ToString() : "";

						System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

						string LocalResource = CommonController.GetCommonResourcesPath();
						_InnerHTML =
							_InnerHTML.Insert(BodyPosition,
								string.Format("{1}<script type=\"text/javascript\">uaInit({0});</script>",
									objSerializer.Serialize(objjsInfo),
									ConfigurationMode ?
										string.Format("<div class=\"uaWizardStop\"><a class=\"dnnPrimaryAction uaWizardButton\" href=\"{0}\" onclick=\"javascript:dnn.dom.setCookie('uaWizard', -1, -1, '/');\">{1}</a></div>{2}",
											DotNetNuke.Common.Globals.NavigateURL(objConfig.TabID),
											Localization.GetString("WizardStop", LocalResource),
											GetWizardHelp(objContext, objjsInfo.path, LocalResource)) :
                                        string.Format("<div id=\"uaPopup\" style=\"display:none;\"><span class=\"NormalRed\">{0}</span><span class=\"uaHidden\">{1}</span><div></div></div>"+
                                                      "<div id=\"uaDiff\" style=\"display:none;\"><span class=\"NormalRed\">{0}</span><span class=\"uaHidden\">{2}</span><div></div>",
											Localization.GetString("PleaseWait", LocalResource),
											Localization.GetString("DialogTitle", LocalResource),
                                            Localization.GetString("DialogTitleDiff", LocalResource))
								)
							);

						_InnerHTML =
							_InnerHTML.Insert(HeadPosition,
								string.Format("<script type=\"text/javascript\" src=\"{0}?{1}\"></script>",
									CommonController.ResolveUrl("js/ua.js", false),
									DateTime.Now.Ticks));
					}
				}
			}
			catch (Exception Exc)
			{
				DotNetNuke.Services.Exceptions.Exceptions.LogException(Exc);
			}

			outdata = System.Text.Encoding.UTF8.GetBytes(_InnerHTML);
			_HTML.Write(outdata, 0, outdata.Length);

			_HTML.Flush();
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			return _HTML.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_InnerHTML +=
				System.Text.Encoding.UTF8.GetString(buffer, offset, count);
		}
		#endregion
	}

	public class UAHttpModule : IHttpModule
	{
		#region Body

		public void Dispose()
		{ }

		public void Init(HttpApplication context)
		{
			context.ReleaseRequestState += new EventHandler(this.context_Clear);
		}

		void context_Clear(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if ((app.Response.ContentType == "text/html") && (app.Request.QueryString["TabID"] != null))
			{
				app.Context.Response.Filter = new UAHtml(app.Context.Response.Filter);
			}
		}

		#endregion
	}

}