using PokeAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PokemonSimulator
{
    class GameMockup
    {
        static TypeWeaknessMap twm = new TypeWeaknessMap();
        /// <summary>
        /// Fight between you and an AI, with randomly chosen moves.
        /// </summary>
        /// <param name="you">The trainer object with your team (will be mutated)</param>
        /// <param name="enemy">The trainer object with the enemy team</param>
        /// <param name="intelligence">The intelligence of the AI (intelligence currently affects only move choice). 
        /// Passing a 0 will make the enemy AI pick moves randomly, passing a 0.5 will make the enemy pick a move randomly 
        /// or pick what move it thinks might do the most damage (it runs the default damage calculation then adds a factor 
        /// of randomness as its prediction of damage dealt). Passing a 1 will make the enemy pick what it thinks is the best move (With 100% accurate damage prediction).</param>
        /// <returns>Whether or not you won</returns>
        public static bool GameEngine(Trainer you, Trainer enemy, [Range(0d, 1d)] double intelligence = 0.5d)
        {
            #region Custom print messages
            void PrintYou(string pre, string text)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(pre);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(text);
            }
            void PrintEnemy(string pre, string text)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(pre);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(text);
            }
            #endregion

            #region Attack method
            void Attack(Pokemon attack, Pokemon defend, int moveIndex, bool isEnemyAttacking)
            {
                if (isEnemyAttacking)
                {
                    PrintEnemy($"{attack.Species} ", $"used {attack.ConsoleMoves[moveIndex].Name}!");
                }
                else
                {
                    PrintYou($"{attack.Species} ", $"used {attack.ConsoleMoves[moveIndex].Name}!");
                }
                double type = 1d;
                for (int j = 0; j < defend.ConsoleTypes.Count; j++)
                {
                    if (twm.TypeMapNotVery.ContainsKey(attack.ConsoleMoves[moveIndex].Type))
                    {
                        if (twm.TypeMapNotVery[attack.ConsoleMoves[moveIndex].Type].Contains(defend.ConsoleTypes[j]))
                        {
                            type *= 0.5d;
                        }
                    }
                    if (twm.TypeMapSuper.ContainsKey(attack.ConsoleMoves[moveIndex].Type))
                    {
                        if (twm.TypeMapSuper[attack.ConsoleMoves[moveIndex].Type].Contains(defend.ConsoleTypes[j]))
                        {
                            type *= 2d;
                        }
                    }
                }
                double crit = Grand.rand.Next(16) == 0 ? 1.5d : 1d;
                //Dont call do damage calculation before the definition.
                double DoDamageCalculation(int tryMoveIndex,/*[Range(0d, 1d)]*/double predictionInnacuracy = 0d)
                {
                    double tempCrit = crit;
                    double power = attack.ConsoleMoves[tryMoveIndex].Damage;
                    const double level = 50d;
                    double pokeAttack = attack.ConsoleMoves[tryMoveIndex].IsPhysical ? attack.Attack : attack.SpecialAttack;
                    double pokeDefense = defend.ConsoleMoves[tryMoveIndex].IsPhysical ? defend.Defense : defend.SpecialDefense;
                    double modifier = (Grand.rand.Next(88, 100) / 100d) * type * crit;
                    double rand = ((Grand.rand.NextDouble() + 0.5d) * predictionInnacuracy * 4d);
                    double pre = ((((((((2d * level) / (5d)) + 2) * power * (pokeAttack / pokeDefense)) / (50d)) + 2d) * modifier));
                    double finalValue = (rand * pre) + pre;
                    crit = tempCrit;
                    return finalValue;
                }
                if (isEnemyAttacking)
                {
                    double[] predictions = { DoDamageCalculation(0, 1 - intelligence), DoDamageCalculation(1, 1 - intelligence), DoDamageCalculation(2, 1 - intelligence), DoDamageCalculation(3, 1 - intelligence) };
                    moveIndex = Array.IndexOf(predictions, predictions.Max());
                    if (moveIndex == -1)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Hey! Enemy couldn't find a good move for some reason. Tell Sam.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                int finalDamage = (int)DoDamageCalculation(moveIndex);
                if (isEnemyAttacking)
                {
                    if (type != 1)
                    {
                        PrintEnemy("", (crit > 1d ? "Critical Hit! " : "") + (type < 1d ? ("Your enemy's pokemon's inneffective attack hit for ") : ("Your enemy's pokemon's attack was super effective! It hit for ")) + $"{(finalDamage <= defend.ActingHP ? finalDamage : defend.ActingHP)} damage! ({(defend.ActingHP - finalDamage >= 0 ? defend.ActingHP - finalDamage : 0)})HP remaining.");
                    }
                    else
                    {
                        PrintEnemy("", (crit > 1d ? "Critical Hit! " : "") + $"Your enemy's pokemon's attack hit for {(finalDamage <= defend.ActingHP ? finalDamage : defend.ActingHP)} damage. ({(defend.ActingHP - finalDamage >= 0 ? defend.ActingHP - finalDamage : 0)})HP remaining.");
                    }
                }
                else
                {
                    if (type != 1)
                    {
                        PrintYou("", (crit > 1d ? "Critical Hit! " : "") + (type < 1d ? ("Your pokemon's inneffective attack hit for ") : ("Your pokemon's attack was super effective! It hit for ")) + $"It hit for {(finalDamage <= defend.ActingHP ? finalDamage : defend.ActingHP)} damage! ({(defend.ActingHP - finalDamage >= 0 ? defend.ActingHP - finalDamage : 0)})HP remaining.");
                    }
                    else
                    {
                        PrintYou("", (crit > 1d ? "Critical Hit! " : "") + $"Your pokemon's attack hit for {(finalDamage <= defend.ActingHP ? finalDamage : defend.ActingHP)} damage! ({(defend.ActingHP - finalDamage >= 0 ? defend.ActingHP - finalDamage : 0)})HP remaining.");
                    }
                }
                defend.ModifyHealth(-finalDamage);
            }
            #endregion

            while (true) //This loop just goes on forever, doing the battle sequence until someone wins.
            {
                //Write declaration of battle
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Battle Begin!");
                Console.ForegroundColor = ConsoleColor.White;
                //Initial choosing of pokemon.
                Pokemon yours;
                Console.WriteLine("Choose your pokemon: (1 - 6) " + string.Join(", ", you.Pokemon.Select(x => x.Species + " " + (((float)x.ActingHP / (float)x.BaseHP)).ToString("P") + $" ({x.ActingHP})HP")));
                do
                {
                    if (int.TryParse(Console.ReadLine(), out int tryParse) && tryParse > 0 && tryParse <= 6)
                    {
                        if (!you.Pokemon[tryParse - 1].IsAlive)
                        {
                            Console.WriteLine("That pokemon has been knocked out! Pick a different one. (1 - 6)");
                        }
                        else
                        {
                            yours = you.Pokemon[tryParse - 1];
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hey! that's not a valid number. Pick a pokemon (1 - 6)");
                    }
                } while (true);
                //Enemy choosing pokemon
                Pokemon enemies = enemy.Pokemon[Grand.rand.Next(0, enemy.Pokemon.Count)];
                //Go Pikachu!
                PrintYou("You: ", $"Go {yours.Species}!");
                PrintEnemy("Enemy: ", $"Go {enemies.Species}!");
                bool gaming = true;
                while (gaming)
                {
                    int yourMovePick = 0;
                    //choose a move, switch pokemon, or view stats.
                    do
                    {
                        Console.WriteLine("Select move: (1 - 4) " + string.Join(", ", yours.ConsoleMoves.Select(x => x.Name)) + ", switch pokemon (5), or look at all pokemon health (6)");
                        if (int.TryParse(Console.ReadLine(), out int tryParse))
                        {
                            if (tryParse < 6 && tryParse > 0)
                            {
                                yourMovePick = tryParse - 1;
                                break;
                            }
                            else if (tryParse == 6)
                            {
                                //If you choose to look at stats, that doesn't remove your turn, just look at the stats and then pick what you want to do.
                                PrintYou("Your Pokemons health: ", string.Join(", ", you.Pokemon.Select(x => x.Species + " " + (((float)x.ActingHP / (float)x.BaseHP)).ToString("P") + $" ({x.ActingHP})HP")));
                                PrintEnemy("Enemy's Pokemons health: ", string.Join(", ", enemy.Pokemon.Select(x => x.Species + " " + (((float)x.ActingHP / (float)x.BaseHP)).ToString("P") + $" ({x.ActingHP})HP")));
                            }
                            else
                            {
                                Console.WriteLine("Hey! Pick a move (1 - 4), switch pokemon (5), or look at all pokemon health (6).");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Hey! Thats not a number, pick a move (1 - 4), switch pokemon (5), or look at all pokemon health (6).");
                        }
                    } while (true);
                    //enemy chooses move (doesn't matter anymore because it'll just pick a new move when it attacks).
                    int enemiesMovePick = Grand.rand.Next(0, 4);
                    if (yourMovePick < 4 && yourMovePick >= 0)
                    {
                        //you and your enemy hit eachother based on speed.
                        if (enemies.Speed > yours.Speed)
                        {
                            //Enemy was faster, so they hit first, then you hit if your pokemon is still alive.
                            Attack(enemies, yours, enemiesMovePick, true);
                            if (yours.IsAlive)
                            {
                                Attack(yours, enemies, yourMovePick, false);
                            }
                        }
                        else if (yours.Speed > enemies.Speed)
                        {
                            //You were faster, so you hit then their pokemon hits if it's still alive
                            Attack(yours, enemies, yourMovePick, false);
                            if (enemies.IsAlive)
                            {
                                Attack(enemies, yours, enemiesMovePick, true);
                            }
                        }
                        else
                        {
                            //if speeds are same pick one randomly
                            switch (Grand.rand.Next(0, 2))
                            {
                                case 0:
                                    {
                                        Attack(enemies, yours, enemiesMovePick, true);
                                        if (yours.IsAlive)
                                        {
                                            Attack(yours, enemies, yourMovePick, false);
                                        }
                                    }
                                    break;
                                case 1:
                                    {
                                        Attack(yours, enemies, yourMovePick, false);
                                        if (enemies.IsAlive)
                                        {
                                            Attack(enemies, yours, enemiesMovePick, true);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    else if (yourMovePick == 4)
                    {
                        //You want to switch pokemon.
                        Console.WriteLine("Choose your pokemon: (1 - 6) " + string.Join(", ", you.Pokemon.Select(x => x.Species + " " + (((float)x.ActingHP / (float)x.BaseHP)).ToString("P") + $" ({x.ActingHP})HP")));
                        do
                        {
                            if (int.TryParse(Console.ReadLine(), out int tryParse) && tryParse > 0 && tryParse <= 6)
                            {
                                if (you.Pokemon[tryParse] == yours)
                                {
                                    Console.WriteLine("That pokemon is already out! Pick a different one. (1 - 6)");
                                }
                                else if (!you.Pokemon[tryParse].IsAlive)
                                {
                                    Console.WriteLine("That pokemon has been knocked out! Pick a different one. (1 - 6)");
                                }
                                else
                                {
                                    yours = you.Pokemon[tryParse];
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Hey! that's not a valid number. Pick a pokemon (1 - 6)");
                            }
                        } while (true);
                        //Enemy gets a free hit in on the freshly switched in pokemon because you switched pokemon.
                        Attack(enemies, yours, enemiesMovePick, true);
                    }
                    else
                    {
                        throw new Exception("Error: This should never happen. If it does, tell Sam.");
                    }
                    //If enemies pokemon is dead and it has any that are alive left, pick a new one at random.
                    if (!enemies.IsAlive && enemy.Pokemon.Any(x => x.IsAlive))
                    {
                        IEnumerable<Pokemon> temp = enemy.Pokemon.Where(x => x.IsAlive);
                        enemies = temp.ElementAtOrDefault(Grand.rand.Next(0, temp.Count()));
                        PrintEnemy("Enemy: ", $"Go {enemies.Species}!");
                    }
                    //Pick a new pokemon if yours was knocked out and you have any left.
                    if (!yours.IsAlive && you.Pokemon.Any(x => x.IsAlive))
                    {
                        PrintYou($"{yours.Species} ", "was knocked out! Choose a new pokemon: (1 - 6) " + string.Join(", ", you.Pokemon.Select(x => x.Species + " " + (((float)x.ActingHP / (float)x.BaseHP)).ToString("P") + $" ({x.ActingHP})HP")));
                        int pokePick = int.Parse(Console.ReadLine()) - 1;
                        do
                        {
                            if (!you.Pokemon[pokePick].IsAlive)
                            {
                                Console.WriteLine("That pokemon has been knocked out! Pick a different one. (1 - 6)");
                                pokePick = int.Parse(Console.ReadLine()) - 1;
                            }
                            else
                            {
                                yours = you.Pokemon[pokePick];
                                break;
                            }
                        } while (true);
                        PrintYou("You: ", $"Go {yours.Species}!");
                    }
                    //If all of enemy's pokemon are KO, you win.
                    if (!enemy.Pokemon.Any(x => x.IsAlive))
                    {
                        PrintYou("You win! ", "");
                        return true;
                    }
                    //If you have no pokemon left, you lose.
                    else if (!you.Pokemon.Any(x => x.IsAlive))
                    {
                        PrintEnemy("You lose. ", "");
                        return false;
                    }
                }
            }
        }
    }
}