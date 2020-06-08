using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace PokemonSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "";
            MySqlConnection server = new MySqlConnection(connectionString);
            server.Open();
            string userID = Console.ReadLine().Trim();
            MySqlCommand command = new MySqlCommand($"SELECT USER_ID FROM USERS WHERE USERS.USER_ID = {userID}", server);
            MySqlDataReader reader = command.ExecuteReader();
            //command.CommandText = $"SELECT * {reader.GetFieldValue<int>(0)}";

            APIPokemonBlueprint apip = APIPokemonBlueprint.GetPokemonBlueprint("crobat");
            Console.WriteLine(apip.ToString().Length + "\n\n\n");
            Pokemon p = new Pokemon(apip);
            Console.WriteLine(p.ToString() + "\n\n\n");
        }
    }
}
