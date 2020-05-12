using DynamicData.Annotations;
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

        /// <summary>
        /// Check if data path for audio files exists
        /// </summary>
        public static void CheckDataPath()
        {
            Directory.CreateDirectory(DataPath);
        }


        /// <summary>
        /// Import local track to localDB and ServerDb
        /// </summary>
        /// <param name="track"></param>
        /// <param name="sourcePath"></param>
        public static void LocalImportTrack(Track track, string sourcePath)
        {
            // <Album check>
            Album newAlbum = null;
            if (MetadataEditor.GetAlbum(sourcePath) != null)
            {
                // Get album from Metadata
                string album = MetadataEditor.GetAlbum(sourcePath);

                // if Album exists get it and assign it to newAlbum
                if (AlbumsManager.db_GetAllAlbums().Find(e => e.name == album) != null)
                {
                    track.album = "/api/v1/album/" + AlbumsManager.db_GetAllAlbums().Find(e => e.name == album).id.ToString() + "/";
                    newAlbum = AlbumsManager.db_GetAllAlbums().Find(e => e.name == album);
                }
                else
                {
                    // If album doesn't exist in Db but isn`t null, create new
                    if (album != null)
                    {
                        newAlbum = new Album(album);
                        // Create new Album in ServerDb 
                        AlbumsNwManager.PostAlbum(newAlbum);
                        // Get new Album from ServerDb and add it in localDb
                        newAlbum = AlbumsNwManager.GetAlbums().Find(e => e.name == newAlbum.name);
                        AlbumsManager.db_Add_Update_Record(newAlbum);
                        //assign newAlbum to track
                        track.album = "/api/v1/album/" + newAlbum.id.ToString() + "/";
                    }
                    // If album doesn't exist in Db and is null -> album = unknown
                    else
                    {
                        // if album unknown doesn't exist -> create it
                        if (AlbumsManager.db_GetAllAlbums().Find(e => e.name == "unknown") == null)
                        {
                            newAlbum = new Album("unknown");
                            AlbumsNwManager.PostAlbum(newAlbum);
                            newAlbum = AlbumsNwManager.GetAlbums().Find(e => e.name == newAlbum.name);
                            AlbumsManager.db_Add_Update_Record(newAlbum);
                        }
                        // if Album allready unknown exists -> assign it to track
                        else
                        {
                            newAlbum = AlbumsManager.db_GetAllAlbums().Find(e => e.name == "unknown");
                        }
                        track.album = "/api/v1/album/" + newAlbum.id.ToString() + "/";
                    }
                }
            }
            // </Album check>

            // <Artists check>
            string[] artists = null;
            if (MetadataEditor.GetArtists(sourcePath) != null)
            {
                //Get Artists from Metadata
                artists = MetadataEditor.GetArtists(sourcePath);

                List<Artist> dbArtists = ArtistsManager.db_GetAllArtists();

                // List for networkpaths from artists
                List<string> artistsNwPath = new List<string>();

                foreach (string artistName in artists)
                {
                    // if Artist exists get it and assign it to artistsNwPath
                    if (dbArtists.Find(e => e.name == artistName) != null)
                    {
                        artistsNwPath.Add("/api/v1/artist/" + dbArtists.Find(e => e.name == artistName).id.ToString() + "/");
                    }
                    // if Artist doesn't exist create it and assign it to artistsNwPath
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
                // assign all paths to track
                track.artists = artistsNwPath.ToArray();

                // If artists doesn't exist in Db and count = 0 -> artists = unknown
                if (track.artists.Count() == 0)
                {
                    Artist newArtist = null;
                    if (ArtistsManager.db_GetAllArtists().Find(e => e.name == "unknown") == null)
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
            }
            // </Artists check>

            // Create Directory for tracks if doesn't exist
            Directory.CreateDirectory(DataPath + "\\Tracks");

            string fileName = Path.GetFileName(sourcePath);
            //intern filepath -> copy audio file to destfile
            string destFile = Path.Combine(DataPath + "\\Tracks", fileName);

            //Create Filepath
            if (File.Exists(destFile) == false)
            {
                // Copy audio file
                File.Copy(sourcePath, destFile, true);
                if(newAlbum != null)
                {
                MetadataEditor.AddAlbum(destFile, newAlbum.name);

                }
                if(artists != null)
                {
                    MetadataEditor.AddArtist(destFile, artists);
                }
                
            }

            // if track doesn't exist in Db create new
            if (TracksManager.db_GetAllTracks().Find(e => e.title == fileName.Split('.')[0]) == null)
            {
                track.title = fileName.Split('.')[0];
                track.Filepath = new Uri(@"file:///" + destFile);
                // Post to ServerDb
                track.audio = destFile;
                TracksNwManager.PostTrack(track);
                // Get new Track and add to localDb
                track = TracksNwManager.GetTracks().FindLast(e => e.title == track.title);
                if(track != null)
                {
                    MetadataEditor.AddTitle(destFile, track.title);
                    track.audio = destFile;
                    TracksManager.db_Add_Update_Record(track);
                    // Add Audio to track
                    TracksNwManager.PutAudio(track);
                }
            }
        }

        /// <summary>
        /// Import files from directory (only .mp3 and .wav) -> Directory as playlist
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="pl"></param>
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
                    // Assign track to playlist
                    TrackInPlaylistManager.db_Add_Update_Record(track.id, pl.id);
                    pl.Tracks.AddLast(track);
                }

            }
        }

        /// <summary>
        /// Import files from directory (only .mp3 and .wav) -> Directory as Album
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="album"></param>
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
                    // Assign Track to Album
                    track = TracksManager.db_GetAllTracks().FindLast(e => e.title == fileName.Split('.')[0]);
                    album.Tracks.AddLast(track);
                }
            }
        }
    }
}