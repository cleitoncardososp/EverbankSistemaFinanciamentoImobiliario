using System.Xml.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using MediatR;
using Dominio.RaizAgregacao;
using Dominio.Entidades;

namespace Aplicacao.CasosDeUso.PropostaCase
{
    public class RemoverProponenteRequest:IRequest<RemoverProponenteResponse>
    {
        public String IdProposta {get; set;}
        public String IdProponente {get; set;}
    }
    public class RemoverProponenteResponseHandler : IRequestHandler<RemoverProponenteRequest, RemoverProponenteResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        public IProponenteRepositorio ProponenteRepositorio {get;set;}

        public IServicoEmissaoContrato ServicoEmissaoContrato{get;set;}

        public RemoverProponenteResponseHandler(IPropostaRepositorio propostaRepositorio, IProponenteRepositorio proponenteRepositorio, IServicoEmissaoContrato servicoEmissaoContrato)
        {
            PropostaRepositorio = propostaRepositorio;
            ProponenteRepositorio = proponenteRepositorio;
            ServicoEmissaoContrato = servicoEmissaoContrato;
        }

        public Task<RemoverProponenteResponse> Handle(RemoverProponenteRequest request, CancellationToken cancellationToken)
        {
            Proposta proposta = PropostaRepositorio.ConsultarProposta(request.IdProposta);

            Proponente proponente = ProponenteRepositorio.Consultar(request.IdProposta, request.IdProponente); 

            proposta.RemoverProponente(proponente);

            PropostaRepositorio.Atualizar(proposta);

            return Task.FromResult(new RemoverProponenteResponse(){Status=0 , Data = proposta});
        }
    }
    
    public class RemoverProponenteResponse
    {
        public int Status{get;set;}
        public Proposta Data {get; set;}
    }

}
