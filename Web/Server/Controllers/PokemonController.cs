using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Shared.Models;

namespace Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        // GET api/<PokemonController>/5
        [HttpGet("{id}")]
        public Pokemon Get(int id)
        {
            // use map here or hit db, idc
            if (id == 1) return new Pokemon() { Name = "Gyrados" };
            if (id == 2) return new Pokemon() { Name = "Charizard" };
            if (id == 3) return new Pokemon() { Name = "Articuno" };
            return new Pokemon() { Name = "SomeOtherPokemon" };
        }
    }
}
