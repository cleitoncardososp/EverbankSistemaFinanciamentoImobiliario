using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using MediatR;

namespace Aplicacao.CasosDeUso.DocumentoCase
{
    public class CarregarArquivoRequest:IRequest<CarregarArquivoResponse>
    {
        public string IdProposta{get;set;}
        public string IdProponente{get; set;}
        public string IdDocumento {get; set;}
        public byte[] ConteudoArquivo { get; set; }
        public string NomeArquivo{get;set;}
    }

    public class CarregarArquivoRequestHandler : IRequestHandler<CarregarArquivoRequest, CarregarArquivoResponse>
    {
        public IProponenteRepositorio ProponenteRepositorio {get; set;}
        public IDocumentoRepositorio DocumentoRepositorio {get; set;}
        public IServicoStorage ServicoStorage{get;set;}

        public CarregarArquivoRequestHandler(IProponenteRepositorio proponenteRepositorio, IDocumentoRepositorio documentoRepositorio, IServicoStorage servicoStorage)
        {
            ProponenteRepositorio = proponenteRepositorio;
            DocumentoRepositorio = documentoRepositorio;
            ServicoStorage = servicoStorage;
        }

        public Task<CarregarArquivoResponse> Handle(CarregarArquivoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Documento documento = DocumentoRepositorio.Consultar(request.IdProponente,request.IdDocumento);
                
                String caminhoArquivo = ServicoStorage.SalvarArquivo(request.IdProposta+ "//" +request.IdProponente+ "//" +request.IdDocumento+ "//" +request.NomeArquivo, request.ConteudoArquivo);

                documento.CarregarArquivo(caminhoArquivo);

                DocumentoRepositorio.Atualizar(documento);

                return Task.FromResult(new CarregarArquivoResponse(){ Status=0 , Data = documento});
            }
            catch (Exception ex)
            {
                return Task.FromResult(new CarregarArquivoResponse() { Status = 1, MensagemErro = ex.Message } );
            }            
            
        }
    }

    public class CarregarArquivoResponse
    {
        public int Status{get;set;}
        public Documento Data {get; set;}
        public string MensagemErro {get; set;}
    }
}
