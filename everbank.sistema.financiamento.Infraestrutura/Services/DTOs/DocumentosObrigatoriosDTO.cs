using System;
using Newtonsoft.Json;

namespace Infraestrutura.Services.DTOs
{
    public class DocumentosObrigatoriosDTO
    {
        [JsonProperty("documento_id")]
        public String IdDocumento{get;set;}

        [JsonProperty("nome_documento")]
        public String NomeDocumento{get;set;}

        [JsonProperty("descricao_documento")]
        public String DescricaoDocumento{get;set;}
    }
}
