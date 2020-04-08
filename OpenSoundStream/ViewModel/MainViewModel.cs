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

namespace OpenSoundStream.ViewModel
{

	public class MainViewModel : ViewModelBase, INotifyPropertyChanged
	{
		public static MainViewModel mainViewModel;
		public Musicplayer musicplayer { get; private set; } = OpenSoundStreamManager.Musicplayer;

		public RelayCommand StartPlayerCommand { get; private set; }
		public RelayCommand PlayPreviousCommand { get; private set; }
		public RelayCommand PlayNextCommand { get; private set; }
		public RelayCommand PlayerSettingCommand { get; private set; }
		public RelayCommand BigPlayerCommand { get; private set; }
		public RelayCommand ArtistsCommand { get; private set; }
		public RelayCommand AlbumsCommand { get; private set; }
		public RelayCommand TracksCommand { get; private set; }
		public RelayCommand<TrackClass> ListViewCommand { get; private set; }
		public RelayCommand<string> PlaylistCommand { get; private set; }


		private string _pauseOrPlay = "./Icons/round_play_arrow_white_18dp.png";
		private string _currentArtist = "";
		private string _currentTrack = "";
		private double _volumn;
		private double _trackTime;
		private string _trackTimeField = "0:00";
		private bool userIsDraggingSlider = false;
		private bool shuffle = false;
		ObservableCollection<TrackClass> _tracks = new ObservableCollection<TrackClass>();
		ObservableCollection<string> _playlists = new ObservableCollection<string>();
		private string _currentPositionText;
		private double _currentPosition;
		private double _maxLength;
		private string _playerMode = "./Icons/round_repeat_white_18dp.png";

		public ObservableCollection<TrackClass> Tracks { get { return _tracks; } }
		public ObservableCollection<string> Playlists { get { return _playlists; } }
		public string PauseOrPlay
		{
			get { return _pauseOrPlay; }
			set
			{
				_pauseOrPlay = value;
				RaisePropertyChanged("PauseOrPlay");

			}
		}
		public string CurrentTrack
		{
			get { return _currentTrack; }
			set
			{
				_currentTrack = value;
				RaisePropertyChanged("CurrentTrack");
			}
		}
		public string CurrentArtist
		{
			get { return _currentArtist; }
			set
			{
				_currentArtist = value;
				RaisePropertyChanged("CurrentArtist");
			}
		}
		public double Volumn
		{
			get { return _volumn; }
			set
			{
				_volumn = value;
				musicplayer.SetVolume(_volumn);
				RaisePropertyChanged("Volumn");
			}
		}
		public double TrackTime
		{
			get { return _trackTime; }
			set
			{
				// Eventuell Tick oder Timer der den selder Setzt?
				_trackTime = value;
				//PropertyChanged(this, new PropertyChangedEventArgs(nameof(TrackTime)));
			}
		}
		public string TrackTimeField
		{
			get { return _trackTimeField; }
			set
			{
				_trackTimeField = value;
				RaisePropertyChanged("TrackTimeField");
			}
		}
		public string CurrentPositionText
		{
			get { return _currentPositionText; }
			set
			{
				_currentPositionText = value;
				RaisePropertyChanged("CurrentPositionText");
			}
		}
		public double CurrentPosition
		{
			get { return _currentPosition; }
			set
			{
				_currentPosition = value;
				musicplayer.Mediaplayer.Position = TimeSpan.FromSeconds(_currentPosition);
				RaisePropertyChanged("CurrentPosition");
			}
		}
		public double MaxLength
		{
			get { return _maxLength; }
			set
			{
				_maxLength = value;
				RaisePropertyChanged("MaxLength");
			}
		}
		public string PlayerMode
		{
			get { return _playerMode; }
			set
			{
				_playerMode = value;
				RaisePropertyChanged("PlayerMode");

			}
		}

		public MainViewModel()
		{
			musicplayer.Musicqueue.PropertyChanged += Mediaplayer_Changed;

			this.StartPlayerCommand = new RelayCommand(this.ChangePlayerState);
			this.PlayPreviousCommand = new RelayCommand(this.PlayPrevious);
			this.PlayNextCommand = new RelayCommand(this.PlayNext);
			this.PlayerSettingCommand = new RelayCommand(this.SetPlayerMode);
			this.BigPlayerCommand = new RelayCommand(this.createBigPlayerView);
			this.ArtistsCommand = new RelayCommand(this.createArtistsView);
			this.AlbumsCommand = new RelayCommand(this.createAlbumsView);
			this.TracksCommand = new RelayCommand(this.createTracksView);
			this.ListViewCommand = new RelayCommand<TrackClass>((item) => this.selectedTitle(item));
			this.PlaylistCommand = new RelayCommand<string>((item) => this.selectedPlaylist(item));


			_volumn = musicplayer.Mediaplayer.Volume;

			foreach (Playlist playlist in Playlist.Playlists)
			{
				Playlists.Add(playlist.Name);
			}

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += timer_Tick;
			timer.Start();
		}

