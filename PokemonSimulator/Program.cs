using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System.IO;

namespace PokemonSimulator
{
    /// <summary>
    /// Author: Samuel Gardner
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            /*const string huffTest = "Lorem ipsum dolor sit amet means roughly " +
                "\'Lorem very pain let it be carrots\'. Interesting, Huh? []I[]Like[]Brackets[]" +
                "Very[]Much[] and you can use them as the letter O, but boxy. H[]wdy there," +
                "my []ctopus like fr[][]t l[][]ps.";*/
            //const string connectionString = "";
            //MySqlConnection server = new MySqlConnection(connectionString);
            //server.Open();
            //string userID = Console.ReadLine().Trim();
            //MySqlCommand command = new MySqlCommand($"SELECT USER_ID FROM USERS WHERE USERS.USER_ID = {userID}", server);
            //MySqlDataReader reader = command.ExecuteReader();
            //command.CommandText = $"SELECT * {reader.GetFieldValue<int>(0)}";

            APIPokemonBlueprint apip = APIPokemonBlueprint.GetPokemonBlueprint("crobat");
            //Grand.HuffmanSerialize(apip.ToString());

            //JsonSerializer serializer = new JsonSerializer();
            //serializer.NullValueHandling = NullValueHandling.Ignore;

            //MemoryStream ms = new MemoryStream();
            //using (StreamWriter sw = new StreamWriter(ms))
            //using (BsonWriter writer = new BsonWriter(sw))
            //{
            //    serializer.Serialize(writer, apip);                
            //    Console.WriteLine(ms.Length + "\n\n\n");
            //}
            Console.WriteLine(apip.ToString().Length + "\n\n\n");
            Pokemon p = new Pokemon(apip);
            Console.WriteLine(p.ToString() + "\n\n\n");
            //Grand.HuffmanSerialize("AAAAbbbCCd");
        }
    }
}
