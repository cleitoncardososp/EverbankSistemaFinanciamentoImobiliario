using System.Net.Http;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.RaizAgregacao;

namespace Infraestrutura.Services
{
    public class ServicoEmissaoContrato : IServicoEmissaoContrato
    {
        public void EmitirContratoProposta(Proposta proposta)
        {
            /*
            using(HttpClient client = new HttpClient())
            {
                

                client.DefaultRequestHeaders.Add("x-api-key", "C5if5BF3WZ3o7EK6o8YthHlBVPejy5j4aEeDyh00");
                Task<HttpResponseMessage> responseTask = client.PostAsync("https://2qiaaxu5yf.execute-api.sa-east-1.amazonaws.com/prod/contrato");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;            
                responseTask.Wait();

                var taxa = response.Content.ReadAsStringAsync().Result; //recebe um json
                
                TaxaJurosDTO taxaJurosDto = JsonConvert.DeserializeObject<TaxaJurosDTO>(taxa); //desserialização
                
                decimal taxa_juros = taxaJurosDto.Taxa;  // convertendo os tipos

            }
            */
        }
    }
}