using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PokemonSimulator
{
    struct APIPokemonBlueprint
    {
        public enum PokemonType : int
        {
            Bug =      0b0000000000000000000000000000001,
            Electric = 0b0000000000000000000000000000010,
            Fire =     0b0000000000000000000000000000100,
            Grass =    0b0000000000000000000000000001000,
            Normal =   0b0000000000000000000000000010000,
            Rock =     0b0000000000000000000000000100000,
            Flying =   0b0000000000000000000000001000000,
            Ground =   0b0000000000000000000000010000000,
            Poison =   0b0000000000000000000000100000000,
            Dragon =   0b0000000000000000000001000000000,
            Fighting = 0b0000000000000000000010000000000,
            Ghost =    0b0000000000000000000100000000000,
            Ice =      0b0000000000000000001000000000000,
            Psychic =  0b0000000000000000010000000000000,
            Water =    0b0000000000000000100000000000000,
            Fairy =    0b0000000000000001000000000000000,
            Dark =     0b0000000000000010000000000000000,
            Steel =    0b0000000000000100000000000000000
        }
        //Trying to avoid properties because I don't want unecessary calls.
        public readonly string name;
        public struct Attack
        {
            string name;
            
        }
        public static APIPokemonBlueprint Factory(string apiRequestID)
        {            
            RestClient client = new RestClient($"https://pokeapi.co/api/v2/move/{apiRequestID}/");
            //return new APIPokemonBlueprint();
        }
        private APIPokemonBlueprint(string name)
        {
            //do stuff.
            this.name = name;
        }
    }
}