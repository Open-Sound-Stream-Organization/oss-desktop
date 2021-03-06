﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    public class TrackInPlaylistManager
    {
        /// <summary>
        /// Add a new Relation (track and playlist)
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="playlistId"></param>
        public static void db_Add_Update_Record(int? trackId, int? playlistId)
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

        /// <summary>
        /// Get all records
        /// </summary>
        /// <param name="playlistId"></param>
        /// <returns></returns>
        public static LinkedList<Track> GetTracksFromPlaylist(int? playlistId)
        {
            string sSQL = "SELECT * FROM TrackInPlaylist WHERE [playlistId] Like '" + playlistId + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            LinkedList<Track> Tracks = new LinkedList<Track>();
            foreach(DataRow row in tbl.Rows)
            {
                Track track = TracksManager.db_Get_Record(Convert.ToInt32(row["trackId"].ToString()));

                if (File.Exists(track.audio))
                {
                    Tracks.AddLast(track);
                }
            }

            return Tracks;
        }

        /// <summary>
        /// Find record
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="playlistId"></param>
        /// <returns></returns>
        public static DataTable db_Get_Record(int? trackId, int? playlistId)
        {
            string sSQL = "SELECT TOP 1 * FROM TrackInPlaylist WHERE [trackId] Like '" + trackId + "' AND [playlistId] Like '" + playlistId + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            return tbl;
        }

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="playlistId"></param>
        public static void db_Delete_Record(int? trackId, int? playlistId)
        {
            string sSQL = "Delete FROM TrackInPlaylist WHERE [trackId] Like '" + trackId + "' AND [playlistId] Like '" + playlistId + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }

        /// <summary>
        /// Delete all records
        /// </summary>
        public static void db_Delete_All()
        {
            string Ssql = "Delete FROM TrackInPlaylist";
            DatabaseHandler.Execute_SQL(Ssql);
        }
    }
}
