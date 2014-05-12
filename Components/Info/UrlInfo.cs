using System;
using System.Data;

namespace forDNN.Modules.UniversalAutosave
{
	public class UrlInfo
	{
		#region Private Members

		private int _UrlID;
		private string _Url;

		#endregion

		#region Constructors
		public UrlInfo()
		{
		}
		#endregion

		#region Public Properties


		public int UrlID
		{
			get
			{
				return _UrlID;
			}
			set
			{
				_UrlID= value;
			}
		}

		public string Url
		{
			get
			{
				return _Url;
			}
			set
			{
				_Url= value;
			}
		}


		#endregion
	}
}		
