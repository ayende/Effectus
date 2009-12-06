namespace Effectus.Design.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;
	using Model;

	public class IsStartedConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((Status) value) == Status.Started ? "Yes" : "No";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}