using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace projektZaliczeniowy
{
	/// <summary>
	/// Interaction logic for addRecord.xaml
	/// </summary>
	public partial class AddRecordWindow : Window
	{
		public DBStructureViewModel AddedRecord = default(DBStructureViewModel); //added record by user

		public bool Added
		{
			get
			{
				return AddedRecord != null;
			}
		}

		private int _innerID = default(int);

		public int InnerID
		{
			get
			{
				return _innerID;
			}
			set
			{
				_innerID = value;
			}
		}

		#region Methods

		public AddRecordWindow(int tmp)
		{
			InitializeComponent();
			addNameText.Focus();
			InnerID = tmp;
			Logger.Instance.LogInfo("addRecord window initialization completed");
		}

		public DBStructureViewModel userRecord()
		{
			try
			{
				if (!convertStringToInt(addPeselText.Text))
				{
					AddErrorText.Text = "Pesel! - use numbers";
					AddErrorBlock.Text = "Error(s):";
					AddErrorText.Foreground = Brushes.Red;
					AddErrorBlock.Foreground = Brushes.Red;
					Logger.Instance.LogError("Error in pesel!");
					if (!convertStringToInt(addPhoneText.Text))
					{
						AddErrorText.Text = "Pesel! Phone! - use numbers";
						Logger.Instance.LogError("Error in phone!");
					}
				}
				else if (!convertStringToInt(addPhoneText.Text))
				{
					AddErrorText.Text = "Phone! - use numbers";
					AddErrorBlock.Text = "Error(s):";
					AddErrorText.Foreground = Brushes.Red;
					AddErrorBlock.Foreground = Brushes.Red;
					Logger.Instance.LogError("Error in phone!");
				}
				else
				{
					this.AddedRecord = new DBStructureViewModel()
					{
						Name = addNameText.Text,
						FamilyName = addFamilyText.Text,
						Phone = addPhoneText.Text,
						BirthDate = addBirthText.DisplayDate.Date,
						Pesel = addPeselText.Text,
						Id = ++InnerID
					};
					Logger.Instance.LogInfo(string.Format("User added record with:\n\tFamilyName: {0}\n\tName: {1}\n\tPhone: {2}\n\tBirthDate: {3}\n\tPesel: {4}", AddedRecord.FamilyName, AddedRecord.Name, AddedRecord.Phone, AddedRecord.BirthDate, AddedRecord.Pesel));
				}
				return this.AddedRecord;
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(string.Format("Error during creation of new record!\n{0}", ex.ToString()));
				return new DBStructureViewModel();
			}
		}

		private bool convertStringToInt(string tmpText)
		{
			int tmp;
			try
			{
				tmp = Convert.ToInt32(tmpText);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private void addNewRecord()
		{
			userRecord();
			if (this.AddedRecord != null)
				this.Close();
		}

		#endregion Methods

		#region Events

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				addNewRecord();
			}
			else if (e.Key == Key.Escape)
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
			this.Close();
			Logger.Instance.LogInfo("Adding new record canceled");
		}

		private void addButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			addNewRecord();
		}

		#endregion Events
	}
}