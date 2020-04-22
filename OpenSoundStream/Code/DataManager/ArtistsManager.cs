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
            DataTable tbl = new DataTable();
            if(artist.id != null) 
            {
                tbl = db_Get_Record(artist.id);
            }
            //</ find record >

            if (tbl.Rows.Count == 0)
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

        public static DataTable db_Get_Record(int? id)
        {
            string sSQL = "SELECT TOP 1 * FROM Artists WHERE [Id] Like '" + id + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            return tbl;
        }

        public static void db_Delete_Record(int id)
        {
            string sSQL = "Delete FROM Artists WHERE [Id] Like '" + id + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }
    }
}
