using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace projektZaliczeniowy
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region properties

		public string Version = "0.17";
		private MainDBViewModel _MainDBViewModel = new MainDBViewModel();
		private bool fileOpened = false;
		private bool fileSaved = false;
		private bool isRecordSearched = false;

		#endregion properties

		#region Constructors

		public MainWindow()
		{
			InitializeComponent();
			Logger.Instance.LogInfo("Initialization completed");
			Logger.Instance.LogInfo("To start using application, select [Create new] or [Open] from [File] menu");
			logTab.IsSelected = true; //TODO: może lepiej okno z wyborem create/open?
			_MainDBViewModel = (MainDBViewModel)base.DataContext;
		}

		#endregion Constructors

		#region Methods

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

		private void createNewDB()
		{
			_MainDBViewModel.CreateNewDB();
			enableTabsAndButton();
			editTab.IsSelected = true;
			this.fileOpened = true;
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
			}
		}

		//TODO: sortowanie - jak search ten samo przekazywanie ale potem słownik i sort w słowniku? dblist.name then slownik po name sort po family -> tablica slownikow.
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
					if (_MainDBViewModel.FiltredDBList.Contains(tmpForSelectedItem))
					{
						_MainDBViewModel.FiltredDBList.Remove(tmpForSelectedItem);
						_MainDBViewModel.FiltredDBList.Add(editWindow.EditedRecord);
					}
					this.fileSaved = false;
					editDataGrid.Items.Refresh();
				}
			}
		}

		private void enableDisableButtons(bool tmp)
		{
		}

		private void enableTabsAndButton()
		{
			homeTab.IsEnabled = true;
			editTab.IsEnabled = true;
			subMenuSaveAs.IsEnabled = true;
			subMenuSave.IsEnabled = true;
			editDataGrid.IsEnabled = true;
			subMenuPrint.IsEnabled = true;
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
				if (_MainDBViewModel.DeletedDBList.Count != 0)
				{
					binTab.IsEnabled = true;
					binDataGrid.IsEnabled = true;
				}
				_MainDBViewModel.ChangeIDs();
			}
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

		private bool saveFileMenuDialog()
		{
			if (string.IsNullOrEmpty(_MainDBViewModel.DataBasePath))
				return saveDialogCreator();
			else
				_MainDBViewModel.Save();

			return true;
		}

		//TODO: enable i disable buttony w edit i bin, kiedy to wywolac?

		#endregion Methods

		#region Events

		private void appIsClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (checkIfYouCanQuit())
				e.Cancel = false;
			else
				e.Cancel = true;
		}

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

		#endregion Events

		#region ClickEvents

		private void binTabDeleteAllClick(object sender, RoutedEventArgs e)
		{
			_MainDBViewModel.PermDeleteAll();
			_MainDBViewModel.ChangeIDs();
		}

		private void binTabDeleteClick(object sender, RoutedEventArgs e)
		{
			deleteSelected();
		}

		private void binTabRestoreAllClick(object sender, RoutedEventArgs e)
		{
			_MainDBViewModel.RestoreAll();
			_MainDBViewModel.ChangeIDs();
		}

		private void binTabRestoreClick(object sender, RoutedEventArgs e)
		{
			if (_MainDBViewModel.DeleteDBSelectedItem != null)
			{
				var tmpForSelectedItem = _MainDBViewModel.DeleteDBSelectedItem;
				if (_MainDBViewModel.Restore())
				{
					checkIfDisableBin();
					this.fileSaved = false;
				}
				_MainDBViewModel.ChangeIDs();
			}
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
					if (checkIfEnableButtons(_MainDBViewModel.DBList))
					{
						//TODO: disable buttons
					}
				}
				_MainDBViewModel.ChangeIDs();
				editDataGrid.Items.Refresh();
			}
		}

		private void editDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			editSelected();
		}

		private void editSelectedClick(object sender, RoutedEventArgs e)
		{
			editSelected();
		}

		private void editTabAddClick(object sender, RoutedEventArgs e)
		{
			Logger.Instance.LogInfo("User tried to add record");
			int tmp = default(int);
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

		private void subMenuAboutClick(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show(string.Format("Projekt zaliczeniowy z przedmiotu języki programowania obiektowego\nAutor: Rafał Kłosek, 3BZI\nwersja {0}", this.Version), "About", MessageBoxButton.OK, MessageBoxImage.Information);
			Logger.Instance.LogInfo("About showed");
		}

		private void subMenuCreateClick(object sender, RoutedEventArgs e)
		{
			if (checkIfYouCanCreateNewOne())
			{
				try
				{
					createNewDB();
					if (checkIfEnableButtons(_MainDBViewModel.DBList))
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

		private void subMenuOpenClick(object sender, RoutedEventArgs e)
		{
			if (checkIfYouCanOpen())
			{
				try
				{
					openFile();
					if (checkIfEnableButtons(_MainDBViewModel.DBList))
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

		private void subMenuPrintClick(object sender, RoutedEventArgs e)
		{
			try
			{
				var printDialog = new PrintDialog();
				bool? printResult = printDialog.ShowDialog();
				if (printResult == true)
				{
					Size pageSize = new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight);
					homeDataGrid.Measure(pageSize);
					if (this.isRecordSearched)
					{
						editDataGrid.Arrange(new Rect(editDataGrid.Columns.Count(), _MainDBViewModel.FiltredDBList.Count(), pageSize.Width, pageSize.Height));
						printDialog.PrintVisual(editDataGrid, Title);
					}
					else
					{
						homeDataGrid.Arrange(new Rect(homeDataGrid.Columns.Count(), _MainDBViewModel.DBList.Count(), pageSize.Width, pageSize.Height));
						printDialog.PrintVisual(homeDataGrid, Title);
					}
				}
			}
			catch (Exception ex)
			{
				messageError(ex, "Error during printing process!");
			}
		}

		private void subMenuQuitClick(object sender, RoutedEventArgs e)
		{
			this.Close();
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

		private void searchClick(object sender, RoutedEventArgs e)
		{
			//TODO:search
			if (!this.isRecordSearched)
			{
				try
				{
					if (!string.IsNullOrEmpty(_MainDBViewModel.FilterString))
					{
						Logger.Instance.LogInfo(string.Format("User tried to search: {0}", _MainDBViewModel.FilterString));
						_MainDBViewModel.Search();
						//_MainDBViewModel.ChangeIDs();
						editDataGrid.ItemsSource = _MainDBViewModel.FiltredDBList;
						Logger.Instance.LogInfo(string.Format("Founded {0} record(s)", _MainDBViewModel.FiltredDBList.Count.ToString()));
						editDataGrid.Items.Refresh();
						_MainDBViewModel.FilterString = default(string);
						SearchBox.IsEnabled = false;
						this.isRecordSearched = true;
					}
				}
				catch (Exception ex)
				{
					MessageBoxResult result = MessageBox.Show(ex.Message.ToString(), "Empty search result", MessageBoxButton.OK, MessageBoxImage.Stop);
					Logger.Instance.LogWarning(ex.Message.ToString());
				}
			}
			else
			{
				//_MainDBViewModel.ChangeIDs();
				editDataGrid.ItemsSource = _MainDBViewModel.DBList;
				editDataGrid.Items.Refresh();
				_MainDBViewModel.FiltredDBList.Clear();
				SearchBox.IsEnabled = true;
				this.isRecordSearched = false;
				Logger.Instance.LogInfo("SearchBox enabled");
			}
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

		#endregion ClickEvents

		private void sortClick(object sender, RoutedEventArgs e)
		{
			throw new Exception();
		}
	}
}