namespace Effectus.Design.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;

	public class CompletionDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value == null)
				return null;
			return Math.Round((((DateTime)value) - DateTime.Now).TotalDays) + " days";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}