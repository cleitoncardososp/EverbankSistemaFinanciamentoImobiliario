using System;
using Aplicacao.Interfaces;

namespace Infraestrutura.Services
{
    public class ServicoStorage : IServicoStorage
    {
        public string SalvarArquivo(string nomeArquivo, byte[] conteudoArquivo)
        {
            return nomeArquivo;
        }
    }
}
