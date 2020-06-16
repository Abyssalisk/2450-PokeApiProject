using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
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






    }
}
