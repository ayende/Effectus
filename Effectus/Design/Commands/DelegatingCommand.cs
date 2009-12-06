namespace Effectus.Design.Commands
{
	using System;
	using System.ComponentModel;
	using System.Windows.Input;
	using System.Windows.Threading;
	using Infrastructure;

	public class DelegatingCommand : ICommand
	{
		private readonly Action action;
		private readonly Fact canExecute;

		public DelegatingCommand(Action action, Fact canExecute)
		{
			this.action = action;
			this.canExecute = canExecute;
			var dispatcher = Dispatcher.CurrentDispatcher;
			if (canExecute != null)
			{
				this.canExecute.PropertyChanged +=
					(sender, args) =>
						dispatcher.Invoke(CanExecuteChanged, this, EventArgs.Empty);
			}
		}

		public void Execute(object parameter)
		{
			action();
		}

		public bool CanExecute(object parameter)
		{
			if (canExecute == null)
				return true;
			return canExecute.Value;
		}

		public event EventHandler CanExecuteChanged = delegate { };
	}
}