using System;
using Newtonsoft.Json;

namespace Infraestrutura.Services.DTOs
{
    public class EmissaoContratoDTO
    {
        [JsonProperty("Status")]
        public string status {get; set;}

        [JsonProperty("Mensagem")]
        public string mensagem {get; set;}
    }
}
