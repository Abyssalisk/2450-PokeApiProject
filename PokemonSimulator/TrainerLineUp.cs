using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    class TrainerLineUp
    {
        int userID;
        string TrainerName;
        MySqlConnection con;
        public PokemonTrainer ghostTrainer;
        public Boolean loopStuck=false;

        public TrainerLineUp(int userID, string trainerName, MySqlConnection con)
        {
            this.userID = userID;
            this.TrainerName = trainerName;
            this.con = con;
            this.ghostTrainer = new PokemonTrainer(userID,TrainerName);

            if(CheckForLineUp()==true)
            {
                Console.WriteLine("A Lineup has been found!");
                loadLineup();
            }else
            {
                Console.WriteLine("No lineup found!");
                newLineupChoice();
            }
        }

        private Boolean CheckForLineUp()
        {
            string lookupLineup = "SELECT `UserID` FROM sql3346222.TrainerLineup WHERE(UserID = " + userID + ");";
            string returnedQuery = "";

            //opens new DB connection with MySql and pulls hashed password from userCredentials table
            con.Open();
            MySqlCommand query = new MySqlCommand(lookupLineup, con);
            MySqlDataReader rdr = query.ExecuteReader();

            //reading returned query
            while (rdr.Read())
            {
                returnedQuery = rdr[0].ToString();
            }
            rdr.Close();
            con.Close();

            if (returnedQuery == userID.ToString())
                return true;
            else
                return false;
        }

        private void loadLineup()
        {
            string getPokemon = "SELECT `Pokemon1`,`Pokemon2`,`Pokemon3`,`Pokemon4`,`Pokemon5`,`Pokemon6`" +
                " FROM sql3346222.TrainerLineup WHERE(UserID = " + userID + ");";
            string pokemon1 = "";
            string pokemon2 = "";
            string pokemon3 = "";
            string pokemon4 = "";
            string pokemon5 = "";
            string pokemon6 = "";

            //opens new DB connection with MySql and pulls hashed password from userCredentials table
            con.Open();
            MySqlCommand query = new MySqlCommand(getPokemon, con);
            MySqlDataReader rdr = query.ExecuteReader();

            //reading returned query
            while (rdr.Read())
            {
                pokemon1 = rdr[0].ToString();
                pokemon2 = rdr[1].ToString();
                pokemon3 = rdr[2].ToString();
                pokemon4 = rdr[3].ToString();
                pokemon5 = rdr[4].ToString();
                pokemon6 = rdr[5].ToString();
            }
            rdr.Close();
            con.Close();
            Console.WriteLine(pokemon1+" \n"+pokemon2+" \n"+pokemon3+" \n"+pokemon4+" \n"+pokemon5+" \n"+pokemon6);

            Console.WriteLine("Use this lineup? (y/n)");
            string choice = Console.ReadLine();
            
            if(choice.ToLower().Equals("y"))
            {
                ghostTrainer.setPokemon1(pokemon1);
                ghostTrainer.setPokemon1(pokemon2);
                ghostTrainer.setPokemon1(pokemon3);
                ghostTrainer.setPokemon1(pokemon4);
                ghostTrainer.setPokemon1(pokemon5);
                ghostTrainer.setPokemon1(pokemon6);
            }
            if (choice.ToLower().Equals("n"))
            {
                newLineupChoice();
            }
        }

        private void newLineupChoice()
        {
            Console.WriteLine("Make a new lineup? (y/n)");
            string choice = Console.ReadLine();
            if (choice.ToLower().Equals("y"))
            {
                loopStuck = false;
                var makeAlineup = new CreateLineUp(ghostTrainer,con);
            }
            if (choice.ToLower().Equals("n"))
            {
                loopStuck = true;
                return;
            }
        }
    }
}
