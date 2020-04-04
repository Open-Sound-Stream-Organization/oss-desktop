using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
	public static class OpenSoundStreamManager
	{
		static OpenSoundStreamManager()
		{
			DatabaseHandler = new DatabaseHandler();
			NetworkHandler = new NetworkHandler();
			Musicplayer = new Musicplayer();

			Track.Tracks = new List<Track>();
			Album.Albums = new List<Album>();
			Playlist.Playlists = new List<Playlist>();
			Artist.Artists = new List<Artist>();

			//DEBUG
			Track a = new Track("01 Titelnummer 1", new Uri(@"file:///C:/Users/cpfro/Music/Track1.mp3"));
			Track b = new Track("02 Titelnummer 2", new Uri(@"file:///C:/Users/cpfro/Music/Track2.mp3"));

			AppHelper.LocalImportTrack(@"C:/Users/cpfro/Music/Track1.mp3");
			AppHelper.LocalImportPlaylist(@"C:/Users/cpfro/Music");

			Playlist pl = new Playlist("Party");
			pl.AddTrack(a);
			pl.AddTrack(b);

			Musicplayer.Queue.Tracks = pl.Tracks;

			//Musicplayer.Queue.AddTrackToQueueLastPos(a);
			//Musicplayer.Queue.AddTrackToQueueLastPos(b);
		}


		public static DatabaseHandler DatabaseHandler { get; set; }

		public static NetworkHandler NetworkHandler { get; set; }

		public static Musicplayer Musicplayer { get; set; }

	}
}