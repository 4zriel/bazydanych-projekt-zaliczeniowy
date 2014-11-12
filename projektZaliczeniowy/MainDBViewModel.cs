using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektZaliczeniowy
{
	public class MainDBViewModel
	{
		ObservableCollection<DBStructureViewModel> _DBList = new ObservableCollection<DBStructureViewModel>();

		public ObservableCollection<DBStructureViewModel> DBList
		{
			get
			{
				return _DBList;
			}
			set
			{
				_DBList = value;
			}
		}
		public void addToList(DBStructureViewModel DBEntry)
		{
			_DBList.Add(DBEntry);
		}
		public void Clear()
		{
			_DBList.Clear();
		}
	}
}
