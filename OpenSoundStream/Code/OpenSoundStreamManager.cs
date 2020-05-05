using OpenSoundStream.Code;
using OpenSoundStream.Code.DataManager;
using OpenSoundStream.Code.NetworkManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class OpenSoundStreamManager
    {
        public OpenSoundStreamManager()
        {
            DatabaseHandler = new DatabaseHandler();
            Musicplayer = new Musicplayer();

            NetworkHandler.Login("testuser", "testuser");
            NetworkHandler.SyncLocalDbWithServerDb();
            //DEBUG
            //Playlist p1 = new Playlist("Cello");

            //Playlist p1 = new Playlist("Good Vibes");
            //p1.initializePlaylist();
            //Playlist p2 = new Playlist("Chill");

            //AppHelper.LocalImportTrack(@"C:/Users/menac/Music/Test1.wav");
            //AppHelper.LocalImportTrack(@"C:/Users/menac/Music/Test1.wav.opensound");
            //AppHelper.LocalImportPlaylist(@"C:/Users/menac/Desktop/Cello/Spielbuch", p1);


            Track track = new Track("Supalonely", new Uri("C:/Users/cpfro/Music/Track2.mp3"));
            //track.album = "/api/v1/album/576/";
            //track.artists = new string[] { "/api/v1/artist/600/" };

            //AppHelper.LocalImportTrack(track, @"C:/Users/cpfro/Music/Track1.mp3");
            //AppHelper.LocalImportTrack(@"C:/Users/cpfro/Music/Track2.mp3");
            //AppHelper.LocalImportTrack(@"C:/Users/cpfro/Music/Track3.mp3");
            //AppHelper.LocalImportTrack(@"C:/Users/cpfro/Music/Party/Bad child.mp3");
            //AppHelper.LocalImportPlaylist(@"C:/Users/cpfro/Music/Party", p1);
            //AppHelper.LocalImportPlaylist(@"C:/Users/cpfro/Music/Chill", p1);
            //Album album = new Album("Silence");
            //album.initializeAlbum();

            //AppHelper.LocalImportAlbum(@"C:/Users/cpfro/Music/Silence", album);

            //Musicplayer.Musicqueue.LoadPlayableContainerInQueue(p1);
            //Musicplayer.Musicqueue.AddTrackToQueueFirstPos(a);

            //ArtistsManager.db_Update_Record(1, artist);

            //ArtistsManager.db_Delete_Record(1);

            //Musicplayer.Musicqueue.AddTrackToQueueLastPos(a);
            //Musicplayer.Musicqueue.AddTrackToQueueLastPos(b);


            //track.id = 17;

            //TracksNwManager.GetTrack(15);
            //TracksNwManager.PostTrack(track);
            //TracksNwManager.PutTrack(track);
            //TracksNwManager.DeleteTrack(track.id);
            //TracksNwManager.GetTrack(15);

            //Artist a1 = ArtistsNwManager.GetArtist(564);
            //ArtistsManager.db_Add_Record(a1);
            //Artist a1 = ArtistsNwManager.GetArtist(564);
            //Artist a2 = ArtistsNwManager.GetArtist(16);
            //ArtistsManager.db_Add_Update_Record(a1);
            //ArtistsManager.db_Add_Update_Record(a2);

            //NetworkHandler.SyncLocalDbWithServerDb();
            //TrackInPlaylistNwManager.GetTrackInPlaylist(7);

            //Track Track = TracksNwManager.GetTrack(317);
            //Track.audio = @"C:/Users/cpfro/Music/Party/Drink.mp3";
            //TracksNwManager.PutAudio(Track);
        }


        public static DatabaseHandler DatabaseHandler { get; set; }

        public static Musicplayer Musicplayer { get; set; }

    }
}