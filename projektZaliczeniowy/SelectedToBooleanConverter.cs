using System;
using System.Windows.Data;

namespace projektZaliczeniowy
{
	[ValueConversion(typeof(DBStructureViewModel), typeof(bool))]
	public class SelectedToBooleanConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			if (targetType != typeof(bool))
				throw new InvalidOperationException("The target must be a boolean");
			if (value == null)
				return false;
			else
				return true;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion IValueConverter Members
	}
}