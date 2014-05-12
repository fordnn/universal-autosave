using System;
using System.Data;

namespace forDNN.Modules.UniversalAutosave
{
	public class AnonymousInfo
	{
		#region Private Members

		private int _AnonymousID;
		private System.Guid _UserGUID;
		private DateTime _CreatedAt;
		private string _IP;

		#endregion

		#region Constructors
		
		public AnonymousInfo()
		{
			this.CreatedAt = DateTime.Now;
		}

		#endregion

		#region Public Properties


		public int AnonymousID
		{
			get
			{
				return _AnonymousID;
			}
			set
			{
				_AnonymousID= value;
			}
		}

		public System.Guid UserGUID
		{
			get
			{
				return _UserGUID;
			}
			set
			{
				_UserGUID= value;
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
				_CreatedAt= value;
			}
		}

		public string IP
		{
			get
			{
				return _IP;
			}
			set
			{
				_IP= value;
			}
		}


		#endregion
	}

	public class TempUserInfo
	{
		public int UserID { get; set; }
		public string UserName { get; set; }
	}
}		
