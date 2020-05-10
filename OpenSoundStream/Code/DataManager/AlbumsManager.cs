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
            if(album.cover_file != null)
            {
                album.cover_file = album.cover_file.Replace("'", "''");
            }
            if (album.cover_url != null)
            {
                album.cover_url = album.cover_url.Replace("'", "''");
            }
            if (album.resource_uri != null)
            {
                album.resource_uri = album.resource_uri.Replace("'", "''");
            }
            string sqlFormattedDateRelease = null;
            if (album.release != null)
            {
                DateTime dt = album.release.Value;
                sqlFormattedDateRelease = dt.ToString("yyyy - MM - dd HH: mm:ss.fff");
            }
            //</ correct>

            //< find record >
            Album dbRecord = null;
            if (album.id != null)
            {
                dbRecord = db_Get_Record(album.id);
            }
            //</ find record >

            if (dbRecord == null)
            {
                string sql_Add = null;
                if(sqlFormattedDateRelease == null)
                {
                    sql_Add = "INSERT INTO Albums ([id], [name], [cover_file], [cover_url], [mbid], [resource_uri]) VALUES('" + album.id + "','" + album.name + "','" + album.cover_file + "','" + album.cover_url + "','" + album.mbid + "','" + album.resource_uri + "')";
                }
                else
                {
                    sql_Add = "INSERT INTO Albums ([id], [name], [cover_file], [cover_url], [mbid], [release], [resource_uri]) VALUES('" + album.id + "','" + album.name + "','" + album.cover_file + "','" + album.cover_url + "','" + album.mbid + "','" + sqlFormattedDateRelease + "','" + album.resource_uri + "')";
                }
                DatabaseHandler.Execute_SQL(sql_Add);
            }
            else
            {
                string sql_Update = "UPDATE Albums SET [name] = '" + album.name + "', [cover_file] = '" + album.cover_file + "', [cover_url] = '" + album.cover_url + "', [mbid] = '" + album.mbid + "', [release] = '" + sqlFormattedDateRelease + "', [resource_uri] = '" + album.resource_uri + "' WHERE Id = " + album.id;
                DatabaseHandler.Execute_SQL(sql_Update);
            }
        }

        public static List<Album> db_GetAllAlbums()
        {
            string sSQL = "SELECT * FROM Albums";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            if (tbl.Rows.Count > 0)
            {
                List<Album> albums = new List<Album>();
                foreach (DataRow row in tbl.Rows)
                {
                    Album album = new Album(row["name"].ToString());
                    album.id = Convert.ToInt32(row["id"].ToString());
                    if(row["mbid"].ToString() != "")
                    {
                        album.mbid = row["mbid"].ToString();
                    }

                    if (row["release"].ToString() != "")
                    {
                        album.release = Convert.ToDateTime(row["release"].ToString());
                    }
                    album.resource_uri = row["resource_uri"].ToString();
                    album.cover_file = row["cover_file"].ToString();
                    album.cover_url = row["cover_url"].ToString();
                    albums.Add(album);
                }

                return albums;
            }
            else
            {
                return new List<Album>();
            }
        }

        public static Album db_Get_Record(int? id)
        {
            string sSQL = "SELECT TOP 1 * FROM Albums WHERE [Id] Like '" + id + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            if (tbl.Rows.Count == 1)
            {
                DataRow row = tbl.Rows[0];
                Album album = new Album(row["name"].ToString());
                album.id = Convert.ToInt32(row["id"].ToString());
                album.mbid = row["mbid"].ToString();
                if(row["release"].ToString() != "")
                {
                    album.release = Convert.ToDateTime(row["release"].ToString());
                }
                album.resource_uri = row["resource_uri"].ToString();
                album.cover_file = row["cover_file"].ToString();
                album.cover_url = row["cover_url"].ToString();
                return album;
            }
            else
            {
                return null;
            }
        }

        public static void db_Delete_Record(int id)
        {
            string sSQL = "Delete FROM Albums WHERE [Id] Like '" + id + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }

        public static void db_Delete_All()
        {
            string Ssql = "Delete FROM Albums";
            DatabaseHandler.Execute_SQL(Ssql);
        }
    }
}
