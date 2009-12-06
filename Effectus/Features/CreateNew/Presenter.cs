namespace Effectus.Features.CreateNew
{
	using System;
	using System.ComponentModel;
	using System.Threading;
	using Effectus.Model;
	using Events;
	using Infrastructure;

	public class Presenter : AbstractPresenter<Model, View>
	{
		private readonly BackgroundWorker saveBackgroundWorker;

		public Presenter()
		{
			saveBackgroundWorker = new BackgroundWorker();
			saveBackgroundWorker.DoWork += (sender, args) => PerformActualSave();
			saveBackgroundWorker.RunWorkerCompleted += (sender, args) => CompleteSave();
			Model = new Model
			{
				Action = DataBindingFactory.Create<ToDoAction>(),
				AllowEditing = new Observable<bool>(true)
			};
		}

		public void OnCancel()
		{
			View.Close();
		}

		public Fact CanSave
		{
			get { return new Fact(Model.AllowEditing, () => Model.AllowEditing); }
		}

		public Fact CanCancel
		{
			get
			{
				// we do not allow canceling when a save is in progress
				return new Fact(Model.AllowEditing, () => Model.AllowEditing);
			}
		}

		public void OnSave()
		{
			Model.AllowEditing.Value = false;
			saveBackgroundWorker.RunWorkerAsync();
		}

		private void CompleteSave()
		{
			Model.AllowEditing.Value = true;
			EventPublisher.Publish(new ActionUpdated
			{
				Id = Model.Action.Id
			}, this);

			View.Close();
		}

		private void PerformActualSave()
		{
			Thread.Sleep(5000);// simulating a long save here
			using (var tx = Session.BeginTransaction())
			{
				Model.Action.CreatedAt = DateTime.Now;

				Session.Save(Model.Action);
				tx.Commit();
			}
		}

		public override void Dispose()
		{
			saveBackgroundWorker.Dispose();
			base.Dispose();
		}
	}
}