using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    public class TrackFromArtistManager
    {
        /// <summary>
        /// Add a new Relation (track from artist)
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="artistId"></param>
        public static void db_Add_Update_Record(int trackId, int artistId)
        {
            //< find record >
            DataTable tbl = new DataTable();
            tbl = db_Get_Record(trackId, artistId);
            //</ find record >

            if (tbl.Rows.Count == 0)
            {
                string sql_Add = "INSERT INTO TrackFromArtist ([trackId], [artistId]) VALUES('" + trackId + "','" + artistId + "')";
                DatabaseHandler.Execute_SQL(sql_Add);
            }
        }

        /// <summary>
        /// Find record
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="artistId"></param>
        /// <returns></returns>
        public static DataTable db_Get_Record(int trackId, int artistId)
        {
            string sSQL = "SELECT TOP 1 * FROM TrackFromArtist WHERE [trackId] Like '" + trackId + "' AND [artistId] Like '" + artistId + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            return tbl;
        }

        /// <summary>
        /// delete record
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="artistId"></param>
        public static void db_Delete_Record(int trackId, int artistId)
        {
            string sSQL = "Delete FROM TrackFromArtist WHERE [trackId] Like '" + trackId + "' AND [artistId] Like '" + artistId + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }

        /// <summary>
        /// Delete all records
        /// </summary>
        public static void db_Delete_All()
        {
            string Ssql = "Delete FROM TrackFromArtist";
            DatabaseHandler.Execute_SQL(Ssql);
        }
    }
}
