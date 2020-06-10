using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    /// <summary>
    /// Author: Samuel Gardner
    /// </summary>
    [Flags]
    public enum PokemonType : int
    {
        Bug = 0b0000000000000000000000000000001,
        Electric = 0b0000000000000000000000000000010,
        Fire = 0b0000000000000000000000000000100,
        Grass = 0b0000000000000000000000000001000,
        Normal = 0b0000000000000000000000000010000,
        Rock = 0b0000000000000000000000000100000,
        Flying = 0b0000000000000000000000001000000,
        Ground = 0b0000000000000000000000010000000,
        Poison = 0b0000000000000000000000100000000,
        Dragon = 0b0000000000000000000001000000000,
        Fighting = 0b0000000000000000000010000000000,
        Ghost = 0b0000000000000000000100000000000,
        Ice = 0b0000000000000000001000000000000,
        Psychic = 0b0000000000000000010000000000000,
        Water = 0b0000000000000000100000000000000,
        Fairy = 0b0000000000000001000000000000000,
        Dark = 0b0000000000000010000000000000000,
        Steel = 0b0000000000000100000000000000000
    }
    /// <summary>
    /// Author: Samuel Gardner
    /// </summary>
    [Obsolete("Don't know enoguh about pokemon to know if this is all the " +
        "kinds of vunerability levels associated between types.")]
    public enum VunerabilityLevel : int
    {
        Invunerable = 1,
        Strong = 2,
        Nuetral = 4,
        Weak = 8,
        Vunerable = 16
    }
}