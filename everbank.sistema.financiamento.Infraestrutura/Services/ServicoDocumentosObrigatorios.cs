using System.Net.Http;
using System.Collections.Generic;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Infraestrutura.Services.DTOs;

namespace Infraestrutura.Services
{
    public class ServicoDocumentosObrigatorios : IServicoDocumentosObrigatorios
    {
        public List<Documento> BuscarDocumentosObrigatorios()
        {

            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-api-key", "C5if5BF3WZ3o7EK6o8YthHlBVPejy5j4aEeDyh00");
                Task<HttpResponseMessage> responseTask = client.GetAsync("https://2qiaaxu5yf.execute-api.sa-east-1.amazonaws.com/prod/documentos-requeridos");
                responseTask.Wait();

                HttpResponseMessage response = responseTask.Result;
                responseTask.Wait();

                var listaRecebida = response.Content.ReadAsStringAsync().Result;

                List<DocumentosObrigatoriosDTO> listaDocumentosDto = JsonConvert.DeserializeObject<List<DocumentosObrigatoriosDTO>>(listaRecebida);               
                
                List<Documento> listaDocumentos = new List<Documento>();
                foreach (var item in listaDocumentosDto)
                {
                    Documento documento = new Documento(item.NomeDocumento, item.DescricaoDocumento);
                    listaDocumentos.Add(documento);
                }
                
                return listaDocumentos;
            }
        }
    }
}