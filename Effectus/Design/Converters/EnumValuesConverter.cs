namespace Effectus.Design.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class EnumValuesConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Enum.GetValues(((Type)parameter));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}