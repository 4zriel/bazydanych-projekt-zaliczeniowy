using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		#region properties
		private bool fileOpened = false;
		private bool fileSaved = false;
		public string Version = "0.8";
		private MainDBViewModel _MainDBViewModel = new MainDBViewModel(); 
		#endregion

		public MainWindow()
		{
			InitializeComponent();
			Logger.Instance.LogInfo("Initialization completed");
			Logger.Instance.LogInfo("To start using application, select [Create new] or [Open] from [File] menu");
			logTab.IsSelected = true; //TODO: może lepiej okno z wyborem create/open?
			_MainDBViewModel = (MainDBViewModel)base.DataContext;
		}

		#region Methods
		private bool saveFileMenuDialog()
		{
			if (string.IsNullOrEmpty(_MainDBViewModel.DataBasePath))
				return saveDialogCreator();
			else
				_MainDBViewModel.Save();

			return true;
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
				_MainDBViewModel.DataBasePath = saveDialog.FileName;
				_MainDBViewModel.Save();
				Logger.Instance.LogInfo(string.Format("User saved DB in {0}", saveDialog.FileName));

				return true;
			}
			else
				return false;
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
			foreach (var item in Logger.Instance.LogList)
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
			Logger.Instance.LogError(string.Format("{1}!\n{0}", ex.Message, message));
		}
		private bool checkIfYouCanQuit()
		{
			if (!this.fileOpened)
			{
				return true;
			}
			else if (!this.fileSaved)
			{
				MessageBoxResult result = MessageBox.Show("Do You really want to close without saving?", "Warrning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
				if (result == MessageBoxResult.Yes)
				{
					Logger.Instance.LogWarning("User closed app without saveing DB file...");
					return true;
				}
				else
				{
					if (saveFileMenuDialog())
					{
						this.fileSaved = true;
						Logger.Instance.LogInfo("User tried to close without save...");
						return true;
					}
					return false;
				}
			}
			else
				return true;
		}
		private bool checkIfYouCanOpen()
		{
			if (this.fileOpened)
			{
				if (this.fileSaved)
				{
					return true;
				}
				else
				{
					MessageBoxResult result = MessageBox.Show("Do You really want to open new one without saving?", "Warrning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (result == MessageBoxResult.Yes)
					{
						Logger.Instance.LogWarning("User open other DB without saving DB file...");
						return true;
					}
					else
					{
						if (saveFileMenuDialog())
						{
							this.fileSaved = true;
							Logger.Instance.LogInfo("User tried to open new one...");
							return true;
						}
						return false;
					}
				}
			}
			else
				return true;
		}
		private bool checkIfYouCanCreateNewOne()
		{
			if (this.fileOpened)
			{
				if (!this.fileSaved)
				{
					return true;
				}
				else
				{
					MessageBoxResult result = MessageBox.Show("Do You really want to create new one without saving?", "Warrning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
					if (result == MessageBoxResult.Yes)
					{
						Logger.Instance.LogWarning("User create other DB without saving DB file...");
						return true;
					}
					else
					{
						if (saveFileMenuDialog())
						{
							this.fileSaved = true;
							Logger.Instance.LogInfo("User tried to create new one...");
							return true;
						}
						return false;
					}
				}
			}
			else
				return true;
		}		
		private void createNewDB()
		{
			_MainDBViewModel.CreateNewDB();
			enableTabsAndButton();
			editTab.IsSelected = true;
			this.fileOpened = true;
		}
		private void openFile()
		{
			OpenFileDialog openDialog = new OpenFileDialog();
			openDialog.DefaultExt = "*.dbfile";
			openDialog.FileName = "";
			openDialog.Filter = "DateBase files|*.dbfile";
			bool? openDialogShow = openDialog.ShowDialog();
			_MainDBViewModel.DBList.Clear();
			if (openDialogShow.HasValue && openDialogShow.Value)
			{
				_MainDBViewModel.DataBasePath = openDialog.FileName;
				Logger.Instance.LogInfo(string.Format("User opened file {0}", openDialog.FileName));
				_MainDBViewModel.Load();
				this.fileOpened = true;
				this.fileSaved = true;
				enableTabsAndButton();
				homeTab.IsSelected = true;
			}
		}
		private void editSelected()
		{
			//TODO: napisac działające z bindingiem commad
			if (_MainDBViewModel.DBSelectedItem != null)
			{
				var tmpForSelectedItem = _MainDBViewModel.DBSelectedItem;
				Logger.Instance.LogInfo("User tried to edit record");
				Logger.Instance.LogInfo(string.Format("Selected record is:\n\tFamilyName: {0}\n\tName: {1}\n\tPhone: {2}\n\tBirthDate: {3}\n\tPesel: {4}", _MainDBViewModel.DBSelectedItem.FamilyName, _MainDBViewModel.DBSelectedItem.Name, _MainDBViewModel.DBSelectedItem.Phone, _MainDBViewModel.DBSelectedItem.BirthDate, _MainDBViewModel.DBSelectedItem.Pesel));
				var editWindow = new EditRecordWindow(_MainDBViewModel.DBSelectedItem);
				editWindow.Owner = this;
				editWindow.ShowDialog();

				if (editWindow.Edited)
				{
					_MainDBViewModel.DBList.Remove(_MainDBViewModel.DBSelectedItem);
					_MainDBViewModel.DBList.Add(editWindow.EditedRecord);
					this.fileSaved = false;
				}
			}
		}
		private void checkIfDisableBin()
		{
			if (_MainDBViewModel.DeletedDBList.Count() == 0)
			{
				binTab.IsEnabled = false;
				editTab.IsSelected = true;
				//TODO: disable buttons
			}
		}
		private bool checkIfEnableButtons(ObservableCollection<DBStructureViewModel> list)
		{
			if (list.Count == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
		private void enableDisableButtons(bool tmp)
		{
		}
		//TODO: enable i disable buttony w edit i bin, kiedy to wywolac?
		//TODO: search
		private void search()
		{
			//TODO otwieramy nowe okno search -> add/edit
			//zwaracam record(y)
			//timer? mruganie?
		}
		private void deleteSelected()
		{
			if (_MainDBViewModel.DeleteDBSelectedItem != null)
			{
				var tmpForSelectedItem = _MainDBViewModel.DeleteDBSelectedItem;
				if (_MainDBViewModel.PermDelete())
				{
					checkIfDisableBin();
				}
				//TODO - zapisać lisę
				//TODO - restore all
				//TODO: przegladanie paczkami
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
						Logger.Instance.LogInfo("LogTab refreshed");
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
			if (checkIfYouCanOpen())
			{
				try
				{
					openFile();
					if(checkIfEnableButtons(_MainDBViewModel.DBList))
					{
						//TODO: enable buttons
					}
				}
				catch (Exception)
				{
					throw;
				}
			}
		}
		private void editTabAddClick(object sender, RoutedEventArgs e)
		{
			Logger.Instance.LogInfo("User tried to add record");
			int tmp =default(int);
			if (_MainDBViewModel.DBList.Count != 0)
			{
				tmp = _MainDBViewModel.DBList.Last().Id;				
			}
			var addWindow = new AddRecordWindow(tmp);
			addWindow.Owner = this;
			addWindow.ShowDialog();
			if (addWindow.Added)
			{
				_MainDBViewModel.AddNewRecord(addWindow.AddedRecord);
				this.fileSaved = false;
			}
		}
		private void subMenuSaveAsClick(object sender, RoutedEventArgs e)
		{
			if (saveDialogCreator())
			{
				this.fileSaved = true;
				Logger.Instance.LogInfo("DataBase file saved");
				Logger.Instance.LogWarning("User use save as option");
			}
		}
		private void subMenuCreateClick(object sender, RoutedEventArgs e)
		{
			if (checkIfYouCanCreateNewOne())
			{
				try
				{
					createNewDB();
					if(checkIfEnableButtons(_MainDBViewModel.DBList))
					{
						//TODO eneable buttons
					}
				}
				catch (Exception ex)
				{
					messageError(ex, "Error during creation process!");
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
					Logger.Instance.LogInfo("DataBase file saved");
				}
			}
			catch (Exception ex)
			{
				messageError(ex, "Błąd zapisu pliku!");
			}
		}
		private void subMenuQuitClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		private void subMenuAboutClick(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(string.Format("Projekt zaliczeniowy z przedmiotu języki programowania obiektowego\nAutor: Rafał Kłosek, 3BZI\nwersja {0}", this.Version), "About", MessageBoxButton.OK, MessageBoxImage.Information);
			Logger.Instance.LogInfo("About showed");
		}
		private void editSelectedClick(object sender, RoutedEventArgs e)
		{
			editSelected();
		}
		private void editDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			editSelected();
		}
		private void deleteSelectedClick(object sender, RoutedEventArgs e)
		{
			if (_MainDBViewModel.DBSelectedItem != null)
			{
				var tmpForSelectedItem = _MainDBViewModel.DBSelectedItem;
				if (_MainDBViewModel.Delete())
				{
					binTab.IsEnabled = true;
					binDataGrid.IsEnabled = true;
					this.fileSaved = false;
					if(checkIfEnableButtons(_MainDBViewModel.DBList))
					{
						//TODO: disable buttons
					}
				}
				//TODO - zmiana id
			}
		}
		private void binTabRestoreClick(object sender, RoutedEventArgs e)
		{
			//TODO: restore
			if (_MainDBViewModel.DeleteDBSelectedItem != null)
			{
				var tmpForSelectedItem = _MainDBViewModel.DeleteDBSelectedItem;
				if (_MainDBViewModel.Restore())
				{
					checkIfDisableBin();
					this.fileSaved = false;
				}
				//TODO - zmiana id?	-> funkcja bo bedzie w kilku miejscach				
			}
		}
		private void binTabDeleteClick(object sender, RoutedEventArgs e)
		{
			deleteSelected();
		}				
		/// <summary>
		/// Podstawowa funkcja do obsługi klawiatury (skrótów) w głównym oknie
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.O))
			{
				if (checkIfYouCanOpen())
				{
					try
					{
						openFile();
					}
					catch (Exception)
					{
						throw;
					}
				}
			}
			if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.S))
			{
				try
				{
					if (saveFileMenuDialog())
					{
						this.fileSaved = true;
						Logger.Instance.LogInfo("DataBase file saved");
					}
				}
				catch (Exception ex)
				{
					messageError(ex, "Błąd zapisu pliku!");
				}
			}
			if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.N))
			{
				if (checkIfYouCanCreateNewOne())
				{
					try
					{
						createNewDB();
					}
					catch (Exception ex)
					{
						messageError(ex, "Error during creation process!");
					}
				}
			}
			if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.Q))
			{
				this.Close();
			}
		}
		#endregion
	}
}