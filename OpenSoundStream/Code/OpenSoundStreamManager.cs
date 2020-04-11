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
			NetworkHandler = new NetworkHandler();
			Musicplayer = new Musicplayer();

			Track.Tracks = new List<Track>();
			Album.Albums = new List<Album>();
			Playlist.Playlists = new List<Playlist>();
			Artist.Artists = new List<Artist>();

			//DEBUG
			Playlist p1 = new Playlist("Cello");

			//AppHelper.LocalImportTrack(@"C:/Users/menac/Music/Test1.wav");
			//AppHelper.LocalImportTrack(@"C:/Users/menac/Music/Test1.wav.opensound");
			AppHelper.LocalImportPlaylist(@"C:/Users/menac/Desktop/Cello/Spielbuch", p1);

			//Musicplayer.Musicqueue.LoadPlaylistInQueue(p1);

			
			//Musicplayer.Queue.AddTrackToQueueLastPos(a);
			//Musicplayer.Queue.AddTrackToQueueLastPos(b);
		}


		public static DatabaseHandler DatabaseHandler { get; set; }

		public static NetworkHandler NetworkHandler { get; set; }

		public static Musicplayer Musicplayer { get; set; }

	}
}