using System;
using System.Collections.Generic;
using System.Linq;
using PokeAPI;

namespace PokemonSimulator
{
    public class Program
    {
        static void Main(string[] args)
        {
            var loginStart = new UserAuthAndLogin();
            System.GC.Collect();

            var currenttrainer = new Trainer();
            currenttrainer.UserId = loginStart.UserID;
            currenttrainer.TrainerName = loginStart.TrainerName;

            var lineup = new TrainerLineUp(currenttrainer.UserId, currenttrainer.TrainerName, loginStart.Connection.myConnection);

            while(lineup.LoopStuck == true)
            {
                Console.WriteLine("Would you like to leave? type 'exit'\nOr press any key to continue");
                string exitChoice = Console.ReadLine();

                if (exitChoice.ToLower().Equals("exit"))
                {
                    System.Environment.Exit(0);
                }
                else
                {
                    loginStart = new UserAuthAndLogin();
                    System.GC.Collect();
                    currenttrainer = new Trainer();
                    currenttrainer.UserId = loginStart.UserID;
                    currenttrainer.TrainerName =loginStart.TrainerName;
                    lineup = new TrainerLineUp(currenttrainer.UserId, currenttrainer.TrainerName, loginStart.Connection.myConnection);
                }
            }

            currenttrainer = lineup.GhostTrainer;
            System.GC.Collect();

            Console.WriteLine("It's time to Battle!");
        }

    }
}
