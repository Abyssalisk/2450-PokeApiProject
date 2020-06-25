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
                Console.WriteLine("Would you like to play again? (y/n)");
                string choice = Console.ReadLine();

                if (choice.ToLower() == "y")
                    return true;
                else if (choice.ToLower() == "n")
                    return false;
                else
                    Console.WriteLine("Invalid choice type y or n!");
            }
        }
    }
}
