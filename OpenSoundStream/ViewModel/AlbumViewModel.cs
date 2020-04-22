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
    class AlbumViewModel : ViewModelBase,INotifyPropertyChanged
    {
        private MainViewModel mainViewModel = MainViewModel.mainViewModel;


        public RelayCommand<string> AlbumCommand { get; private set; }
        public RelayCommand<TrackMetadata> TitleCommand { get; private set; }


        private string _albumCover;
        private ObservableCollection<string> _albumNames;
        private ObservableCollection<TrackMetadata> _trackList;
        private string _albumName;
        private int _albumYear;
        private Visibility _listVisi = Visibility.Hidden;

        public string AlbumCover 
        {
            get { return _albumCover; }
            set
            {
                _albumCover = value;
                RaisePropertyChanged("AlbumCover");
            } 
        }

        public ObservableCollection<string> AlbumNames { get; private set; }


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

        public ObservableCollection<TrackMetadata> TrackList { get; private set; }

        public Visibility ListVisi
        {
            get { return _listVisi; }
            set
            {
                _listVisi = value;
                RaisePropertyChanged("ListVisi");
            }
        }


        public AlbumViewModel()
        {
            this.AlbumCommand = new RelayCommand<string>((item) => this.showSelectedAlbum(item));
            this.TitleCommand = new RelayCommand<TrackMetadata>((item) => this.playSelectedTitle(item));

            showAlbumList();
        }


        private void showSelectedAlbum(string albumName)
        {
            Album currentAlbum = Album.Albums.Find(x => x.name == albumName);

            AlbumName = currentAlbum.name;

            foreach (Track track in currentAlbum.Tracks)
            {
                TrackList.Add(new TrackMetadata { Title = track.title });
            }
            ListVisi = Visibility.Visible;

        }

        private void showAlbumList()
        {
            foreach (Album album in Album.Albums)
            {
                AlbumNames.Add(album.name);
            }
        }

        private void playSelectedTitle(TrackMetadata track)
        {
            MainViewModel.musicplayer.SetActiveTrack(Track.Tracks.Find(x => x.title == track.Title));
            mainViewModel.playMusic();
        }

    }
}
