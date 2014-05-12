using System;
using System.Data;

namespace forDNN.Modules.UniversalAutosave
{
	public class EventInfo
	{
		#region Private Members

		private int _EventID;
		private int _ConfigurationID;
		private string _Selector;
		private string _EventName;
		private bool _Enabled;

		#endregion

		#region Constructors
		
		public EventInfo()
		{
			this.EventName = "click";
		}

		#endregion

		#region Public Properties


		public int EventID
		{
			get
			{
				return _EventID;
			}
			set
			{
				_EventID= value;
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

		public string Selector
		{
			get
			{
				return _Selector;
			}
			set
			{
				_Selector= value;
			}
		}

		public string EventName
		{
			get
			{
				return _EventName;
			}
			set
			{
				_EventName= value;
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
				_Enabled= value;
			}
		}


		#endregion
	}
}		
