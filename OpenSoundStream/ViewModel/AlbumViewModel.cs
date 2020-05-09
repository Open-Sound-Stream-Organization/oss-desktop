using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using OpenSoundStream.Code.DataManager;

namespace OpenSoundStream.ViewModel
{
    class AlbumViewModel : ViewModelBase,INotifyPropertyChanged
    {
        private MainViewModel mainViewModel = MainViewModel.mainViewModel;

        #region Local Binding Variabkes

        //TODO
        private string _albumCover;
        private string _albumName;
        private int _albumYear;
        private Visibility _listVisi = Visibility.Hidden;
        private ObservableCollection<string> _albumNames;
        private ObservableCollection<TrackMetadata> _trackList;

        #endregion

        #region Binding Properties

        public RelayCommand<string> AlbumCommand { get; private set; }
        public RelayCommand<TrackMetadata> TitleCommand { get; private set; }
        public ObservableCollection<string> AlbumNames { get; private set; }
        public ObservableCollection<TrackMetadata> TrackList { get; private set; }
        public string AlbumCover 
        {
            get { return _albumCover; }
            set
            {
                _albumCover = value;
                RaisePropertyChanged("AlbumCover");
            } 
        }
        public string AlbumName
        {
            get { return _albumName; }
            set
            {
                _albumName = value;
                RaisePropertyChanged("AlbumName");
            }
        }
        public int AlbumYear
        {
            get { return _albumYear; }
            set
            {
                _albumYear = value;
                RaisePropertyChanged("AlbumYear");
            }
        }
        public Visibility ListVisi
        {
            get { return _listVisi; }
            set
            {
                _listVisi = value;
                RaisePropertyChanged("ListVisi");
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public AlbumViewModel()
        {
            this.AlbumCommand = new RelayCommand<string>((item) => this.showSelectedAlbum(item));
            this.TitleCommand = new RelayCommand<TrackMetadata>((item) => this.playSelectedTitle(item));
            this.AlbumNames = new ObservableCollection<string>();

            showAlbumList();
        }

        #region Methods

        /// <summary>
        /// Display all tracks from a selected album
        /// </summary>
        /// <param name="albumName"></param>
        private void showSelectedAlbum(string albumName)
        {
            // Find selected album in database
            Album currentAlbum = AlbumsManager.db_GetAllAlbums().Find(x => x.name == albumName);

            AlbumName = currentAlbum.name;

            // Display all tracks from the selected album
            foreach (Track track in currentAlbum.Tracks)
            {
                TrackList.Add(new TrackMetadata { Title = track.title, Number = track.id, Length = track.Metadata.Length.ToString("hh:mm:ss")  });
            }

            ListVisi = Visibility.Visible;
        }

        /// <summary>
        /// Display all Albums
        /// </summary>
        private void showAlbumList()
        {
            foreach (Album album in AlbumsManager.db_GetAllAlbums())
            {
                AlbumNames.Add(album.name);
            }
        }

        /// <summary>
        /// Play selected tracks
        /// </summary>
        /// <param name="track"></param>
        private void playSelectedTitle(TrackMetadata track)
        {
            MainViewModel.musicplayer.SetActiveTrack(TracksManager.db_GetAllTracks().Find(x => x.title == track.Title));
            mainViewModel.playMusic();
        }

        #endregion
    }
}
