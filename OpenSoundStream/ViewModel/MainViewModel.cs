using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.ComponentModel;

namespace OpenSoundStream.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        private Musicplayer musicplayer = OpenSoundStreamManager.Musicplayer;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public RelayCommand StartPlayerCommand { get; private set; }
        public RelayCommand PlayPreviousCommand { get; private set; }
        public RelayCommand PlayNextCommand { get; private set; }
        public  RelayCommand PlayerSettingCommand { get; private set; }
        public RelayCommand BigPlayerCommand { get; private set; }
        public RelayCommand ArtistsCommand { get; private set; }
        public RelayCommand AlbumsCommand { get; private set; }
        public RelayCommand TracksCommand { get; private set; }

        private string _pauseOrPlay = "./Icons/round_play_arrow_white_18dp.png";
        private string _currentArtist = "";
        private string _currentTrack = "";
        private double _volumn;
        private double _trackTime;
        private string _trackTimeField = "0:00";

        public string PauseOrPlay
        {
            get { return _pauseOrPlay; }
            set
            {
                _pauseOrPlay = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(PauseOrPlay)));
            }
        }

        public string CurrentTrack
        {
            get { return _currentTrack; }
            set
            {
                _currentTrack = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentTrack)));
            }
        }

        public string CurrentArtist
        {
            get { return _currentArtist; }
            set
            {
                _currentArtist = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentArtist)));
            }
        }

        public double Volumn
        {
            get { return _volumn; }
            set
            {
                _volumn = value;
                musicplayer.SetVolume(_volumn / 100);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Volumn)));
            }
        }

        public double TrackTime
        {
            get { return _trackTime; }
            set
            {
                // Eventuell Tick oder Timer der den selder Setzt?
                _trackTime = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TrackTime)));
            }
        }

        public string TrackTimeField
        {
            get { return _trackTimeField; }
            set
            {
                // Eventuell mit Ticker/Timer Verbunden oder MediaPlayer Event?
                _trackTimeField = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TrackTimeField)));
            }
        }

        public MainViewModel()
        {
            this.StartPlayerCommand = new RelayCommand(this.StartPlayer);
            this.PlayPreviousCommand = new RelayCommand(this.PlayPrevious);
            this.PlayNextCommand = new RelayCommand(this.PlayNext);
            this.PlayerSettingCommand = new RelayCommand(this.SetPlayerMode);
            this.BigPlayerCommand = new RelayCommand(this.createBigPlayerView);
            this.ArtistsCommand = new RelayCommand(this.createArtistsView);
            this.AlbumsCommand = new RelayCommand(this.createAlbumsView);
            this.TracksCommand = new RelayCommand(this.createTracksView);
        }

        private void StartPlayer()
        {
            if (musicplayer.State == PlayerState.Play)
            {
                musicplayer.Pause();
                PauseOrPlay = "./Icons/round_pause_white_18dp.png";
            }
            else
            {
                PauseOrPlay = "./Icons/round_play_arrow_white_18dp.png";
                musicplayer.Play();
            }
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

        }


        private void SetTitleInformation()
        {

        }
    }
}