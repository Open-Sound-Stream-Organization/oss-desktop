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
        static public void AddArtist(string path, string[] Artists)
        {
            TagLib.File f = TagLib.File.Create(path);
            f.Tag.Performers = Artists;
            f.Save();
        }

        static public void AddAlbum(string path, string Album)
        {
            TagLib.File f = TagLib.File.Create(path);
            f.Tag.Album = Album;
            f.Save();
        }

        static public string[] GetArtists(string path)
        {
            TagLib.File f = TagLib.File.Create(path);
            return f.Tag.Performers;
        }
        
        static public string GetAlbum(string path)
        {
            TagLib.File f = TagLib.File.Create(path);
            return f.Tag.Album;
        }

    }
}
