using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PokemonSimulator
{
    /// <summary>
    /// Do not call the default CTOR on this object, call GetPokemonBlueprint to get "".
    /// </summary>
    struct APIPokemonBlueprint
    {
        #region Typedefs
        public struct Ability
        {
            /// <summary>
            /// Contains the name and URL of the ability; (in that order).
            /// </summary>
            public readonly ValueTuple<string, string> ability;
            /// <summary>
            /// Whether or not the ability is publicly visible to the opponent.
            /// </summary>
            public readonly bool isHidden;
            /// <summary>
            /// The slot that this ability occupies.
            /// </summary>
            public readonly int slotNum;
        }        
        public struct GameOccurances
        {
            /// <summary>
            /// The game index.
            /// </summary>
            public readonly int gameIndex;
            /// <summary>
            /// Name and URL of the game version this pokemon appears in.
            /// </summary>
            public readonly ValueTuple<string, string> version;
        }
        public struct Item
        {
            
            /// <summary>
            /// Name of the item.
            /// </summary>
            public readonly string name;
            /// <summary>
            /// URL of the item.
            /// </summary>
            public readonly string url;
        }
        public struct ItemVersion
        {
            /// <summary>
            /// The rarity of the item in that version.
            /// </summary>
            public readonly int rarity;
            /// <summary>
            /// The game version; (name and version).
            /// </summary>
            public readonly ValueTuple<string, string> version;
        }
        public struct Move
        {
            /// <summary>
            /// The name of the move.
            /// </summary>
            public readonly string name;
            /// <summary>
            /// The URL.
            /// </summary>
            public readonly string URL;
        }
        public struct MoveVersion
        {
            /// <summary>
            /// The level that the move is able to be learned at.
            /// </summary>
            public readonly int levelLearnedAt;
            /// <summary>
            /// The method the move is learned, and URL.
            /// </summary>
            public readonly ValueTuple<string, string> moveLearnMethod;
            /// <summary>
            /// The name of the version and the URL.
            /// </summary>
            public readonly ValueTuple<string, string> version;
        }
        public struct Stat
        {
            /// <summary>
            /// The name of the statistic.
            /// </summary>
            public readonly string name;
            /// <summary>
            /// The url of the statistic.
            /// </summary>
            public readonly string url;
        }
        public struct Type
        {
            /// <summary>
            /// The name of the type.
            /// </summary>
            public readonly string name;
            /// <summary>
            /// The url of the type.
            /// </summary>
            public readonly string url;
        }
        #endregion

        #region Fields
        //Trying to avoid properties because I don't want unecessary calls.
        /// <summary>
        /// The abilities that this pokemon has.
        /// </summary>
        public readonly Ability[] abilities;
        /// <summary>
        /// The base experience this pokemon has.
        /// </summary>
        public readonly int baseExp;
        /// <summary>
        /// The forms that this pokemon can take; (name and URL).
        /// </summary>
        public readonly ValueTuple<string, string>[] forms;
        /// <summary>
        /// The games this pokemon appears in.
        /// </summary>
        public readonly GameOccurances[] games;
        /// <summary>
        /// The height of this pokemon.
        /// </summary>
        public readonly int height;
        /// <summary>
        /// The items held by this pokemon.
        /// </summary>
        public readonly ValueTuple<Item, ItemVersion>[] heldItems;
        /// <summary>
        /// The ID of this pokemon.
        /// </summary>
        public readonly int id;
        /// <summary>
        /// Don't know what this field is honestly.
        /// </summary>
        public readonly bool isDefault;
        /// <summary>
        /// The places you will encounter this pokemon.
        /// </summary>
        public readonly string locationAreaEncounters;
        /// <summary>
        /// The moves this pokemon has.
        /// </summary>
        public readonly ValueTuple<Move, MoveVersion>[] moves;
        /// <summary>
        /// This pokemon's name.
        /// </summary>
        public readonly string name;
        /// <summary>
        /// The value to order by this pokemon when sorting.
        /// </summary>
        public readonly int order;
        /// <summary>
        /// The name and URL of the species.
        /// </summary>
        public readonly ValueTuple<string, string> species;
        /// <summary>
        /// Back default, back female, back shiny, back shiny female, front default, front female, front shiny, front shiny female.
        /// </summary>
        public readonly string[] sprites;
        /// <summary>
        /// Base stat, effort, and Stat.
        /// </summary>
        public readonly ValueTuple<int, int, Stat> stats;
        /// <summary>
        /// The slot of the type and the type.
        /// </summary>
        public readonly ValueTuple<int, Type>[] types;
        /// <summary>
        /// The weight of the pokemon.
        /// </summary>
        public readonly int weight;
        #endregion

        #region Ctors
        public APIPokemonBlueprint(APIPokemonBlueprint deep)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        public static APIPokemonBlueprint GetPokemonBlueprint(string apiRequestID)
        {
            RestClient client = new RestClient($"https://pokeapi.co/api/v2/pokemon/{apiRequestID}/");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            APIPokemonBlueprint pokemon = JsonConvert.DeserializeObject<APIPokemonBlueprint>(response.Content);            
            return pokemon;            
        }
        #endregion
    }
}