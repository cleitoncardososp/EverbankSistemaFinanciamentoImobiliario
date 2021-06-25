using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using Dominio.RaizAgregacao;
using MediatR;

namespace Aplicacao.CasosDeUso.PropostaCase
{
    public class AdicionarProponenteRequest:IRequest<AdicionarProponenteResponse>
    {
        public String IdProposta {get; set;}
        public String NomeCompleto {get; set;}
        public String Cpf {get; set;}
        public DateTime DataNascimento {get; set;}
        public String EstadoCivil { get; set;}
        public Decimal RendaBruta {get; set;}
    }

    public class AdicionarProponenteRequestHandler : IRequestHandler<AdicionarProponenteRequest, AdicionarProponenteResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        public IServicoDocumentosObrigatorios ServicoDocumentosObrigatorios { get; set; }

        public AdicionarProponenteRequestHandler(IPropostaRepositorio propostaRepositorio, IServicoDocumentosObrigatorios servicoDocumentosObrigatorios)
        {
            PropostaRepositorio = propostaRepositorio;
            ServicoDocumentosObrigatorios = servicoDocumentosObrigatorios;
        }

        public Task<AdicionarProponenteResponse> Handle(AdicionarProponenteRequest request, CancellationToken cancellationToken)
        {
            Proposta proposta = PropostaRepositorio.ConsultarProposta(request.IdProposta);
            List<Documento> documentosObrigatorios = ServicoDocumentosObrigatorios.BuscarDocumentosObrigatorios();
            Proponente proponente = new Proponente(documentosObrigatorios, request.NomeCompleto, request.Cpf, request.DataNascimento, request.EstadoCivil, request.RendaBruta);

            proposta.AdicionarProponente(proponente);

            PropostaRepositorio.Atualizar(proposta); // verificar se um proponente foi adicionado ou removido

            return Task.FromResult(new AdicionarProponenteResponse(){Status=0 , Data = proposta});
        }
    }

    public class AdicionarProponenteResponse
    {
        public int Status{get;set;}
        public Proposta Data {get; set;}
    }
}
