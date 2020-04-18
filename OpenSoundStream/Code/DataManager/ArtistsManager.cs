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
        public static void db_Add_Record(Artist artist)
        {
            //< correct>
            artist.Name = artist.Name.Replace("'", "''");
            //</ correct>

            string sql_Add = "INSERT INTO Artists ([name]) VALUES('" + artist.Name + "')";

            DatabaseHandler.Execute_SQL(sql_Add);
        }

        public static void db_Update_Record(int id, Artist artist)
        {
            //< correct>
            artist.Name = artist.Name.Replace("'", "''");
            //</ correct>

            string sql_Update = "UPDATE Artists SET [name] = '" + artist.Name + "' WHERE Id = " + id;
            DatabaseHandler.Execute_SQL(sql_Update);
        }

        public static DataTable db_Get_Record(int id)
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
