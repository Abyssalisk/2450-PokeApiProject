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
                Console.WriteLine("Dispaying list of moves......");
                Task<PokeAPI.Pokemon> p = DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(Name);
                var returnedmoves = p.Result.Moves;
                int linecountspace = 0;

                foreach (PokeAPI.PokemonMove element in returnedmoves)
                {

                    Task<PokeAPI.Move> m = DataFetcher.GetNamedApiObject<PokeAPI.Move>(element.Move.Name.ToString());

                    if (m.Result.Power != 0 && m.Result.Power.ToString() != "" && m.Result.Power != null)
                    {
                        string movestring = element.Move.Name.ToString();
                        Console.Write(movestring + ", ");
                        AvailbleMoves.Add(movestring);
                        linecountspace++;
                        if (linecountspace == 4)
                        {
                            Console.Write("\n");
                            linecountspace = 0;
                        }
                    }
                }
                return true;
            }catch(Exception e)
            {
                Console.WriteLine("Error most likely with pokemon name! \n"+e.Message);
                return false;
            }
        }

        public string ChoseMove()
        {
            while (true)
            {
                Console.WriteLine("Type the name of the move you wish to add:");
                string movechoice = Console.ReadLine();
                if (AvailbleMoves.Contains(movechoice))
                {
                    Console.WriteLine(movechoice+" added!");
                    return movechoice;
                }
                else
                {
                    Console.WriteLine("move not found");
                }
            }
        }


    }
}
