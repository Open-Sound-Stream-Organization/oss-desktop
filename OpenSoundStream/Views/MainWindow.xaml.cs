using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenSoundStream.ViewModel;

namespace OpenSoundStream.Views
{

	public partial class MainWindow : Window
	{
		public MainWindow(MainViewModel t_mainWindowModel)
		{
			DataContext = t_mainWindowModel;
			MainViewModel.mainViewModel = t_mainWindowModel;
			InitializeComponent();

			MainViewModel.mainWindow = this;
		}
	}
}
