using PokeAPI;
using System;
using System.Collections.Generic;
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
        /// <returns>Whether or not you won</returns>
        public static bool GameEngine(Trainer you, Trainer enemy)
        {
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Battle Begin!");
                Console.ForegroundColor = ConsoleColor.White;
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
                Pokemon enemies = enemy.Pokemon[Grand.rand.Next(0, enemy.Pokemon.Count)];
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
                void Attack(Pokemon attack, Pokemon defend, int moveIndex, bool isEnemyAttacking)
                {
                    //const double makeThisNumberBiggerToMakeAttacksDoLessDamage = 2d;
                    if (isEnemyAttacking)
                    {
                        PrintEnemy($"{attack.Species} ", $"used {attack.ConsoleMoves[moveIndex].Name}!");
                    }
                    else
                    {
                        PrintYou($"{attack.Species} ", $"used {attack.ConsoleMoves[moveIndex].Name}!");
                    }
                    double power = attack.ConsoleMoves[moveIndex].Damage;
                    //double modifier = PokemonType.CalculateDamageMultiplier(attack.Type, defend.Type);
                    double type = 1d;
                    for (int j = 0; j < defend.ConsoleTypes.Count; j++)
                    {
                        //modifier *= twm.//PokemonType.CalculateDamageMultiplier(attack.ConsoleTypes[i], defend.ConsoleTypes[j]);
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
                    const double level = 50d;
                    double pokeAttack = attack.ConsoleMoves[moveIndex].IsPhysical ? attack.Attack : attack.SpecialAttack;
                    double pokeDefense = defend.ConsoleMoves[moveIndex].IsPhysical ? defend.Defense : defend.SpecialDefense;
                    double modifier = (Grand.rand.Next(88, 100) / 100d) * type;
                    int final = (int)(((((((2d * level) / (5d)) + 2) * power * (pokeAttack / pokeDefense)) / (50d)) + 2d) * modifier);
                    //if (!attack.ConsoleMoves[moveIndex].IsPhysical)
                    //{
                    //    damage = (int)(((double)damage * (double)type * (double)attack.SpecialAttack * crit)/ ((double)defend.SpecialDefense * makeThisNumberBiggerToMakeAttacksDoLessDamage));
                    //}
                    //else
                    //{
                    //    damage = (int)(((double)damage * (double)type * (double)attack.Attack * crit) / ((double)defend.Defense * makeThisNumberBiggerToMakeAttacksDoLessDamage));
                    //}
                    if (type > 1d)
                    {
                        if (isEnemyAttacking)
                        {
                            PrintEnemy("", (crit > 1d ? "Critical Hit! " : "") + $"Your enemy's pokemon's attack was super effective! It hit for {(final <= defend.ActingHP ? final : defend.ActingHP)} damage! ({(defend.ActingHP - final >= 0 ? defend.ActingHP - final : 0)})HP remaining.");
                        }
                        else
                        {
                            PrintYou("", (crit > 1d ? "Critical Hit! " : "") + $"Your pokemon's attack was super effective! It hit for {(final <= defend.ActingHP ? final : defend.ActingHP)} damage! ({(defend.ActingHP - final >= 0 ? defend.ActingHP - final : 0)})HP remaining.");
                        }
                    }
                    else if (type == 1d)
                    {
                        if (isEnemyAttacking)
                        {
                            PrintEnemy("", (crit > 1d ? "Critical Hit! " : "") + $"Your enemy's pokemon's attack hit for {(final <= defend.ActingHP ? final : defend.ActingHP)} damage. ({(defend.ActingHP - final >= 0 ? defend.ActingHP - final : 0)})HP remaining.");
                        }
                        else
                        {
                            PrintYou("", (crit > 1d ? "Critical Hit! " : "") + $"Your pokemon's attack hit for {(final <= defend.ActingHP ? final : defend.ActingHP)} damage! ({(defend.ActingHP - final >= 0 ? defend.ActingHP - final : 0)})HP remaining.");
                        }
                    }
                    else
                    {
                        if (isEnemyAttacking)
                        {
                            PrintEnemy("", (crit > 1d ? "Critical Hit! " : "") + $"Your enemy's pokemon's attack hit for {(final <= defend.ActingHP ? final : defend.ActingHP)} damage. It wasn't very effective... ({(defend.ActingHP - final >= 0 ? defend.ActingHP - final : 0)})HP remaining.");
                        }
                        else
                        {
                            PrintYou("", (crit > 1d ? "Critical Hit! " : "") + $"Your pokemon's attack hit for {(final <= defend.ActingHP ? final : defend.ActingHP)} damage. It wasn't very effective... ({(defend.ActingHP - final >= 0 ? defend.ActingHP - final : 0)})HP remaining.");
                        }
                    }
                    defend.ModifyHealth(-final);
                }
                PrintYou("You: ", $"Go {yours.Species}!");
                PrintEnemy("Enemy: ", $"Go {enemies.Species}!");
                bool gaming = true;
                while (gaming)
                {
                    int yourMovePick = 0;
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
                    int enemiesMovePick = Grand.rand.Next(0, 4);
                    if (yourMovePick < 4 && yourMovePick >= 0)
                    {
                        //you and your enemy hit eachother based on speed.
                        if (enemies.Speed > yours.Speed)
                        {
                            Attack(enemies, yours, enemiesMovePick, true);
                            if (yours.IsAlive)
                            {
                                Attack(yours, enemies, yourMovePick, false);
                            }
                        }
                        else if (yours.Speed > enemies.Speed)
                        {
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
                        //yours.ModifyHealth(-int.Parse(enemies.Moves[enemiesMovePick].Damage) * (yours.TypeWeaknesses.Count(x => x == enemies.Moves[enemiesMovePick].Type) + 1));
                        Attack(enemies, yours, enemiesMovePick, true);
                    }
                    else
                    {
                        throw new Exception("Error: This should never happen. If it does, tell Sam.");
                    }
                    if (!enemies.IsAlive && enemy.Pokemon.Any(x => x.IsAlive))
                    {
                        IEnumerable<Pokemon> temp = enemy.Pokemon.Where(x => x.IsAlive);
                        enemies = temp.ElementAtOrDefault(Grand.rand.Next(0, temp.Count()));
                        PrintEnemy("Enemy: ", $"Go {enemies.Species}!");
                    }
                    if (!yours.IsAlive && you.Pokemon.Any(x => x.IsAlive))
                    {
                        PrintYou($"{yours.Species} ", "was knocked out! Choose a new pokemon: (1 - 6) " + string.Join(", ", you.Pokemon.Select(x => x.Species + " " + (((float)x.ActingHP / (float)x.BaseHP)).ToString("P") + $" ({x.ActingHP})HP")));
                        int pokePick = int.Parse(Console.ReadLine()) - 1;
                        do
                        {
                            if (you.Pokemon[pokePick] == yours)
                            {
                                Console.WriteLine("That pokemon is already out! Pick a different one. (1 - 6)");
                                pokePick = int.Parse(Console.ReadLine()) - 1;
                            }
                            else if (!you.Pokemon[pokePick].IsAlive)
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
                    if (!enemy.Pokemon.Any(x => x.IsAlive))
                    {
                        PrintYou("You win! ", "");
                        return true;
                        //gaming = false;
                    }
                    else if (!you.Pokemon.Any(x => x.IsAlive))
                    {
                        PrintEnemy("You lose. ", "");
                        return false;
                        //gaming = false;
                    }
                }
                //bool quit = false;
                //do
                //{
                //    string reply = Console.ReadLine();
                //    if (Grand.yes.IsMatch(reply))
                //    {
                //        gaming = true;
                //        you = enemy = null;
                //        yours = enemies = null;
                //        quit = false;
                //        Console.Clear();
                //        break;
                //    }
                //    else if (Grand.no.IsMatch(reply))
                //    {
                //        gaming = false;
                //        quit = true;
                //        break;
                //    }
                //    else
                //    {
                //        Console.WriteLine("Hey! That's not a valid response. Say yes or no.");
                //    }
                //}
                //while (true);
                //if (quit)
                //{
                //    break;
                //}
            }
            while (true);
        }
    }
}