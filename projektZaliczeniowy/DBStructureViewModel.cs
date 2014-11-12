using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektZaliczeniowy
{
	public class DBStructureViewModel : INotifyPropertyChanged 
	{

		private static int _globalIdCounter = 0;
		#region contructor
		public DBStructureViewModel()
		{
			_dbstructure = new DBStructure();
			_globalIdCounter++;
			DBStructure.Id = _globalIdCounter;
		} 
		#endregion

		#region membrers
		DBStructure _dbstructure;
		#endregion	
		#region Properties
		public DBStructure DBStructure
		{
			get
			{
				return _dbstructure;
			}
			set
			{
				_dbstructure = value;
			}
		}
		public int Id 
		{ 
			get
			{
				return DBStructure.Id;
			}
			set
			{
				DBStructure.Id = value;
				notifyMe("ID");
			}
		}
		public string Name
		{
			get
			{
				return DBStructure.Name;
			}
			set
			{
				DBStructure.Name = value;
				notifyMe("name");
			}
		}
		public string FamilyName
		{
			get
			{
				return DBStructure.FamilyName;
			}
			set
			{
				DBStructure.FamilyName = value;
				notifyMe("name");
			}
		}
		public DateTime BirthDate
		{
			get
			{
				return DBStructure.BirthDate;
			}
			set
			{
				DBStructure.BirthDate = value;
				notifyMe("Birth date");
			}
		}
		public string Phone
		{
			get
			{
				return DBStructure.Phone;
			}
			set
			{
				DBStructure.Phone = value;
				notifyMe("phone");
			}
		}
		public string Pesel
		{
			get
			{
				return DBStructure.Pesel;
			}
			set
			{
				DBStructure.Pesel = value;
				notifyMe("PESEL");
			}
		} 
		public bool Selected
		{
			get
			{
				return DBStructure.Selected;
			}
			set
			{
				DBStructure.Selected = value;
				notifyMe("selected");
			}
		}
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		private void notifyMe(string p)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(p));
			}
		}
	}
}