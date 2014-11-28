using System;
using System.Collections.ObjectModel;

namespace projektZaliczeniowy
{
	public class MainDBViewModel : ViewModelBase
	{
		#region Constructors

		public MainDBViewModel()
		{
			_MainDBXml = new DataBaseXML();
			_DeletedDBXml = new DataBaseXML();
			_DBList = new ObservableCollection<DBStructureViewModel>();
			_DeletedDBList = new ObservableCollection<DBStructureViewModel>();
			_FiltredDBList = new ObservableCollection<DBStructureViewModel>();
			_filterString = default(string);
		}

		#endregion Constructors

		//private ICollectionView _DBListView;
		//public ICollectionView DBListView
		//{
		//	get
		//	{
		//		return _DBListView;
		//	}
		//	set
		//	{
		//		_DBListView = value;
		//		NotifyMe("DBListView");
		//	}
		//}

		#region Properties

		private string _DataBasePath = default(string);

		public string DataBasePath
		{
			get
			{
				return _DataBasePath;
			}
			set
			{
				_DataBasePath = value;
			}
		}

		private ObservableCollection<DBStructureViewModel> _DBList = default(ObservableCollection<DBStructureViewModel>);

		public ObservableCollection<DBStructureViewModel> DBList
		{
			get
			{
				return _DBList;
			}
			set
			{
				_DBList = value;
				NotifyMe("DBList");
			}
		}

		private DBStructureViewModel _DBSelectedItem = default(DBStructureViewModel);

		public DBStructureViewModel DBSelectedItem
		{
			get
			{
				return _DBSelectedItem;
			}
			set
			{
				_DBSelectedItem = value;
				NotifyMe("DBSelectedItem");
			}
		}

		private DBStructureViewModel _DeleteDBSelectedItem = default(DBStructureViewModel);

		public DBStructureViewModel DeleteDBSelectedItem
		{
			get
			{
				return _DeleteDBSelectedItem;
			}
			set
			{
				_DeleteDBSelectedItem = value;
				NotifyMe("DeleteDBSelectedItem");
			}
		}

		private ObservableCollection<DBStructureViewModel> _DeletedDBList = default(ObservableCollection<DBStructureViewModel>);

		public ObservableCollection<DBStructureViewModel> DeletedDBList
		{
			get
			{
				return _DeletedDBList;
			}
			set
			{
				_DeletedDBList = value;
				NotifyMe("DeletedDBList");
			}
		}

		private string _filterString;

		public string FilterString
		{
			get
			{
				return _filterString;
			}
			set
			{
				_filterString = value;
				NotifyMe("FilterString");
				//if (value.Count() >= 1)
				//{
				//	filtrMe(value, true);
				//}
				//else
				//	filtrMe(value, false);
			}
		}

		private ObservableCollection<DBStructureViewModel> _FiltredDBList = default(ObservableCollection<DBStructureViewModel>);

		public ObservableCollection<DBStructureViewModel> FiltredDBList
		{
			get
			{
				return _FiltredDBList;
			}
			set
			{
				_FiltredDBList = value;
				NotifyMe("FiltredList");
			}
		}

		private DataBaseXML _MainDBXml;
		private DataBaseXML _DeletedDBXml;

		#endregion Properties

		#region Methods

		public void AddNewRecord(DBStructureViewModel record)
		{
			try
			{
				_DBList.Add(record);
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(ex.Message);
			}
		}

		public void CreateNewDB()
		{
			try
			{
				_MainDBXml = new DataBaseXML();
				Logger.Instance.LogInfo("DataBase created");
				DBList.Clear();
				_DataBasePath = default(string);
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(ex.Message);
			}
		}

		public bool Delete()
		{
			try
			{
				DeletedDBList.Add(this.DBSelectedItem);
				Logger.Instance.LogInfo(string.Format("Deleted record is:\n\tFamilyName: {0}\n\tName: {1}\n\tPhone: {2}\n\tBirthDate: {3}\n\tPesel: {4}", this.DBSelectedItem.FamilyName, this.DBSelectedItem.Name, this.DBSelectedItem.Phone, this.DBSelectedItem.BirthDate, this.DBSelectedItem.Pesel));
				DBList.Remove(DBSelectedItem);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(ex.Message);
				return false;
			}
		}

		public void Load()
		{
			try
			{
				_MainDBXml = new DataBaseXML(DataBasePath);
				DBList = new ObservableCollection<DBStructureViewModel>(_MainDBXml.LoadDB());
				_MainDBXml = new DataBaseXML();
				_DeletedDBXml = new DataBaseXML(_DataBasePath + "_deleted");
				DeletedDBList = new ObservableCollection<DBStructureViewModel>(_DeletedDBXml.LoadDB());
				_DeletedDBXml = new DataBaseXML();
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(ex.Message);
			}
		}

		public void Save()
		{
			try
			{
				foreach (var item in DBList)
				{
					_MainDBXml.addToDataBaseXML(item);
				}
				_MainDBXml.Save(_DataBasePath);
				_MainDBXml = new DataBaseXML();
				foreach (var item in DeletedDBList)
				{
					_DeletedDBXml.addToDataBaseXML(item);
				}
				_DeletedDBXml.Save(_DataBasePath + "_deleted");
				_DeletedDBXml = new DataBaseXML();
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(ex.Message);
			}
		}

		internal void ChangeIDs()
		{
			int i = 1;
			foreach (var item in DBList)
			{
				item.Id = i;
				i++;
			}
			i = 1;
			foreach (var item in DeletedDBList)
			{
				item.Id = i;
				i++;
			}
		}

