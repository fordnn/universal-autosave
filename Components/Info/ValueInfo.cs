using System;
using System.Data;

namespace forDNN.Modules.UniversalAutosave
{
	public class ValueInfo
	{
		#region Private Members

		private int _ValueID;
		private int _ControlID;
		private int _UrlID;
		private DateTime _CreatedAt;
		private int _UserID;
		private bool _Anonymous;
		private bool _Canned;
		private bool _Closed;
		private string _Value;

		#endregion

		#region Constructors
		
		public ValueInfo()
		{
			this.CreatedAt = DateTime.Now;
		}

		#endregion

		#region Public Properties


		public int ValueID
		{
			get
			{
				return _ValueID;
			}
			set
			{
				_ValueID = value;
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

		public int UrlID
		{
			get
			{
				return _UrlID;
			}
			set
			{
				_UrlID = value;
			}
		}

		public DateTime CreatedAt
		{
			get
			{
				return _CreatedAt;
			}
			set
			{
				_CreatedAt = value;
			}
		}

		public int UserID
		{
			get
			{
				return _UserID;
			}
			set
			{
				_UserID = value;
			}
		}

		public bool Anonymous
		{
			get
			{
				return _Anonymous;
			}
			set
			{
				_Anonymous = value;
			}
		}

		public bool Canned
		{
			get
			{
				return _Canned;
			}
			set
			{
				_Canned = value;
			}
		}

		public bool Closed
		{
			get
			{
				return _Closed;
			}
			set
			{
				_Closed = value;
			}
		}

		public string Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value = value;
			}
		}


		#endregion
	}
}		
