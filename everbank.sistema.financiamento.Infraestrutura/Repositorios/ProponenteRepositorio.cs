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
    public class ProponenteRepositorio : IProponenteRepositorio
    {
        public ApplicationContext Context{get;set;}
        public IProponenteFabrica ProponenteFabrica {get; set;}
        public IDocumentoFabrica DocumentoFabrica {get; set;}

        //Comparar a lista de documentos do repositorio com a lista de documentos do proponente e realizar a devida atualização
        public void Atualizar(Proponente prop)
        {
            ProponenteDTO propToUpdate = Context.Proponentes.Find(prop.IdProponente); // Repositorio
            List<DocumentoDTO> docsToUpdate = Context.Documentos.Where(p => p.IdProponente == prop.IdProponente).ToList(); // Repositorio
            foreach (var item in prop.Documentos) 
            {
                DocumentoDTO docDTO = docsToUpdate.FirstOrDefault(c=>c.IdDocumento == item.IdDocumento);
                if(docDTO==null)
                {   
                    DocumentoDTO documentoDTO = new DocumentoDTO();

                    documentoDTO.Nome = item.Nome;
                    documentoDTO.Descricao = item.Descricao;
                    documentoDTO.CaminhoArquivo = item.CaminhoArquivo;
                    documentoDTO.IsDocumentoAprovado = item.IsDocumentoAprovado;
                    documentoDTO.MotivoRecusaAprovacao =item.MotivoRecusaAprovacao;

                    Context.Documentos.Update(documentoDTO); //Adicionar o documento

                    Context.SaveChanges();
                }
                else{
                    //Atualizar o documento
                    docDTO.Nome = item.Nome;
                    docDTO.Descricao = item.Descricao;
                    docDTO.CaminhoArquivo = item.CaminhoArquivo;
                    docDTO.IsDocumentoAprovado = item.IsDocumentoAprovado;
                    docDTO.MotivoRecusaAprovacao = item.MotivoRecusaAprovacao;

                    Context.SaveChanges();
                }
            }
            foreach(var item in docsToUpdate) 
            {
                if(prop.Documentos.Any(c=>c.IdDocumento == item.IdDocumento)==false)
                {
                    docsToUpdate.Remove(item); //Remover do repositório
                    Context.SaveChanges();
                }
            }
            propToUpdate.NomeCompleto = prop.NomeCompleto;
            propToUpdate.Cpf = prop.Cpf;
            propToUpdate.DataNascimento = prop.DataNascimento;
            propToUpdate.EstadoCivil = prop.EstadoCivil;
            propToUpdate.RendaBruta = prop.RendaBruta;
            propToUpdate.PossuiAlgumaDoencaGrave = prop.PossuiAlgumaDoencaGrave;
            propToUpdate.PossuiRestricao = prop.PossuiRestricao;
            propToUpdate.Restricao = prop.Restricao;

            Context.SaveChanges();
        }

        //Recebe idProposta e idProponente e pesquisa nos respositorios
        //converte os dados de Dto para do domínio e os retorna.
        public Proponente Consultar(string idProposta, string idProponente)
        {
            ProponenteDTO proponenteDto = Context.Proponentes.Where(c=>c.IdProposta == idProposta && c.IdProponente == idProponente).FirstOrDefault();


            List<DocumentoDTO> documentoDto = Context.Documentos.Where(p => p.IdProponente == idProponente).ToList(); 
            List<Documento> documento = new List<Documento>();

            foreach (var item in documentoDto)
            {
                Documento doc = DocumentoFabrica.CriarInstancia(item.IdDocumento, item.Nome, item.Descricao, item.CaminhoArquivo, item.IsDocumentoAprovado, item.MotivoRecusaAprovacao);
                documento.Add(doc);
            }

            Proponente proponente = ProponenteFabrica.CriarInstancia(proponenteDto.IdProponente, documento, proponenteDto.NomeCompleto, proponenteDto.Cpf, proponenteDto.DataNascimento, proponenteDto.EstadoCivil, proponenteDto.RendaBruta, proponenteDto.PossuiAlgumaDoencaGrave, proponenteDto.PossuiRestricao, proponenteDto.Restricao);

            return proponente;
        }

        public void Inserir(Proponente prop)
        {          
            ProponenteDTO proponenteDto = new ProponenteDTO();

                proponenteDto.IdProponente = prop.IdProponente;
                proponenteDto.NomeCompleto = prop.NomeCompleto;
                proponenteDto.Cpf = prop.Cpf;
                proponenteDto.DataNascimento = prop.DataNascimento;
                proponenteDto.EstadoCivil = prop.EstadoCivil;
                proponenteDto.RendaBruta = prop.RendaBruta;
                proponenteDto.PossuiAlgumaDoencaGrave = prop.PossuiAlgumaDoencaGrave;
                proponenteDto.PossuiRestricao = prop.PossuiRestricao;
                proponenteDto.Restricao = prop.Restricao;

            Context.Proponentes.Add(proponenteDto);
            
            foreach (var item in prop.Documentos)
            {
                DocumentoDTO documentoDto = new DocumentoDTO();
                
                documentoDto.IdDocumento = item.IdDocumento; 
                documentoDto.Nome = item.Nome; 
                documentoDto.Descricao = item.Descricao;
                documentoDto.CaminhoArquivo = item.CaminhoArquivo;
                documentoDto.IsDocumentoAprovado = item.IsDocumentoAprovado;
                documentoDto.MotivoRecusaAprovacao = item.MotivoRecusaAprovacao;

                Context.Documentos.Add(documentoDto);
            }         

            Context.SaveChanges();
        }
    }
}
