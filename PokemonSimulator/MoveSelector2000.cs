using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using PokeAPI;

namespace PokemonSimulator
{
    class MoveSelector2000
    {
        public string Move1 {get;set;}
        public string Move2 { get; set; }
        public string Move3 { get; set; }
        public string Move4 { get; set; }

        public List<string> AvailbleMoves;
        string Name;
        public MoveSelector2000(string name)
        {
            AvailbleMoves = null;
            Move1 = null;
            Move2 = null;
            Move3 = null;
            Move4 = null;
            Name = name;
            DisplayMoves();
            ChoseMove();
        }
        
        public void DisplayMoves()
        {
            Console.WriteLine("Dispaying list of moves......");
            Task<PokeAPI.Pokemon> p = DataFetcher.GetNamedApiObject<PokeAPI.Pokemon>(Name);
            var returnedmoves = p.Result.Moves;

            foreach (PokeAPI.PokemonMove element in returnedmoves)
            {
                Console.WriteLine(element.Move.Name.ToString());
                AvailbleMoves.Add(element.Move.Name.ToString());
            }

        }

        public void ChoseMove()
        {
            int moveconter = 0;
            Boolean movevalid = false;
            while (movevalid==false&&moveconter<=4)
            {
                Console.WriteLine("Type the name of the move you wish to add:");
                string movechoice = Console.ReadLine();
                if (AvailbleMoves.Contains(movechoice))
                {
                    StoreMove(movechoice);
                    Console.WriteLine(movechoice+" added!");

                    moveconter++;
                }
                else
                {
                    Console.WriteLine("move not found");
                }
            }
        }

        public void StoreMove(string move)
        {
            if(Move1==null)
            {
                Move1 = move;
                return;
            }
            else if (Move2 == null)
            {
                Move2 = move;
                return;
            }
            else if (Move3 == null)
            {
                Move3 = move;
                return;
            }
            else if (Move4 == null)
            {
                Move4 = move;
                return;
            }
        }

    }
}
