using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using MediatR;

namespace Aplicacao.CasosDeUso.ProponenteCase
{
    public class RemoverRestricaoProponenteRequest:IRequest<RemoverRestricaoProponenteResponse>
    {
        public String IdProposta {get; set;}
        public String IdProponente {get; private set;}
    }

    public class RemoverRestricaoProponenteRequestHandler : IRequestHandler<RemoverRestricaoProponenteRequest, RemoverRestricaoProponenteResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        public IProponenteRepositorio ProponenteRepositorio {get; set;}
        
        public RemoverRestricaoProponenteRequestHandler(IPropostaRepositorio propostaRepositorio, IProponenteRepositorio proponenteRepositorio)
        {
            PropostaRepositorio = propostaRepositorio;
            ProponenteRepositorio = proponenteRepositorio;
        }

        public Task<RemoverRestricaoProponenteResponse> Handle(RemoverRestricaoProponenteRequest request, CancellationToken cancellationToken)
        {
            
            Proponente proponente = ProponenteRepositorio.Consultar(request.IdProposta,request.IdProponente); 

            proponente.RemoverRestricao();

            return Task.FromResult(new RemoverRestricaoProponenteResponse(){Status=0, Data = proponente});
        }
    }

    public class RemoverRestricaoProponenteResponse
    {
        public int Status{get;set;}
        public Proponente Data {get; set;}
    }
}
