using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PokemonSimulator
{
    public struct APIMovesBlueprint
    {
        /// <summary>
        /// The identifier for this resource.
        /// </summary>
        public int id;
        /// <summary>
        /// The name for this resource.
        /// </summary>
        public string name;
        /// <summary>
        /// The percent value of how likely this move is to be successful.
        /// </summary>
        public int accuracy;
        /// <summary>
        /// The percent value of how likely it is this moves effect will happen.
        /// </summary>
        public int? effect_chance;
        /// <summary>
        /// Power points. The number of times this move can be used.
        /// </summary>
        public int pp;
        /// <summary>
        /// A value between -8 and 8. Sets the order in which moves are executed during battle.
        /// </summary>
        [Range(-8, 8)]
        public int priority;
        /// <summary>
        /// The base power of this move with a value of 0 if it does not have a base power.
        /// </summary>
        public int power;
        //public  contest_combos;
    }
}