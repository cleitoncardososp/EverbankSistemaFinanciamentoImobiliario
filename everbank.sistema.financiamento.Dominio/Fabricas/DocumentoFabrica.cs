using System;
using Dominio.Entidades;

namespace Dominio.Fabricas
{
    public interface IDocumentoFabrica
    {
        Documento CriarInstancia(string idDocumento, string nome, string descricao, string caminhoArquivo, bool isDocumentoAprovado, string motivoRecusaAprovacao);
    }

    public class DocumentoFabrica : IDocumentoFabrica
    {
        public Documento CriarInstancia(string idDocumento, string nome, string descricao, string caminhoArquivo, bool isDocumentoAprovado, string motivoRecusaAprovacao)
        {
            return new Documento(idDocumento,nome,descricao,caminhoArquivo,isDocumentoAprovado,motivoRecusaAprovacao);
        }
    }
}
