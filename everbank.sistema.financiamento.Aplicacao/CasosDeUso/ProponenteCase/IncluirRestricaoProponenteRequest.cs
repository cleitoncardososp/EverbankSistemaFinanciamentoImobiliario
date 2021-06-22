using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using MediatR;

namespace Aplicacao.CasosDeUso.ProponenteCase
{
    public class IncluirRestricaoProponenteRequest:IRequest<IncluirRestricaoProponenteResponse>
    {
        public string IdProposta {get; set;}
        public String IdProponente {get; set;}
        public String Restricao{get; set;}
    }

    public class IncluirRestricaoProponenteRequestHandler : IRequestHandler<IncluirRestricaoProponenteRequest, IncluirRestricaoProponenteResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        public IProponenteRepositorio ProponenteRepositorio {get; set;}

        public IncluirRestricaoProponenteRequestHandler(IProponenteRepositorio proponenteRepositorio, IPropostaRepositorio propostaRepositorio)
        {
            ProponenteRepositorio = proponenteRepositorio;
            PropostaRepositorio = propostaRepositorio;
        }

        public Task<IncluirRestricaoProponenteResponse> Handle(IncluirRestricaoProponenteRequest request, CancellationToken cancellationToken)
        {
            
            Proponente proponente = ProponenteRepositorio.Consultar(request.IdProposta,request.IdProponente);

            proponente.IncluirRestricao(request.Restricao);

            ProponenteRepositorio.Atualizar(proponente);
            
            return Task.FromResult(new IncluirRestricaoProponenteResponse(){Status=0 , Data = proponente});
        }
    }

    public class IncluirRestricaoProponenteResponse
    {
        public int Status{get;set;}
        public Proponente Data {get; set;}
    }
}
