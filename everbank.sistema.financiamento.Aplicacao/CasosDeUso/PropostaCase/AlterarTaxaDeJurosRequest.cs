using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.RaizAgregacao;
using MediatR;

namespace Aplicacao.CasosDeUso.PropostaCase
{
    public class AlterarTaxaDeJurosRequest:IRequest<AlterarTaxaDeJurosResponse>
    {
        public string IdProposta {get; set;}
        public int TaxaJurosAnual { get; set; }
    }

    public class AlterarTaxaDeJurosRequestHandler : IRequestHandler<AlterarTaxaDeJurosRequest, AlterarTaxaDeJurosResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}

        public AlterarTaxaDeJurosRequestHandler(IPropostaRepositorio propostaRepositorio)
        {
            PropostaRepositorio = propostaRepositorio;
        }

        public Task<AlterarTaxaDeJurosResponse> Handle(AlterarTaxaDeJurosRequest request, CancellationToken cancellationToken)
        {
            Proposta proposta = PropostaRepositorio.ConsultarProposta(request.IdProposta);

            proposta.AlterarTaxaJurosAnual(request.TaxaJurosAnual);

            PropostaRepositorio.Atualizar(proposta);
            
            return Task.FromResult(new AlterarTaxaDeJurosResponse(){Status=0 , Data = proposta});
        }
    }

    public class AlterarTaxaDeJurosResponse
    {
        public int Status{get;set;}
        public Proposta Data {get; set;}
    }
}
