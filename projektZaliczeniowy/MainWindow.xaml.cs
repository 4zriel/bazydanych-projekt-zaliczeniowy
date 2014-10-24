using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace projektZaliczeniowy
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		//Zmienne i stałe
		public DataBaseXML mainDBFile;
		public List<DBstructure> mainDataBaseList = new List<DBstructure>();
		public bool fileOpened = false;
		public bool fileEdited = false;
		public string dataBasePath = string.Empty;


		public MainWindow()
		{			
			InitializeComponent();
			Logger.LogInstance.LogInfo("Initialization completed");
			Logger.LogInstance.LogInfo("To start using application, select [Create new] or [Open] from [File] menu");
			logTab.IsSelected = true;
		}

		#region Events
		private bool refreshLogTab()
		{
			if (!(listBoxForLogs.Items.IsEmpty))
			{
				listBoxForLogs.Items.Clear();
			}
			foreach (var item in Logger.LogInstance.logList)
			{
				listBoxForLogs.Items.Add(item);
			}
			return true;
		}
		private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.OriginalSource is TabControl)
			{
				if (logTab.IsSelected)
				{
					if (refreshLogTab())
					{
						Logger.LogInstance.LogInfo("LogTab refreshed");
					}
				}
			}
		}
		private void subMenuCreate_Click(object sender, RoutedEventArgs e)
		{
			if (!this.fileOpened)
			{
				try
				{
					dataBaseCreator();
					homeTab.IsEnabled = true;
					editTab.IsEnabled = true;
					editTab.IsSelected = true;
					homeDataGrid.ItemsSource = mainDataBaseList;
					editDataGrid.ItemsSource = mainDataBaseList;
					editDataGrid.IsEnabled = true;

				}
				catch (Exception ex)
				{
					MessageBoxResult result = MessageBox.Show(string.Format("Error during creation process! \n{0}", ex.ToString()), "Error!", MessageBoxButton.OK, MessageBoxImage.Stop);
					Logger.LogInstance.LogError("Critical error during creating DB process!");
				}
			}
			else
			{
				MessageBoxResult result = MessageBox.Show("Do You really want to create new DB without saving this one?", "Warrning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes)
				{
					dataBaseCreator();
					Logger.LogInstance.LogWarning("User destroy his DB and create new one...");
				}
				else
				{
					Logger.LogInstance.LogInfo("User try to create new one...");
				}
			}

		}
		private void subMenuSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				saveFileMenuDialog();
				Logger.LogInstance.LogInfo("DataBase file saved");
			}
			catch (Exception ex)
			{
				MessageBoxResult result = MessageBox.Show(string.Format("Błąd zapisu pliku!\n{0}", ex.ToString()), "Error! Błąd zapisu pliku", MessageBoxButton.OK, MessageBoxImage.Stop);
				Logger.LogInstance.LogError(string.Format("Critical error during saving!\n{0}", ex.Message));
			}
		}
		private void subMenuQuit_click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		#endregion
		
		#region Methods

		private void dataBaseCreator()
		{
			this.mainDBFile = new DataBaseXML();
			Logger.LogInstance.LogInfo("DataBase created");
			this.fileOpened = true;
			subMenuSave.IsEnabled = true;
		}

		private bool saveFileMenuDialog()
		{
			SaveFileDialog saveDialog = new SaveFileDialog();
			saveDialog.DefaultExt = "*.dbfile";
			saveDialog.FileName = "";
			saveDialog.Filter = "DateBase files|*.dbfile";
			bool? dialogShow = saveDialog.ShowDialog();
			if (dialogShow.HasValue && dialogShow.Value)
			{
				this.dataBasePath = saveDialog.FileName;
				this.saveFile(this.dataBasePath);
				return true;
			}
			else
				return false;
		}

		private void saveFile(string p)
		{
			this.mainDBFile.Save(p);
		}
		private void subMenuOpen_click(object sender, RoutedEventArgs e)
		{
			//TODO: napisać obsluge dialogu etc
			homeTab.IsEnabled = true;
			editTab.IsEnabled = true;
			homeTab.IsSelected = true;
		}

		private void editTabAdd_Click(object sender, RoutedEventArgs e)
		{
			var addWindow = new addRecord();
			addWindow.ShowDialog();
			Logger.LogInstance.LogInfo("User tried to add record");
			if (addWindow.added)
			{
				var tmpRecord = addWindow.userRecord(); //TODO: trzeba sprawdzić czy 2x czasem nie wywołuje tego
				mainDataBaseList.Add(tmpRecord);
				mainDBFile.addToDataBaseXML(tmpRecord);
			}
			editDataGrid.Items.Refresh();
			homeDataGrid.Items.Refresh(); //refresh na datagridzie
		}
		#endregion
	}
}
