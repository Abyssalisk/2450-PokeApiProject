using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PokeAPI;
using RestSharp;
using Web.Shared.Models;

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        // GET api/<PokemonController>/id/id 
        [HttpGet("id/{id}")] // just a test endpoint
        public PokemonModel Get(int id)
        {
            // use map here or hit db, idc
            if (id == 1) return new PokemonModel() { Name = "Gyrados" };
            if (id == 2) return new PokemonModel() { Name = "Charizard" };
            if (id == 3) return new PokemonModel() { Name = "Articuno" };
            return new PokemonModel() { Name = "SomeOtherPokemon" };
        }

        // GET api/<PokemonController>/name
        [HttpGet("name/{name}")]
        public async Task<PokemonModel> GetName(string name)
        {
            var client = new RestClient("https://pokeapi.co/api/v2/pokemon/" + name);
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteAsync(request, new CancellationToken());

            var obj = (IDictionary<string, object>)JsonConvert.DeserializeObject<ExpandoObject>(response.Content, new ExpandoObjectConverter());
            return new PokemonModel()
            {
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj[obj.Keys.First(k => k.ToLower().Equals("name"))].ToString())
            };
        }

        [HttpGet("moves/{name}")]
        public async Task<List<string>> GetMoves(string name)
        {
            var obj = var info = await GetAdditionInfo(m.ResourceUri);
            var moves = obj.Moves.Select(m => m.Move.Name).ToList();
            return moves;
        }

        [HttpGet("info/{uri}")] // can be used to get any additional info on types, moves, etc
        public async Task<IDictionary<string, object>> GetInfo(string uri)
        {
            var infoObj = await GetAdditionInfo(uri);
            return infoObj;
        }

        // get all info on a pokemon needed for web app, models from PokeApi.Pokemon to Web.PokemonModel
        [HttpGet] // https://localhost:44392/api/pokemon?name=charizard
        public async Task<PokemonModel> GetPokemon([FromQuery] string name)
        {
            var obj = await DataFetcher.GetNamedApiObject<Pokemon>(name);

            // create PokemonModel from PokeApi.Pokemon
            var pokemon = new PokemonModel()
            {
                Id = obj.ID,
                Name = obj.Name,
                BackImageUri = obj.Sprites.BackMale,
                FrontImageUri = obj.Sprites.FrontMale,

                BaseHP = obj.Stats.Where(s => s.Stat.Name.ToLower().Equals("hp")).Select(s => s.BaseValue).FirstOrDefault(),
                Attack = obj.Stats.Where(s => s.Stat.Name.ToLower().Equals("attack")).Select(s => s.BaseValue).FirstOrDefault(),
                Defense = obj.Stats.Where(s => s.Stat.Name.ToLower().Equals("defense")).Select(s => s.BaseValue).FirstOrDefault(),
                SpecialAttack = obj.Stats.Where(s => s.Stat.Name.ToLower().Equals("special-attack")).Select(s => s.BaseValue).FirstOrDefault(),
                SpecialDefense = obj.Stats.Where(s => s.Stat.Name.ToLower().Equals("special-defense")).Select(s => s.BaseValue).FirstOrDefault(),
                Speed = obj.Stats.Where(s => s.Stat.Name.ToLower().Equals("speed")).Select(s => s.BaseValue).FirstOrDefault()

            };

            // add list of available moves to pokemon
            obj.Moves.Select(m => m.Move).ToList().ForEach(m =>
            {
                pokemon.Moves.Add(
                    new MoveModel()
                    {
                        Name = m.Name,
                        ResourceUri = m.Url.AbsoluteUri.ToString()
                    });
            });
            pokemon.Moves.ForEach(async m =>
            {
                var info = await GetAdditionInfo(m.ResourceUri);

                if (info != null)
                {
                    m.Id = info.ContainsKey("id") ? int.Parse(info["id"].ToString()) : 0;
                    m.Damage = info.ContainsKey("damage") ? int.Parse(info["damage"].ToString()) : 0;
                    m.Category = info.ContainsKey("category") ? info["category"].ToString() : string.Empty;
                    m.Type = info.ContainsKey("type") ? ((ExpandoObject)info["type"]).First().Value.ToString() : string.Empty;
                }
            });

            // add any types to the pokemon
            obj.Types.ToList().ForEach(t =>
            {
                pokemon.Types.Add(new Shared.Models.PokemonType
                {
                    Name = t.Type.Name,
                    ResourceUri = t.Type.Url.AbsoluteUri.ToString()
                });
            });
            
            return pokemon;
        }



        public async Task<IDictionary<string, object>> GetAdditionInfo(string uri)
        {
            var client = new RestClient(uri);
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteAsync(request, new CancellationToken());

            var obj = (IDictionary<string, object>)JsonConvert.DeserializeObject<ExpandoObject>(response.Content, new ExpandoObjectConverter());
            return obj;
        }

    }
}
