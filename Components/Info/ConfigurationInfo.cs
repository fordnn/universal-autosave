using System;
using System.Data;

namespace forDNN.Modules.UniversalAutosave
{
	public class ConfigurationInfo : ConfigurationInfoLimited
	{
		#region Private Members

		private string _Title;
		//private string _LinkTitle;
		private string _Description;
		//private int _TabID;
		private bool _AutosaveEnabled;
		private int _HistoryLength;
		private int _HistoryExpiry;
		//private int _AutosaveLocation;

		#endregion

		#region Constructors
		public ConfigurationInfo()
		{
		}
		#endregion

		#region Public Properties

		public string Title
		{
			get
			{
				return _Title;
			}
			set
			{
				_Title= value;
			}
		}

		public string Description
		{
			get
			{
				return _Description;
			}
			set
			{
				_Description= value;
			}
		}

        //public int TabID
        //{
        //    get
        //    {
        //        return _TabID;
        //    }
        //    set
        //    {
        //        _TabID= value;
        //    }
        //}

		public bool AutosaveEnabled
		{
			get
			{
				return _AutosaveEnabled;
			}
			set
			{
				_AutosaveEnabled = value;
			}
		}

		public int HistoryLength
		{
			get
			{
				return _HistoryLength;
			}
			set
			{
				_HistoryLength= value;
			}
		}

		public int HistoryExpiry
		{
			get
			{
				return _HistoryExpiry;
			}
			set
			{
				_HistoryExpiry= value;
			}
		}

        //public int AutosaveLocation
        //{
        //    get
        //    {
        //        return _AutosaveLocation;
        //    }
        //    set
        //    {
        //        _AutosaveLocation= value;
        //    }
        //}


		#endregion
	}

	public class ConfigurationInfoLimited
	{
		#region Private Members

		private int _ConfigurationID;
		private bool _AutosaveIcon;
		private bool _AutosaveOnBlur;
		private int _AutosavePeriod;
		private int _AutosaveLocation;
		private bool _UrlIndependent;
		private bool _ConfigurationMode;
        private int _TabID;
        private bool _IsGlobalConfig;

		#endregion

		#region Constructors

		public ConfigurationInfoLimited()
		{
		}

		public ConfigurationInfoLimited(ConfigurationInfo objConfiguration)
		{
			this.AutosaveIcon = objConfiguration.AutosaveIcon;
			this.AutosaveLocation = objConfiguration.AutosaveLocation;
			this.AutosaveOnBlur = objConfiguration.AutosaveOnBlur;
			this.AutosavePeriod = objConfiguration.AutosavePeriod;
			this.ConfigurationID = objConfiguration.ConfigurationID;
			this.UrlIndependent = objConfiguration.UrlIndependent;
			this.ConfigurationMode = objConfiguration.ConfigurationMode;
            this.TabID = objConfiguration.TabID;
            this.IsGlobalConfig = objConfiguration.IsGlobalConfig;
		}

		#endregion

		#region Public Properties

		public int ConfigurationID
		{
			get
			{
				return _ConfigurationID;
			}
			set
			{
				_ConfigurationID = value;
			}
		}

		public bool AutosaveIcon
		{
			get
			{
				return _AutosaveIcon;
			}
			set
			{
				_AutosaveIcon = value;
			}
		}

		public bool AutosaveOnBlur
		{
			get
			{
				return _AutosaveOnBlur;
			}
			set
			{
				_AutosaveOnBlur = value;
			}
		}

		public int AutosavePeriod
		{
			get
			{
				return _AutosavePeriod;
			}
			set
			{
				_AutosavePeriod = value;
			}
		}

		public int AutosaveLocation
		{
			get
			{
				return _AutosaveLocation;
			}
			set
			{
				_AutosaveLocation = value;
			}
		}

		public bool ConfigurationMode
		{
			get
			{
				return _ConfigurationMode;
			}
			set
			{
				_ConfigurationMode = value;
			}
		}

		public bool UrlIndependent
		{
			get
			{
				return _UrlIndependent;
			}
			set
			{
				_UrlIndependent = value;
			}
		}
        
        public int TabID
        {
            get { return _TabID; }
            set { _TabID = value; }
        }

        public bool IsGlobalConfig
        {
            get { return _IsGlobalConfig; }
            set { _IsGlobalConfig = value; }
        }
		#endregion
	}
}		
