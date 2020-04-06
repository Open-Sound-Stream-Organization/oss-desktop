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

			////DEBUG
			//Track a = new Track("01 Titelnummer 1", new Uri(@"file:///C:/Users/kaiuw/Music/Test1.wav"));
			//Track b = new Track("02 Titelnummer 2", new Uri(@"file:///C:/Users/kaiuw/Music/Test2.wav"));
			//Track c = new Track("Test", new Uri(@"file:///C:/Users/kaiuw/Music/Test3.wav.opensound"));

			//Playlist p1 = new Playlist("Celloschule");
			//Playlist p2 = new Playlist("Test");

			//AppHelper.LocalImportTrack(@"C:/Users/kaiuw/Music/Test1.wav");
			//AppHelper.LocalImportPlaylist(@"C:/Users/kaiuw/Music/Cello/Celloschule", p1);

			//DEBUG
			Track a = new Track("01 Titelnummer 1", new Uri(@"file:///C:/Users/menac/Music/Test1.wav"));
			Track b = new Track("02 Titelnummer 2", new Uri(@"file:///C:/Users/menac/Music/Test1.wav"));
			Track c = new Track("Test", new Uri(@"file:///C:/Users/menac/Music/Test1.wav.opensound"));

			Playlist p1 = new Playlist("Celloschule");
			Playlist p2 = new Playlist("Test");

			AppHelper.LocalImportTrack(@"C:/Users/menac/Music/Test1.wav");
			AppHelper.LocalImportPlaylist(@"C:/Users/menac/Desktop/Cello/Celloschule", p1);


			p2.AddTrack(a);
			p2.AddTrack(b);
			p2.AddTrack(c);

			Musicplayer.Musicqueue.LoadPlaylistInQueue(p1);

			
			//Musicplayer.Queue.AddTrackToQueueLastPos(a);
			//Musicplayer.Queue.AddTrackToQueueLastPos(b);
		}


		public static DatabaseHandler DatabaseHandler { get; set; }

		public static NetworkHandler NetworkHandler { get; set; }

		public static Musicplayer Musicplayer { get; set; }

	}
}