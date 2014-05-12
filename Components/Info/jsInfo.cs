using System;
using System.Collections.Generic;
using System.Data;

namespace forDNN.Modules.UniversalAutosave
{
	public class jsInfo
	{
		#region Private Properties

		private List<ControlInfo> _controls;
		private List<EventInfo> _events;
		private ConfigurationInfoLimited _config;

		#endregion

		#region Constructors

		public jsInfo()
		{
			this.config = null;
			this.path = CommonController.ResolveUrl("", false);
			this.urlID = CommonController.GetCurrentUrlID();
		}

		public jsInfo(ConfigurationInfoLimited _configuration, List<ControlInfo> _controls)
			: this()
		{
			this.config = _configuration;
			this.controls = _controls;
		}

		public jsInfo(int _configID, List<ControlInfo> _controls)
			: this(ConfigurationController.Configuration_GetByPrimaryKey(_configID), _controls)
		{
		}

		#endregion

		#region Public Properties

		public ConfigurationInfoLimited config
		{
			get
			{
				return _config;
			}

			set
			{
				_config = value;
			}
		}

		public List<ControlInfo> controls
		{
			get
			{
				return _controls;
			}
			set
			{
				_controls = value;
			}
		}

		public List<EventInfo> events
		{
			get
			{
				return _events;
			}
			set
			{
				_events = value;
			}
		}

		public string path { get; set; }

		public int urlID { get; set; }

		public string anonymousGUID { get; set; }

		#endregion
	}
}
