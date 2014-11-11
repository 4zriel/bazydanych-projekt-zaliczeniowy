using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektZaliczeniowy
{
	public class DBStructure
	{

		private string _name;
		private string _familyName;
		private DateTime _birthDate;
		private string _phone;
		private string _pesel;
		private bool _selected;
		private int _id;

		#region properties
		public int Id 
		{ 
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}
		private bool _deleted { get; set; }
		
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
		public string FamilyName
		{
			get
			{
				return _familyName;
			}
			set
			{
				_familyName = value;
			}
		}
		public DateTime BirthDate
		{
			get
			{
				return _birthDate;
			}
			set
			{
				_birthDate = value;
			}
		}
		public string Phone
		{
			get
			{
				return _phone;
			}
			set
			{
				_phone = value;
			}
		}
		public string Pesel
		{
			get
			{
				return _pesel;
			}
			set
			{
				_pesel = value;
			}
		}
		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				_selected = value;
			}
		}
		#endregion
	}
}
