using OpenSoundStream.Code.NetworkManager;
using System;

namespace OpenSoundStream
{
    public class OpenSoundStreamManager
    {
        public OpenSoundStreamManager()
        {
            DatabaseHandler = new DatabaseHandler();
            Musicplayer = new Musicplayer();

            NetworkHandler.Login("testuser", "testuser");

            Track track = new Track("test", new Uri(@"file:///C:/Users/cpfro/Music/Party/Drink123.mp3"));
            track.album = "/api/v1/album/592";
            track.artists = new String[] { "/api/v1/artist/620" };

            TracksNwManager.PostTrack(track);
        }

        public static DatabaseHandler DatabaseHandler { get; set; }

        public static Musicplayer Musicplayer { get; set; }

    }
}