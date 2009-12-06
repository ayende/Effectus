using System.Windows;

namespace Effectus
{
	using Features.Main;
	using Infrastructure;

	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			BootStrapper.Initialize();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			Presenters.Show("Main");
		}
	}
}
