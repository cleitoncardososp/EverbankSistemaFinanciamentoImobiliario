using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.RaizAgregacao;
using MediatR;

namespace Aplicacao.CasosDeUso.PropostaCase
{
    public class AlterarValorEntradaRequest:IRequest<AlterarValorEntradaResponse>
    {
        public string IdProposta {get; set;}
        public decimal ValorEntrada{ get; set; }
    }

    public class AlterarValorEntradaHandler : IRequestHandler<AlterarValorEntradaRequest, AlterarValorEntradaResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        public AlterarValorEntradaHandler(IPropostaRepositorio propostaRepositorio)
        {
            PropostaRepositorio = propostaRepositorio;
        }

        public Task<AlterarValorEntradaResponse> Handle(AlterarValorEntradaRequest request, CancellationToken cancellationToken)
        {
            Proposta proposta = PropostaRepositorio.ConsultarProposta(request.IdProposta);

            proposta.AlterarValorEntrada(request.ValorEntrada);

            PropostaRepositorio.Atualizar(proposta);

            return Task.FromResult(new AlterarValorEntradaResponse(){Status=0 , Data = proposta});
        }
    }
    
    public class AlterarValorEntradaResponse
    {
        public int Status{get;set;}
        public Proposta Data {get; set;}
    }
}
