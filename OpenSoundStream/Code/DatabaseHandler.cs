using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;

namespace OpenSoundStream
{
    public class DatabaseHandler
    {
        /// <summary>
        /// Connect to Database
        /// </summary>
        /// <returns></returns>
        public static SqlConnection Get_DB_Connection()
        {
            //Db connection string
            string cn_String = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath + "\\_Data" + "\\localdb_OSS.mdf;Integrated Security=True";
            string toReplace = @"\bin\Debug";
            cn_String = cn_String.Replace(toReplace, "");

            //Connect to Db
            SqlConnection cn_connection = new SqlConnection(cn_String);
            if (cn_connection.State != ConnectionState.Open) cn_connection.Open();

            return cn_connection;
        }

        /// <summary>
        /// Get Tabels from database
        /// </summary>
        /// <param name="SQL_Text"></param>
        /// <returns></returns>
        public static DataTable Get_DataTable(string SQL_Text)
        {
            SqlConnection cn_connection = Get_DB_Connection();

            // Get Tabels
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(SQL_Text, cn_connection);
            adapter.Fill(table);

            Close_DB_Connection(cn_connection);

            return table;
        }

        /// <summary>
        /// Execute SQL Statement
        /// </summary>
        /// <param name="SQL_Text"></param>
        public static void Execute_SQL(string SQL_Text)
        {
            SqlConnection cn_connection = Get_DB_Connection();

            //Execute
            SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);
            cmd_Command.ExecuteNonQuery();


            Close_DB_Connection(cn_connection);
        }

        /// <summary>
        /// Close Database Connection
        /// </summary>
        /// <param name="cn_connection"></param>
        public static void Close_DB_Connection(SqlConnection cn_connection)
        {
            if (cn_connection.State != ConnectionState.Closed) cn_connection.Close();
        }
    }
}