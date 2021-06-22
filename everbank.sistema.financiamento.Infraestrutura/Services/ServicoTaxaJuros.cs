using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Infraestrutura.Services.DTOs;
using Newtonsoft.Json;

namespace Infraestrutura.Services
{
    public class ServicoTaxaJuros : IServicoTaxaJuros
    {
        public decimal ObterTaxaJuros()
        {
            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", "C5if5BF3WZ3o7EK6o8YthHlBVPejy5j4aEeDyh00");
                Task<HttpResponseMessage> responseTask = client.GetAsync("https://2qiaaxu5yf.execute-api.sa-east-1.amazonaws.com/prod/juros-cliente");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;            
                responseTask.Wait();

                var taxa = response.Content.ReadAsStringAsync().Result; //recebe um json
                
                TaxaJurosDTO taxaJurosDto = JsonConvert.DeserializeObject<TaxaJurosDTO>(taxa); //desserialização
                
                decimal taxa_juros = taxaJurosDto.Taxa;  // convertendo os tipos

                return taxa_juros;
            }
        }
    }
}