using System.Threading;
using System.Threading.Tasks;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using MediatR;

namespace Aplicacao.CasosDeUso.DocumentoCase
{
    public class RecusarDocumentoRequest:IRequest<RecusarDocumentoResponse>
    {
        public string IdProposta{get; set;}
        public string IdProponente {get; set;}
        public string IdDocumento{get;set;}
        public string MotivoRecusaAprovacao{get;  set; }
    }

    public class RecusarDocumentoRequestHandler : IRequestHandler<RecusarDocumentoRequest, RecusarDocumentoResponse>
    {
        public IProponenteRepositorio ProponenteRepositorio {get; set;}
        public IDocumentoRepositorio DocumentoRepositorio {get; set;}
        public IPropostaRepositorio PropostaRepositorio {get; set;}

        public RecusarDocumentoRequestHandler(IProponenteRepositorio proponenteRepositorio, IDocumentoRepositorio documentoRepositorio, IPropostaRepositorio propostaRepositorio)
        {
            ProponenteRepositorio = proponenteRepositorio;
            DocumentoRepositorio = documentoRepositorio;
            PropostaRepositorio = propostaRepositorio;
        }

        public Task<RecusarDocumentoResponse> Handle(RecusarDocumentoRequest request, CancellationToken cancellationToken)
        {
            
            Documento documento = DocumentoRepositorio.Consultar(request.IdProponente, request.IdDocumento);
            
            documento.RecusarDocumento(request.MotivoRecusaAprovacao);

            DocumentoRepositorio.Atualizar(documento);

            return Task.FromResult(new RecusarDocumentoResponse(){Status=0 , Data = documento});
        }
    }

    public class RecusarDocumentoResponse
    {
        public int Status{get;set;}
        public Documento Data {get; set;}
    }
}
