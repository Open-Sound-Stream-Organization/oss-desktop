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
			Track a = new Track("01 Titelnummer 1", new Uri(@"file:///C:/Users/kaiuw/Music/Cello/Celloschule/01 Titelnummer 1.wav"));
			Track b = new Track("02 Titelnummer 2", new Uri(@"file:///C:/Users/kaiuw/Music/Cello/Celloschule/02 Titelnummer 2.wav"));
			Track c = new Track("03 Titelnummer 3", new Uri(@"file:///C:/Users/kaiuw/Music/Cello/Celloschule/03 Titelnummer 3.wav"));
			Musicplayer.Queue.AddTrackToQueueLastPos(a);
			Musicplayer.Queue.AddTrackToQueueLastPos(b);
			Musicplayer.Queue.AddTrackToQueueLastPos(c);
		}


		public static DatabaseHandler DatabaseHandler { get; set; }

		public static NetworkHandler NetworkHandler { get; set; }

		public static Musicplayer Musicplayer { get; set; }

	}
}