using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonSimulator
{
    class GameMockup
    {
        public static void Main(string[] args)
        {
            //Tie this to the stuff to get real things.
            Trainer you = new Trainer();
            Trainer enemy = new Trainer();
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Battle Begin!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Choose your pokemon: (1 - 6) " + string.Join(", ", you.Pokemon.Select(x => x.Species)));
                Pokemon yours;
                do
                {
                    int.TryParse(Console.ReadLine(), out int tryParse);
                    tryParse -= 1;
                    if (tryParse >= 0 && tryParse < 6)
                    {
                        yours = you.Pokemon[tryParse];
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Hey! that's not a valid number. Pick a pokemon (1 - 6)");
                    }
                } while (true);
                Pokemon enemies = enemy.Pokemon[Grand.rand.Next(0, enemy.Pokemon.Count)];
                PrintYou("You: ", $" Go {yours.Species}!");
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
                PrintEnemy("Enemy: ", $" Go {enemies.Species}!");
                bool gaming = true;
                while (gaming)
                {
                    Console.WriteLine("Select move: (1 - 4) " + string.Join(", ", yours.Moves.Select(x => x.Name)) + " or switch pokemon (5)");
                    int yourMovePick = int.Parse(Console.ReadLine()) - 1;
                    int enemiesMovePick = Grand.rand.Next(0, 4);
                    if (yourMovePick != 4)
                    {
                        //you and your enemy hit eachother based on speed.
                        if (enemies.BaseSpeed > yours.BaseSpeed)
                        {
                            PrintEnemy($"{enemies.Species} ", $"used {enemies.Moves[enemiesMovePick].Name}!");
                            int damage = int.Parse(enemies.Moves[enemiesMovePick].Damage);
                            int modifier = (yours.TypeWeaknesses.Count(x => x == enemies.Moves[enemiesMovePick].Type) + 1);
                            if (modifier > 1)
                            {
                                PrintEnemy("", $"Your enemy's pokemon's attack was super effective! It hit for {damage * modifier} damage!");
                            }
                            else
                            {
                                PrintEnemy("", $"Your enemy's pokemon's attack hit for {damage * modifier} damage.");
                            }
                            yours.ModifyHealth(-damage * modifier);
                            if (yours.IsAlive)
                            {
                                PrintYou($"{yours.Species} ", $"used {yours.Moves[yourMovePick].Name}!");
                                damage = int.Parse(yours.Moves[yourMovePick].Damage);
                                modifier = (enemies.TypeWeaknesses.Count(x => x == yours.Moves[yourMovePick].Type) + 1);
                                if (modifier > 1)
                                {
                                    PrintYou("", $"Your pokemon's attack was super effective! It hit for {damage * modifier} damage!");
                                }
                                else
                                {
                                    PrintYou("", $"Your pokemon's attack hit for {damage * modifier} damage!");
                                }
                                enemies.ModifyHealth(-damage * modifier);
                            }
                        }
                        else if (yours.BaseSpeed > enemies.BaseSpeed)
                        {
                            PrintYou($"{yours.Species} ", $"used {yours.Moves[yourMovePick].Name}!");
                            int damage = int.Parse(yours.Moves[yourMovePick].Damage);
                            int modifier = (enemies.TypeWeaknesses.Count(x => x == yours.Moves[yourMovePick].Type) + 1);
                            if (modifier > 1)
                            {
                                PrintYou("", $"Your pokemon's attack was super effective! It hit for {damage * modifier} damage!");
                            }
                            else
                            {
                                PrintYou("", $"Your pokemon's attack hit for {damage * modifier} damage!");
                            }
                            enemies.ModifyHealth(-damage * modifier);
                            if (enemies.IsAlive)
                            {
                                PrintEnemy($"{enemies.Species} ", $"used {enemies.Moves[enemiesMovePick].Name}!");
                                damage = int.Parse(enemies.Moves[enemiesMovePick].Damage);
                                modifier = (yours.TypeWeaknesses.Count(x => x == enemies.Moves[enemiesMovePick].Type) + 1);
                                if (modifier > 1)
                                {
                                    PrintEnemy("", $"Your enemy's pokemon's attack was super effective! It hit for {damage * modifier} damage!");
                                }
                                else
                                {
                                    PrintEnemy("", $"Your enemy's pokemon's attack hit for {damage * modifier} damage.");
                                }
                                yours.ModifyHealth(-damage * modifier);
                            }
                        }
                        else
                        {
                            //if speeds are same pick one randomly
                            switch (Grand.rand.Next(0, 2))
                            {
                                case 0:
                                    {
                                        PrintEnemy($"{enemies.Species} ", $"used {enemies.Moves[enemiesMovePick].Name}!");
                                        int damage = int.Parse(enemies.Moves[enemiesMovePick].Damage);
                                        int modifier = (yours.TypeWeaknesses.Count(x => x == enemies.Moves[enemiesMovePick].Type) + 1);
                                        if (modifier > 1)
                                        {
                                            PrintEnemy("", $"Your enemy's pokemon's attack was super effective! It hit for {damage * modifier} damage!");
                                        }
                                        else
                                        {
                                            PrintEnemy("", $"Your enemy's pokemon's attack hit for {damage * modifier} damage.");
                                        }
                                        yours.ModifyHealth(-damage * modifier);
                                        if (yours.IsAlive)
                                        {
                                            PrintYou($"{yours.Species} ", $"used {yours.Moves[yourMovePick].Name}!");
                                            damage = int.Parse(yours.Moves[yourMovePick].Damage);
                                            modifier = (enemies.TypeWeaknesses.Count(x => x == yours.Moves[yourMovePick].Type) + 1);
                                            if (modifier > 1)
                                            {
                                                PrintYou("", $"Your pokemon's attack was super effective! It hit for {damage * modifier} damage!");
                                            }
                                            else
                                            {
                                                PrintYou("", $"Your pokemon's attack hit for {damage * modifier} damage!");
                                            }
                                            enemies.ModifyHealth(-damage * modifier);
                                        }
                                    }
                                    break;
                                case 1:
                                    {
                                        PrintYou($"{yours.Species} ", $"used {yours.Moves[yourMovePick].Name}!");
                                        int damage = int.Parse(yours.Moves[yourMovePick].Damage);
                                        int modifier = (enemies.TypeWeaknesses.Count(x => x == yours.Moves[yourMovePick].Type) + 1);
                                        if (modifier > 1)
                                        {
                                            PrintYou("", $"Your pokemon's attack was super effective! It hit for {damage * modifier} damage!");
                                        }
                                        else
                                        {
                                            PrintYou("", $"Your pokemon's attack hit for {damage * modifier} damage!");
                                        }
                                        enemies.ModifyHealth(-damage * modifier);
                                        if (enemies.IsAlive)
                                        {
                                            PrintEnemy($"{enemies.Species} ", $"used {enemies.Moves[enemiesMovePick].Name}!");
                                            damage = int.Parse(enemies.Moves[enemiesMovePick].Damage);
                                            modifier = (yours.TypeWeaknesses.Count(x => x == enemies.Moves[enemiesMovePick].Type) + 1);
                                            if (modifier > 1)
                                            {
                                                PrintEnemy("", $"Your enemy's pokemon's attack was super effective! It hit for {damage * modifier} damage!");
                                            }
                                            else
                                            {
                                                PrintEnemy("", $"Your enemy's pokemon's attack hit for {damage * modifier} damage.");
                                            }
                                            yours.ModifyHealth(-damage * modifier);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    else if (yourMovePick == 4)
                    {
                        Console.WriteLine("Choose your pokemon: (1 - 6) " + string.Join(", ", you.Pokemon.Select(x => x.Species + " " + ((float)x.ActingHP / (float)x.BaseHP) + "%HP")));
                        int pokePick = int.Parse(Console.ReadLine()) - 1;
                        do
                        {
                            if (you.Pokemon[pokePick] == yours)
                            {
                                Console.WriteLine("That pokemon is already out! Pick a different one. (1 - 6)");
                            }
                            else if (!you.Pokemon[pokePick].IsAlive)
                            {
                                Console.WriteLine("That pokemon has been knocked out! Pick a different one. (1 - 6)");
                            }
                            else
                            {
                                yours = you.Pokemon[pokePick];
                                break;
                            }
                        } while (true);
                        yours.ModifyHealth(-int.Parse(enemies.Moves[enemiesMovePick].Damage) * (yours.TypeWeaknesses.Count(x => x == enemies.Moves[enemiesMovePick].Type) + 1));
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                    if (!enemies.IsAlive)
                    {
                        IEnumerable<Pokemon> temp = enemy.Pokemon.Where(x => x.IsAlive);
                        enemies = temp.ElementAtOrDefault(Grand.rand.Next(0, temp.Count()));
                    }
                    if (!yours.IsAlive)
                    {
                        PrintYou($"{yours.Species} ", "was knocked out! Choose a new pokemon: (1 - 6) " + string.Join(", ", you.Pokemon.Select(x => x.Species + " " + ((float)x.ActingHP / (float)x.BaseHP) + "%HP")));
                        int pokePick = int.Parse(Console.ReadLine()) - 1;
                        do
                        {
                            if (you.Pokemon[pokePick] == yours)
                            {
                                Console.WriteLine("That pokemon is already out! Pick a different one. (1 - 6)");
                            }
                            else if (!you.Pokemon[pokePick].IsAlive)
                            {
                                Console.WriteLine("That pokemon has been knocked out! Pick a different one. (1 - 6)");
                            }
                            else
                            {
                                yours = you.Pokemon[pokePick];
                                break;
                            }
                        } while (true);
                    }
                    if (!enemy.Pokemon.Any(x => x.IsAlive))
                    {
                        PrintYou("You win! ", "Would you like to play again? (Y/N)");
                        gaming = false;
                    }
                    else if (!you.Pokemon.Any(x => x.IsAlive))
                    {
                        PrintEnemy("You lose. ", "Would you like to play again? (Y/N)");
                        gaming = false;
                    }
                }
                bool quit = false;
                do
                {
                    string reply = Console.ReadLine();
                    if (Grand.yes.IsMatch(reply))
                    {
                        gaming = true;
                        you = enemy = null;
                        yours = enemies = null;
                        quit = false;
                        Console.Clear();
                        break;
                    }
                    else if (Grand.no.IsMatch(reply))
                    {
                        gaming = false;
                        quit = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Hey! That's not a valid response. Say yes or no.");
                    }
                }
                while (true);
                if (quit)
                {
                    break;
                }
            }
            while (true);
        }
    }
}