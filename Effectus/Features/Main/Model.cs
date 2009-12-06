namespace Effectus.Features.Main
{
	using System.Collections.ObjectModel;
	using Effectus.Model;

	public class Model : IPagingInformation
	{
		public ObservableCollection<ToDoAction> Actions { get; set; }

		public int NumberOfPages { get; set; }

		public int CurrentPage { get; set; }
	}
}