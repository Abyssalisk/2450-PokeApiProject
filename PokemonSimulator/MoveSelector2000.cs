using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using PokeAPI;

namespace PokemonSimulator
{
    public class MoveSelector2000
    {

        public List<string> AvailbleMoves;
        string Name;
        public MoveSelector2000(string name)
        {
            AvailbleMoves = new List<string>();
            Name = name;
        }

        public bool DisplayMoves()
        {
            try
            {
                Console.WriteLine("Displaying list of moves......");
                Task<PokeAPI.Pokemon> p = DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(Name);
                PokemonMove[] returnedmoves = p.Result.Moves;
                int linecountspace = 0;
                Task<PokeAPI.Move>[] moves = new Task<PokeAPI.Move>[returnedmoves.Length];
                for (int i = 0; i < returnedmoves.Length; i++)
                {
                    moves[i] = (DataFetcher.GetNamedApiObject<PokeAPI.Move>(returnedmoves[i].Move.Name.ToString()));
                }
                Task.WaitAll(moves);
                foreach (Task<PokeAPI.Move> e in moves)
                {
                    if (e.Result.Power != null && !string.IsNullOrEmpty(e.Result.Power.ToString()))
                    {
                        string movestring = e.Result.Name.ToString();
                        Console.Write(movestring + ", ");
                        AvailbleMoves.Add(movestring);
                        linecountspace++;
                        if (linecountspace == 4)
                        {
                            Console.Write("\n");
                            linecountspace = 0;
                        }
                    }
                    e.Dispose();
                }
                //foreach (PokeAPI.PokemonMove element in returnedmoves)
                //{

                //    Task<PokeAPI.Move> m = DataFetcher.GetNamedApiObject<PokeAPI.Move>(element.Move.Name.ToString());

                //    if (m.Result.Power != 0 && m.Result.Power.ToString() != "" && m.Result.Power != null)
                //    {
                //        string movestring = element.Move.Name.ToString();
                //        Console.Write(movestring + ", ");
                //        AvailbleMoves.Add(movestring);
                //        linecountspace++;
                //        if (linecountspace == 4)
                //        {
                //            Console.Write("\n");
                //            linecountspace = 0;
                //        }
                //    }
                //}
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error most likely with pokemon name! \n" + e.Message);
                return false;
            }
        }

        //Probably should be an IEnumerator
        public string ChoseMove()
        {
            while (true)
            {
                Console.WriteLine("\nType the name of the move you wish to add:");
                string movechoice = Console.ReadLine().Trim().ToLower();
                if (AvailbleMoves.Contains(movechoice))
                {
                    Console.WriteLine(movechoice + " added!");
                    return movechoice;
                }
                else
                {
                    Console.WriteLine($"Move \"{movechoice}\" not found. Try again.");
                }
            }
        }
    }
}