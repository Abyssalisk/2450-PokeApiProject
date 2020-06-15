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
        public new bool Equals(object o)
        {
            if (o != null && typeof(APIPokemonBlueprint).IsInstanceOfType(o))
            {
                //fast way bad way
                //return ((APIPokemonBlueprint)o).ToString() == this.ToString();
                //not working way
                //return ((APIPokemonBlueprint)o).GetHashCode() == this.GetHashCode();
                //proper way
                //Below literally checks all the fields manually in case we need it.
                #region Manual Check
                bool result = true;
                if (((APIPokemonBlueprint)o).abilities.Length == this.abilities.Length)
                {
                    for (int i = 0; i < this.abilities.Length; i++)
                    {
                        result &= (((APIPokemonBlueprint)o).abilities[i].is_hidden == this.abilities[i].is_hidden);
                        result &= (((APIPokemonBlueprint)o).abilities[i].slot == this.abilities[i].slot);
                        result &= (((APIPokemonBlueprint)o).abilities[i].ability.name == this.abilities[i].ability.name);
                        result &= (((APIPokemonBlueprint)o).abilities[i].ability.url == this.abilities[i].ability.url);
                    }
                }
                else
                {
                    return false;
                }
                if (!result)
                {
                    return false;
                }
                result &= ((APIPokemonBlueprint)o).base_experience == this.base_experience;
                if (((APIPokemonBlueprint)o).forms.Length == this.forms.Length)
                {
                    for (int i = 0; i < this.forms.Length; i++)
                    {
                        result &= ((APIPokemonBlueprint)o).forms[i].name == this.forms[i].name;
                        result &= ((APIPokemonBlueprint)o).forms[i].url == this.forms[i].url;
                    }
                }
                else
                {
                    return false;
                }
                if (!result)
                {
                    return false;
                }
                if (((APIPokemonBlueprint)o).game_indices.Length == this.game_indices.Length)
                {
                    for (int i = 0; i < this.game_indices.Length; i++)
                    {
                        result &= ((APIPokemonBlueprint)o).game_indices[i].game_index == this.game_indices[i].game_index;
                        result &= ((APIPokemonBlueprint)o).game_indices[i].version.name == this.game_indices[i].version.name;
                        result &= ((APIPokemonBlueprint)o).game_indices[i].version.url == this.game_indices[i].version.url;
                    }
                }
                else
                {
                    return false;
                }
                if (!result)
                {
                    return false;
                }
                result &= ((APIPokemonBlueprint)o).height == this.height;
                if (((APIPokemonBlueprint)o).held_items.Length == this.held_items.Length)
                {
                    for (int i = 0; i < this.held_items.Length; i++)
                    {
                        result &= ((APIPokemonBlueprint)o).held_items[i].item.name == this.held_items[i].item.name;
                        result &= ((APIPokemonBlueprint)o).held_items[i].item.url == this.held_items[i].item.url;
                        if (((APIPokemonBlueprint)o).held_items[i].version_details.Length == this.held_items[i].version_details.Length)
                        {
                            for (int j = 0; j < this.held_items[i].version_details.Length; j++)
                            {
                                result &= ((APIPokemonBlueprint)o).held_items[i].version_details[j].rarity == this.held_items[i].version_details[j].rarity;
                                result &= ((APIPokemonBlueprint)o).held_items[i].version_details[j].version.name == this.held_items[i].version_details[j].version.name;
                                result &= ((APIPokemonBlueprint)o).held_items[i].version_details[j].version.url == this.held_items[i].version_details[j].version.url;
                            }
                        }
                        else
                        {
                            return false;
                        }
                        if (!result)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
                if (!result)
                {
                    return false;
                }
                result &= ((APIPokemonBlueprint)o).id == this.id;
                result &= ((APIPokemonBlueprint)o).is_default == this.is_default;
                result &= ((APIPokemonBlueprint)o).location_area_encounters == this.location_area_encounters;
                if (((APIPokemonBlueprint)o).moves.Length == this.moves.Length)
                {
                    for (int i = 0; i < this.moves.Length; i++)
                    {
                        result &= ((APIPokemonBlueprint)o).moves[i].move.name == this.moves[i].move.name;
                        result &= ((APIPokemonBlueprint)o).moves[i].move.url == this.moves[i].move.url;
                        if (((APIPokemonBlueprint)o).moves[i].version_group_details.Length == this.moves[i].version_group_details.Length)
                        {
                            for (int j = 0; j < this.moves[i].version_group_details.Length; j++)
                            {
                                result &= ((APIPokemonBlueprint)o).moves[i].version_group_details[j].level_learned_at == this.moves[i].version_group_details[j].level_learned_at;
                                result &= ((APIPokemonBlueprint)o).moves[i].version_group_details[j].move_learn_method.name == this.moves[i].version_group_details[j].move_learn_method.name;
                                result &= ((APIPokemonBlueprint)o).moves[i].version_group_details[j].move_learn_method.url == this.moves[i].version_group_details[j].move_learn_method.url;
                                result &= ((APIPokemonBlueprint)o).moves[i].version_group_details[j].version_group.name == this.moves[i].version_group_details[j].version_group.name;
                                result &= ((APIPokemonBlueprint)o).moves[i].version_group_details[j].version_group.url == this.moves[i].version_group_details[j].version_group.url;
                            }
                        }
                        else
                        {
                            return false;
                        }
                        if (!result)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
                if (!result)
                {
                    return false;
                }
                result &= ((APIPokemonBlueprint)o).name == this.name;
                result &= ((APIPokemonBlueprint)o).order == this.order;
                result &= ((APIPokemonBlueprint)o).species.name == this.species.name;
                result &= ((APIPokemonBlueprint)o).species.url == this.species.url;
                result &= ((APIPokemonBlueprint)o).sprites.back_default == this.sprites.back_default;
                result &= ((APIPokemonBlueprint)o).sprites.back_female == this.sprites.back_female;
                result &= ((APIPokemonBlueprint)o).sprites.back_shiny == this.sprites.back_shiny;
                result &= ((APIPokemonBlueprint)o).sprites.back_shiny_female == this.sprites.back_shiny_female;
                result &= ((APIPokemonBlueprint)o).sprites.front_default == this.sprites.front_default;
                result &= ((APIPokemonBlueprint)o).sprites.front_female == this.sprites.front_female;
                result &= ((APIPokemonBlueprint)o).sprites.front_shiny == this.sprites.front_shiny;
                result &= ((APIPokemonBlueprint)o).sprites.front_shiny_female == this.sprites.front_shiny_female;
                if (((APIPokemonBlueprint)o).stats.Length == this.stats.Length)
                {
                    for (int i = 0; i < this.stats.Length; i++)
                    {
                        result &= ((APIPokemonBlueprint)o).stats[i].base_stat == this.stats[i].base_stat;
                        result &= ((APIPokemonBlueprint)o).stats[i].effort == this.stats[i].effort;
                        result &= ((APIPokemonBlueprint)o).stats[i].stat.name == this.stats[i].stat.name;
                        result &= ((APIPokemonBlueprint)o).stats[i].stat.url == this.stats[i].stat.url;
                    }
                }
                else
                {
                    return false;
                }
                if (!result)
                {
                    return false;
                }
                if (((APIPokemonBlueprint)o).types.Length == this.types.Length)
                {
                    for (int i = 0; i < this.types.Length; i++)
                    {
                        result &= ((APIPokemonBlueprint)o).types[i].slot == this.types[i].slot;
                        result &= ((APIPokemonBlueprint)o).types[i].type.name == this.types[i].type.name;
                        result &= ((APIPokemonBlueprint)o).types[i].type.url == this.types[i].type.url;
                    }
                }
                else
                {
                    return false;
                }
                result &= ((APIPokemonBlueprint)o).weight == this.weight;
                return result;
                #endregion
            }
            else
            {
                return false;
            }
        }
        public static bool operator ==(APIPokemonBlueprint lhs, APIPokemonBlueprint rhs)
        {
            return lhs.Equals(rhs);
        }
        public static bool operator !=(APIPokemonBlueprint lhs, APIPokemonBlueprint rhs)
        {
            return !lhs.Equals(rhs);
        }
        #endregion

        #region Method Implements
        //public void GetObjectData(SerializationInfo sInfo, StreamingContext context)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}