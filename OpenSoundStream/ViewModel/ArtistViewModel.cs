﻿using GalaSoft.MvvmLight;
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
    class ArtistViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private MainViewModel mainViewModel = MainViewModel.mainViewModel;


        public RelayCommand<string> ArtistCommand { get; private set; }
        public RelayCommand<TrackMetadata> TitleCommand { get; private set; }

        public List<Track> Tracks = TracksManager.db_GetAllTracks();

        private string _albumCover;
        private ObservableCollection<string> _artistNames;
        private ObservableCollection<TrackMetadata> _trackList;
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

        public ObservableCollection<string> ArtistNames { get; private set; }

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


        public ArtistViewModel()
        {
            this.ArtistCommand = new RelayCommand<string>((item) => this.showSelectedArtist(item));
            this.TitleCommand = new RelayCommand<TrackMetadata>((item) => this.playSelectedTitle(item));

            this.ArtistNames = new ObservableCollection<string>();

            showArtistList();
        }


        private void showSelectedArtist(string artistName)
        {
            Artist currentArtist = ArtistsManager.db_GetAllArtists().Find(x => x.name == artistName);


            foreach (Album album in currentArtist.Albums)
            {
                foreach (Track track in album.Tracks)
                {
                    TrackList.Add(new TrackMetadata { Title = track.title, Album = track.Album.name, Genre = track.Metadata.Genre, Year = (track.Metadata.Year).ToString() });
                }
            }
            ListVisi = Visibility.Visible;

        }

        private void showArtistList()
        {
            foreach (Artist artist in ArtistsManager.db_GetAllArtists())
            {
                ArtistNames.Add(artist.name);
            }
        }

        private void playSelectedTitle(TrackMetadata track)
        {
            MainViewModel.musicplayer.SetActiveTrack(Tracks.Find(x => x.title == track.Title));
            mainViewModel.playMusic();
        }

    }
}
