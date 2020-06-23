using System;
using System.Collections.Generic;
using System.Text;
using PokeAPI;

namespace PokemonSimulator
{
    class DamageBuff
    {

        PokeAPI.Move MoveType;
        PokeAPI.Pokemon PokemonType;
        double DamageModifier;

        public DamageBuff(PokeAPI.Move movetype, PokeAPI.Pokemon pokemontype)
        {
            MoveType = movetype;
            PokemonType = pokemontype;
            DamageModifier = 1;

            TypeWeaknessMap BattleTypes = new TypeWeaknessMap();
            TypeComparison(BattleTypes);
        }

        public void TypeComparison(TypeWeaknessMap battletypes)
        {
            List<string> value = new List<string>();

            if (battletypes.TypeMapSuper.TryGetValue(MoveType.Type.ToString(), out value))
            {
                if (value.Contains(PokemonType.Types.ToString()))
                {
                    SuperEffectiveDamage();
                }
            }
            else if (battletypes.TypeMapNotVery.TryGetValue(MoveType.Type.ToString(), out value))
            {
                if (value.Contains(PokemonType.Types.ToString()))
                {
                    NotVeryEffectiveDamage();
                }
            }
        }
        /// <summary>
        /// cast as float and divide by 2
        /// </summary>
        public void SuperEffectiveDamage()
        {
            DamageModifier = 4;
        }

        public void NotVeryEffectiveDamage()
        {

            DamageModifier = 0.5;
        }
    }
}
