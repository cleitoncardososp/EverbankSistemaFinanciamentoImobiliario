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
        public string IdProposta {get; private set;}
    }
    public class AprovarPropostaHandler : IRequestHandler<AprovarPropostaRequest, AprovarPropostaResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        public AprovarPropostaHandler(IPropostaRepositorio propostaRepositorio)
        {
            PropostaRepositorio = propostaRepositorio;
        }

        public Task<AprovarPropostaResponse> Handle(AprovarPropostaRequest request, CancellationToken cancellationToken)
        {
            Proposta proposta = PropostaRepositorio.ConsultarProposta(request.IdProposta);

            proposta.AprovarProposta();

            PropostaRepositorio.Atualizar(proposta);

            return Task.FromResult(new AprovarPropostaResponse(){Status=0 , Data = proposta});
        }
    }
    public class AprovarPropostaResponse
    {
        public int Status{get;set;}
        public Proposta Data {get; set;}
    }
}
