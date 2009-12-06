namespace Effectus.Features.Main
{
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using Effectus.Model;
	using Events;
	using Infrastructure;
	using NHibernate.Criterion;

	public class Presenter : AbstractPresenter<Model, View>
	{
		const int PageSize = 3;

		public Observable<int> NumberOfPages { get; set; }

		public Observable<int> CurrentPage { get; set; }

		public Presenter()
		{
			NumberOfPages = new Observable<int>();
			CurrentPage = new Observable<int>();

			EventPublisher.Register<ActionUpdated>(RefreshCurrentPage);
		}

		private void RefreshCurrentPage(ActionUpdated _)
		{
			LoadPage(CurrentPage);
		}

		public Fact CanMovePrev
		{
			get { return new Fact(CurrentPage, () => CurrentPage > 0); }
		}

		public Fact CanMoveNext
		{
			get { return new Fact(CurrentPage, () => CurrentPage + 1 < NumberOfPages); }
		}

		public void OnCreateNew()
		{
			Presenters.Show("CreateNew");
		}

		public void OnActionsChoosen(ToDoAction action)
		{
			Presenters.Show("Edit", action.Id);
		}

		public void OnLoaded()
		{
			LoadPage(0);
		}

		public void OnMoveNext()
		{
			LoadPage(CurrentPage + 1);
		}

		public void OnMovePrev()
		{
			LoadPage(CurrentPage - 1);
		}

		private void LoadPage(int page)
		{
			using (var tx = StatelessSession.BeginTransaction())
			{
				var actions = StatelessSession.CreateCriteria<ToDoAction>()
					.SetFirstResult(page * PageSize)
					.SetMaxResults(PageSize)
					.List<ToDoAction>();

				var total = StatelessSession.CreateCriteria<ToDoAction>()
					.SetProjection(Projections.RowCount())
					.UniqueResult<int>();

				this.NumberOfPages.Value = total / PageSize + (total % PageSize == 0 ? 0 : 1);
				this.Model = new Model
				{
					Actions = new ObservableCollection<ToDoAction>(actions),
					NumberOfPages = NumberOfPages,
					CurrentPage = CurrentPage + 1
				};
				this.CurrentPage.Value = page;

				tx.Commit();
			}
		}
	}
}