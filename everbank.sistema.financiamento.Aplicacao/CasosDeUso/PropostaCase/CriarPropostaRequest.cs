using System;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Domain.Entidades;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Dominio.ObjetosValor;
using Dominio.Entidades;
using Dominio.RaizAgregacao;

namespace Aplicacao.CasosDeUso.PropostaCase
{
    public class CriarPropostaRequest : IRequest<CriarPropostaResponse>
    {

        //Imovel e Endereço
        public class ImovelDTO
        {   
            [Required]
            public int InscricaoMunicipal { get; set; }
            [Required]
            public decimal ValorImovel { get; set; }
            [Required]
            public string Logradouro {get; set;}
            [Required]
            public string Numero {get; set;}
            
            public string Complemento {get; set;}
            [Required]
            public string Cep {get; set;}
            [Required]
            public string Bairro {get; set;}
            [Required]
            public string Cidade {get; set;}
            [Required]
            public string Estado {get; set;}
            public ImovelDTO(int inscricaoMunicipal, decimal valorImovel, string logradouro, string numero, string complemento, string cep, string bairro, string cidade, string estado)
            {
                InscricaoMunicipal = inscricaoMunicipal;
                ValorImovel = valorImovel;
                Logradouro = logradouro;
                Numero = numero;
                Complemento = complemento;
                Cep = cep;
                Bairro = bairro;
                Cidade = cidade;
                Estado = estado;
            }
        }

        //Proponente
        public class ProponenteDTO
        {
            [Required]
            public string NomeCompleto {get; private set;}
            [Required]
            public string Cpf {get; private set;}
            [Required]
            public DateTime DataNascimento {get; private set;}
            [Required]
            public string EstadoCivil { get; private set;}
            [Required]
            public decimal RendaBruta {get; private set;}
            [Required]
            public bool PossuiAlgumaDoencaGrave {get; private set;}
            public ProponenteDTO( string nomeCompleto, string cpf, DateTime dataNascimento, string estadoCivil, decimal rendaBruta, bool possuiAlgumaDoencaGrave)
            {
                
                NomeCompleto = nomeCompleto;
                Cpf = cpf;
                DataNascimento = dataNascimento;
                EstadoCivil = estadoCivil;
                RendaBruta = rendaBruta;
                PossuiAlgumaDoencaGrave = possuiAlgumaDoencaGrave;
            }
        }


        //Proposta
        [Required]
        public ImovelDTO ImovelRecebido { get; set; }
        [Required]
        public List<ProponenteDTO> ProponentesRecebido { get; set; }
        [Required]
        public decimal ValorEntrada { get; set; }
        [Required]
        public int PrazoFinanciamento { get; set; }

    }

    public class CriarPropostaRequestHandler : IRequestHandler<CriarPropostaRequest, CriarPropostaResponse>
    {
        public IPropostaRepositorio PropostaRepositorio { get; set; }
        public IServicoTaxaJuros ServicoTaxaDeJuros { get; set; }
        public IServicoDocumentosObrigatorios ServicoDocumentosObrigatorios { get; set; }
        public ILogger<CriarPropostaRequestHandler> Logger{get;set;}


        public CriarPropostaRequestHandler(IPropostaRepositorio propostaRepositorio, IServicoTaxaJuros servicoTaxaDeJuros, IServicoDocumentosObrigatorios servicoDocumentosObrigatorios, ILogger<CriarPropostaRequestHandler> logger)
        {
            PropostaRepositorio = propostaRepositorio;
            ServicoTaxaDeJuros = servicoTaxaDeJuros;
            ServicoDocumentosObrigatorios = servicoDocumentosObrigatorios;
            Logger = logger;
        }

        
        public Task<CriarPropostaResponse> Handle(CriarPropostaRequest request, CancellationToken cancellationToken)
        {
            try{
            Logger.LogInformation("Convertendo o ImovelRecebido do Request para o Endereco do domínio");
            Endereco endereco = new Endereco(
                request.ImovelRecebido.Logradouro,
                request.ImovelRecebido.Numero,
                request.ImovelRecebido.Complemento,
                request.ImovelRecebido.Cep,
                request.ImovelRecebido.Bairro,
                request.ImovelRecebido.Cidade,
                request.ImovelRecebido.Estado
            );

            Logger.LogInformation("Convertendo o ImovelRecebido do request para o Imóvel do domínio");
            Imovel imovel = new Imovel(
                endereco,
                request.ImovelRecebido.InscricaoMunicipal,
                request.ImovelRecebido.ValorImovel
            );


            Logger.LogInformation("Convertendo a lista de Documentos do request em uma lista de Documentos do domínio");
            List<Documento> listaDocuemntosOBrigatorios = ServicoDocumentosObrigatorios.BuscarDocumentosObrigatorios();


            Logger.LogInformation("Convertendo a lista de Proponentes do request em uma lista de Proponentes do domínio");
            List<Proponente> listaProponentes = new List<Proponente>();
            foreach (var item in request.ProponentesRecebido)
            {
                Proponente proponente = new Proponente(
                    listaDocuemntosOBrigatorios,
                    item.NomeCompleto,
                    item.Cpf,
                    item.DataNascimento,
                    item.EstadoCivil,
                    item.RendaBruta
                );
                listaProponentes.Add(proponente);
            }
            
            decimal juros = ServicoTaxaDeJuros.ObterTaxaJuros();
            
            Logger.LogInformation("Criando a proposta do domínio");
            Proposta proposta = new Proposta(imovel,listaProponentes,request.ValorEntrada, request.PrazoFinanciamento, juros);

            
            Logger.LogInformation("Inserindo a proposta no repositorio");  
            PropostaRepositorio.InserirProposta(proposta);

            return Task.FromResult(new CriarPropostaResponse() { Status = 0 , Data = proposta });
            }
            catch(Exception ex)
            {
                return Task.FromResult(new CriarPropostaResponse() { Status = 1 , MensagemErro=ex.Message + " StackTrace: " + ex.StackTrace });
            }
        }
    }

    public class CriarPropostaResponse
    {
        public Proposta Data {get; set;}
        public int Status { get; set; }
        public String MensagemErro{get;set;}
        
    }
}
