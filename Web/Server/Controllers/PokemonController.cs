using System.IO;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PokeAPI;
using RestSharp;
using Web.Server.Classes;
using Web.Shared.Models;
using Microsoft.VisualBasic.FileIO;
using Web.Client;
using System;

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
        
        [HttpGet("allnames")] // https://localhost:44392/api/pokemon/allnames
        public List<string> AllPokemonNames()
        {
            var names = GetAllPokemonNames();
            return names;
        }

        [HttpPost("lineup")] // https://localhost:44392/api/pokemon/lineup?trainername=srosy
        public void NewLineup([FromQuery] string trainername, [FromBody] string lineupJson)
        {
            CreateLineupDB(trainername, lineupJson);
        }

        [HttpGet("trainer/{name}")] // https://localhost:44392/api/pokemon/trainer/srosy
        public TrainerModel GetTrainer(string name)
        {
            var trainer = GetTrainerFromDB(name.Replace("\"", string.Empty));
            return trainer;
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
            var obj = await DataFetcher.GetNamedApiObject<Pokemon>(name);
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
            var obj = await DataFetcher.GetNamedApiObject<Pokemon>(name.ToLower()); // wrapper can't handle any uppercase letters, wah wah

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

                if (info != null && info.Count > 0)
                {
                    m.Id = info.ContainsKey("id") ? int.Parse(info?["id"].ToString()) : 0;
                    m.Damage = info.ContainsKey("power") && info?["power"] != null ? int.Parse(info?["power"].ToString()) : 0;
                    m.Category = info.ContainsKey("damage_class") && info?["damage_class"] != null ? ((ExpandoObject)info?["damage_class"]).First().Value.ToString() : string.Empty;
                    m.Type = info.ContainsKey("type") ? ((ExpandoObject)info?["type"]).First().Value.ToString() : string.Empty;
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

        public void CreateLineupDB(string trainername, string lineupJson)
        {
            var con = new DBConnect().MyConnection;
            con.Open();
            var querystring = $"INSERT INTO sql3346222.userCredentials(TrainerName, Team) VALUES ('{trainername}', '{lineupJson}')";
            MySqlCommand cmd = new MySqlCommand(querystring, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public TrainerModel GetTrainerFromDB(string name)
        {
            var trainer = new TrainerModel()
            {
                Handle = name
            };

            var con = new DBConnect().MyConnection;
            con.Open();
            var querystring = $"SELECT * FROM sql3346222.userCredentials WHERE(TrainerName = '{name}');";
            MySqlCommand cmd = new MySqlCommand(querystring, con);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                trainer.Id = int.Parse(rdr[0].ToString());
                trainer.HighScore = int.Parse(rdr[5].ToString());
                trainer.Lineups = Lineup.DeserializeLineupList(rdr[4].ToString());
            }
            rdr.Close();
            con.Close();
            return trainer;
        }

        public List<PokemonModel> GetLineUp(string trainerName)
        {
            //todo @derek
            return null;
        }

        public List<string> GetAllPokemonNames()
        {
            var names = new List<string>();
            //using (TextFieldParser parser = new TextFieldParser(Environment.CurrentDirectory + @"\Data\PokemonNames.csv"))
            //using (TextFieldParser parser = new TextFieldParser(ApplicationDeployment.CurrentDeployment.DataDirectory + @"\Data\PokemonNames.csv"))
            //{
            //    parser.TextFieldType = FieldType.Delimited;
            //    parser.SetDelimiters(",");
            //    while (!parser.EndOfData)
            //    {
            //        names = new List<string>(parser.ReadFields());
            //    }
            //}

            var con = new DBConnect().MyConnection;
            con.Open();
            var querystring = $"SELECT FileContent FROM sql3346222.Files WHERE FileName='AllPokemonGen1CSV'";
            MySqlCommand cmd = new MySqlCommand(querystring, con);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                names = rdr[0].ToString().Split(',').ToList();
            }
            rdr.Close();
            con.Close();

            return names;
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
