using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonSimulator
{
    class Program
    {
        static void Main(string[] args)
        {            
            Console.WriteLine(APIPokemonBlueprint.GetPokemonBlueprint("ditto").ToString());
        }
    }
}
