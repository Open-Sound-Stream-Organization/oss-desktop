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
        /// <summary>
        /// Add or Update a playlist
        /// </summary>
        /// <param name="pl"></param>
        public static void db_Add_Update_Record(Playlist pl)
        {
            //< correct>
            pl.name = pl.name.Replace("'", "''");
            pl.resource_uri = pl.resource_uri.Replace("'", "''");
            //</ correct>

            //< find record >
            Playlist dbRecord = null;
            if(pl.id != null)
            {
                dbRecord = db_Get_Record(pl.id);
            }
            //</ find record >

            if (dbRecord == null)
            {
                string sql_Add = "INSERT INTO Playlists ([id], [name], [resource_uri]) VALUES('" + pl.id + "','" + pl.name + "','" + pl.resource_uri + "')";
                DatabaseHandler.Execute_SQL(sql_Add);
            }
            else
            {
                string sql_Update = "UPDATE Playlists SET [name] = '" + pl.name + "', [resource_uri] = '" + pl.resource_uri + "' WHERE Id = " + pl.id;
                DatabaseHandler.Execute_SQL(sql_Update);
            }
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public static List<Playlist> db_GetAllPlaylists()
        {
            string sSQL = "SELECT * FROM Playlists";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            if (tbl.Rows.Count > 0)
            {
                List<Playlist> playlists = new List<Playlist>();
                foreach (DataRow row in tbl.Rows)
                {
                    Playlist playlist = new Playlist(row["name"].ToString());
                    playlist.id = Convert.ToInt32(row["id"].ToString());
                    playlist.resource_uri = row["resource_uri"].ToString();
                    playlist.Tracks = TrackInPlaylistManager.GetTracksFromPlaylist(playlist.id);
                    
                    playlists.Add(playlist);
                }

                return playlists;
            }
            else
            {
                return new List<Playlist>();
            }
        }

        /// <summary>
        /// Find record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Playlist db_Get_Record(int? id)
        {
            string sSQL = "SELECT TOP 1 * FROM Playlists WHERE [Id] Like '" + id + "'";
            DataTable tbl = DatabaseHandler.Get_DataTable(sSQL);

            if (tbl.Rows.Count == 1)
            {
                DataRow row = tbl.Rows[0];
                Playlist playlist = new Playlist(row["name"].ToString());
                playlist.id = Convert.ToInt32(row["id"].ToString());
                playlist.resource_uri = row["resource_uri"].ToString();
                return playlist;
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
        public static void db_Delete_Record(int? id)
        {
            string sSQL = "Delete FROM Playlists WHERE [Id] Like '" + id + "'";
            DatabaseHandler.Execute_SQL(sSQL);
        }

        /// <summary>
        /// Delete all record
        /// </summary>
        public static void db_Delete_All()
        {
            string Ssql = "Delete FROM Playlists";
            DatabaseHandler.Execute_SQL(Ssql);
        }
    }
}
