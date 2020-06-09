using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;

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
            //Console.WriteLine(apip.ToString().Length + "\n\n\n");
            //Pokemon p = new Pokemon(apip);
            //Console.WriteLine(p.ToString() + "\n\n\n");
            //Grand.HuffmanSerialize("AAAAbbbCCd");
            Regex yes = new Regex("^[y|Ye|Es|S]|[y|Y]");
            Regex no = new Regex("^[n|No|O]|[n|N]");
            bool playing = true;
            while (playing)
            {
                APIPokemonBlueprint enemyApi = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString());
                APIPokemonBlueprint yoursApi = APIPokemonBlueprint.GetPokemonBlueprint(Grand.rand.Next(1, 785).ToString());
                Pokemon enemy = new Pokemon(enemyApi);
                Pokemon yours = new Pokemon(yoursApi);
                Console.WriteLine($"Welcome to the battle! Your pokemon is {yours.Nickname}, " +
                    $"and your enemy's is {enemy.Nickname}.\n");
                do
                {
                    //Not necessary to see if you're still alive to attack because the while checks it.
                    enemy.ModifyHealth(-yours.ActingAttack);
                    Console.WriteLine($"*SMACK*; your enemy's pokemon has been hit for " +
                        $"{yours.ActingAttack} damage! Remaining HP on your enemy's pokemon: " +
                        $"{enemy.ActingHP}");
                    if (enemy.IsAlive)
                    {
                        yours.ModifyHealth(-enemy.ActingAttack);
                        Console.WriteLine($"*SMACK*; your pokemon has been hit for " +
                            $"{enemy.ActingAttack} damage! Remaining HP on your pokemon: " +
                            $"{yours.ActingHP}\n");
                    }
                } while (enemy.IsAlive && yours.IsAlive);
                Console.Write($"Battle over! Winner is ");
                Console.ForegroundColor = (yours.ActingHP > 0 ? ConsoleColor.Green : ConsoleColor.Red); 
                Console.Write($"{(yours.ActingHP > 0 ? "you" : "enemy")}\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Would you like to play again? (Y/N)");
                bool validInput = false;
                do
                {
                    string input = Console.ReadLine().Trim();
                    if (yes.IsMatch(input))
                    {
                        validInput = true;
                        continue;
                    }
                    else if (no.IsMatch(input))
                    {
                        playing = false;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Hey! That input isn't valid, say yes or no.");
                    }
                } while (!validInput);
            }
        }
    }
}
