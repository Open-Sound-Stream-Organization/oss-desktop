﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace OpenSoundStream
{
    public class DatabaseHandler
    {
        public static SqlConnection Get_DB_Connection()
        {
            string cn_String = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath + "\\_Data" + "\\localdb_OSS.mdf;Integrated Security=True";
            SqlConnection cn_connection = new SqlConnection(cn_String);
            if (cn_connection.State != ConnectionState.Open) cn_connection.Open();

            return cn_connection;
        }


        public static DataTable Get_DataTable(string SQL_Text)
        {
            SqlConnection cn_connection = Get_DB_Connection();

            //< get Table >

            DataTable table = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(SQL_Text, cn_connection);

            adapter.Fill(table);


            Close_DB_Connection(cn_connection);

            //</ get Table >
            return table;
        }



        public static void Execute_SQL(string SQL_Text)
        {
            SqlConnection cn_connection = Get_DB_Connection();

            //< get Table >

            SqlCommand cmd_Command = new SqlCommand(SQL_Text, cn_connection);

            cmd_Command.ExecuteNonQuery();

            //</ get Table >

            Close_DB_Connection(cn_connection);
        }


        public static void Close_DB_Connection(SqlConnection cn_connection)
        {
            if (cn_connection.State != ConnectionState.Closed) cn_connection.Close();
        }
    }
}