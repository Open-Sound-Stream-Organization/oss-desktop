using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using OpenSoundStream.Code.DataManager;

namespace OpenSoundStream.ViewModel
{
    public class TitleViewModel : ViewModelBase, INotifyPropertyChanged
    {
		private static ObservableCollection<TrackMetadata> _tracks = new ObservableCollection<TrackMetadata>();
		public static ObservableCollection<TrackMetadata> Tracks { get { return _tracks; } }

		private static MainViewModel mainViewModel = MainViewModel.mainViewModel;

		public RelayCommand<TrackMetadata> ListViewCommand { get; private set; }

		public TitleViewModel()
		{
			this.ListViewCommand = new RelayCommand<TrackMetadata>((item) => this.playSelectedTrack(item));
		}

		/// <summary>
		/// Plays a selected track from ListView
		/// </summary>
		/// <param name="selectedTrack"></param>
		private void playSelectedTrack(TrackMetadata selectedTrack)
		{
			MainViewModel.musicplayer.SetActiveTrack(TracksManager.db_GetAllTracks().Find(x => x.title == selectedTrack.Title));
			mainViewModel.playMusic();
		}

	}

}
