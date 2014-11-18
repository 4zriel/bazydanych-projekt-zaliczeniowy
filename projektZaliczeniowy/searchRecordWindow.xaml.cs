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
	/// Interaction logic for searchRecordWindow.xaml
	/// </summary>
	public partial class searchRecordWindow : Window
	{
		public searchRecordWindow()
		{
			InitializeComponent();
		}
		public bool Searched
		{
			get
			{
				return SearchedRecord != null;
			}
		}
		public DBStructureViewModel _SearchedRecord = default(DBStructureViewModel);
		public DBStructureViewModel SearchedRecord 
		{
			get
			{
				return _SearchedRecord;
			}
			set
			{
				_SearchedRecord = value;
			}
		}

		private void searchButtoncancel_Click(object sender, RoutedEventArgs e)
		{
			closeWithoutSearch();
		}

		private void searchButtonSearch_Click(object sender, RoutedEventArgs e)
		{
			searchRecord();
			this.Close();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				searchRecord();
				this.Close();
			}
			else if (e.Key == Key.Escape)
			{
				closeWithoutSearch();
			}
		}

		private void closeWithoutSearch()
		{
			this.Close();
			Logger.Instance.LogInfo("Searching record canceled");
		}

		private void searchRecord()
		{
			//TODO CONVERTER :if(string.IsNullOrEmpty(searchNameText.Text)
			if (!string.IsNullOrEmpty(searchNameText.Text))
				this.SearchedRecord.Name = searchNameText.Text;
			if (!string.IsNullOrEmpty(searchFamilyText.Text))
				this.SearchedRecord.FamilyName = searchFamilyText.Text;
			if (!string.IsNullOrEmpty(searchPeselText.Text))
				this.SearchedRecord.Pesel = searchPeselText.Text;
			if (!string.IsNullOrEmpty(searchPhoneText.Text))
				this.SearchedRecord.Phone = searchPhoneText.Text;
		}
	}
}
