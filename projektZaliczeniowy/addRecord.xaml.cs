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
		public DBstructure addedRecord; //added record by user

		#region Methods
		public addRecord()
		{
			InitializeComponent();
			addNameText.Focus();
			Logger.LogInstance.LogInfo("addRecord window initialization completed");
		}
		public DBstructure userRecord()
		{
			try
			{
				//TODO: Walidacja etc
				this.addedRecord = new DBstructure()
				{
					firstName = addNameText.Text,
					lastName = addFamilyText.Text,
					phoneNumber = Convert.ToInt32(addPhoneText.Text),
					birth = addBirthText.DisplayDate.Date,
					peselNumber = Convert.ToInt32(addPeselText.Text)
				};
				Logger.LogInstance.LogInfo(string.Format("User added record with:\n\tFamilyName: {0}\n\tName: {1}\n\tPhone: {2}\n\tBirthDate: {3}\n\tPesel: {4}", addedRecord.lastName, addedRecord.firstName, addedRecord.phoneNumber, addedRecord.birth, addedRecord.peselNumber));
				return this.addedRecord;
			}
			catch (Exception ex)
			{
				Logger.LogInstance.LogError(string.Format("Error during creation of new record!\n{0}", ex.ToString()));
				return new DBstructure();
			}
		}
		private void addNewRecord()
		{
			userRecord();
			this.added = true;
			this.Close();
		} 
		#endregion

		#region Events
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				addNewRecord();
			}
			else if(e.Key == Key.Escape)
			{
				closeWithoutAdding();
			}
		}
		private void addButtoncancel_Click(object sender, RoutedEventArgs e)
		{
			closeWithoutAdding();
		}

		private void closeWithoutAdding()
		{
			this.added = false;
			this.Close();
			Logger.LogInstance.LogInfo("Adding new record canceled");
		}
		private void addButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			addNewRecord();
		}
		#endregion
	}
}