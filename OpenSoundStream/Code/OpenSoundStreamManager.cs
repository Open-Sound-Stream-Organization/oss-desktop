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
			NetworkHandler = new NetworkHandler();
			Musicplayer = new Musicplayer();

			Track.Tracks = new List<Track>();
			Album.Albums = new List<Album>();
			Playlist.Playlists = new List<Playlist>();
			Artist.Artists = new List<Artist>();

			//DEBUG
			//Playlist p1 = new Playlist("Cello");

			Playlist p1 = new Playlist("Good Vibes");
			//Playlist p2 = new Playlist("Chill");

			//AppHelper.LocalImportTrack(@"C:/Users/menac/Music/Test1.wav");
			//AppHelper.LocalImportTrack(@"C:/Users/menac/Music/Test1.wav.opensound");
			//AppHelper.LocalImportPlaylist(@"C:/Users/menac/Desktop/Cello/Spielbuch", p1);

			//AppHelper.LocalImportTrack(@"C:/Users/cpfro/Music/Track1.mp3");
			//AppHelper.LocalImportTrack(@"C:/Users/cpfro/Music/Track2.mp3");
			//AppHelper.LocalImportTrack(@"C:/Users/cpfro/Music/Track3.mp3");
			//AppHelper.LocalImportTrack(@"C:/Users/cpfro/Music/Party/Bad child.mp3");
			AppHelper.LocalImportPlaylist(@"C:/Users/cpfro/Music/Party", p1);
			//AppHelper.LocalImportPlaylist(@"C:/Users/cpfro/Music/Chill", p2);

			//Musicplayer.Musicqueue.LoadPlayableContainerInQueue(p1);
			//Musicplayer.Musicqueue.AddTrackToQueueFirstPos(a);

			//ArtistsManager.db_Update_Record(1, artist);

			//ArtistsManager.db_Delete_Record(1);

			//Musicplayer.Musicqueue.AddTrackToQueueLastPos(a);
			//Musicplayer.Musicqueue.AddTrackToQueueLastPos(b);

			//Track track = new Track("Superlonely", new Uri(@"file:///C:/Users/cpfro/Music/Track3.mp3"));
			//track.album = "/api/v1/album/1/";
			//track.artists = new string[] {"/api/v1/artist/2/" };
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
			TrackInPlaylistNwManager.GetTrackInPlaylist(7);
		}


		public static DatabaseHandler DatabaseHandler { get; set; }

		public static NetworkHandler NetworkHandler { get; set; }

		public static Musicplayer Musicplayer { get; set; }

	}
}