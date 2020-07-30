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
using System.Net.Http;
using System.Net;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

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

        [HttpPost("lineup")] // https://localhost:44392/api/pokemon/lineup
        public HttpResponseMessage UpdateLineup([FromBody] TrainerModel trainer)
        {
            UpdateLineups(trainer);
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
        }

        [HttpPost("trainer/update")] // https://localhost:44392/api/pokemon/lineup?trainername=srosy
        public HttpResponseMessage UpdateTrainer([FromBody] TrainerModel trainer)
        {
            //CreateLineupDB(trainername, lineupJson);
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        [HttpPost("score/update")] // https://localhost:44392/api/pokemon/lineup?trainername=srosy
        public HttpResponseMessage UpdateScore([FromBody] TrainerModel trainer)
        {
            UpdateHighScore(trainer);
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        [HttpGet("trainer/{name}")] // https://localhost:44392/api/pokemon/trainer/srosy
        public TrainerModel GetTrainer(string name)
        {
            var trainer = GetTrainerFromDB(name.Replace("\"", string.Empty));
            if (trainer.Lineups == null) trainer.Lineups = new List<LineupModel>();
            if (trainer.Team == null) trainer.Team = new LineupModel();
            return trainer;
        }

        [HttpGet("trainer/elite4")] // https://localhost:44392/api/pokemon/trainer/elite4
        public List<TrainerModel> GetElite4()
        {
            List<TrainerModel> elite4AndChampion = new List<TrainerModel>();
            elite4AndChampion = GetElite4AndChampion();
            return elite4AndChampion;
        }

        [HttpGet("trainer/topten")] // https://localhost:44392/api/pokemon/trainer/topten
        public List<TrainerModel> GetTopTen()
        {
            List<TrainerModel> topTen = new List<TrainerModel>();
            topTen = GetTopTenTrainers();
            return topTen;
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
        public IDictionary<string, object> GetInfo(string uri)
        {
            var infoObj = GetAdditionInfo(uri);
            return infoObj;
        }

        // get all info on a pokemon needed for web app, models from PokeApi.Pokemon to Web.PokemonModel
        [HttpGet] // https://localhost:44392/api/pokemon?name=charizard
        public async Task<PokemonModel> GetPokemon([FromQuery] string name)
        {
            var client = new RestClient($"https://pokeapi.co/api/v2/pokemon/{name.ToLower()}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(response.Content, new ExpandoObjectConverter());
            //var obj = await DataFetcher.GetNamedApiObject<Pokemon>(name.ToLower()); // wrapper stopped working 7/29/2020

            // create PokemonModel from PokeApi.Pokemon
            var pokemon = new PokemonModel();
            pokemon.Id = Convert.ToInt32(obj.id);
            pokemon.Name = obj.name.ToString();
            pokemon.BackImageUri = obj.sprites.back_default.ToString();
            pokemon.FrontImageUri = obj.sprites.front_default.ToString();

            //0 hp, 1 attack, 2 defense, 3 special attack, 4 special defense, 5 speed
            pokemon.BaseHP = (int)obj.stats[0].base_stat * 5;
            pokemon.ActingHP = (int)pokemon.BaseHP;
            pokemon.Attack = (int)obj.stats[1].base_stat;
            pokemon.Defense = (int)obj.stats[2].base_stat;
            pokemon.SpecialAttack = (int)obj.stats[3].base_stat;
            pokemon.SpecialDefense = (int)obj.stats[4].base_stat;
            pokemon.Speed = (int)obj.stats[5].base_stat;

            pokemon.Moves = DoParallelMoves((List<object>)obj.moves);
            pokemon.Moves.RemoveAll(m => m.Damage == 0);
            pokemon.Moves = pokemon.Moves.OrderByDescending(m => m.Damage).ToList();

            // add any types to the pokemon
            foreach (var t in obj.types)
            {
                var type = ((dynamic)t).type;
                pokemon.Types.Add(new Shared.Models.PokemonType
                {
                    Name = type.name.ToString(),
                    ResourceUri = type.url.ToString()
                });
            }

            return pokemon;
        }

        private List<MoveModel> DoParallelMoves(List<object> moves, [Optional] List<MoveModel> movesOut)
        {
            if (movesOut == null) movesOut = new List<MoveModel>();

            var tasks = new List<Task<MoveModel>>();
            moves.ToList().ForEach(
            m =>
            {
                Task<MoveModel> task = Task.Run(() => ProcessMove(m));
                tasks.Add(task);
            });
            Task.WaitAll(tasks.ToArray());
            tasks.ForEach(t =>
            {
                if (!movesOut.Any(x => x.Id == t.Result.Id))
                    movesOut.Add(t.Result);
            });

            return movesOut;
        }

        private MoveModel ProcessMove(dynamic m)
        {
            var move = ((dynamic)m).move;
            var newMove = new MoveModel();
            newMove.Name = move.name.ToString();
            newMove.ResourceUri = move.url.ToString();

            var info = GetAdditionInfo(newMove.ResourceUri);
            if (info != null && info.Count > 0)
            {
                newMove.Id = info.ContainsKey("id") ? int.Parse(info?["id"].ToString()) : 0;
                newMove.Damage = info.ContainsKey("power") && info?["power"] != null ? int.Parse(info?["power"].ToString()) : 0;
                newMove.Category = info.ContainsKey("damage_class") && info?["damage_class"] != null ? ((ExpandoObject)info?["damage_class"]).First().Value.ToString() : string.Empty;
                newMove.Type = info.ContainsKey("type") ? ((ExpandoObject)info?["type"]).First().Value.ToString() : string.Empty;
            }

            if (newMove.Type == null)
            {
                ProcessMove(m);
            }
            return newMove;
        }

        public List<TrainerModel> GetElite4AndChampion()
        {
            var elite4PlusChampionStrings = new List<string>();
            var con = new DBConnect().MyConnection;
            con.Open();
            var query = "SELECT TrainerName FROM sql3346222.userCredentials ORDER BY HighScore DESC LIMIT 5;";
            var rdr = new MySqlCommand(query, con).ExecuteReader();

            while (rdr.Read())
            {
                elite4PlusChampionStrings.Add(rdr[0].ToString());
            }
            rdr.Close();
            con.Close();

            var elite4PlusChampion = new List<TrainerModel>();
            elite4PlusChampionStrings.ForEach(t =>
            {
                elite4PlusChampion.Add(GetTrainerFromDB(t));
            });

            return elite4PlusChampion;
        }

        public List<TrainerModel> GetTopTenTrainers()
        {
            var topTenNames = new List<string>();
            var con = new DBConnect().MyConnection;
            con.Open();
            var query = "SELECT TrainerName FROM sql3346222.userCredentials ORDER BY HighScore DESC LIMIT 10;";
            var rdr = new MySqlCommand(query, con).ExecuteReader();

            while (rdr.Read())
            {
                topTenNames.Add(rdr[0].ToString());
            }
            rdr.Close();
            con.Close();

            var topTenTrainers = new List<TrainerModel>();
            topTenNames.ForEach(t =>
            {
                topTenTrainers.Add(GetTrainerFromDB(t));
            });

            return topTenTrainers;
        }

        public void UpdateLineups(TrainerModel trainer)
        {
            if (trainer.Lineups.Any(l => l.Checked == true))
                trainer.Lineups.Where(l => l.Checked == true).Select(l => l).ToList().ForEach(l => { l.Checked = false; }); // reset the check

            var con = new DBConnect().MyConnection;
            con.Open();

            var json = JsonConvert.SerializeObject(trainer.Team);
            var querystring = $"UPDATE sql3346222.userCredentials SET CurrentLineup = '{json}' WHERE TrainerName= '{trainer.Handle}';";
            new MySqlCommand(querystring, con).ExecuteNonQuery();

            json = JsonConvert.SerializeObject(trainer.Lineups);
            querystring = $"UPDATE sql3346222.userCredentials SET Lineups = '{json}' WHERE TrainerName= '{trainer.Handle}';";
            new MySqlCommand(querystring, con).ExecuteNonQuery();

            con.Close();
        }

        public void UpdateHighScore(TrainerModel trainer)
        {
            var con = new DBConnect().MyConnection;
            con.Open();

            var querystring = $"UPDATE sql3346222.userCredentials SET HighScore = {trainer.HighScore} WHERE TrainerName= '{trainer.Handle}';";
            new MySqlCommand(querystring, con).ExecuteNonQuery();

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
                trainer.Team = Lineup.DeserializeLineup(rdr[6].ToString());
            }
            rdr.Close();
            con.Close();
            return trainer;
        }

        public List<string> GetAllPokemonNames()
        {
            var names = new List<string>();

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

        public IDictionary<string, object> GetAdditionInfo(string uri)
        {
            var client = new RestClient(uri);
            var request = new RestRequest(Method.GET);
            var response =  client.Execute(request);

            var obj = (IDictionary<string, object>)JsonConvert.DeserializeObject<ExpandoObject>(response.Content, new ExpandoObjectConverter());
            return obj;
        }
    }
}
