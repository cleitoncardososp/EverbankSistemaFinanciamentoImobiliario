using System;
using System.Collections.Generic;
using Dominio.Entidades;

namespace Aplicacao.Interfaces
{
    public interface IServicoDocumentosObrigatorios
    {
        List<Documento> BuscarDocumentosObrigatorios();
    }
}
