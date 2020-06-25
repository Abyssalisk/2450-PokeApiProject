using System;
using System.Collections.Generic;
using System.Linq;
using PokeAPI;
using System.Threading.Tasks;

namespace PokemonSimulator
{
    public class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            var loginStart = new UserAuthAndLogin();
            System.GC.Collect();
            Trainer CurrentTrainer = new Trainer();
            CurrentTrainer.UserId = loginStart.UserID;
            CurrentTrainer.TrainerName = loginStart.TrainerName;

            Boolean Win;
            Boolean Exit = false;
            while (Exit == false)
            {
                int WinCounter = 0;
                var Lineup = new TrainerLineUp(CurrentTrainer.UserId, CurrentTrainer.TrainerName, loginStart.Connection.myConnection);

                while (Lineup.LoopStuck == true)
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
                        CurrentTrainer = new Trainer();
                        CurrentTrainer.UserId = loginStart.UserID;
                        CurrentTrainer.TrainerName = loginStart.TrainerName;

                        Lineup = new TrainerLineUp(CurrentTrainer.UserId, CurrentTrainer.TrainerName, loginStart.Connection.myConnection);
                    }
                }

                CurrentTrainer = Lineup.GhostTrainer;
                System.GC.Collect();

                Console.WriteLine("Let's Battle! ");

                for(int j = 5;j>0;j--)
                {
                    Trainer EliteTrainer = new Trainer();
                    LoadOpponent EliteLoader = new LoadOpponent(j, loginStart.Connection.myConnection);

                    EliteTrainer.Pokemon = EliteLoader.OpponentLineUp;
                    EliteTrainer.TrainerName = EliteLoader.OpponentName;

                    Win = GameMockup.GameEngine(CurrentTrainer, EliteTrainer);

                    if (Win == false)
                    {
                        ScoreBoard ScoreAndRanking = new ScoreBoard(WinCounter,CurrentTrainer,loginStart.Connection.myConnection);
                        PlayAgain Again = new PlayAgain();
                        Exit = Again.Decsision();
                    }
                    else
                    {
                        WinCounter++;
                    }
                    System.GC.Collect();
                }
            }
            System.Environment.Exit(0);
        }
       
    }
}
