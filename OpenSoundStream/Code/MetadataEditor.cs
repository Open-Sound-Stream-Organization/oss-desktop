using OpenSoundStream.Code.NetworkManager;
using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TagLib;

namespace OpenSoundStream.Code
{
    public class MetadataEditor
    {
        /// <summary>
        /// Add Title Information
        /// </summary>
        /// <param name="path"></param>
        /// <param name="title"></param>
        public static void AddTitle(string path, string title)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                f.Tag.Title = title;
                f.Save();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Add Artist Information
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Artists"></param>
        public static void AddArtist(string path, string[] Artists)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                f.Tag.Performers = Artists;
                f.Save();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Add Album Information
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Album"></param>
        public static void AddAlbum(string path, string Album)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                f.Tag.Album = Album;
                f.Save();
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Add Year Information
        /// </summary>
        /// <param name="path"></param>
        /// <param name="date"></param>
        public static void AddYear(string path, DateTime date)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                f.Tag.Year = (uint)date.Year;
                f.Save();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Add Genre Information
        /// </summary>
        /// <param name="path"></param>
        /// <param name="genres"></param>
        public static void AddGenre(string path, string[] genres)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                f.Tag.Genres = genres;
                f.Save();
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// Get all Artist 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetArtists(string path)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                return f.Tag.Performers;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Get Album
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAlbum(string path)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                return f.Tag.Album;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Get Year
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static uint? GetYear(string path)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                return f.Tag.Year;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Get all Genres
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetGenres(string path)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                return f.Tag.Genres;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Duration
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TimeSpan GetDuration(string path)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                return f.Properties.Duration;
            }
            catch (Exception)
            {
                return new TimeSpan(0, 0, 0);
            }
        }

        /// <summary>
        /// Sync edited metadata
        /// </summary>
        /// <param name="track"></param>
        public static void SyncNewMetadata(Track track)
        {
            TracksNwManager.PutAudio(track);
        }
    }
}
