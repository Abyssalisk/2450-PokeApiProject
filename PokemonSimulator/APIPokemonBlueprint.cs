using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace PokemonSimulator
{
    /// <summary>
    /// Do not call the default CTOR on this object, call GetPokemonBlueprint to get "".
    /// Author: Samuel Gardner
    /// </summary>
    public struct APIPokemonBlueprint// : ISerializable
    {
        #region Typedefs
        public struct NameURL
        {
            /// <summary>
            /// The name of this object.
            /// </summary>
            public string name;
            /// <summary>
            /// The URL resource of this object.
            /// </summary>
            public string url;
        }
        public struct Ability
        {
            /// <summary>
            /// Contains the name and URL of the ability; (in that order).
            /// </summary>
            public NameURL ability;
            /// <summary>
            /// Whether or not the ability is publicly visible to the opponent.
            /// </summary>
            public bool is_hidden;
            /// <summary>
            /// The slot that this ability occupies.
            /// </summary>
            public int slot;
        }
        public struct GameOccurances
        {
            /// <summary>
            /// The game index.
            /// </summary>
            public int game_index;
            /// <summary>
            /// Name and URL of the game version this pokemon appears in.
            /// </summary>
            public NameURL version;
        }
        public struct Item
        {
            public struct VersionDetails
            {
                /// <summary>
                /// The rarity of this item.
                /// </summary>
                public int rarity;
                /// <summary>
                /// The version associated with the above rarity.
                /// </summary>
                public NameURL version;
            }
            /// <summary>
            /// The name/url of the item.
            /// </summary>
            public NameURL item;
            /// <summary>
            /// List of the version details for this item.
            /// </summary>
            public VersionDetails[] version_details;
        }
        public struct SpritePack
        {
            public string back_default;
            public string back_female;
            public string back_shiny;
            public string back_shiny_female;
            public string front_default;
            public string front_female;
            public string front_shiny;
            public string front_shiny_female;
        }
        public struct StatPack
        {
            /// <summary>
            /// The base stat for this stat.
            /// </summary>
            public int base_stat;
            /// <summary>
            /// Idk what this field means.
            /// </summary>            
            public int effort;
            /// <summary>
            /// The name/URL of this stat.
            /// </summary>
            public NameURL stat;
        }
        public struct Type
        {
            /// <summary>
            /// The slot this type takes up in the pokemon's definition.
            /// </summary>
            public int slot;
            /// <summary>
            /// The name/URL of this type.
            /// </summary>
            public NameURL type;
        }
        public struct Move
        {
            public struct MoveVersion
            {
                /// <summary>
                /// The level that the move is able to be learned at.
                /// </summary>
                public int level_learned_at;
                /// <summary>
                /// The method the move is learned, and URL.
                /// </summary>
                public NameURL move_learn_method;
                /// <summary>
                /// The name of the version and the URL.
                /// </summary>
                public NameURL version_group;
            }
            /// <summary>
            /// The name/URL of this move.
            /// </summary>
            public NameURL move;
            /// <summary>
            /// The version details for each version.
            /// </summary>
            public MoveVersion[] version_group_details;
        }
        #endregion

        #region Fields
        //Trying to avoid properties because I don't want unecessary calls.
        /// <summary>
        /// The abilities that this pokemon has.
        /// </summary>
        public Ability[] abilities;
        /// <summary>
        /// The base experience this pokemon has.
        /// </summary>
        public int base_experience;
        /// <summary>
        /// The forms that this pokemon can take; (name and URL).
        /// </summary>
        public NameURL[] forms;
        /// <summary>
        /// The games this pokemon appears in.
        /// </summary>
        public GameOccurances[] game_indices;
        /// <summary>
        /// The height of this pokemon.
        /// </summary>
        public int height;
        /// <summary>
        /// The items held by this pokemon.
        /// </summary>
        public Item[] held_items;
        /// <summary>
        /// The ID of this pokemon.
        /// </summary>
        public int id;
        /// <summary>
        /// Don't know what this field is honestly.
        /// </summary>
        public bool is_default;
        /// <summary>
        /// The places you will encounter this pokemon.
        /// </summary>
        public string location_area_encounters;
        /// <summary>
        /// The moves this pokemon has.
        /// </summary>
        public Move[] moves;
        /// <summary>
        /// This pokemon's name.
        /// </summary>
        public string name;
        /// <summary>
        /// The value to order by this pokemon when sorting.
        /// </summary>
        public int order;
        /// <summary>
        /// The name and URL of the species.
        /// </summary>
        public NameURL species;
        /// <summary>
        /// Back default, back female, back shiny, back shiny female, front default, front female, front shiny, front shiny female.
        /// </summary>
        public SpritePack sprites;
        /// <summary>
        /// Base stat, effort, and Stat.
        /// </summary>
        public StatPack[] stats;
        /// <summary>
        /// The slot of the type and the type.
        /// </summary>
        public Type[] types;
        /// <summary>
        /// The weight of the pokemon.
        /// </summary>
        public int weight;
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

        #region Redefinitions/Overrides
        public new string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion

        #region Method Implements
        public void GetObjectData(SerializationInfo sInfo, StreamingContext context)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}