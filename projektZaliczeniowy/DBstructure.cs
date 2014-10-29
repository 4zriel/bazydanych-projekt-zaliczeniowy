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

		private static int globalIdCounter = 0;
		private string name;
		private string familyName;
		private DateTime birthDate;
		private int phone;
		private int pesel; //TODO: zmienić na stringi ze względu na długość
		private bool selected;
		private bool deleted { get; set; }


		#region GetSet
		public DBstructure()
		{
			globalIdCounter++;
			Id = globalIdCounter;
		}
		public int Id { get; set; }
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
		public bool isSelected
		{
			get
			{
				return this.selected;
			}
			set
			{
				this.selected = value;
				notifyMe("selected");
			}
		}
		#endregion

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