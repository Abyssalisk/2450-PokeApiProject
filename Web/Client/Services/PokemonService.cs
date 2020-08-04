using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Org.BouncyCastle.Asn1.Ocsp;
using PokeAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Web.Shared.Models;

namespace Web.Client.Services
{
    public interface IPokemonService
    {
        List<PokemonModel> Team { get; set; }
        TrainerModel Trainer { get; set; }
    }

    public class PokemonService : IPokemonService
    {
        public List<PokemonModel> Team { get; set; }
        public TrainerModel Trainer { get; set; }

        public PokemonService() { }


        public async Task<bool> UpdateTrainer(HttpClient client, TrainerModel trainer)
        {
            var result = await client.PostAsJsonAsync<TrainerModel>($"api/pokemon/trainer/update", trainer);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateScore(HttpClient client, TrainerModel trainer)
        {
            var result = await client.PostAsJsonAsync<TrainerModel>($"api/pokemon/score/update", trainer);
            return result.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateLineup(HttpClient client, TrainerModel trainer)
        {
            var result = await client.PostAsJsonAsync<TrainerModel>($"api/pokemon/lineup", trainer);
            return result.IsSuccessStatusCode;
        }

        public async Task<List<string>> GetAllPokemonNames(HttpClient client)
        {
            var result = await client.GetFromJsonAsync<List<string>>($"api/pokemon/allnames");
            return result;
        }

        public async Task<List<TrainerModel>> GetElite4AndChampion(HttpClient client)
        {
            var result = await client.GetFromJsonAsync<List<TrainerModel>>($"api/pokemon/trainer/elite4");
            return result;
        }

        public async Task<List<TrainerModel>> GetTopTenTrainers(HttpClient client)
        {
            var result = await client.GetFromJsonAsync<List<TrainerModel>>($"api/pokemon/trainer/topten");
            return result;
        }

        public async Task<TrainerModel> GetTrainer(HttpClient client, string trainerHandle)
        {
            var result = await client.GetFromJsonAsync<TrainerModel>($"api/pokemon/trainer/{trainerHandle}");
            return result;
        }

        public void CreateTrainer(HttpClient client, string trainerHandle)
        {
            Task.Run( () => client.GetStringAsync($"api/pokemon/trainer/create/{trainerHandle}"));
        }

        public async Task<PokemonMove[]> GetMoves(HttpClient client, string pokemonname)
        {
            var result = await client.GetFromJsonAsync<PokemonMove[]>($"api/pokemon/moves/{pokemonname}");
            return result;
        }

        public async Task<PokemonModel> GetPokemonByName(HttpClient client, string pokemonname)
        {
            var result = await client.GetFromJsonAsync<PokemonModel>($"api/pokemon?name={pokemonname}");
            return result;
        }

        public async Task<IDictionary<string, object>> GetInfo(HttpClient client, string uri)
        {
            var result = await client.GetFromJsonAsync<IDictionary<string, object>>($"api/pokemon/info/{uri}");
            return result;
        }
    }
}
