using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;

namespace OpenSoundStream.ViewModel
{

    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public static Musicplayer musicplayer = OpenSoundStreamManager.Musicplayer;
        public static MainViewModel mainViewModel;

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
        private double _volumn = musicplayer.Mediaplayer.Volume * 100;
        private double _trackTime;
        private string _trackTimeField = "0:00";
        ObservableCollection<TrackClass> tracks = new ObservableCollection<TrackClass>();

        public ObservableCollection<TrackClass> Tracks { get { return tracks; } }
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
                musicplayer.SetVolume(_volumn / 100);
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

        public MainViewModel()
        {
            musicplayer.Mediaplayer.Changed += Mediaplayer_Changed;
            setTime();

            this.StartPlayerCommand = new RelayCommand(this.StartPlayer);
            this.PlayPreviousCommand = new RelayCommand(this.PlayPrevious);
            this.PlayNextCommand = new RelayCommand(this.PlayNext);
            this.PlayerSettingCommand = new RelayCommand(this.SetPlayerMode);
            this.BigPlayerCommand = new RelayCommand(this.createBigPlayerView);
            this.ArtistsCommand = new RelayCommand(this.createArtistsView);
            this.AlbumsCommand = new RelayCommand(this.createAlbumsView);
            this.TracksCommand = new RelayCommand(this.createTracksView);
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
        private void StartPlayer()
        {
            if (musicplayer.State == PlayerState.Play)
            {
                musicplayer.Pause();
                _pauseOrPlay = "./Icons/round_play_arrow_white_18dp.png";
            }
            else
            {
                _pauseOrPlay = "./Icons/round_pause_white_18dp.png";
                musicplayer.Play();
                try
                {
                    CurrentTrack = musicplayer.Musicqueue.ActiveTrack.Title;
                    CurrentArtist = "Current Artist";
                }
                catch (Exception)
                {

                }
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
        private void setTime()
        {
            Timer timer = new Timer(1000);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        private void createTracksView()
        {
            List<Track> trackList = Track.Tracks;

            foreach (Track track in trackList)
            {
                Tracks.Add(new TrackClass { Title = track.Title, Genre = track.Metadata.Genre});
                
            }
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                mainViewModel.TrackTimeField = musicplayer.Mediaplayer.Position.ToString();

            }
            catch (Exception)
            {
            }
        }
    }

    public class TrackClass
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Length { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
    }
}