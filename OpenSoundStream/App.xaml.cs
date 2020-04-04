using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using OpenSoundStream.ViewModel;
using OpenSoundStream.Views;

namespace OpenSoundStream
{
	/// <summary>
	/// Interaktionslogik für "App.xaml"
	/// </summary>
	public partial class App : Application
	{
		private void OnStartup(object sender, StartupEventArgs e)
		{
			AppHelper.CheckDataPath();
			MainWindow mainWindow = new MainWindow(new MainViewModel());
			mainWindow.ShowDialog();

			Environment.Exit(1);

		}
	}
}
