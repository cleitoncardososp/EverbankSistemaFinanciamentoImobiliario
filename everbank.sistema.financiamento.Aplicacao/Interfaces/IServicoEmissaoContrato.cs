using System;
using Dominio.RaizAgregacao;

namespace Aplicacao.Interfaces
{
    public interface IServicoEmissaoContrato
    {
        void EmitirContratoProposta(Proposta proposta);
    }
}
