namespace Effectus.Design.Converters
{
	using System;
	using System.Globalization;
	using System.Windows.Data;
	using Features.Main;

	public class PagingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;
			var pagingInformation = ((IPagingInformation)value);
			return "Page " + pagingInformation.CurrentPage + " of " + pagingInformation.NumberOfPages;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}