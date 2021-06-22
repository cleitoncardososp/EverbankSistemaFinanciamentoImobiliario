using System;
using Newtonsoft.Json;

namespace Infraestrutura.Services.DTOs
{
    public class TaxaJurosDTO
    {
        [JsonProperty("taxa_juros")]
        public Decimal Taxa{get;set;}
    }
}
