using System;

namespace projektZaliczeniowy
{
	public class DBStructureViewModel : ViewModelBase
	{
		//private static int GlobalIdCounter = 0;//TODO: przerobić to bo ID się źle generuje
		#region contructor
		public DBStructureViewModel()
		{
			//GlobalIdCounter++;
			//Id = GlobalIdCounter;
		}
		#endregion
		
		private string _Name = default(string);
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
				NotifyMe("Name");
			}
		}

		private string _FamilyName = default(string);
		public string FamilyName
		{
			get
			{
				return _FamilyName;
			}
			set
			{
				_FamilyName = value;
				NotifyMe("FamilyName");
			}
		}

		private DateTime _BirthDate = default(DateTime);
		public DateTime BirthDate
		{
			get
			{
				return _BirthDate;
			}
			set
			{
				_BirthDate = value;
				NotifyMe("BirthDate");
			}
		}

		private string _Phone = default(string);
		public string Phone
		{
			get
			{
				return _Phone;
			}
			set
			{
				_Phone = value;
				NotifyMe("Phone");
			}
		}

		private string _Pesel = default(string);
		public string Pesel
		{
			get
			{
				return _Pesel;
			}
			set
			{
				_Pesel = value;
				NotifyMe("Pesel");
			}
		}

		private int _Id = default(int);
		public int Id
		{
			get
			{
				return _Id;
			}
			set
			{
				_Id = value;
				NotifyMe("Id");
			}
		}

		public void resetID()
		{
			this._Id = 0;
		}
	}
}
