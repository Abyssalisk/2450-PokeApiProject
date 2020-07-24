using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using Web.Client.Pages;
//using System.Configuration;

namespace Web.Server.Classes
{
    internal class DBInterface
    {
        private MySqlConnection myConnection;
        private MySqlConnection MyConnection
        {
            get
            {
                if (myConnection == null/* || myConnection.State != System.Data.ConnectionState.Open*/)
                {
                    myConnection = new MySqlConnection(GetRDSConnectionString());
                }
                return myConnection;
            }
        }
        private bool performingSequentialQuery = false;
        //public DBInterface()
        //{
        //    //using (MySqlConnection connect = new MySqlConnection())
        //    //{
        //    //    try
        //    //    {
        //    //        connect.ConnectionString = GetRDSConnectionString();
        //    //        connect.Open();
        //    //    }
        //    //    catch { }
        //    //    finally
        //    //    {
        //    //        if (connect != null || connect.State == System.Data.ConnectionState.Open)
        //    //            MyConnection = connect;
        //    //    }
        //    //}
        //}
        ~DBInterface()
        {
            MyConnection?.Close();
            MyConnection?.Dispose();
        }

        #region Utility
        #region Type Utilities
        public static string GetRDSConnectionString()
        {
            const string dbname = "sql3346222";

            if (string.IsNullOrEmpty(dbname)) return null;

            const string username = "sql3346222";
            const string password = "wTvU3pVa7f";
            const string hostname = "sql3.freemysqlhosting.net";
            const string port = "3306";
            const string connectionString = "Server=" + hostname + "; Port=" + port + "; Database=" + dbname + "; Uid=" + username + "; Pwd=" + password + ";";
            return connectionString;
        }
        #endregion

        #region Instance Utilities
        private const string usernameRegex = "^ +[\\w]|[_]+ +$"; //untested
        private const string passHashRegex = "^[^\0]{300}$"; //untested. Does that null terminate the string? lol I hope not.
        private const string noSqlCommentsRegex = "^[^[-]|[#]]$"; //untested

        /// <summary>
        /// Performs a sequential series of queries with this DBInterface. This is more efficent if you're doing two or more DB interactions in sequence.
        /// </summary>
        /// <param name="actions">List of parameter packed anonymous delegates containing sequential thiscalls to DB interface methods on this object.</param>
        public void SequentialQuery(params Action[] actions)
        {
            MyConnection.Open();
            performingSequentialQuery = true;
            foreach (Action a in actions)
            {
                a?.Invoke();
            }
            performingSequentialQuery = false;
            MyConnection.Close();
        }

        public void ExposeNonQuery(string query)
        {
            if (!performingSequentialQuery)
            {
                MyConnection.Open();
            }
            using (MySqlCommand command = new MySqlCommand(query))
            {
                command.Connection = MyConnection;
                command.ExecuteNonQuery();
            }
            if (!performingSequentialQuery)
            {
                MyConnection.Close();
            }
        }

        public MySqlDataReader ExposeQuery(string query, out Action disposeCallback)
        {
            if (!performingSequentialQuery)
            {
                MyConnection.Open();
            }
            MySqlDataReader reader = null;
            disposeCallback = () =>
            {
                reader?.Close();
                reader?.Dispose();
                if (!performingSequentialQuery)
                {
                    MyConnection.Close();
                }
            };
            using (MySqlCommand command = new MySqlCommand(query))
            {
                command.Connection = MyConnection;
                return command.ExecuteReader();
            }
        }

        //public static class DBInterfaceConformanceFail
        //{

