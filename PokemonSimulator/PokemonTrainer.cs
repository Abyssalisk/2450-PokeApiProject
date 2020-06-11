using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonSimulator
{
    class PokemonTrainer
    {
        public int userID;
        public string TrainerName;
        public string Pokemon1;
        public string Pokemon2;
        public string Pokemon3;
        public string Pokemon4;
        public string Pokemon5;
        public string Pokemon6;
        public PokemonTrainer(int userID, string TrainerName)
        {
            this.userID = userID;
            this.TrainerName = TrainerName;
        }

        //getters
        public string getTrainerName()
        {
            return this.TrainerName;
        }
        public string getPokemon1()
        {
            return this.Pokemon1;
        }

        public string getPokemon2()
        {
            return this.Pokemon2;
        }
        public string getPokemon3()
        {
            return this.Pokemon3;
        }
        public string getPokemon4()
        {
            return this.Pokemon4;
        }
        public string getPokemon5()
        {
            return this.Pokemon5;
        }
        public string getPokemon6()
        {
            return this.Pokemon6;
        }

        //setters
        public void setPokemon1(string newPokemon)
        {
            this.Pokemon1 = newPokemon;
        }
        public void setPokemon2(string newPokemon)
        {
            this.Pokemon2 = newPokemon;
        }
        public void setPokemon3(string newPokemon)
        {
            this.Pokemon3 = newPokemon;
        }
        public void setPokemon4(string newPokemon)
        {
            this.Pokemon4 = newPokemon;
        }
        public void setPokemon5(string newPokemon)
        {
            this.Pokemon5 = newPokemon;
        }
        public void setPokemon6(string newPokemon)
        {
            this.Pokemon6 = newPokemon;
        }
    }
}
