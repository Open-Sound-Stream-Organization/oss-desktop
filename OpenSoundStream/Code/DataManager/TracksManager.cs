using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    class TracksManager
    {
        public static void db_Add_Update_Record(Track track)
        {
            //< correct>
            track.title = track.title.Replace("'", "''");
            track.resource_uri = track.resource_uri.Replace("'", "''");
            track.audio = track.audio.Replace("'", "''");
            //</ correct>

            //< find record >
            DataTable tbl = new DataTable();
            if (track.id != null)
            {
                tbl = db_Get_Record(track.id);
            }
            //</ find record >

            string[] splitAlbumPath = track.album.Split('/');
            track.album = splitAlbumPath[splitAlbumPath.Length - 1];
            if (tbl.Rows.Count == 0)
            {
                string sql_Add = "INSERT INTO Albums ([title], [resource_uri], [albumId], [audio], [mbid]) VALUES('" + track.title + "','" + track.resource_uri + "','" + track.album + "','" + track.audio + "','" + track.mbid + "')";
                DatabaseHandler.Execute_SQL(sql_Add);
            }
            else
            {
                string sql_Update = "UPDATE Albums SET [name] = '" + track.title + "', [resource_uri] = '" + track.resource_uri + "', [albumId] = '" + track.album + "', [audio] = '" + track.audio + "', [mbid] = '" + track.mbid + "' WHERE Id = " + track.id;
                DatabaseHandler.Execute_SQL(sql_Update);
            }
        }

        public static DataTable db_Get_Record(int? id)
        {
            string sSQL = "SELECT TOP 1 * FROM Albums WHERE [Id] Like '" + id + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            return tbl;
        }

        public static void db_Delete_Record(int id)
        {
            string sSQL = "Delete FROM Albums WHERE [Id] Like '" + id + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }
    }
}
