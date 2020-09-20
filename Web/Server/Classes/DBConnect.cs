using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;

namespace Web.Server.Classes
{
    public static class DBConnect
    {
        private static SqlConnectionStringBuilder _builder { get; set; }

        private static void Build()
        {
            _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = "pokemanz.database.windows.net";
            _builder.UserID = "srosy";
            _builder.Password = "GaviSpe64!";
            _builder.InitialCatalog = "Pokemanz";
        }

        public static SqlConnection BuildSqlConnection()
        {
            Build();
            return new SqlConnection(_builder.ConnectionString);
        }

        public static SqlDataReader GetReader(string query)
        {
            if (string.IsNullOrEmpty(query))
                return null;

            Build();

            try
            {
                using (SqlConnection conn = new SqlConnection(_builder.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    conn.Open();
                    
                    return command.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing query: {ex.Message}");
            }
        }

        public static string GetSingleString(string query)
        {
            if (string.IsNullOrEmpty(query))
                return null;

            Build();

            try
            {
                var _string = string.Empty;
                using (SqlConnection conn = new SqlConnection(_builder.ConnectionString))
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    conn.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _string = reader[0].ToString();
                        }
                    }
                }

                return _string;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing query: {ex.Message}");
            }
        }

        public static DataSet GetDataSet(string query)
        {
            if (string.IsNullOrEmpty(query))
                return null;

            Build();

            try
            {
                using (SqlConnection conn = new SqlConnection(_builder.ConnectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing query: {ex.Message}");
            }
        }

        public static int ExecuteScalar(string query)
        {
            if (string.IsNullOrEmpty(query))
                return 0;

            Build();

            try
            {
                using (SqlConnection conn = new SqlConnection(_builder.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    var num = Convert.ToInt32(cmd.ExecuteScalar());
                    return num;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing query: {ex.Message}");
            }
        }

        public static bool ExecuteNonQuery(string query)
        {
            var numRowsAffected = 0;

            if (string.IsNullOrEmpty(query))
                return false;

            Build();

            try
            {
                using (SqlConnection conn = new SqlConnection(_builder.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    numRowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing query: {ex.Message}");
            }

            return numRowsAffected > 0;
        }
    }
}
