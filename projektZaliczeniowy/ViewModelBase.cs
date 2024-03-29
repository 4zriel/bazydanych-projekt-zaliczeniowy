﻿using System.ComponentModel;

namespace projektZaliczeniowy
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyMe(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				Logger.Instance.LogInfo(string.Format("Property changed {0}", propertyName));
			}
		}
	}
}