		internal bool PermDelete()
		{
			try
			{
				Logger.Instance.LogInfo(string.Format("Permanently deleted record is:\n\tFamilyName: {0}\n\tName: {1}\n\tPhone: {2}\n\tBirthDate: {3}\n\tPesel: {4}", this.DeleteDBSelectedItem.FamilyName, this.DeleteDBSelectedItem.Name, this.DeleteDBSelectedItem.Phone, this.DeleteDBSelectedItem.BirthDate, this.DeleteDBSelectedItem.Pesel));
				DeletedDBList.Remove(this.DeleteDBSelectedItem);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(ex.Message);
				return false;
			}
		}

		internal bool PermDeleteAll()
		{
			try
			{
				foreach (var item in DeletedDBList)
				{
					Logger.Instance.LogInfo(string.Format("Restored record is:\n\tFamilyName: {0}\n\tName: {1}\n\tPhone: {2}\n\tBirthDate: {3}\n\tPesel: {4}", item.FamilyName, item.Name, item.Phone, item.BirthDate, item.Pesel));
					DeletedDBList.Remove(item);
				}
				return true; ;
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(ex.Message);
				return false;
			}
		}

		internal bool Restore()
		{
			try
			{
				DBList.Add(this.DeleteDBSelectedItem);
				Logger.Instance.LogInfo(string.Format("Restored record is:\n\tFamilyName: {0}\n\tName: {1}\n\tPhone: {2}\n\tBirthDate: {3}\n\tPesel: {4}", this.DeleteDBSelectedItem.FamilyName, this.DeleteDBSelectedItem.Name, this.DeleteDBSelectedItem.Phone, this.DeleteDBSelectedItem.BirthDate, this.DeleteDBSelectedItem.Pesel));
				DeletedDBList.Remove(this.DeleteDBSelectedItem);
				return true;
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(ex.Message);
				return false;
			}
		}

		internal bool RestoreAll()
		{
			try
			{
				foreach (var item in DeletedDBList)
				{
					DBList.Add(item);
					Logger.Instance.LogInfo(string.Format("Restored record is:\n\tFamilyName: {0}\n\tName: {1}\n\tPhone: {2}\n\tBirthDate: {3}\n\tPesel: {4}", item.FamilyName, item.Name, item.Phone, item.BirthDate, item.Pesel));
					DeletedDBList.Remove(item);
				}
				return true;
			}
			catch (Exception ex)
			{
				Logger.Instance.LogError(ex.Message);
				return false;
			}
		}

		private void filtrMe(string filtr, bool isFull)
		{
			if (isFull)
				foreach (var item in FiltredDBList)
				{
					if (item.Name.Contains(filtr))
					{
						continue;
					}
					else if (item.FamilyName.Contains(filtr))
					{
						continue;
					}
					else if (item.Pesel.Contains(filtr))
					{
						continue;
					}
					else if (item.Phone.Contains(filtr))
					{
						continue;
					}
					else
					{
						FiltredDBList.Remove(item);
					}
				}
		}

		#endregion Methods

		internal void Search()
		{
			try
			{
				string[] tmpSearch = default(string[]);
				tmpSearch = FilterString.Split(';');
				DBStructureViewModel searchedRecord = new DBStructureViewModel();
				foreach (var item in tmpSearch)
				{
					searchedRecord = fillMyDBRecord(item);
					checkIfExiste(searchedRecord);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private void checkIfExiste(DBStructureViewModel searchedRecord)
		{
			foreach (var item in this.DBList)
			{
				if (FiltredDBList.Contains(item))
				{
					continue;
				}
				else
				{
					if (!string.IsNullOrEmpty(searchedRecord.Name))
					{
						if ((item.Name).ToLower().Contains(searchedRecord.Name))
						{
							FiltredDBList.Add(item);
							break;
						}
					}
					else if (!string.IsNullOrEmpty(searchedRecord.FamilyName))
					{
						if ((item.FamilyName).ToLower().Contains(searchedRecord.FamilyName))
						{
							FiltredDBList.Add(item);
							break;
						}
					}
					else if (!string.IsNullOrEmpty(searchedRecord.Pesel))
					{
						if ((item.Pesel).ToLower().Contains(searchedRecord.Pesel))
						{
							FiltredDBList.Add(item);
							break;
						}
					}
					else if (!string.IsNullOrEmpty(searchedRecord.Phone))
					{
						if ((item.Phone).ToLower().Contains(searchedRecord.Phone))
						{
							FiltredDBList.Add(item);
							break;
						}
					}
				}
			}
		}

		private DBStructureViewModel fillMyDBRecord(string tmpSearch)
		{
			string name, familyName, phone, pesel;
			string[] tmp = tmpSearch.Split('=');
			DBStructureViewModel tmpRecord = new DBStructureViewModel();
			string caseString = tmp[0];
			switch (caseString.ToLower())
			{
				case ("name"):
					{
						name = tmp[1];
						tmpRecord.Name = name;
						break;
					}
				case ("family name"):
					{
						familyName = tmp[1];
						tmpRecord.FamilyName = familyName;
						break;
					}
				case ("phone number"):
					{
						phone = tmp[1];
						tmpRecord.Phone = phone;
						break;
					}
				case ("pesel"):
					{
						pesel = tmp[1];
						tmpRecord.Pesel = pesel;
						break;
					}
				default:
					{
						Exception ex = new Exception("No records found or wrong syntax");
						throw ex;
					}
			}
			return tmpRecord;
		}
	}
}