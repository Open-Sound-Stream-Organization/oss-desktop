using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using OpenSoundStream.Code.DataManager;

namespace OpenSoundStream.ViewModel
{
    class ArtistViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Local Binding Variables

        private string _albumCover;
        private ObservableCollection<string> _artistNames;
        private ObservableCollection<TrackMetadata> _trackList;
        private Visibility _listVisi = Visibility.Hidden;
        private MainViewModel mainViewModel = MainViewModel.mainViewModel;

        #endregion

        #region Binding Properties

        public RelayCommand<string> ArtistCommand { get; private set; }

        public RelayCommand<TrackMetadata> TitleCommand { get; private set; }

        public ObservableCollection<string> ArtistNames { get; private set; }

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
        public ArtistViewModel()
        {
            this.ArtistCommand = new RelayCommand<string>((item) => this.showSelectedArtist(item));
            this.TitleCommand = new RelayCommand<TrackMetadata>((item) => this.playSelectedTitle(item));

            this.ArtistNames = new ObservableCollection<string>();

            showArtistList();
        }

        #region Methods

        /// <summary>
        /// Display all tracks from a slected Artists
        /// </summary>
        /// <param name="artistName"></param>
        private void showSelectedArtist(string artistName)
        {
            // Find selected Artist in DB
            Artist currentArtist = ArtistsManager.db_GetAllArtists().Find(x => x.name == artistName);

            // Find all tracks from selected Artist
            foreach (Album album in currentArtist.Albums)
            {
                foreach (Track track in album.Tracks)
                {
                    TrackList.Add(new TrackMetadata { Title = track.title, Album = track.Album.name, Genre = track.Metadata.Genre, Year = (track.Metadata.Year).ToString(), Length = track.Metadata.Length.ToString("hh:mm:ss") }); ;
                }
            }
            ListVisi = Visibility.Visible;
        }

        /// <summary>
        /// Display all artists
        /// </summary>
        private void showArtistList()
        {
            foreach (Artist artist in ArtistsManager.db_GetAllArtists())
            {
                ArtistNames.Add(artist.name);
            }
        }

        /// <summary>
        /// Play selected track
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
