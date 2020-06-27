using System;
using System.Collections.Generic;
using System.Linq;
using PokeAPI;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PokemonSimulator
{
    public class Program
    {
#if WINDOWS
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;
#endif
        [Obsolete]
        static void Main(string[] args)
        {
#if WINDOWS
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);
#endif
            var loginStart = new UserAuthAndLogin();
            //Commented this about because the application just started, there won't be much to clean up, and this brings a lot of overhead on call regardless of heap size.
            //One might describe the impact of this function as t(s) = 0.1s^2 + 100; where s is the size of the heap, and t(s) returns the time it takes to GC.
            //Since it's the start of the application, "s" would be very small in this case, but the GC takes 0.1*s^2 + 100 time to clear anyways. That + 100 at the end makes GC's take a long time regardless of heap size.
            //System.GC.Collect(); //Doing this a lot isn't a good idea, I get doing it in loading breaks, but the CLR takes care of this. Regardless, it's also not a huge deal in the console app.
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

                //Console.WriteLine("Let's Battle! "); //Derek, my code already includes this.

                for (int j = 5; j > 0; j--)
                {
                    Trainer EliteTrainer = new Trainer();
                    LoadOpponent EliteLoader = new LoadOpponent(j, loginStart.Connection.myConnection);

                    EliteTrainer.Pokemon = EliteLoader.OpponentLineUp;
                    EliteTrainer.TrainerName = EliteLoader.OpponentName;
                    
                    //Game 1 will have dumbest AI at intelligence = 35%, Game 5 at 75% (increments of 10%).
                    Win = GameMockup.GameEngine(CurrentTrainer, EliteTrainer, ((((double)j + 1d) / 10d) + 0.25d));

                    if (Win == false)
                    {
                        ScoreBoard ScoreAndRanking = new ScoreBoard(WinCounter, CurrentTrainer, loginStart.Connection.myConnection);
                        PlayAgain Again = new PlayAgain();
                        Exit = !Again.Decsision(); //Again.Decision() returns true if they want to play again, which is notted for whether or not they want to quit.
                        if (Exit)
                        {
                            //The person quit, so it should break the fighting loop. Right? if it starts over on the match they lost on, their pokemon's HP should be restored to the values they were before this match.
                            break;
                        }
                    }
                    else
                    {
                        WinCounter++;
                    }
                    System.GC.Collect(); //I think this is appropriate here, as this is when loading happens, and all those json and pokemon objects get cleaned up, freeing lots of memory.
                }
            }
            System.Environment.Exit(0);
        }

    }
}
