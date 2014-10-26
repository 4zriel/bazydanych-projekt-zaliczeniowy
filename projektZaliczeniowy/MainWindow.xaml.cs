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
		private bool fileOpened = false;
		private bool fileEdited = false;
		private bool fileSaved = false;
		public string dataBasePath = string.Empty;
		public double version = 0.6;
		public MainWindow()
		{			
			InitializeComponent();
			Logger.LogInstance.LogInfo("Initialization completed");
			Logger.LogInstance.LogInfo("To start using application, select [Create new] or [Open] from [File] menu");
			logTab.IsSelected = true; //TODO: może lepiej okno z wyborem create/open?
		}

		#region Methods
		/// <summary>
		/// metoda do tworzenia nowej bazy
		/// </summary>
		private void dataBaseCreator()
		{
			this.mainDBFile = new DataBaseXML();
			Logger.LogInstance.LogInfo("DataBase created");
			this.fileOpened = true;			
		}
		private bool saveFileMenuDialog()
		{
			if (this.dataBasePath.Length == 0)
			{
				return saveDialogCreator();
			}
			else
			{
				this.saveFile(this.dataBasePath); //if alredy path added (once saved)
				return true;
			}
		}
		private bool saveDialogCreator()
		{
			SaveFileDialog saveDialog = new SaveFileDialog();
			saveDialog.DefaultExt = "*.dbfile";
			saveDialog.FileName = "";
			saveDialog.Filter = "DateBase files|*.dbfile";
			bool? saveDialogShow = saveDialog.ShowDialog();
			if (saveDialogShow.HasValue && saveDialogShow.Value)
			{
				this.dataBasePath = saveDialog.FileName;
				this.saveFile(this.dataBasePath);
				Logger.LogInstance.LogInfo(string.Format("User saved DB in {0}", this.dataBasePath));
				return true;
			}
			else
				return false;
		}
		private void saveFile(string p)
		{
			this.mainDBFile.Save(p);
		}		
		/// <summary>
		/// głowna metoda odpowiadająca za odświeżanie tabów
		/// </summary>
		/// <returns></returns>
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
		private void enableTabsAndButton()
		{
			homeTab.IsEnabled = true;
			editTab.IsEnabled = true;
			subMenuSaveAs.IsEnabled = true;
			subMenuSave.IsEnabled = true;
			editDataGrid.IsEnabled = true;
		}
		/// <summary>
		/// obsługa wyjątków przez messageboxa
		/// </summary>
		/// <param name="ex">wyjątek</param>
		/// <param name="message">string wyswietlany w wiadomości</param>
		private static void messageError(Exception ex, string message)
		{
			MessageBoxResult result = MessageBox.Show(string.Format("{1}\n{0}", ex.ToString(), message), string.Format("Error! {0}", message), MessageBoxButton.OK, MessageBoxImage.Stop);
			Logger.LogInstance.LogError(string.Format("{1}!\n{0}", ex.Message, message));
		}
		private bool checkIfYouCanQuit()
		{
			if (this.fileSaved && !this.fileEdited)
			{
				return true;
			}
			else
			{
				MessageBoxResult result = MessageBox.Show("Do You really want to close without saving?", "Warrning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes)
				{
					Logger.LogInstance.LogWarning("User closed app without saveing DB file...");
					return true;
				}
				else
				{
					if (saveFileMenuDialog())
					{
						this.fileSaved = true;
						this.fileEdited = false;
						Logger.LogInstance.LogInfo("User tried to create new one...");
						return true;
					}
					return false;
				}
			}
		}
#endregion

		#region Events
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
		private void appIsClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (checkIfYouCanQuit())
				e.Cancel = false;
			else
				e.Cancel = true;
		}
		#endregion

		#region ClickEvents
		private void subMenuOpenClick(object sender, RoutedEventArgs e)
		{
			//TODO: napisać obsluge dialogu etc
			OpenFileDialog openDialog = new OpenFileDialog();
			openDialog.DefaultExt = "*.dbfile";
			openDialog.FileName = "";
			openDialog.Filter = "DateBase files|*.dbfile";
			bool? openDialogShow = openDialog.ShowDialog();
			if(openDialogShow.HasValue && openDialogShow.Value)
			{
				this.dataBasePath = openDialog.FileName;
				Logger.LogInstance.LogInfo("User opened file...");
				this.mainDBFile = new DataBaseXML(this.dataBasePath, ref this.mainDataBaseList);
				//TODO: dodanie do struktury
			}
			this.fileOpened = true;
			enableTabsAndButton();
			homeTab.IsSelected = true;
			homeDataGrid.ItemsSource = mainDataBaseList;
			editDataGrid.ItemsSource = mainDataBaseList;
		}
		private void editTabAddClick(object sender, RoutedEventArgs e)
		{
			Logger.LogInstance.LogInfo("User tried to add record");
			var addWindow = new addRecord();
			addWindow.ShowDialog();			
			if (addWindow.added)
			{
				DBstructure tmpRecord = addWindow.addedRecord;
				mainDataBaseList.Add(tmpRecord);
				mainDBFile.addToDataBaseXML(tmpRecord);
				this.fileEdited = true;
			}
			editDataGrid.Items.Refresh();
			homeDataGrid.Items.Refresh(); //refresh na datagridzie
		}
		private void subMenuSaveAsClick(object sender, RoutedEventArgs e)
		{
			if (saveFileMenuDialog())
			{
				this.fileSaved = true;
				this.fileEdited = false;
				Logger.LogInstance.LogInfo("DataBase file saved");
				Logger.LogInstance.LogWarning("User use save as option");
			}			
		}
		private void subMenuCreateClick(object sender, RoutedEventArgs e)
		{
			if (!this.fileOpened)
			{
				try
				{
					dataBaseCreator();
					enableTabsAndButton();
					editTab.IsSelected = true;
					homeDataGrid.ItemsSource = mainDataBaseList;
					editDataGrid.ItemsSource = mainDataBaseList;
				}
				catch (Exception ex)
				{
					messageError(ex, "Error during creation process!");
				}
			}
			else
			{
				MessageBoxResult result = MessageBox.Show("Do You really want to create new DB without saving this one?", "Warrning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes)
				{
					dataBaseCreator();
					Logger.LogInstance.LogWarning("User destroyed his DB and created new one...");
				}
				else
				{
					Logger.LogInstance.LogInfo("User tried to create new one...");
				}
			}

		}
		private void subMenuSaveClick(object sender, RoutedEventArgs e)
		{
			try
			{
				if (saveFileMenuDialog())
				{
					this.fileSaved = true;
					this.fileEdited = false;
					Logger.LogInstance.LogInfo("DataBase file saved");
				}
			}
			catch (Exception ex)
			{
				messageError(ex, "Błąd zapisu pliku!");
			}
		}		
		private void subMenuQuitClick(object sender, RoutedEventArgs e)
		{
			if (checkIfYouCanQuit())
				this.Close();	
		}
		private void subMenuAboutClick(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(string.Format("Projekt zaliczeniowy z przedmiotu języki programowania obiektowego\nAutor: Rafał Kłosek, 3BZI\nwersja {0}", this.version),"About", MessageBoxButton.OK, MessageBoxImage.Information);
			Logger.LogInstance.LogInfo("About showed");
		}
		#endregion
	}
}