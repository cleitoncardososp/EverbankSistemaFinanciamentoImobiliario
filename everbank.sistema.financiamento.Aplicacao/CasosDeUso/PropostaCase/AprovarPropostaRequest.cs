using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.RaizAgregacao;
using MediatR;

namespace Aplicacao.CasosDeUso.PropostaCase
{
    public class AprovarPropostaRequest:IRequest<AprovarPropostaResponse>
    {
        public string IdProposta {get; set;}

    }
    public class AprovarPropostaHandler : IRequestHandler<AprovarPropostaRequest, AprovarPropostaResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        public IServicoEmissaoContrato ServicoEmissaoContrato {get; set;}
        public AprovarPropostaHandler(IPropostaRepositorio propostaRepositorio, IServicoEmissaoContrato servicoEmissaoContrato)
        {
            PropostaRepositorio = propostaRepositorio;
            ServicoEmissaoContrato = servicoEmissaoContrato;
        }

        public Task<AprovarPropostaResponse> Handle(AprovarPropostaRequest request, CancellationToken cancellationToken)
        {
            Proposta proposta = PropostaRepositorio.ConsultarProposta(request.IdProposta);

            proposta.AprovarProposta(); // domínio

            PropostaRepositorio.Atualizar(proposta); // repositorio

            Object mensagem = ServicoEmissaoContrato.EmitirContratoProposta(proposta); // sersvico

            return Task.FromResult(new AprovarPropostaResponse(){Mensagem = mensagem});
        }
    }
    public class AprovarPropostaResponse
    {
        public Object Mensagem {get; set;}
    }
}
