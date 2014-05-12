using System;
using System.Web.UI;

using DotNetNuke;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

namespace forDNN.Modules.UniversalAutosave
{
	partial class Settings : ModuleSettingsBase
	{

		#region "Base Method Implementations"

		public override void LoadSettings()
		{
			try
			{
				if (!IsPostBack)
				{
					//if ((string)TabModuleSettings["template"] != "")
					//{
					//    txtTemplate.Text = (string)TabModuleSettings["template"];
					//}
				}
			}
			catch (Exception exc)
			{
				//Module failed to load 
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		public override void UpdateSettings()
		{
			try
			{
				//ModuleController objModules = new ModuleController();

				//objModules.UpdateTabModuleSetting(TabModuleId, "template", txtTemplate.Text);
			}

			catch (Exception exc)
			{
				//Module failed to load 
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

	}

}

