using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    public class TrackInPlaylistManager
    {
        public static void db_Add_Update_Record(int trackId, int playlistId)
        {
            //< find record >
            DataTable tbl = new DataTable();
            tbl = db_Get_Record(trackId, playlistId);
            //</ find record >

            if (tbl.Rows.Count == 0)
            {
                string sql_Add = "INSERT INTO TrackInPlaylist ([trackId], [playlistId]) VALUES('" + trackId + "','" + playlistId + "')";
                DatabaseHandler.Execute_SQL(sql_Add);
            }
        }

        public static DataTable db_Get_Record(int trackId, int playlistId)
        {
            string sSQL = "SELECT TOP 1 * FROM TrackInPlaylist WHERE [trackId] Like '" + trackId + "' AND [playlistId] Like '" + playlistId + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            return tbl;
        }

        public static void db_Delete_Record(int trackId, int playlistId)
        {
            string sSQL = "Delete FROM TrackInPlaylist WHERE [trackId] Like '" + trackId + "' AND [playlistId] Like '" + playlistId + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }
    }
}
