using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    public static class AlbumsManager
    {
        public static void db_Add_Update_Record(Album album)
        {
            //< correct>
            album.name = album.name.Replace("'", "''");
            album.cover_file = album.cover_file.Replace("'", "''");
            album.cover_url = album.cover_url.Replace("'", "''");
            album.resource_uri = album.resource_uri.Replace("'", "''");
            string sqlFormattedDateRelease = null;
            if (album.release != null)
            {
                DateTime dt = album.release.Value;
                sqlFormattedDateRelease = dt.ToString("yyyy - MM - dd HH: mm:ss.fff");
            }
            //</ correct>

            //< find record >
            DataTable tbl = new DataTable();
            if (album.id != null)
            {
                tbl = db_Get_Record(album.id);
            }
            //</ find record >

            if (tbl.Rows.Count == 0)
            {
                string sql_Add = "INSERT INTO Albums ([name], [cover_file], [cover_url], [mbid], [release], [resource_uri]) VALUES('" + album.name + "','" + album.cover_file + "','" + album.cover_url + "','" + album.mbid + "','" + album.release + "','" + album.resource_uri + "')";
                DatabaseHandler.Execute_SQL(sql_Add);
            }
            else
            {
                string sql_Update = "UPDATE Albums SET [name] = '" + album.name + "', [cover_file] = '" + album.cover_file + "', [cover_url] = '" + album.cover_url + "', [mbid] = '" + album.mbid + "', [release] = '" + album.release + "', [resource_uri] = '" + album.resource_uri + "' WHERE Id = " + album.id;
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
