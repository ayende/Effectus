namespace Effectus.Features.Merge
{
	using Effectus.Model;
	using Infrastructure;

	public class Model
	{
		public ToDoAction DatabaseVersion { get; set;}
		public ToDoAction UserVersion { get; set; }
		public Observable<bool> AllowEditing { get; set; }
	}
}