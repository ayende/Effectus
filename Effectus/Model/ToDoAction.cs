namespace Effectus.Model
{
	using System;

	public class ToDoAction
	{
		public virtual long Id { get; set; }
		public virtual string Title { get; set;}
		public virtual string Content { get; set; }
		public virtual Status Status { get; set; }

		public virtual int Version { get; set; }

		public virtual DateTime CreatedAt { get; set; }
		public virtual DateTime? CompleteBy { get; set; }
	}
}