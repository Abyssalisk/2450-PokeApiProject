using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonSimulator
{
    class Program
    {
        static void Main(string[] args)
        {            
            APIPokemonBlueprint apip = APIPokemonBlueprint.GetPokemonBlueprint("crobat");
            Console.WriteLine(apip.ToString().Length + "\n\n\n");
            Pokemon p = new Pokemon(apip);
            Console.WriteLine(p.ToString() + "\n\n\n");
        }
    }
}
