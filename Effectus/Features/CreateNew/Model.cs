namespace Effectus.Features.CreateNew
{
	using System;
	using Effectus.Model;
	using Infrastructure;

	public class Model
	{
		public ToDoAction Action { get; set; }

		public Observable<bool> AllowEditing { get; set; }
	}
}