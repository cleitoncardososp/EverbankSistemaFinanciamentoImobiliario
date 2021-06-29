using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.RaizAgregacao;
using Infraestrutura.Services.DTOs;
using Newtonsoft.Json;

namespace Infraestrutura.Services
{
    public class ServicoEmissaoContrato : IServicoEmissaoContrato
    {
        public Object EmitirContratoProposta(Proposta proposta)
        {
            using(HttpClient client = new HttpClient())
            {
                string output = JsonConvert.SerializeObject(proposta);

                client.DefaultRequestHeaders.Add("x-api-key", "C5if5BF3WZ3o7EK6o8YthHlBVPejy5j4aEeDyh00");
                Task<HttpResponseMessage> responseTask = client.PostAsJsonAsync("https://2qiaaxu5yf.execute-api.sa-east-1.amazonaws.com/prod/contrato", output);
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;            
                responseTask.Wait();

                string retornado = response.Content.ReadAsStringAsync().Result; // recebe um json

                EmissaoContratoDTO mensagem = JsonConvert.DeserializeObject<EmissaoContratoDTO>(retornado); //desserialização

                return mensagem;
            }
        }
    }
}