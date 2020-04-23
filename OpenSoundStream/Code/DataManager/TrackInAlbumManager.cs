using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    public class TrackInAlbumManager
    {
        public static void db_Add_Update_Record(int trackId, int albumId)
        {
            //< find record >
            DataTable tbl = new DataTable();
            tbl = db_Get_Record(trackId, albumId);
            //</ find record >

            if (tbl.Rows.Count == 0)
            {
                string sql_Add = "INSERT INTO TrackInAlbum ([trackId], [albumId]) VALUES('" + trackId + "','" + albumId + "')";
                DatabaseHandler.Execute_SQL(sql_Add);
            }
        }

        public static DataTable db_Get_Record(int trackId, int albumId)
        {
            string sSQL = "SELECT TOP 1 * FROM TrackInAlbum WHERE [trackId] Like '" + trackId + "' AND [albumId] Like '" + albumId + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            return tbl;
        }

        public static void db_Delete_Record(int trackId, int albumId)
        {
            string sSQL = "Delete FROM TrackInAlbum WHERE [trackId] Like '" + trackId + "' AND [albumId] Like '" + albumId + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }
    }
}