		private void selectedTitle(TrackClass parameter)
		{
			musicplayer.SetActiveTrack(Track.Tracks.Find(x => x.Title == parameter.Title));
			PlayMusic();
		}

		private void selectedPlaylist(string parameter)
		{
			musicplayer.Musicqueue.Queue = new LinkedList<Track>();
			musicplayer.Musicqueue.LoadPlaylistInQueue(Playlist.Playlists.Find(x => x.Name == parameter));
			PlayMusic();
		}

		private void Mediaplayer_Changed(object sender, System.EventArgs e)
		{
			try
			{
				CurrentTrack = musicplayer.Musicqueue.ActiveTrack.Title;
			}
			catch (Exception)
			{

			}
		}
		private void ChangePlayerState()
		{
			if (musicplayer.State == PlayerState.Play)
			{
				musicplayer.Pause();
				SelectPlayerIcon();
			}
			else
			{
				PlayMusic();
			}
		}
		private void SelectPlayerIcon()
		{
			if (musicplayer.State == PlayerState.Play)
			{
				PauseOrPlay = "./Icons/round_pause_white_18dp.png";
			}
			else
			{
				PauseOrPlay = "./Icons/round_play_arrow_white_18dp.png";
			}
		}
		private void PlayMusic()
		{
			musicplayer.Play();

			Track activeTrack = musicplayer.Musicqueue.ActiveTrack;
			if (activeTrack == null)
				return;


			Artist artist = activeTrack.Artist;
			CurrentTrack = activeTrack.Title;
			CurrentArtist = artist != null ? artist.Name : "Unknown Artist";

			SelectPlayerIcon();
		}
		private void PlayPrevious()
		{
			musicplayer.PrevTrack();
		}
		private void PlayNext()
		{
			musicplayer.NextTrack();
		}
		private void SetPlayerMode()
		{
			if (shuffle)
			{
				PlayerMode = "./Icons/round_shuffle_white_18dp.png";
				musicplayer.Musicqueue.Shuffle = true;
				shuffle = true;
			}
			else
			{
				PlayerMode = "./Icons/round_repeat_white_18dp.png";
				musicplayer.Musicqueue.Repeat = true;
				shuffle = false;
			}

		}
		private void createBigPlayerView()
		{

		}
		private void createArtistsView()
		{

		}
		private void createAlbumsView()
		{

		}
		private void createTracksView()
		{
			List<Track> trackList = Track.Tracks;

			foreach (Track track in trackList)
			{
				Tracks.Add(new TrackClass { Title = track.Title, Genre = track.Metadata.Genre });

			}
		}

		private static void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			try
			{
				mainViewModel.TrackTimeField = OpenSoundStreamManager.Musicplayer.Mediaplayer.Position.ToString();
			}
			catch (Exception)
			{
			}
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if ((musicplayer.Mediaplayer.Source != null) && (musicplayer.Mediaplayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
			{
				MaxLength = musicplayer.Mediaplayer.NaturalDuration.TimeSpan.TotalSeconds;
				CurrentPosition = musicplayer.Mediaplayer.Position.TotalSeconds;
				CurrentPositionText = TimeSpan.FromSeconds(CurrentPosition).ToString(@"hh\:mm\:ss");
			}
		}

		public void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
		{
			userIsDraggingSlider = true;
		}

		public void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			userIsDraggingSlider = false;
			musicplayer.Mediaplayer.Position = TimeSpan.FromSeconds(CurrentPosition);
		}

		public void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			CurrentPositionText = TimeSpan.FromSeconds(CurrentPosition).ToString(@"hh\:mm\:ss");
		}


	}

	public class TrackClass
	{
		public string Title { get; set; }
		public string Artist { get; set; }
		public string Album { get; set; }
		public string Length { get; set; }
		public string Genre { get; set; }
		public string Year { get; set; }
	}
}