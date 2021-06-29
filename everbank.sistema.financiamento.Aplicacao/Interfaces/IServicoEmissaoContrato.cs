using System;
using Dominio.RaizAgregacao;

namespace Aplicacao.Interfaces
{
    public interface IServicoEmissaoContrato
    {
        Object EmitirContratoProposta(Proposta proposta);
    }
}