        //}
        /// <summary>
        /// Inserts a new user into the Users table in the database. (For when a new user is created, it's assumed it's already been checked if a user with this name already exists).
        /// </summary>
        /// <param name="name">The unique name of the user.</param>
        /// <param name="encryptedPass">The hashed password for the user.</param>
        /// <param name="email">The users email. (By this point it is assumed to be validly formatted and verified to belong to the user).</param>
        /// <param name="nick">The nickname/tag of the user (Defaults to the name if not provided).</param>
        /// <param name="data">The data to associate with the user (DBData class).</param> //DBData currently not implemented.
        public void InsertDBcredentials(
            [RegularExpression(usernameRegex, ErrorMessage = "Error: Provided string for name contains non-alphanumerics. (Leading and trailing whitespace is fine)."/*, ErrorMessageResourceType = typeof(DBInterfaceConformanceFail)*/)] string name,
            [RegularExpression(passHashRegex)] string encryptedPass,
            /*[RegularExpression("")]*/ string email,
            [RegularExpression(usernameRegex, ErrorMessage = "Error: Provided string for nick contains non-alphanumerics. (Leading and trailing whitespace is fine)."/*, ErrorMessageResourceType = typeof(DBInterfaceConformanceFail)*/)] string nick = null,
            string data = null)
        {
#warning needs to be asynced.
            name = name.Trim();
            nick = nick.Trim() ?? name;
            if (!performingSequentialQuery)
            {
                MyConnection.Open();
            }
            using (MySqlCommand command = new MySqlCommand("INSERT INTO sql3346222.Users(TrainerName, TrainerTag, PassHash, Email, Data) VALUES(@Name, @Nick, @Pass, @Email, @Data);", MyConnection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Nick", nick);
                command.Parameters.AddWithValue("@Pass", encryptedPass);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Data", (object)data ?? (object)DBNull.Value);
                command.Connection = MyConnection;
                command.ExecuteNonQuery();
            }
            if (!performingSequentialQuery)
            {
                MyConnection.Close();
            }
            //string query = $"INSERT INTO sql3346222.userCredentials(TrainerName, Password, Email, HighScore) VALUES('{name}', '{encryptedPass}', '{email}', 0);";
        }

        public bool DoesNameExist([RegularExpression(usernameRegex, ErrorMessage = "Error: Provided string for name contains non-alphanumerics. (Leading and trailing whitespace is fine)."/*, ErrorMessageResourceType = typeof(DBInterfaceConformanceFail)*/)] string name)
        {
            if (!performingSequentialQuery)
            {
                MyConnection.Open();
            }
            bool result = false;
            using (MySqlCommand command = new MySqlCommand("SELECT sql3346222.Users.TrainerName FROM sql3346222.Users WHERE sql3346222.Users.TrainerName = @Name;"))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Connection = MyConnection;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    result = reader.HasRows;
                    reader.Close();
                }
            }
            if (!performingSequentialQuery)
            {
                MyConnection.Close();
            }
            return result;
        }

