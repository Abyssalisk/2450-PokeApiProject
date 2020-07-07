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
        public Trainer GhostTrainer { get; set; }
        public Boolean LoopStuck = false;
        public Boolean HasLineup = false;

        public TrainerLineUp(int userID, string trainerName, MySqlConnection con, bool getEliteTable = false)
        {
            UserID = userID;
            TrainerName = trainerName;
            Con = con;
            GhostTrainer = new Trainer() { UserId = UserID, TrainerName = trainerName, Pokemon = new List<Pokemon>() };

            if (CheckForLineUp() == true)
            {
                Console.WriteLine("A Lineup has been found!");
                HasLineup = true;
                LoadLineup();
            }
            else
            {
                Console.WriteLine("No lineup found!");
                HasLineup = false;
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

        public void LoadLineup()
        {
            var tempLineUp = new List<Pokemon>();
            var getPokemonQuery = "SELECT `Pokemon1`,`Pokemon2`,`Pokemon3`,`Pokemon4`,`Pokemon5`,`Pokemon6`" +
                " FROM sql3346222.TrainerLineup WHERE(UserID = " + UserID + ");";

            //Get pokemon from DB
            Con.Open();
            MySqlCommand cmd = new MySqlCommand(getPokemonQuery, Con);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < 6; i++)
                    {
                         Console.WriteLine( reader[i].ToString() );
                    }
                }
            }
            Con.Close();
            while (true)
            {
                Console.WriteLine("Use this lineup? (y/n)");
                var choice = Console.ReadLine();

                if (choice.ToLower().Equals("y"))
                {
                    var loader = new LoadPokemonFromDB(UserID, Con);
                    tempLineUp = loader.LoadedLineUp;
                    GhostTrainer.Pokemon = tempLineUp;
                    break;
                }
                else if (choice.ToLower().Equals("n"))
                {
                    NewLineupChoice();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice !");
                }
            }
        }

        private void NewLineupChoice()
        {
            while (true)
            {
                Console.WriteLine("Make a new lineup? (y/n)");
                var choice = Console.ReadLine();
                if (choice.ToLower().Equals("y"))
                {
                    LoopStuck = false;
                    var makeAlineup = new CreateLineUpIO(GhostTrainer, Con, HasLineup);
                    HasLineup = false;
                    return;
                }
                else if (choice.ToLower().Equals("n"))
                {
                    LoopStuck = true;
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid Choice! ");
                }
            }
        }
    }
}