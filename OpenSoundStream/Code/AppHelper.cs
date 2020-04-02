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
        public static string ProgramPath
        {
            get => Assembly.GetExecutingAssembly().CodeBase;
        }

        public static string DataPath
        {
            get => Assembly.GetExecutingAssembly().CodeBase + "/Data";
        }

        public static void SaveToDrive(Playlist playlist, string path)
        {
            throw new System.NotImplementedException();
        }

        public static void SaveToDrive(Album album, string path)
        {
            throw new System.NotImplementedException();
        }

        public static void SaveToDrive(Track track, string path)
        {
            throw new System.NotImplementedException();
        }

        public static void SaveToDrive(Playlist playlist) => SaveToDrive(playlist, DataPath);

        public static void SaveToDrive(Album album) => SaveToDrive(album, DataPath);

        public static void SaveToDrive(Track track) => SaveToDrive(track, DataPath);

        public static void SaveToDrive(List<Playlist> playlists) => playlists.ForEach(delegate(Playlist p) { SaveToDrive(p, DataPath); });

        public static void SaveToDrive(List<Album> albums) => albums.ForEach(delegate (Album a) { SaveToDrive(a, DataPath); });

        public static void SaveToDrive(List<Track> tracks) => tracks.ForEach(delegate (Track t) { SaveToDrive(t, DataPath); });

    }
}