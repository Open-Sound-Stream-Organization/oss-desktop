using OpenSoundStream.Code.DataManager;
using OpenSoundStream.Code.NetworkManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenSoundStream
{
    public static class AppHelper
    {
        public static List<Track> Tracks = TracksManager.db_GetAllTracks();
        public static string ProgramPath
        {
            get => Assembly.GetExecutingAssembly().CodeBase;
        }

        public static string DataPath
        {
            get => new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath + "\\Data";
            //get => Assembly.GetExecutingAssembly().CodeBase + "/Data";
        }

        public static void CheckDataPath()
        {
            Directory.CreateDirectory(DataPath);
        }

        public static void LocalImportTrack(Track track, string sourcePath)
        {
            Directory.CreateDirectory(DataPath + "\\Tracks");

            string fileName = Path.GetFileName(sourcePath);
            string destFile = Path.Combine(DataPath + "\\Tracks", fileName);
            if (File.Exists(destFile))
            {
                //TODO Hash abgleich
                if (Tracks.Find(e => e.title == fileName.Split('.')[0]) == null)
                {
                    track.title = fileName.Split('.')[0];
                    track.Filepath = new Uri(@"file:///" + destFile);
                    TracksNwManager.PostTrack(track);
                    track = TracksNwManager.GetTracks().Find(e => e.title == track.title);
                    track.audio = destFile;
                    TracksManager.db_Add_Update_Record(track);
                    TracksNwManager.PutAudio(track);
                }
            }
            else
            {
                try
                {
                    if(Tracks.Find(e => e.title == fileName.Split('.')[0] ) == null)
                    {
                        File.Copy(sourcePath, destFile, true);
                        track.title = fileName.Split('.')[0];
                        track.Filepath = new Uri(@"file:///" + destFile);
                        TracksNwManager.PostTrack(track);
                        track = TracksNwManager.GetTracks().Find(e => e.title == track.title);
                        track.audio = destFile;
                        TracksManager.db_Add_Update_Record(track);
                        TracksNwManager.PutAudio(track);
                    }
                }
                catch (Exception ex)
                {
                    //TODO Fehlermanagement
                    throw ex;
                }
            }
        }

        public static void LocalImportPlaylist(string sourcePath, Playlist pl)
        {
            Directory.CreateDirectory(DataPath + "\\Playlists");
            string path = DataPath + "\\Playlists" + "\\" + Path.GetFileName(sourcePath);
            string[] files = System.IO.Directory.GetFiles(sourcePath);
            Directory.CreateDirectory(path);

            // Copy the files and overwrite destination files if they already exist.
            foreach (string s in files)
            {
                string fileName = System.IO.Path.GetFileName(s);
                //File Extension check (only move .mp3, .wav, ...)
                string format = fileName.Split('.')[fileName.Split('.').Length - 1];
                if (format == "mp3" || format == "wav")
                {
                    string destFile = System.IO.Path.Combine(path, fileName);
                    System.IO.File.Copy(s, destFile, true);

                    if (Tracks.Find(e => e.title == fileName.Split('.')[0]) == null)
                    {
                        Track track = new Track(fileName.Split('.')[0], new Uri(destFile));
                        track.audio = destFile;
                        TracksNwManager.PostTrack(track);
                        TracksManager.db_Add_Update_Record(TracksNwManager.GetTracks().Find(e => e.title == track.title));
                        track.id = TracksManager.db_GetAllTracks().Find(e => e.title == track.title).id;
                        TrackInPlaylistManager.db_Add_Update_Record(track.id, pl.id);
                        pl.AddTrack(track);
                    }
                    else
                    {
                        pl.AddTrack(Tracks.Find(e => e.title == fileName.Split('.')[0]));
                    }
                }

            }
        }

        public static void LocalImportAlbum(string sourcePath, Album album)
        {
            Directory.CreateDirectory(DataPath + "\\Albums");
            string path = DataPath + "\\Albums" + "\\" + Path.GetFileName(sourcePath);
            string[] files = System.IO.Directory.GetFiles(sourcePath);

            Directory.CreateDirectory(path);

            foreach (string s in files)
            {
                string fileName = System.IO.Path.GetFileName(s);
                //File Extension check (only move .mp3, .wav, ...)
                string format = fileName.Split('.')[fileName.Split('.').Length - 1];
                if (format == "mp3" || format == "wav")
                {
                    string destFile = System.IO.Path.Combine(path, fileName);
                    System.IO.File.Copy(s, destFile, true);
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