using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    class PlayAgain
    {
        public PlayAgain()
        {

        }

        public bool Decsision()
        {
            while (true)
            {
                Console.WriteLine("Would you like to play again? (Y/N)");
                string choice = Console.ReadLine().Trim();

                if (Grand.yes.IsMatch(choice))
                    return true;
                else if (Grand.no.IsMatch(choice))
                    return false;
                else
                    Console.WriteLine("Invalid choice type y or n!");
            }
        }
    }
}
