using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.Interfaces;
using Dominio.Entidades;
using Dominio.Fabricas;
using Infraestrutura.Repositorios;
using Infraestrutura.Repositorios.Dtos;

namespace Infraestrutura.Repositorios
{
    public class DocumentoRepositorio : IDocumentoRepositorio
    {
        public ApplicationContext Context{get;set;}

        public IDocumentoFabrica DocumentoFabrica{get;set;}

        public DocumentoRepositorio(ApplicationContext context, IDocumentoFabrica documentoFabrica)
        {
            Context = context;
            DocumentoFabrica = documentoFabrica;
        }

        //Recebe um IdDocumento, pesquisa o documento na tabela documentos e o remove.
        public void Apagar(string IdDocumento)
        {
            DocumentoDTO documentoDto = Context.Documentos.Where(c=>c.IdDocumento == IdDocumento).FirstOrDefault();
            Context.Documentos.Remove(documentoDto);

            Context.SaveChanges();
        }

        //Recebe um documento do dominio, converte para DTO e depois atualia no repositorio
        public void Atualizar(Documento doc)
        {
            DocumentoDTO documentoDto = new DocumentoDTO();

                documentoDto.IdDocumento = doc.IdDocumento; 
                documentoDto.Nome = doc.Nome; 
                documentoDto.Descricao = doc.Descricao;
                documentoDto.CaminhoArquivo = doc.CaminhoArquivo;
                documentoDto.IsDocumentoAprovado = doc.IsDocumentoAprovado;
                documentoDto.MotivoRecusaAprovacao = doc.MotivoRecusaAprovacao;
            
            Context.Documentos.Update(documentoDto);

            Context.SaveChanges();
        }

        //Recebe um idProponente, pesquisa os documentos deste proponente e carrega na listaDocucmentosDto
        //converte essa lista DocumentosDto para uma lista de Documentos do domínio e retorna essa lista. 
        
        public List<Documento> BuscarPorProponente(string idProponente)
        {
            List<DocumentoDTO> listaDocumentosDTO = new List<DocumentoDTO>();
            listaDocumentosDTO = Context.Documentos.Where(d => d.IdProponente == idProponente).ToList();

            List<Documento> listaDocumentos = new List<Documento>();
            foreach (var item in listaDocumentosDTO)
            {
                Documento documento = DocumentoFabrica.CriarInstancia(item.IdDocumento, item.Nome, item.Descricao, item.CaminhoArquivo, item.IsDocumentoAprovado, item.MotivoRecusaAprovacao);
                listaDocumentos.Add(documento);
            }
            return listaDocumentos;
        }

        //Recebe IdProponente e IdDocumento verifica se as chaves são iguais e retorna essa instancia de um objeto do ticpo documento
        public Documento Consultar(string IdProponente, string IdDocumento)
        {
            DocumentoDTO documentoDto = Context.Documentos.Where(c=>c.IdProponente == IdProponente && c.IdDocumento == IdDocumento).FirstOrDefault();
            Documento documento = DocumentoFabrica.CriarInstancia(documentoDto.IdDocumento,documentoDto.Nome,documentoDto.Descricao,documentoDto.CaminhoArquivo,documentoDto.IsDocumentoAprovado,documentoDto.MotivoRecusaAprovacao);
            return documento;
        }

        //Recebe um Documento, converte este documento para DocumentoDTO, depois adiciona no Context Documento e atualiza o banco de dados
        public void Inserir(Documento documento)
        {
            DocumentoDTO documentoDto = new DocumentoDTO();

                documentoDto.IdDocumento = documento.IdDocumento; 
                documentoDto.Nome = documento.Nome; 
                documentoDto.Descricao = documento.Descricao;
                documentoDto.CaminhoArquivo = documento.CaminhoArquivo;
                documentoDto.IsDocumentoAprovado = documento.IsDocumentoAprovado;
                documentoDto.MotivoRecusaAprovacao = documento.MotivoRecusaAprovacao;
            
            Context.Documentos.Add(documentoDto);

            Context.SaveChanges();
        }
    }
}
