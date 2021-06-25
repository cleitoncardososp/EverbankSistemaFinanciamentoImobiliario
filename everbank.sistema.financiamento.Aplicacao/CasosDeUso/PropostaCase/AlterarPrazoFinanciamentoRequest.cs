using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.RaizAgregacao;
using MediatR;

namespace Aplicacao.CasosDeUso.PropostaCase

{
    public class AlterarPrazoFinanciamentoRequest:IRequest<AlterarPrazoFinanciamentoResponse>
    {
        public string IdProposta {get; set;}
        public int PrazoFinanciamento { get; set; }
    }

    public class AlterarPrazoFinanciamentoRequestHandler : IRequestHandler<AlterarPrazoFinanciamentoRequest, AlterarPrazoFinanciamentoResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}

        public AlterarPrazoFinanciamentoRequestHandler(IPropostaRepositorio propostaRepositorio)
        {
            PropostaRepositorio = propostaRepositorio;
        }

        public Task<AlterarPrazoFinanciamentoResponse> Handle(AlterarPrazoFinanciamentoRequest request, CancellationToken cancellationToken)
        {
            Proposta proposta = PropostaRepositorio.ConsultarProposta(request.IdProposta);  
            
            proposta.AlterarPrazoFinanciamento(request.PrazoFinanciamento); 

            PropostaRepositorio.Atualizar(proposta);

            return Task.FromResult(new AlterarPrazoFinanciamentoResponse(){Status=0, Data = proposta});
        }
    }
    
    public class AlterarPrazoFinanciamentoResponse
    {
        public int Status{get;set;}
        public Proposta Data {get; set;}
    }
}
