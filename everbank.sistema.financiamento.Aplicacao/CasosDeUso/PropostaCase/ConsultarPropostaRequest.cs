using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.RaizAgregacao;
using MediatR;

namespace Aplicacao.CasosDeUso.PropostaCase
{
    public class ConsultarPropostaRequest:IRequest<ConsultarPropostaResponse>
    {
        public string IdProposta {get; set;}
    }

    public class ConsultarPropostaHandler : IRequestHandler<ConsultarPropostaRequest, ConsultarPropostaResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        
        public ConsultarPropostaHandler(IPropostaRepositorio propostaRepositorio)
        {
            PropostaRepositorio = propostaRepositorio;
        }

        public Task<ConsultarPropostaResponse> Handle(ConsultarPropostaRequest request, CancellationToken cancellationToken)
        {
            Proposta proposta = PropostaRepositorio.ConsultarProposta(request.IdProposta);

            return Task.FromResult(new ConsultarPropostaResponse(){Status=0 , Data = proposta});
        }
    }
    public class ConsultarPropostaResponse
    {
        public int Status{get;set;}
        public Proposta Data {get; set;}
    }
}
