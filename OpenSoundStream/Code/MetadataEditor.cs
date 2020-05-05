using OpenSoundStream.Code.NetworkManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code
{
    public class MetadataEditor
    {
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

        public static void AddYear(string path, DateTime date)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(path);
                f.Tag.Year = (uint)date.Year;
                f.Save();
            }
            catch(Exception) 
            {
            }
        }

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

        public static void SyncNewMetadata(Track track)
        {
            TracksNwManager.PutAudio(track);
        }
    }
}
