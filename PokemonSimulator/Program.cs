using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            //var loginStart = new ConsoleOutputInput();
            //System.GC.Collect();
            //var CurrentTrainer = new PokemonTrainer(loginStart.userID,loginStart.TrainerName);
            //var lineup = new TrainerLineUp(CurrentTrainer.userID,CurrentTrainer.TrainerName, loginStart.connection.myConnection);
            DBconnect con = new DBconnect();
            var CurrentTrainer = new PokemonTrainer() { TrainerName = "Misty", UserId = 10 };
            var lineup = new TrainerLineUp(CurrentTrainer.UserId, CurrentTrainer.TrainerName, con.myConnection);

            while(lineup.loopStuck == true)
            {
                Console.WriteLine("Would you like to leave? type 'exit'\nOr press any key to continue");
                string exitChoice = Console.ReadLine();

                if (exitChoice.ToLower().Equals("exit"))
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    //loginStart = new ConsoleOutputInput();
                    //System.GC.Collect();
                    //CurrentTrainer = new PokemonTrainer(loginStart.userID, loginStart.TrainerName);
                    //lineup = new TrainerLineUp(CurrentTrainer.userID, CurrentTrainer.TrainerName, loginStart.conn
                }
            }

            CurrentTrainer = lineup.ghostTrainer;
            System.GC.Collect();
        }
    }
}
