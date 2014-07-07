using System;
using System.Data;

namespace forDNN.Modules.UniversalAutosave
{

	public class ControlInfo
	{
		#region Private Members

		private int _ControlID;
		private int _ConfigurationID;
		private string _Selector;
		private bool _Enabled;
		private string _Value;
		private bool _RestoreOnLoad;
		private bool _RestoreIfEmpty;
		private bool _ShowCannedOnly;
        private string _RTFType;
		
        #endregion

		#region Constructors

		public ControlInfo()
		{
			this.RestoreOnLoad = true;
			this.RestoreIfEmpty = true;
			this.ShowCannedOnly = false;
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

		public string Selector
		{
			get
			{
				return _Selector;
			}
			set
			{
				_Selector = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return _Enabled;
			}
			set
			{
				_Enabled = value;
			}
		}

		public string Value
		{
			get
			{
				return (_Value == null) ? "" : _Value;
			}
			set
			{
				_Value = value;
			}
		}

		public bool RestoreOnLoad
		{
			get
			{
				return _RestoreOnLoad;
			}
			set
			{
				_RestoreOnLoad = value;
			}
		}

		public bool RestoreIfEmpty
		{
			get
			{
				return _RestoreIfEmpty;
			}
			set
			{
				_RestoreIfEmpty = value;
			}
		}

		public bool ShowCannedOnly
		{
			get
			{
				return _ShowCannedOnly;
			}
			set
			{
				_ShowCannedOnly = value;
			}
		}

        public int ControlID
        {
            get
            {
                return _ControlID;
            }
            set
            {
                _ControlID = value;
            }
        }

        public string RTFType
        {
            get { return _RTFType; }
            set { _RTFType = value; }
        }


		#endregion
	}
}
