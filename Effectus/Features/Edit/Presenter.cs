namespace Effectus.Features.Edit
{
	using System;
	using System.Windows;
	using Effectus.Model;
	using Events;
	using Infrastructure;
	using Merge;
	using NHibernate;

	public class Presenter : AbstractPresenter<Model, View>
	{
		public Presenter()
		{
			EventPublisher.Register<ActionUpdated>(RefreshAction);
		}

		private void RefreshAction(ActionUpdated actionUpdated)
		{
			if (actionUpdated.Id != Model.Action.Id)
				return;
			Session.Refresh(Model.Action);
		}

		public void Initialize(long id)
		{
			ToDoAction action;
			using (var tx = Session.BeginTransaction())
			{
				action = Session.Get<ToDoAction>(id);
				tx.Commit();
			}

			if (action == null)
				throw new InvalidOperationException("Action " + id + " does not exists");

			this.Model = new Model
			{
				Action = action
			};
		}

		public void OnCancel()
		{
			View.Close();
		}

		public void OnCreateConcurrencyConflict()
		{
			using (var session = SessionFactory.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var anotherActionInstance = session.Get<ToDoAction>(Model.Action.Id);
				anotherActionInstance.Title = anotherActionInstance.Title + " - ";
				tx.Commit();
			}
			MessageBox.Show("Concurrency conflict created");
		}

		public void OnSave()
		{
			bool successfulSave;
			try
			{
				using (var tx = Session.BeginTransaction())
				{
					// this isn't strictly necessary, NHibernate will 
					// automatically do it for us, but it make things
					// more explicit
					Session.Update(Model.Action);

					tx.Commit();
				}
				successfulSave = true;
			}
			catch (StaleObjectStateException)
			{
				var mergeResult = Presenters.ShowDialog<MergeResult?>("Merge", Model.Action);
				successfulSave = mergeResult != null;

				ReplaceSessionAfterError();
			}

			// we call ActionUpdated anyway, either we updated the value ourselves
			// or we encountered a concurrency conflict, in which case we _still_
			// want other parts of the application to update themselves with the values
			// from the db
			EventPublisher.Publish(new ActionUpdated
			{
				Id = Model.Action.Id
			}, this);

			if (successfulSave)
				View.Close();
		}

		protected override void ReplaceEntitiesLoadedByFaultedSession()
		{
			Initialize(Model.Action.Id);
		}
	}
}