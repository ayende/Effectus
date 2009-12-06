namespace Effectus.Infrastructure
{
	using System;
	using System.Windows;
	using NHibernate;

	public interface IPresenter
	{
		DependencyObject View { get; }
		void SetSessionFactory(ISessionFactory theSessionFactory);
		void Show();
		object Result{ get;}
		event Action Disposed;
		void ShowDialog();
	}
}