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

		//TODO Delete() i obsługa kosza tj. nowa lista z deleted, obsluga list.delete poczym od razu listdel.add(deleted), flaga deleted? plus pierdolki jak dodanie do grida binding etc
	}
}
