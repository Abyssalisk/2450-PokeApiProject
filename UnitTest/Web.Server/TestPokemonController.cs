using RestSharp;
using Web.Shared.Models;
using Xunit;
using FluentAssertions.Common;
using Nancy.Json;

namespace UnitTest.Web.Server
{
    public class TestPokemonController
    {
        public RestClient Client { get; set; }
        public RestRequest Request { get; set; }
        public IRestResponse Response { get; set; }
        public string BaseUrl = "https://webserver20200610162320.azurewebsites.net/api/Pokemon/name/";
        JavaScriptSerializer JS = new JavaScriptSerializer();

        [Fact]
        public void GetPokemonByName()
        {
            var testPokemon = new PokemonModel() { Name = "Charizard" };
            Client = new RestClient(BaseUrl + "charizard");
            Request = new RestRequest(Method.GET);
            Response = Client.ExecuteAsync(Request).Result;
            var pokemon = JS.Deserialize<PokemonModel>(Response.Content);

            pokemon.IsSameOrEqualTo(testPokemon);
        }
    }
}
