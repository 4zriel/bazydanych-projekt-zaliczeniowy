using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektZaliczeniowy
{
	public class MainDBViewModel : ViewModelBase
	{
		public MainDBViewModel()
		{
			_MainDBXml = new DataBaseXML();
			_DBList = new ObservableCollection<DBStructureViewModel>();
			_DeletedDBList = new ObservableCollection<DBStructureViewModel>();
		}

		private DataBaseXML _MainDBXml;

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
			}
			catch (Exception ex)
			{
				
				Logger.Instance.LogError(ex.Message);
			}
		}
		public void Load()
		{
			try
			{
				_MainDBXml = new DataBaseXML(DataBasePath);
				DBList = new ObservableCollection<DBStructureViewModel>(_MainDBXml.LoadDB());
				_MainDBXml = new DataBaseXML();
			}
			catch (Exception ex)
			{
				
				Logger.Instance.LogError(ex.Message);
			}
		}
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
		//TODO Delete() i obsługa kosza tj. nowa lista z deleted, obsluga list.delete poczym od razu listdel.add(deleted), flaga deleted? plus pierdolki jak dodanie do grida binding etc

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
			//TODO: poprawki
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
	}
}
