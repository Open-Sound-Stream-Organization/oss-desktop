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

            //NetworkHandler.Login("testuser", "testuser");
            //NetworkHandler.Logout();
            //NetworkHandler.SyncLocalDbWithServerDb();
            //DEBUG
            //Playlist p1 = new Playlist("Cello");

            //AppHelper.LocalImportTrack(@"C:/Users/menac/Music/Test1.wav");
            //AppHelper.LocalImportTrack(@"C:/Users/menac/Music/Test1.wav.opensound");
            //AppHelper.LocalImportPlaylist(@"C:/Users/menac/Desktop/Cello/Spielbuch", p1);
        }

        public static DatabaseHandler DatabaseHandler { get; set; }

        public static Musicplayer Musicplayer { get; set; }

    }
}