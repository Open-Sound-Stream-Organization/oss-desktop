using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    public class PlaylistsManager
    {
        public static void db_Add_Update_Record(Playlist pl)
        {
            //< correct>
            pl.name = pl.name.Replace("'", "''");
            pl.resource_uri = pl.resource_uri.Replace("'", "''");
            //</ correct>

            //< find record >
            DataTable tbl = new DataTable();
            if(pl.id != null)
            {
                tbl = db_Get_Record(pl.id);
            }
            //</ find record >

            if (tbl.Rows.Count == 0)
            {
                string sql_Add = "INSERT INTO Albums ([name], [resource_uri]) VALUES('" + pl.name + "','" + pl.resource_uri + "')";
                DatabaseHandler.Execute_SQL(sql_Add);
            }
            else
            {
                string sql_Update = "UPDATE Albums SET [name] = '" + pl.name + "', [resource_uri] = '" + pl.resource_uri + "' WHERE Id = " + pl.id;
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
