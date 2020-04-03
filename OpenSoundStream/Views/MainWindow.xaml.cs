using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using OpenSoundStream.ViewModel;

namespace OpenSoundStream.Views
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static bool playing = false;

		public MainWindow(MainViewModel t_mainWindowModel)
		{
			DataContext = t_mainWindowModel;
			InitializeComponent();
		}

		private void Section_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
		}

		private void StartPlayer(object sender, RoutedEventArgs e)
		{
			if (playing)
			{
				PlayButtonImage.Source = new BitmapImage(new Uri("./Icons/round_play_circle_filled_white_18dp.png", UriKind.Relative));
				playing = false;
				OpenSoundStreamManager.Musicplayer.Pause();
			}
			else
			{
				PlayButtonImage.Source = new BitmapImage(new Uri("./Icons/round_pause_white_18dp.png", UriKind.Relative));
				playing = true;
				OpenSoundStreamManager.Musicplayer.Play();
			}
		}

		private void adjustVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			// MediaElement.Volume Propertie [0.00-1.00]
		}

		private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			OpenSoundStreamManager.Musicplayer.NextTrack();
		}

		private void Image_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			OpenSoundStreamManager.Musicplayer.PrevTrack();
		}
	}
}
