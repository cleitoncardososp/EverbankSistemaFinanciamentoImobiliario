using System;
using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Aplicacao.CasosDeUso.DocumentoCase
{
    public class AprovarDocumentoRequest : IRequest<AprovarDocumentoResponse>
    {
        public string IdProposta {get; set;}
        public string IdProponente {get; set;}
        public string IdDocumento {get;set;}
    }

    public class AprovarDocumentoResponseHandler : IRequestHandler<AprovarDocumentoRequest, AprovarDocumentoResponse>
    {
        public IPropostaRepositorio PropostaRepositorio {get; set;}
        public IProponenteRepositorio ProponenteRepositorio {get; set;}
        public IDocumentoRepositorio DocumentoRepositorio {get; set;}
        public ILogger<AprovarDocumentoResponseHandler> Logger{get;set;}

        public AprovarDocumentoResponseHandler(IPropostaRepositorio propostaRepositorio, IProponenteRepositorio proponenteRepositorio, IDocumentoRepositorio documentoRepositorio, ILogger<AprovarDocumentoResponseHandler> logger)
        {
            PropostaRepositorio = propostaRepositorio;
            ProponenteRepositorio = proponenteRepositorio;
            DocumentoRepositorio = documentoRepositorio;
            Logger = logger;
        }

        public Task<AprovarDocumentoResponse> Handle(AprovarDocumentoRequest request, CancellationToken cancellationToken)
        {
            try{
                Logger.LogDebug("Recebida uma solicitação de Aprovação de Documento",request);
                
                Documento documento = DocumentoRepositorio.Consultar(request.IdProponente, request.IdDocumento);

                documento.AprovarDocumento();

                DocumentoRepositorio.Atualizar(documento);

                Logger.LogDebug("Aprovação realizada com sucesso");
                return Task.FromResult(new AprovarDocumentoResponse(){Status=0 , Data = documento});
            }
            catch(Exception ex)
            {
                Logger.LogError("Erro ao aprovar um documento",ex);
                return Task.FromResult(new AprovarDocumentoResponse(){Status=1});
            }
        }

    }

    public class AprovarDocumentoResponse
    {
        public int Status {get; set;}
        public Documento Data {get; set;}
    }
}
