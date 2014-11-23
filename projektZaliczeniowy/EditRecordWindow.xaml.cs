using System;
using System.Windows;
using System.Windows.Input;

namespace projektZaliczeniowy
{
	/// <summary>
	/// Interaction logic for EditRecordWindow.xaml
	/// </summary>
	public partial class EditRecordWindow : Window
	{
		public DBStructureViewModel EditedRecord = default(DBStructureViewModel);
		public DBStructureViewModel SelectedRecord = default(DBStructureViewModel);

		public bool Edited
		{
			get
			{
				return EditedRecord != null;
			}
		}

		public EditRecordWindow(DBStructureViewModel tmpSelectedRecord)
		{
			InitializeComponent();
			editNameText.Focus();
			SelectedRecord = tmpSelectedRecord;
			selectedToEditConverter(tmpSelectedRecord);
			Logger.Instance.LogInfo("editRecord window initialization completed");
		}

		#region Methods

		private void selectedToEditConverter(DBStructureViewModel selectedToEdited)
		{
			try
			{
				editNameText.Text = selectedToEdited.Name;
				editFamilyText.Text = selectedToEdited.FamilyName;
				editBirthText.Text = selectedToEdited.BirthDate.ToString();
				editPeselText.Text = selectedToEdited.Pesel;
				editPhoneText.Text = selectedToEdited.Phone;
				Logger.Instance.LogInfo("Conversion completed");
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void closeWithoutAdding()
		{
			this.Close();
			Logger.Instance.LogInfo("Editing record canceled");
		}

		private DBStructureViewModel editSaveRecord()
		{
			Logger.Instance.LogInfo("Trying to save record");
			this.EditedRecord = new DBStructureViewModel()
			{
				Name = editNameText.Text,
				FamilyName = editFamilyText.Text,
				Phone = editPhoneText.Text,
				BirthDate = editBirthText.DisplayDate.Date,
				Pesel = editPeselText.Text,
				Id = this.SelectedRecord.Id
			};
			Logger.Instance.LogInfo(string.Format("User edited record with:\n\tFamilyName: {0}\n\tName: {1}\n\tPhone: {2}\n\tBirthDate: {3}\n\tPesel: {4}", EditedRecord.FamilyName, EditedRecord.Name, EditedRecord.Phone, EditedRecord.BirthDate, EditedRecord.Pesel));
			return this.EditedRecord;
		}

		#endregion Methods

		#region Events

		private void editButtoncancel_Click(object sender, RoutedEventArgs e)
		{
			closeWithoutAdding();
		}

		private void editButtonSave_Click(object sender, RoutedEventArgs e)
		{
			editSaveRecord();
			this.Close();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				editSaveRecord();
				this.Close();
			}
			else if (e.Key == Key.Escape)
			{
				closeWithoutAdding();
			}
		}

		#endregion Events
	}
}