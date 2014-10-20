using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektZaliczeniowy
{
	public class DBstructure : INotifyPropertyChanged 
	{

		public static int ID = 0;
		public string name;
		public string familyName;
		public DateTime birthDate;
		public int phone;
		public int pesel;
		
		public static int idNumber 
		{
			get 
			{ 
				return ID; 
			}
			set
			{
				ID++;
			}
		}
		public string firstName
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				notifyMe("name");
			}
		}
		public string lastName
		{
			get
			{
				return familyName;
			}
			set
			{
				familyName = value;
				notifyMe("name");
			}
		}
		public DateTime birth
		{
			get
			{
				return this.birthDate;
			}
			set
			{
				this.birthDate = value;
				notifyMe("Birth date");
			}
		}
		public int phoneNumber
		{
			get
			{
				return this.phone;
			}
			set
			{
				this.phone = value;
				notifyMe("phone");
			}
		}
		public int peselNumber
		{
			get
			{
				return this.pesel;
			}
			set
			{
				this.pesel = value;
				notifyMe("PESEL");
			}
		}

		//autogenerate 
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
