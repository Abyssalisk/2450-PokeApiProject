using System;
using System.Collections.Generic;
using System.Linq;
using PokeAPI;
using System.Threading.Tasks;

namespace PokemonSimulator
{
    public class Program
    {
        static void Main(string[] args)
        {
            UserAuthAndLogin loginStart = new UserAuthAndLogin();
            System.GC.Collect();

            Trainer CurrentTrainer = new Trainer();
            CurrentTrainer.UserId = loginStart.UserID;
            CurrentTrainer.TrainerName = loginStart.TrainerName;

            var Lineup = new TrainerLineUp(CurrentTrainer.UserId, CurrentTrainer.TrainerName, loginStart.Connection.myConnection);


            while (lineup.LoopStuck == true)
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
                    currentTrainer = new Trainer();
                    currentTrainer.UserId = loginStart.UserID;
                    currentTrainer.TrainerName = loginStart.TrainerName;

                    lineup = new TrainerLineUp(currentTrainer.UserId, currentTrainer.TrainerName, loginStart.Connection.myConnection);
                }
            }
            currentTrainer = lineup.GhostTrainer;
            Trainer dupe = new Trainer(currentTrainer);
            System.GC.Collect();


            Console.WriteLine("Let's Battle! ");
            Trainer EliteTrainerOne = new Trainer();
            LoadOpponent EliteLoader = new LoadOpponent(1,loginStart.Connection.myConnection);

            EliteTrainerOne.Pokemon = EliteLoader.OpponentLineUp;
            EliteTrainerOne.TrainerName = EliteLoader.OpponentName;

            GameMockup.GameEngine(CurrentTrainer,EliteTrainerOne);
        }
       

    }
}