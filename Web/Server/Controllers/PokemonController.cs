using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using Web.Shared.Models;

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        // GET api/<PokemonController>/5
        [HttpGet("id/{id}")]
        public Pokemon Get(int id)
        {
            // use map here or hit db, idc
            if (id == 1) return new Pokemon() { Name = "Gyrados" };
            if (id == 2) return new Pokemon() { Name = "Charizard" };
            if (id == 3) return new Pokemon() { Name = "Articuno" };
            return new Pokemon() { Name = "SomeOtherPokemon" };
        }

        // GET api/<PokemonController>/name=
        [HttpGet("name/{name}")]
        public async Task<Pokemon> Get(string name)
        {
            var client = new RestClient("https://pokeapi.co/api/v2/pokemon/" + name);
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteAsync(request, new CancellationToken());

            var obj = (IDictionary<string, object>)JsonConvert.DeserializeObject<ExpandoObject>(response.Content, new ExpandoObjectConverter());
            return new Pokemon() { 
                Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(obj[obj.Keys.First(k => k.ToLower().Equals("name"))].ToString())
            };
        }

        // POST api/<PokemonController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PokemonController>/id
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PokemonController>/id
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
