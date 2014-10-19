using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projektZaliczeniowy
{
	/// <summary>
	/// Interaction logic for addRecord.xaml
	/// </summary>
	public partial class addRecord : Window
	{
		public bool added = false;
		public addRecord()
		{
			InitializeComponent();
		}

		public DBstructure userRecord()
		{
			DBstructure addedRecord = new DBstructure();
			try
			{
				//TODO: Walidacja etc
				addedRecord.name = addNameText.Text;
				addedRecord.familyName = addFamilyText.Text;
				addedRecord.phone = Convert.ToInt32(addPhoneText.Text);
				//addedRecord.birthDate = Convert.ToDateTime(addBirthText.Text);
				addedRecord.pesel = Convert.ToInt32(addPeselText.Text);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			return addedRecord;
		}

		private void addButtoncancel_Click(object sender, RoutedEventArgs e)
		{
			this.added = false;
			this.Close();
		}

		private void addButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			userRecord();
			this.added = true;
		}
	}
}
