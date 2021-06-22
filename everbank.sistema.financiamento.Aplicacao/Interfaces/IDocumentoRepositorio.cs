using System;
using System.Collections.Generic;
using Dominio.Entidades;

namespace Aplicacao.Interfaces
{
    public interface IDocumentoRepositorio
    {
        Documento Consultar(string IdProponente, string IdDocumento);
        
        void Atualizar(Documento documento);

        void Apagar(String id);

        void Inserir(Documento documento);

        List<Documento> BuscarPorProponente(String proponenteId);
    }
}
