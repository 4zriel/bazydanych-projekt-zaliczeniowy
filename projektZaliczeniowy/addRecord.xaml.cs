using System;
using System.Collections.Generic;
using System.Globalization;
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
		private int counter = 0;
		public addRecord()
		{
			InitializeComponent();
			addNameText.Focus();
			Logger.logInstance.logInfo("addRecord window initialization completed");
		}

		public DBstructure userRecord()
		{
			//DBstructure addedRecord = new DBstructure(counter);
			try
			{
				//TODO: Walidacja etc
				string formatDate = "dd MM yyyy";
				DBstructure addedRecord = new DBstructure(counter)
				{
					firstName = addNameText.Text,
					lastName = addFamilyText.Text,
					phoneNumber = Convert.ToInt32(addPhoneText.Text),
					birth = addBirthText.DisplayDate,//DateTime.ParseExact(addBirthText.Text, formatDate, CultureInfo.InvariantCulture),
					peselNumber= Convert.ToInt32(addPeselText.Text)
				};
				Logger.logInstance.logInfo(string.Format("User added record with:\nFN:{0}\tN:{1}\tPh:{2}\tB:{3}\tP:{4}",addedRecord.lastName,addedRecord.firstName,addedRecord.phoneNumber,addedRecord.birth,addedRecord.peselNumber));
				this.counter++;
				return addedRecord;
			}
			catch (Exception ex)
			{
				Logger.logInstance.logError(string.Format("Error during creation of new record!\n{0}",ex.ToString()));
				return new DBstructure(counter);
			}			
		}

		private void addButtoncancel_Click(object sender, RoutedEventArgs e)
		{
			this.added = false;
			this.Close();
			Logger.logInstance.logInfo("Adding new record canceled");
		}

		private void addButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			addNewRecord();
		}

		private void addNewRecord()
		{
			userRecord();
			this.added = true;
			this.Close();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				addNewRecord();
			}
			else if(e.Key == Key.Escape)
			{
				this.Close();
			}
		}
	}
}