        /// <summary>
        /// Gets all of a users info with the given UserID.
        /// </summary>
        /// <param name="userID">The UserID to use to search for a users info.</param>
        /// <param name="trainerName">The user's trainer name if a user was found, null otherwise.</param>
        /// <param name="trainerTag">The user's trainer tag if a user was found, null otherwise.</param>
        /// <param name="passHash">The user's password hash if a user was found, null otherwise.</param>
        /// <param name="email">The user's email if a user was found, null otherwise.</param>
        /// <param name="data">The user's data if a user was found, null otherwise.</param>
        public void GetUserInfo(int userID, out string trainerName, out string trainerTag, out string passHash, out string email, out string data)
        {
            trainerName = null;
            trainerTag = null;
            passHash = null;
            email = null;
            data = null;
            if (!performingSequentialQuery)
            {
                MyConnection.Open();
            }
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM sql3346222.Users.TrainerName WHERE sql3346222.Users.UserID = @UID;"))
            {
                command.Parameters.AddWithValue("@UID", userID);
                command.Connection = MyConnection;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        bool oneAccount = true;
                        while (reader.Read())
                        {
                            if (!oneAccount)
                            {
                                reader.Close();
                                throw new ArgumentException("Error: the provided UserID had more than one match in the database. This shouldn't be possible. (This means somehow multiple users have the same UserID).");
                            }
                            else
                            {
                                oneAccount = false;
                            }
                            trainerName = reader.GetString(1);
                            trainerTag = reader.GetString(2);
                            passHash = reader.GetString(3);
                            email = reader.GetString(4);
                            data = reader.GetString(5);
                        }
                    }
                    else
                    {
                        reader.Close();
                        throw new ArgumentException("Error: no user was found in the database with the given UserID", "userID");
                    }
                    reader.Close();
                }
            }
            if (!performingSequentialQuery)
            {
                MyConnection.Close();
            }
        }

        public void ChangePassword(int userID, [RegularExpression(passHashRegex)] string newPassHash)
        {
            if (!performingSequentialQuery)
            {
                MyConnection.Open();
            }
            using (MySqlCommand command = new MySqlCommand("UPDATE Users SET PassHash = @NewPass WHERE UserID = @UID;"))
            {
                command.Parameters.AddWithValue("@UID", userID);
                command.Parameters.AddWithValue("@NewPass", newPassHash);
                command.Connection = MyConnection;
                command.ExecuteNonQuery();
            }
            if (!performingSequentialQuery)
            {
                MyConnection.Close();
            }
        }

        public string[] AllPokemonNames()
        {
            throw new NotImplementedException();
            string csvContent;
            return csvContent.Split(',', options: StringSplitOptions.RemoveEmptyEntries);
        }

        public bool LoginValidation([RegularExpression(usernameRegex, ErrorMessage = "Error: Provided string for name contains non-alphanumerics. (Leading and trailing whitespace is fine)."/*, ErrorMessageResourceType = typeof(DBInterfaceConformanceFail)*/)] string trainerName, [RegularExpression(passHashRegex)] string passHash)
        {
            if (!performingSequentialQuery)
            {
                MyConnection.Open();
            }
            string ret = null;
            using (MySqlCommand command = new MySqlCommand("SELECT sql3346222.Users.PassHash FROM sql3346222.Users WHERE sql3346222.Users.TrainerName = @Name;"))
            {
                command.Parameters.AddWithValue("@Name", trainerName);
                command.Connection = MyConnection;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        bool oneAccount = true;
                        while (reader.Read())
                        {
                            if (!oneAccount)
                            {
                                reader.Close();
                                throw new ArgumentException("Error: The provided trainer name had more than one account match. This shouldn't be possible. (Somehow there are multiple accounts with the same name).");
                            }
                            else
                            {
                                oneAccount = false;
                            }
                            ret = reader.GetString(0);
                        }
                    }
                    else
                    {
                        reader.Close();
                        throw new ArgumentException("Error: The provided trainer name had no matches in the database.", "trainerName");
                    }
                    reader.Close();
                }
            }
            if (!performingSequentialQuery)
            {
                MyConnection.Close();
            }
            return passHash == ret;
        }

        /// <summary>
        /// Looks up all user accounts associated with the given email.
        /// </summary>
        /// <param name="email">The email to lookup.</param>
        /// <returns>An array of the user id's of who's accounts matched the given email or null if no accounts associated with the given email were found.</returns>
        public int[] LookupUserByEmail(string email)
        {
            if (!performingSequentialQuery)
            {
                MyConnection.Open();
            }
            int[] result = null;
            using (MySqlCommand command = new MySqlCommand("SELECT sql3346222.Users.UserID FROM sql3346222.Users WHERE sql3346222.Users.Email = @Email;"))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Connection = MyConnection;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        List<int> stuff = new List<int>();
                        while (reader.Read())
                        {
                            stuff.Add(reader.GetInt32(0));
                        }
                        result = stuff.ToArray();
                    }
                    reader.Close();
                }
            }
            if (!performingSequentialQuery)
            {
                MyConnection.Close();
            }
            return result;
        }
        #endregion
        #endregion
    }
}