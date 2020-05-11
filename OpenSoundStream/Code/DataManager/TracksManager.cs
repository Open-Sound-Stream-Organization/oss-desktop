using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.Code.DataManager
{
    class TracksManager
    {
        /// <summary>
        /// Add or Update a track
        /// </summary>
        /// <param name="track"></param>
        public static void db_Add_Update_Record(Track track)
        {
            //< correct>
            track.title = track.title.Replace("'", "''");
            track.resource_uri = track.resource_uri.Replace("'", "''");
            track.audio = track.audio.Replace("'", "''");
            track.album = track.album.Replace("'", "''");
            if(track.mbid != null)
            {
                track.mbid = track.mbid.Replace("'", "''");
            }
            //</ correct>

            Track dbRecord = null;
            //< find record >
            if (track.id != null)
            {
                dbRecord = db_Get_Record(track.id);
            }
            //</ find record >

       
            if (dbRecord == null)
            {
                string sql_Add = "INSERT INTO Tracks ([id], [title], [resource_uri], [albumId], [audio], [mbid]) VALUES('" + track.id + "','" + track.title + "','" + track.resource_uri + "','" + track.album + "','" + track.audio + "','" + track.mbid + "')";
                DatabaseHandler.Execute_SQL(sql_Add);
            }
            else
            {
                string sql_Update = "UPDATE Tracks SET [title] = '" + track.title + "', [resource_uri] = '" + track.resource_uri + "', [albumId] = '" + track.album + "', [audio] = '" + track.audio + "', [mbid] = '" + track.mbid + "' WHERE Id = " + track.id;
                DatabaseHandler.Execute_SQL(sql_Update);
            }
        }

        /// <summary>
        /// Only get records with an audio file
        /// </summary>
        /// <returns></returns>
        public static List<Track> db_GetAllTracks()
        {
            string sSQL = "SELECT * FROM Tracks";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            if (tbl.Rows.Count > 0)
            {
                List<Track> tracks = new List<Track>();
                foreach (DataRow row in tbl.Rows)
                {
                    Track track = new Track(row["title"].ToString(), new Uri(@"file:///" + row["audio"].ToString()));
                    track.id = Convert.ToInt32(row["id"].ToString());
                    track.mbid = row["mbid"].ToString();
                    track.audio = row["audio"].ToString();
                    track.album = row["albumId"].ToString();
                    track.resource_uri = row["resource_uri"].ToString();
                    tracks.Add(track);
                }

                List<Track> Tracks = new List<Track>();
                foreach (Track Track in tracks)
                {
                    if (File.Exists(Track.audio))
                    {
                        Tracks.Add(Track);
                    }
                }

                return Tracks;
            }
            else
            {
                return new List<Track>();
            }
        }

        /// <summary>
        /// Find record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Track db_Get_Record(int? id)
        {
            string sSQL = "SELECT TOP 1 * FROM Tracks WHERE [Id] Like '" + id + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            if (tbl.Rows.Count == 1)
            {
                DataRow row = tbl.Rows[0];
                Track track = new Track(row["title"].ToString(), new Uri(@"file:///" + row["audio"].ToString()));
                track.id = Convert.ToInt32(row["id"].ToString());
                track.mbid = row["mbid"].ToString();
                track.audio = row["audio"].ToString();
                track.album = row["albumId"].ToString();
                track.resource_uri = row["resource_uri"].ToString();
                return track;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="id"></param>
        public static void db_Delete_Record(int id)
        {
            string sSQL = "Delete FROM Tracks WHERE [Id] Like '" + id + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }

        /// <summary>
        /// Delete all records
        /// </summary>
        public static void db_Delete_All() 
        {
            string Ssql = "Delete FROM Tracks";
            DatabaseHandler.Execute_SQL(Ssql);
        }
    }
}
