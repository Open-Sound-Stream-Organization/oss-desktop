using OpenSoundStream.Code;
using OpenSoundStream.Code.DataManager;
using OpenSoundStream.Code.NetworkManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace OpenSoundStream
{
    //TODO
    public static class AppHelper
    {
        public static string ProgramPath
        {
            get => Assembly.GetExecutingAssembly().CodeBase;
        }

        public static string DataPath
        {
            get => new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath + "\\Data";
        }

        public static void CheckDataPath()
        {
            Directory.CreateDirectory(DataPath);
        }

        public static void LocalImportTrack(Track track, string sourcePath)
        {
            string album = MetadataEditor.GetAlbum(sourcePath);

            Album newAlbum = null;

            if (AlbumsManager.db_GetAllAlbums().Find(e => e.name == album) != null)
            {
                track.album = "/api/v1/album/" + AlbumsManager.db_GetAllAlbums().Find(e => e.name == album).id.ToString() + "/";
                newAlbum = AlbumsManager.db_GetAllAlbums().Find(e => e.name == album);
            }
            else
            {
                if (album != null)
                {
                    newAlbum = new Album(album);
                    AlbumsNwManager.PostAlbum(newAlbum);
                    newAlbum = AlbumsNwManager.GetAlbums().Find(e => e.name == newAlbum.name);
                    AlbumsManager.db_Add_Update_Record(newAlbum);
                    track.album = "/api/v1/album/" + newAlbum.id.ToString() + "/";
                }
                else
                {
                    if (AlbumsManager.db_GetAllAlbums().Find(e => e.name == "unknown") == null)
                    {
                        newAlbum = new Album("unknown");
                        AlbumsNwManager.PostAlbum(newAlbum);
                        newAlbum = AlbumsNwManager.GetAlbums().Find(e => e.name == newAlbum.name);
                        AlbumsManager.db_Add_Update_Record(newAlbum);
                    }
                    else
                    {
                        newAlbum = AlbumsManager.db_GetAllAlbums().Find(e => e.name == "unknown");
                    }
                    track.album = "/api/v1/album/" + newAlbum.id.ToString() + "/";
                }
            }

            string[] artists = MetadataEditor.GetArtists(sourcePath);

            List<Artist> dbArtists = ArtistsManager.db_GetAllArtists();

            List<string> artistsNwPath = new List<string>();

            foreach (string artistName in artists)
            {
                if (dbArtists.Find(e => e.name == artistName) != null)
                {
                    artistsNwPath.Add("/api/v1/artist/" + dbArtists.Find(e => e.name == artistName).id.ToString() + "/");
                }
                else
                {
                    if (artistName != null)
                    {
                        Artist newArtist = new Artist(artistName);
                        ArtistsNwManager.PostArtist(newArtist);
                        newArtist = ArtistsNwManager.GetArtists().FindLast(e => e.name == newArtist.name);
                        ArtistsManager.db_Add_Update_Record(newArtist);
                        artistsNwPath.Add("/api/v1/artist/" + newArtist.id.ToString() + "/");
                    }
                }
            }
            track.artists = artistsNwPath.ToArray();

            if(track.artists.Count() == 0)
            {
                Artist newArtist = null;
                if(ArtistsManager.db_GetAllArtists().Find(e => e.name == "unknown") == null)
                {
                    newArtist = new Artist("unknown");
                    ArtistsNwManager.PostArtist(newArtist);
                    newArtist = ArtistsNwManager.GetArtists().FindLast(e => e.name == newArtist.name);
                    ArtistsManager.db_Add_Update_Record(newArtist);
                }
                else
                {
                    newArtist = ArtistsManager.db_GetAllArtists().Find(e => e.name == "unknown");
                }
                artists = new string[] { newArtist.name };
                track.artists = new string[] { "/api/v1/artist/" + newArtist.id.ToString() + "/" };
            }

            Directory.CreateDirectory(DataPath + "\\Tracks");

            string fileName = Path.GetFileName(sourcePath);
            string destFile = Path.Combine(DataPath + "\\Tracks", fileName);

            if (File.Exists(destFile) == false)
            {
                File.Copy(sourcePath, destFile, true);
                MetadataEditor.AddAlbum(destFile, newAlbum.name);
                MetadataEditor.AddArtist(destFile, artists);
            }

            if (TracksManager.db_GetAllTracks().Find(e => e.title == fileName.Split('.')[0]) == null)
            {
                track.title = fileName.Split('.')[0];
                track.Filepath = new Uri(@"file:///" + destFile);
                TracksNwManager.PostTrack(track);
                track = TracksNwManager.GetTracks().FindLast(e => e.title == track.title);
                track.audio = destFile;
                TracksManager.db_Add_Update_Record(track);
                TracksNwManager.PutAudio(track);
            }
        }

        public static void LocalImportPlaylist(string sourcePath, Playlist pl)
        {
            string[] files = System.IO.Directory.GetFiles(sourcePath);
            Directory.CreateDirectory(DataPath + "\\Tracks");

            // Copy the files and overwrite destination files if they already exist.
            foreach (string s in files)
            {
                string fileName = System.IO.Path.GetFileName(s);
                //File Extension check (only move .mp3, .wav, ...)
                string format = fileName.Split('.')[fileName.Split('.').Length - 1];
                if (format == "mp3" || format == "wav")
                {
                    LocalImportTrack(new Track(fileName.Split('.')[0], new Uri(@"file:///" + s)), s);
                    Track track = TracksManager.db_GetAllTracks().FindLast(e => e.title == fileName.Split('.')[0]);
                    TrackInPlaylistManager.db_Add_Update_Record(track.id, pl.id);
                    pl.Tracks.AddLast(track);
                }

            }
        }

        public static void LocalImportAlbum(string sourcePath, Album album)
        {
            string[] files = System.IO.Directory.GetFiles(sourcePath);
            Directory.CreateDirectory(DataPath + "\\Tracks");

            foreach (string s in files)
            {
                string fileName = System.IO.Path.GetFileName(s);
                //File Extension check (only move .mp3, .wav, ...)
                string format = fileName.Split('.')[fileName.Split('.').Length - 1];
                if (format == "mp3" || format == "wav")
                {
                    Track track = new Track(fileName.Split('.')[0], new Uri(@"file:///" + s));
                    MetadataEditor.AddAlbum(s, album.name);
                    LocalImportTrack(track, s);
                    track = TracksManager.db_GetAllTracks().FindLast(e => e.title == fileName.Split('.')[0]);
                    album.Tracks.AddLast(track);
                }
            }
        }

        public static void SaveToDrive(Playlist playlist, string path)
        {
            Directory.CreateDirectory(DataPath + playlist.name);

            throw new System.NotImplementedException();
        }

        public static void SaveToDrive(Album album, string path)
        {
            Directory.CreateDirectory(DataPath + album.name);
            throw new System.NotImplementedException();
        }

        public static void SaveToDrive(Track track, string path)
        {
            Directory.CreateDirectory(DataPath + "Tracks");
            throw new System.NotImplementedException();
        }

        public static void SaveToDrive(Playlist playlist) => SaveToDrive(playlist, DataPath);

        public static void SaveToDrive(Album album) => SaveToDrive(album, DataPath);

        public static void SaveToDrive(Track track) => SaveToDrive(track, DataPath);

        public static void SaveToDrive(List<Playlist> playlists) => playlists.ForEach(delegate (Playlist p) { SaveToDrive(p, DataPath); });

        public static void SaveToDrive(List<Album> albums) => albums.ForEach(delegate (Album a) { SaveToDrive(a, DataPath); });

        public static void SaveToDrive(List<Track> tracks) => tracks.ForEach(delegate (Track t) { SaveToDrive(t, DataPath); });

    }
}