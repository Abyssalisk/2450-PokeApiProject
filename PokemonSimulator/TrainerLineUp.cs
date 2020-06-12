using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    public class TrainerLineUp
    {
        public int UserID { get; set; }
        public string TrainerName { get; set; }
        public MySqlConnection Con { get; set; }
        public PokemonTrainer GhostTrainer { get; set; }
        public Boolean LoopStuck = false;

        public TrainerLineUp(int userID, string trainerName, MySqlConnection con)
        {
            UserID = userID;
            TrainerName = trainerName;
            Con = con;
            GhostTrainer = new PokemonTrainer() { UserId = UserID, TrainerName = trainerName, Pokemon = new List<string>() };

            if (CheckForLineUp() == true)
            {
                Console.WriteLine("A Lineup has been found!");
                LoadLineup();
            }
            else
            {
                Console.WriteLine("No lineup found!");
                NewLineupChoice();
            }
        }

        public bool CheckForLineUp()
        {
            string lookupLineup = "SELECT `UserID` FROM sql3346222.TrainerLineup WHERE(UserID = " + UserID + ");";
            string returnedQuery = "";

            //opens new DB Connection with MySql and pulls hashed password from userCredentials table
            Con.Open();
            MySqlCommand query = new MySqlCommand(lookupLineup, Con);
            MySqlDataReader reader = query.ExecuteReader();

            //reading returned query
            while (reader.Read())
            {
                returnedQuery = reader[0].ToString();
            }
            reader.Close();
            Con.Close();

            if (returnedQuery == UserID.ToString())
                return true;
            else
                return false;
        }

        private void LoadLineup()
        {
            var tempLineUp = new List<string>();
            var getPokemonQuery = "SELECT `Pokemon1`,`Pokemon2`,`Pokemon3`,`Pokemon4`,`Pokemon5`,`Pokemon6`" +
                " FROM sql3346222.TrainerLineup WHERE(UserID = " + UserID + ");";

            //Get pokemon from DB
            Con.Open();
            MySqlCommand cmd = new MySqlCommand(getPokemonQuery, Con);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                var index = 0;
                while (reader.Read())
                {
                    tempLineUp.Add(reader[index].ToString());
                    index++;
                }
            }
            Con.Close();

            //Print Pokemon
            tempLineUp.ForEach(p => { Console.WriteLine(p); });

            Console.WriteLine("Use this lineup? (y/n)");
            var choice = Console.ReadLine();

            if (choice.ToLower().Equals("y"))
            {
                GhostTrainer.Pokemon = tempLineUp;
            }
            if (choice.ToLower().Equals("n"))
            {
                NewLineupChoice();
            }
        }

        private void NewLineupChoice()
        {
            Console.WriteLine("Make a new lineup? (y/n)");
            var choice = Console.ReadLine();
            if (choice.ToLower().Equals("y"))
            {
                LoopStuck = false;
                var makeAlineup = new CreateLineUp(GhostTrainer, Con);
            }
            if (choice.ToLower().Equals("n"))
            {
                LoopStuck = true;
                return;
            }
        }
    }
}
