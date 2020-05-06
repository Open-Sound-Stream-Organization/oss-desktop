using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    class ArtistsManager
    {
        public static void db_Add_Update_Record(Artist artist)
        {
            //< correct>
            artist.name = artist.name.Replace("'", "''");

            string sqlFormattedDateBegin = null;
            if (artist.begin != null)
            {
                DateTime dt = artist.begin.Value;
                sqlFormattedDateBegin = dt.ToString("yyyy - MM - dd HH: mm:ss.fff");
            }
            string sqlFormattedDateEnd = null;
            if (artist.end != null)
            {
                DateTime dt = artist.begin.Value;
                sqlFormattedDateEnd = dt.ToString("yyyy - MM - dd HH: mm:ss.fff");

            }
            //</ correct>

            //< find record >
            Artist dbRecord = null;
            if(artist.id != null) 
            {
                dbRecord = db_Get_Record(artist.id);
            }
            //</ find record >

            if (dbRecord == null)
            {
                string sql_Add = "INSERT INTO Artists ([id], [name], [begin], [end], [mbid], [resource_uri], [type]) VALUES('" + artist.id + "','" + artist.name + "','" + sqlFormattedDateBegin + "','" + sqlFormattedDateEnd + "','" + artist.mbid + "','" + artist.resource_uri + "','" + artist.type + "')";
                DatabaseHandler.Execute_SQL(sql_Add);
            }
            else
            {
                string sql_Update = "UPDATE Artists SET [name] = '" + artist.name + "', [begin] = '" + sqlFormattedDateBegin + "', [end] = '" + sqlFormattedDateEnd + "', [mbid] = '" + artist.mbid + "', [resource_uri] = '" + artist.resource_uri + "', [type] = '" + artist.type + "' WHERE Id = " + artist.id;
                DatabaseHandler.Execute_SQL(sql_Update);
            }
        }

        public static List<Artist> db_GetAllArtists()
        {
            string sSQL = "SELECT * FROM Artists";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            if (tbl.Rows.Count > 0)
            {
                List<Artist> artists = new List<Artist>();
                foreach (DataRow row in tbl.Rows)
                {
                    Artist artist = new Artist(row["name"].ToString());
                    artist.id = Convert.ToInt32(row["id"].ToString());
                    artist.mbid = Convert.ToInt32(row["mbid"].ToString());
                    artist.begin = Convert.ToDateTime(row["begin"].ToString());
                    artist.end = Convert.ToDateTime(row["end"].ToString());
                    artist.resource_uri = row["resource_uri"].ToString();
                    artist.type = row["type"].ToString();
                    artists.Add(artist);
                }

                return artists;
            }
            else
            {
                return new List<Artist>();
            }
        }

        public static Artist db_Get_Record(int? id)
        {
            string sSQL = "SELECT TOP 1 * FROM Artists WHERE [Id] Like '" + id + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            if (tbl.Rows.Count == 1)
            {
                DataRow row = tbl.Rows[0];
                Artist artist = new Artist(row["name"].ToString());
                artist.id = Convert.ToInt32(row["id"].ToString());
                artist.mbid = Convert.ToInt32(row["mbid"].ToString());
                artist.begin =  Convert.ToDateTime(row["begin"].ToString());
                artist.end = Convert.ToDateTime(row["end"].ToString());
                artist.resource_uri = row["resource_uri"].ToString();
                artist.type = row["type"].ToString();
                return artist;
            }
            else
            {
                return null;
            }
        }

        public static void db_Delete_Record(int id)
        {
            string sSQL = "Delete FROM Artists WHERE [Id] Like '" + id + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }

        public static void db_Delete_All()
        {
            string Ssql = "Delete FROM Artists";
            DatabaseHandler.Execute_SQL(Ssql);
        }
    }
}
