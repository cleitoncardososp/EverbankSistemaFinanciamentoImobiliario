using Dominio.RaizAgregacao;
using Dominio.Entidades;

namespace Aplicacao.Interfaces
{
    public interface IPropostaRepositorio
    {
        void InserirProposta(Proposta proposta);
        Proposta ConsultarProposta(string IdProposta);
        void InserirProponente(Proponente proponente);
        void Atualizar(Proposta proposta);
                
    }
}
