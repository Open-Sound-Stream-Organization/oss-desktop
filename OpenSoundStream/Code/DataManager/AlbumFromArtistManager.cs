using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    public class AlbumFromArtistManager
    {
        public static void db_Add_Update_Record(int albumId, int artistId)
        {
            //< find record >
            DataTable tbl = new DataTable();
            tbl = db_Get_Record(albumId, artistId);
            //</ find record >

            if (tbl.Rows.Count == 0)
            {
                string sql_Add = "INSERT INTO AlbumFromArtist ([albumId], [artistId]) VALUES('" + albumId + "','" + artistId + "')";  
                DatabaseHandler.Execute_SQL(sql_Add);
            }
        }

        public static DataTable db_Get_Record(int albumId, int artistId)
        {
            string sSQL = "SELECT TOP 1 * FROM AlbumFromArtist WHERE [albumId] Like '" + albumId + "' AND [artistId] Like '" + artistId + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            return tbl;
        }

        public static void db_Delete_Record(int albumId, int artistId)
        {
            string sSQL = "Delete FROM AlbumFromArtist WHERE [albumId] Like '" + albumId + "' AND [artistId] Like '" + artistId + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }

        public static void db_Delete_All()
        {
            string Ssql = "Delete FROM AlbumFromArtist";
            DatabaseHandler.Execute_SQL(Ssql);
        }
    }
}
