namespace Effectus.Infrastructure
{
	using System;
	using System.Windows;
	using NHibernate;

	public abstract class AbstractPresenter<TModel, TView> : IDisposable, IPresenter
		where TView : Window, new()
	{
		private TModel model;
		private ISessionFactory sessionFactory;
		private ISession session;
		private IStatelessSession statelessSession;

		protected AbstractPresenter()
		{
			View = new TView();
			View.Closed += (sender, args) => Dispose();
		}

		DependencyObject IPresenter.View { get { return View; } }

		public object Result { get; protected set; }

		protected TView View { get; set; }

		protected ISessionFactory SessionFactory
		{
			get { return sessionFactory; }
		}

		protected ISession Session
		{
			get
			{
				if (session == null)
					session = sessionFactory.OpenSession();
				return session;
			}
		}

		protected IStatelessSession StatelessSession
		{
			get
			{
				if (statelessSession == null)
					statelessSession = sessionFactory.OpenStatelessSession();
				return statelessSession;
			}
		}

		protected TModel Model
		{
			get { return model; }
			set
			{
				model = value;
				View.DataContext = model;
			}
		}

		protected void ReplaceSessionAfterError()
		{
			if(session!=null)
			{
				session.Dispose();
				session = sessionFactory.OpenSession();
				ReplaceEntitiesLoadedByFaultedSession();
			}
			if(statelessSession!=null)
			{
				statelessSession.Dispose();
				statelessSession = sessionFactory.OpenStatelessSession();
			}
		}

		protected virtual void ReplaceEntitiesLoadedByFaultedSession()
		{
			throw new InvalidOperationException(
				@"ReplaceSessionAfterError was called, but the presenter does not override ReplaceEntitiesLoadedByFaultedSession!
You must override ReplaceEntitiesLoadedByFaultedSession to call ReplaceSessionAfterError.");
		}


		public void SetSessionFactory(ISessionFactory theSessionFactory)
		{
			sessionFactory = theSessionFactory;
		}

		public void Show()
		{
			View.Show();
		}

		public void ShowDialog()
		{
			View.ShowDialog();
		}

		public event Action Disposed = delegate { };

		public virtual void Dispose()
		{
			if(session!=null)
				session.Dispose();
			if (statelessSession != null)
				statelessSession.Dispose();
			View.Close();
			Disposed();
		}
	}
}