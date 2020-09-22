using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PokeAPI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Web.Server.Classes;
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

        [HttpGet("allnames")] // https://localhost:44392/api/pokemon/allnames
        public List<string> AllPokemonNames()
        {
            var names = GetAllPokemonNames();
            return names;
        }

        [HttpPost("deletelineup")] // https://localhost:44392/api/pokemon/deletelineup
        public HttpResponseMessage DeleteLineup([FromBody] int teamId)
        {
            DBConnect.ExecuteNonQuery($"DELETE FROM TEAMS WHERE TeamId = {teamId};");
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
        }

        [HttpPost("lineup")] // https://localhost:44392/api/pokemon/lineup
        public HttpResponseMessage UpdateLineup([FromBody] TrainerModel trainer)
        {
            UpdateLineups(trainer);
            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
        }

        [HttpPost("score/update")] // https://localhost:44392/api/pokemon/lineup?trainername=srosy
        public HttpResponseMessage UpdateScore([FromBody] TrainerModel trainer)
        {
            UpdateHighScore(trainer);
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }

        [HttpPost("avatar/update")]
        public HttpResponseMessage UpdateAvatar([FromBody] TrainerModel trainer)
        {
            DoUpdateAvater(trainer);
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

            var query = "SELECT TOP 5 t.TrainerHandle FROM Trainers t JOIN Scores s on t.TrainerId = s.TrainerId ORDER BY s.Score DESC;";
            using (var conn = DBConnect.BuildSqlConnection())
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                var rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    elite4PlusChampionStrings.Add(rdr[0].ToString());
                }
            }

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

            var query = "SELECT DISTINCT TOP 10 t.TrainerHandle, s.Score FROM Trainers t JOIN Scores s on t.TrainerId = s.TrainerId ORDER BY s.Score DESC;";
            using (var conn = DBConnect.BuildSqlConnection())
            {
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                var rdr = command.ExecuteReader();
                while (rdr.Read())
                {
                    topTenNames.Add(rdr[0].ToString());
                }
            }

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

            if (trainer.Team.TeamId <= 0)
            {
                // new team to be saved
                var nextTeamId = DBConnect.ExecuteScalar($"SELECT TOP 1 TeamId FROM Teams;");
                trainer.Team.TeamId = ++nextTeamId;
                var json = JsonConvert.SerializeObject(trainer.Team);
                var sql = $"INSERT INTO Teams(TrainerId, JSON) VALUES({trainer.Id}, '{json}');";
                DBConnect.ExecuteNonQuery(sql);
            }

            trainer.Lineups.ForEach(t =>
            {
                if (t.TeamId > 0 && t.TeamId != trainer.Team.TeamId)
                {
                    // new team to be saved
                    var json = JsonConvert.SerializeObject(t);
                    var sql = $"UPDATE Teams SET JSON = '{json}' WHERE TeamId = {t.TeamId};";
                    DBConnect.ExecuteNonQuery(sql);
                }
            });
        }

        public void UpdateHighScore(TrainerModel trainer)
        {
            var query = $"INSERT INTO Scores(TrainerId, Score, TeamId) VALUES ({trainer.Id}, {trainer.HighScore}, {trainer.Team.TeamId});";
            DBConnect.ExecuteNonQuery(query);
        }

        public void DoUpdateAvater(TrainerModel trainer)
        {
            var query = $"UPDATE Trainers SET AvatarUrl = '{trainer.AvatarUrl}' WHERE TrainerId = {trainer.Id};";
            DBConnect.ExecuteNonQuery(query);
        }

        public TrainerModel GetTrainerFromDB(string name)
        {
            var trainer = new TrainerModel()
            {
                Handle = name
            };

            try
            {
                trainer.Id = GetTrainerId(name);

                if (trainer.Id <= 0)
                {
                    CreateNewTrainer(name);
                    trainer.Id = GetTrainerId(name);
                    var userId = DBConnect.ExecuteScalar($"SELECT UserId FROM Users WHERE UserName = '{name}';");
                    DBConnect.ExecuteNonQuery($"UPDATE Users SET TrainerId = {trainer.Id} WHERE UserId = {userId};");
                }

                var query = $"SELECT * FROM Trainers WHERE(TrainerHandle = '{name}');";
                using (var conn = DBConnect.BuildSqlConnection())
                {
                    SqlCommand command = new SqlCommand(query, conn);
                    conn.Open();
                    var rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        trainer.Id = int.Parse(rdr[0].ToString());
                        trainer.Handle = rdr[1].ToString();
                        trainer.CurrentTeamId = !string.IsNullOrEmpty(rdr[2].ToString()) ? int.Parse(rdr[2].ToString()) : 0;
                        trainer.HighScore = GetHighScore(trainer.Id);
                        trainer.Lineups = Lineup.DeserializeLineupList(Lineup.GetLineups(trainer));
                        trainer.Team = Lineup.DeserializeLineupList(Lineup.GetLineups(trainer)).FirstOrDefault();
                        trainer.AvatarUrl = !string.IsNullOrEmpty(rdr[4].ToString()) ? rdr[4].ToString() : string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing query: {ex.Message}");
            }

            return trainer;
        }

        private int GetTrainerId(string name)
        {
            try
            {
                var trainerId = DBConnect.ExecuteScalar($"SELECT TrainerId FROM Trainers WHERE TrainerHandle = '{name}';");
                return trainerId;
            }
            catch { }

            return 0;
        }

        private int GetHighScore(int id)
        {
            var query = $"SELECT TOP 1 Score from Scores WHERE TrainerId = {id} ORDER BY Score DESC;";
            var score = DBConnect.ExecuteScalar(query);
            return score;
        }

        public List<string> GetAllPokemonNames()
        {
            var query = $"SELECT Data FROM Files WHERE FileId = 1;";
            var result = DBConnect.GetSingleString(query);
            var names = result.Split(',').ToList();

            return names;
        }

        public IDictionary<string, object> GetAdditionInfo(string uri)
        {
            var client = new RestClient(uri);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            var obj = (IDictionary<string, object>)JsonConvert.DeserializeObject<ExpandoObject>(response.Content, new ExpandoObjectConverter());
            return obj;
        }

        internal static void CreateNewTrainer(string username)
        {
            DBConnect.ExecuteNonQuery($"INSERT INTO Trainers(TrainerHandle, CurrentTeamId) VALUES('{username}', 0);");
        }
    }
}
