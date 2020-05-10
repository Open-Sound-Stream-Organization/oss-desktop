using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using OpenSoundStream.Views;
using OpenSoundStream.Code.DataManager;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using OpenSoundStream.Code;
using TagLib.Flac;
using System.Linq;

namespace OpenSoundStream.ViewModel
{

	public class MainViewModel : ViewModelBase, INotifyPropertyChanged
	{
        #region Locale Variables

        public static MainViewModel mainViewModel;
		public static MainWindow mainWindow;
		public static Musicplayer musicplayer { get; private set; } = OpenSoundStreamManager.Musicplayer;
		public static BigPlayerView bigPlayerWindow;
		private bool SelectAllTracks { get; set; }
		private bool sleepTimerOn = false;

        #endregion

        #region Local Binding Variables

        private PackIcon _pauseOrPlay = new PackIcon { Kind = PackIconKind.Play };
		private string _currentArtist = "";
		private string _currentTrack = "";
		private string _trackTimeField = "0:00";
		private string _currentFrame;
		private string _currentPositionText;
		private bool userIsDraggingSlider = false;
		private bool shuffle = false;
		private double _currentPosition;
		private double _maxLength;
		private double _volumn;
		private double _trackTime;
		private ObservableCollection<TrackMetadata> _tracks = new ObservableCollection<TrackMetadata>();
		private ObservableCollection<string> _playlists = new ObservableCollection<string>();
		private PackIcon _playerMode = new PackIcon { Kind = PackIconKind.Repeat };
		private BitmapImage _albumCover = new BitmapImage(new Uri("https://lh3.googleusercontent.com/proxy/xYtt3U6QqGknFyap8--0oPArycyJwVdee8TPzapJPAMoj8_fljy5Rq4uXmAEYqBZ3RrQ9JE-QwMWimYEQCzq--CsJVOnyjYUFTATjBBhUTsbPHzlqXh4rGgkddfl6lJ4StuSlGEWUg", UriKind.RelativeOrAbsolute));

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
		public RelayCommand ChangeViewCommand { get; private set; }
		public RelayCommand LoginCommand { get; private set; }
		public RelayCommand SleepCommand { get; private set; }
		public RelayCommand UploadCommand { get; private set; }
		public RelayCommand DownloadCommand { get; private set; }
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
		public BitmapImage AlbumCover
		{
			get { return _albumCover; }
			set
			{
				_albumCover = value;
				RaisePropertyChanged("AlbumCover");

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
		public string CurrentFrame
		{
			get { return _currentFrame; }
			set
			{
				_currentFrame = value;
				RaisePropertyChanged("CurrentFrame");
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
			this.ChangeViewCommand = new RelayCommand(this.changeView);
			this.LoginCommand = new RelayCommand(this.openLoginDialog);
			this.SleepCommand = new RelayCommand(this.controlSleepTimer);
			this.UploadCommand = new RelayCommand(this.uploadFiles);
			this.DownloadCommand = new RelayCommand(this.synchronizeDatabase);


			// Set inital values
			_volumn = musicplayer.Mediaplayer.Volume;

			foreach (Playlist playlist in PlaylistsManager.db_GetAllPlaylists())
			{
				Playlists.Add(playlist.name);
			}

			DispatcherTimer timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += timer_Tick;
			timer.Start();
		}

        #endregion

        #region Methods

		/// <summary>
		/// Synchronize locale Database with Server
		/// </summary>
		private void synchronizeDatabase()
		{
			NetworkHandler.SyncLocalDbWithServerDb();

			// Synchronize View with local database
			foreach (Playlist playlist in PlaylistsManager.db_GetAllPlaylists())
			{
				Playlists.Add(playlist.name);
			}
		}

		/// <summary>
		/// Upload mp3 files to server
		/// </summary>
		private void uploadFiles()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();

			string filePath;

			// Filedialog Settings
			openFileDialog.InitialDirectory = "c:\\";
			openFileDialog.Filter = "mp3 files (*.mp3)|*.mp3";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				//Get the path of specified file
				filePath = openFileDialog.FileName;

				Track track = new Track(Path.GetFileName(filePath), new Uri(filePath));
				AppHelper.LocalImportTrack(track, filePath);

				//Read the contents of the file into a stream
				var fileStream = openFileDialog.OpenFile();

			}
		}

		//TODO
		/// <summary>
		/// Authentification with server
		/// </summary>
		private void openLoginDialog()
		{
			CostumInputDialog inputDialog = new CostumInputDialog("", "");

			inputDialog.ShowDialog();
		}

		/// <summary>
		/// Activate or Deactive 15min Sleep Timer
		/// </summary>
		private void controlSleepTimer()
		{
			// Check if sleep timer is already active
			if (sleepTimerOn)
			{
				musicplayer.StopSleepTimer();
				sleepTimerOn = false;
			}
			else
			{
				musicplayer.SetSleepTimer();
				sleepTimerOn = true;
			}

		}

		/// <summary>
		/// Plays a selected track from ListView
		/// </summary>
		/// <param name="selectedTrack"></param>
		private void playSelectedTrack(TrackMetadata selectedTrack)
		{
			if (SelectAllTracks != true)
			{
				musicplayer.SetActiveTrackInPlayableContainer(TracksManager.db_GetAllTracks().Find(x => x.title == selectedTrack.Title));
				playMusic();
			}
			else
			{
				musicplayer.SetActiveTrack(TracksManager.db_GetAllTracks().Find(x => x.title == selectedTrack.Title));
				playMusic();
				SelectAllTracks = false;
			}

		}

		/// <summary>
		/// Plays a selected playlist from ListView
		/// </summary>
		/// <param name="selectedPlaylist"></param>
		private void playSelectedPlaylist(string selectedPlaylist)
		{
			Playlist currentPlaylist = PlaylistsManager.db_GetAllPlaylists().Find(x => x.name == selectedPlaylist);
			musicplayer.Musicqueue.LoadPlayableContainerInQueue(currentPlaylist);

			//To decide between all Tracks and a Playlist
			SelectAllTracks = false;
			musicplayer.NextTrack();
			playMusic();

			Tracks.Clear();

			CurrentFrame = "TitleView.xaml";

			foreach (Track track in currentPlaylist.Tracks)
			{
				Tracks.Add(new TrackMetadata { Title = track.title }) ;
			}
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
		public void playMusic()
		{
			musicplayer.Play();

			Track activeTrack = musicplayer.Musicqueue.ActiveTrack;

			if (activeTrack == null)
				return;

			Artist artist = new Artist("muss geändert werden");
			//Artist artist = activeTrack.artists; TODO
			CurrentTrack = activeTrack.title;
			CurrentArtist = artist != null ? artist.name : "Unknown Artist";

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
				musicplayer.Musicqueue.ShuffleQueue();
			}
			else
			{
				PlayerMode = new PackIcon { Kind = PackIconKind.Repeat };
				musicplayer.Musicqueue.RepeatQueue = true;
				shuffle = false;
				musicplayer.Musicqueue.UnshuffleQueue();
			}

		}

		/// <summary>
		/// Opens BigPlayerView
		/// </summary>
		private void createBigPlayerView()
		{
			mainWindow.Hide();

			BigPlayerView bigPlayerViewCreated = new BigPlayerView();
			bigPlayerViewCreated.ShowDialog();
		}

		/// <summary>
		/// Opens ArtistView
		/// </summary>
		private void createArtistsView()
		{
			CurrentFrame = "ArtistView.xaml";
		}

		/// <summary>
		/// Opens AlbumView
		/// </summary>
		private void createAlbumsView()
		{
			CurrentFrame = "AlbumView.xaml";
		}

		/// <summary>
		/// Passes data from all tracks to View
		/// </summary>
		private void createTracksView()
		{
			TitleViewModel.Tracks.Clear();

			foreach (Track track in TracksManager.db_GetAllTracks())
			{
				TrackMetadata md = new TrackMetadata();
				if (track.title == null)
				{
					md.Title = "unkown";
					MetadataEditor.AddTitle(track.audio, "unknown");
				}
				else
				{
					md.Title = track.title;
					MetadataEditor.AddTitle(track.audio, track.title);
				}
				if(MetadataEditor.GetGenres(track.audio).Count() == 0)
				{
					md.Genre = "unkown";
					MetadataEditor.AddGenre(track.audio, new String[] { "unknown" });
				}
				else
				{
					md.Genre = MetadataEditor.GetGenres(track.audio)[0];
				}
				if (MetadataEditor.GetArtists(track.audio).Count() == 0)
				{
					md.Artist = "unknown";
					MetadataEditor.AddArtist(track.audio, new String[] { "unknown" });
				}
				else
				{
					md.Artist = MetadataEditor.GetArtists(track.audio)[0];
				}
				if (MetadataEditor.GetYear(track.audio) == 0)
				{
					md.Year = "unknown";
				}
				else
				{
					md.Year = MetadataEditor.GetYear(track.audio).ToString();
				}
				if (MetadataEditor.GetAlbum(track.audio) == null)
				{
					md.Album = "unknown";
					MetadataEditor.AddAlbum(track.audio, "unknown");
				}
				else
				{
					md.Album = MetadataEditor.GetAlbum(track.audio);
				}
				if (MetadataEditor.GetDuration(track.audio).ToString(@"hh\:mm\:ss") == "00:00:00")
				{
					md.Length = "unknown";
				}
				else
				{
					md.Length = MetadataEditor.GetDuration(track.audio).ToString(@"hh\:mm\:ss");
				}
				TitleViewModel.Tracks.Add(md);
			}

			SelectAllTracks = true;
			CurrentFrame = "TitleView.xaml";
		}

		/// <summary>
		/// Hides MainView and opens BigPlayerView
		/// </summary>
		public void changeView()
		{
			bigPlayerWindow.Hide();
			mainWindow.ShowDialog();
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
				CurrentTrack = musicplayer.Musicqueue.ActiveTrack.title;
				AlbumCover = new BitmapImage(new Uri(musicplayer.Musicqueue.ActiveTrack.Metadata.CoverFile, UriKind.RelativeOrAbsolute));
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

		/// <summary>
		/// Notifies ViewModel if SliderDrag started
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
		{
			userIsDraggingSlider = true;
		}

		/// <summary>
		/// Notifies ViewModel if SliderDrag ended
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>	
		public void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			userIsDraggingSlider = false;
			musicplayer.Mediaplayer.Position = TimeSpan.FromSeconds(CurrentPosition);
		}
		
		/// <summary>
		/// Notifies ViewModel if SliderProgess changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			CurrentPositionText = TimeSpan.FromSeconds(CurrentPosition).ToString(@"hh\:mm\:ss");
		}

		#endregion
	}

}