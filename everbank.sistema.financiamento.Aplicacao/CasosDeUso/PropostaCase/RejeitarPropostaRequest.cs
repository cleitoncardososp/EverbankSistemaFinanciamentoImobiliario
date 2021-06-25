using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.RaizAgregacao;
using MediatR;

namespace Aplicacao.CasosDeUso.PropostaCase
{
    public class RejeitarPropostaRequest : IRequest<RejeitarPropostaResponse>
    {
        public string IdProposta {get; set;}

        public string Motivo {get; set;}
    }
    public class RejeitarPropostaHandler : IRequestHandler<RejeitarPropostaRequest, RejeitarPropostaResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        
        public RejeitarPropostaHandler(IPropostaRepositorio propostaRepositorio)
        {
            PropostaRepositorio = propostaRepositorio;
        }

        public Task<RejeitarPropostaResponse> Handle(RejeitarPropostaRequest request, CancellationToken cancellationToken)
        {
            Proposta proposta = PropostaRepositorio.ConsultarProposta(request.IdProposta);

            proposta.RejeitarProposta(request.Motivo);

            PropostaRepositorio.Atualizar(proposta);

            return Task.FromResult(new RejeitarPropostaResponse(){Status=0 , Data = proposta});
        }
    }
    public class RejeitarPropostaResponse
    {
        public int Status{get;set;}
        public Proposta Data {get; set;}
    }
}
