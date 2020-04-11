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

namespace OpenSoundStream.ViewModel
{

	public class MainViewModel : ViewModelBase, INotifyPropertyChanged
	{
		public static MainViewModel mainViewModel;
		public Musicplayer musicplayer { get; private set; } = OpenSoundStreamManager.Musicplayer;

        #region Binding Variables

        private PackIcon _pauseOrPlay = new PackIcon { Kind = PackIconKind.Play };
		private string _currentArtist = "";
		private string _currentTrack = "";
		private double _volumn;
		private double _trackTime;
		private string _trackTimeField = "0:00";
		private bool userIsDraggingSlider = false;
		private bool shuffle = false;
		private ObservableCollection<TrackMetadata> _tracks = new ObservableCollection<TrackMetadata>();
		private ObservableCollection<string> _playlists = new ObservableCollection<string>();
		private string _currentPositionText;
		private double _currentPosition;
		private double _maxLength;
		private PackIcon _playerMode = new PackIcon { Kind = PackIconKind.Repeat };

		#endregion

		#region Binding Properties

		public RelayCommand StartPlayerCommand { get; private set; }
		public RelayCommand PlayPreviousCommand { get; private set; }
		public RelayCommand PlayNextCommand { get; private set; }
		public RelayCommand PlayerSettingCommand { get; private set; }
		public RelayCommand BigPlayerCommand { get; private set; }
		public RelayCommand ArtistsCommand { get; private set; }
		public RelayCommand AlbumsCommand { get; private set; }
		public RelayCommand TracksCommand { get; private set; }
		public RelayCommand<TrackMetadata> ListViewCommand { get; private set; }
		public RelayCommand<string> PlaylistCommand { get; private set; }
		public ObservableCollection<TrackMetadata> Tracks { get { return _tracks; } }
		public ObservableCollection<string> Playlists { get { return _playlists; } }
		public PackIcon PauseOrPlay
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
		public PackIcon PlayerMode
		{
			get { return _playerMode; }
			set
			{
				_playerMode = value;
				RaisePropertyChanged("PlayerMode");

			}
		}

        #endregion

        #region Constructor

        public MainViewModel()
		{
			musicplayer.Musicqueue.PropertyChanged += TrackChanged;

			// Subscribes to all View Commands
			this.StartPlayerCommand = new RelayCommand(this.changePlayerState);
			this.PlayPreviousCommand = new RelayCommand(this.playPrevious);
			this.PlayNextCommand = new RelayCommand(this.playNext);
			this.PlayerSettingCommand = new RelayCommand(this.setRenderingModeIcon);
			this.BigPlayerCommand = new RelayCommand(this.createBigPlayerView);
			this.ArtistsCommand = new RelayCommand(this.createArtistsView);
			this.AlbumsCommand = new RelayCommand(this.createAlbumsView);
			this.TracksCommand = new RelayCommand(this.createTracksView);
			this.ListViewCommand = new RelayCommand<TrackMetadata>((item) => this.playSelectedTrack(item));
			this.PlaylistCommand = new RelayCommand<string>((item) => this.playSelectedPlaylist(item));

			// Set inital values
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

        #endregion

        #region Methods

		/// <summary>
		/// Plays a selected track from ListView
		/// </summary>
		/// <param name="selectedTrack"></param>
        private void playSelectedTrack(TrackMetadata selectedTrack)
		{
			musicplayer.SetActiveTrack(Track.Tracks.Find(x => x.Title == selectedTrack.Title));
			playMusic();
		}

		/// <summary>
		/// Plays a selected playlist from ListView
		/// </summary>
		/// <param name="selectedPlaylist"></param>
		private void playSelectedPlaylist(string selectedPlaylist)
		{
			musicplayer.Musicqueue.Queue = new LinkedList<Track>();
			musicplayer.Musicqueue.LoadPlaylistInQueue(Playlist.Playlists.Find(x => x.Name == selectedPlaylist));
			playMusic();
		}

		/// <summary>
		/// Tracks the current player state
		/// </summary>
		private void changePlayerState()
		{
			if (musicplayer.State == PlayerState.Play)
			{
				musicplayer.Pause();
				changePlayerStateIcon();
			}
			else
			{
				playMusic();
			}
		}

		/// <summary>
		/// Changes play/pause button icon according to the state of the player
		/// </summary>
		private void changePlayerStateIcon()
		{
			if (musicplayer.State == PlayerState.Play)
			{
				PauseOrPlay = new PackIcon { Kind = PackIconKind.Pause };
			}
			else
			{
				PauseOrPlay = new PackIcon { Kind = PackIconKind.Play };
			}
		}

		/// <summary>
		/// Plays tracks in mediaplayer and sets related informations in the view 
		/// </summary>
		private void playMusic()
		{
			musicplayer.Play();

			Track activeTrack = musicplayer.Musicqueue.ActiveTrack;

			if (activeTrack == null)
				return;

			Artist artist = activeTrack.Artist;
			CurrentTrack = activeTrack.Title;
			CurrentArtist = artist != null ? artist.Name : "Unknown Artist";

			changePlayerStateIcon();
		}

		/// <summary>
		/// Plays last track from queue
		/// </summary>
		private void playPrevious()
		{
			musicplayer.PrevTrack();
		}

		/// <summary>
		/// Plays next track from queue
		/// </summary>
		private void playNext()
		{
			musicplayer.NextTrack();
		}

		/// <summary>
		/// Changes rendering mode button icon according to the state of the player
		/// </summary>
		private void setRenderingModeIcon()
		{
			if (!shuffle)
			{
				PlayerMode = new PackIcon { Kind = PackIconKind.Shuffle };
				musicplayer.Musicqueue.Shuffle = true;
				shuffle = true;
			}
			else
			{
				PlayerMode = new PackIcon { Kind = PackIconKind.Repeat };
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

		/// <summary>
		/// Gives data from all tracks to View
		/// </summary>
		private void createTracksView()
		{
			List<Track> trackList = Track.Tracks;

			foreach (Track track in trackList)
			{
				Tracks.Add(new TrackMetadata { Title = track.Title, Genre = track.Metadata.Genre });

			}
		}

		#endregion

		#region EventHandler

		/// <summary>
		/// Notifies View if active track changed 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TrackChanged(object sender, System.EventArgs e)
		{
			try
			{
				CurrentTrack = musicplayer.Musicqueue.ActiveTrack.Title;
			}
			catch (Exception)
			{

			}
		}

		/// <summary>
		/// Notifies View if played time of a track was updated
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
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

		/// <summary>
		/// Notifies View if played time of a track was updated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		#endregion
	}

    public class TrackMetadata
	{
		public string Title { get; set; }
		public string Artist { get; set; }
		public string Album { get; set; }
		public string Length { get; set; }
		public string Genre { get; set; }
		public string Year { get; set; }
	}
}