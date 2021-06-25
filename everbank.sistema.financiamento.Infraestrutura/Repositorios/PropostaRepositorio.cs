using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.Interfaces;
using Domain.Entidades;
using Dominio.Entidades;
using Dominio.Fabricas;
using Dominio.ObjetosValor;
using Dominio.RaizAgregacao;
using Everbank.SistemaFinanciamento.Infraestrutura.Repositorios.Dtos;
using Infraestrutura.Repositorios.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios
{
    public class PropostaRepositorio : IPropostaRepositorio
    {
        public ApplicationContext Context{get;set;}
        public IPropostaFabrica PropostaFabrica {get; set;}
        public IProponenteFabrica ProponenteFabrica {get; set;}
        public IDocumentoFabrica DocumentoFabrica {get; set;}
        public IImovelFabrica ImovelFabrica {get; set;}
        public IEnderecoFabrica EnderecoFabrica {get; set;}

        public PropostaRepositorio(ApplicationContext context, IPropostaFabrica propostaFabrica, IProponenteFabrica proponenteFabrica, IDocumentoFabrica documentoFabrica, IImovelFabrica imovelFabrica, IEnderecoFabrica enderecoFabrica)
        {
            Context = context;
            PropostaFabrica = propostaFabrica;
            ProponenteFabrica = proponenteFabrica;
            DocumentoFabrica = documentoFabrica;
            ImovelFabrica = imovelFabrica;
            EnderecoFabrica = enderecoFabrica;
        }

        //Recebe o idProposta, consulta no Repositorio, 
        //converte de PropostaDto para Proposta do domínio. 
        public Proposta ConsultarProposta(string idProposta)
        {
            PropostaDTO propostaDto = Context.Proposta.Where(p => p.IdProposta == idProposta).FirstOrDefault();

            //Não encontrou a proposta digitada, lança exceção. TOFIX: PropostaNaoLocalizadaException
            if(propostaDto == null)
            {
                throw new Exception("Proposta Não localizada");
            }

            //Proponentes
            List<ProponenteDTO> listaProponenteDto = Context.Proponentes.Where(c=>c.IdProposta == idProposta).Include(c=>c.Documentos).ToList();
            List<Proponente> listaProponentes = new List<Proponente>();
            
            
            foreach (var item in listaProponenteDto)
            {
                //Documentos
                List<Documento> listaDocumentos = new List<Documento>();
                foreach (var doc in item.Documentos)
                {
                    Documento documento = DocumentoFabrica.CriarInstancia(doc.IdDocumento, doc.Nome, doc.Descricao, doc.CaminhoArquivo, doc.IsDocumentoAprovado, doc.MotivoRecusaAprovacao);
                    listaDocumentos.Add(documento);
                }
                Proponente prop = ProponenteFabrica.CriarInstancia(item.IdProponente, listaDocumentos, item.NomeCompleto, item.Cpf, item.DataNascimento, item.EstadoCivil, item.RendaBruta, item.PossuiAlgumaDoencaGrave, item.PossuiRestricao,item.Restricao);
                listaProponentes.Add(prop);
            }

            //Imovel
            ImovelDTO imovelDto = Context.Imovel.Where(c => c.IdProposta == idProposta).FirstOrDefault();

            Endereco endereco = EnderecoFabrica.CriarInstancia(imovelDto.Logradouro, imovelDto.Numero, imovelDto.Complemento, imovelDto.Cep, imovelDto.Bairro, imovelDto.Cidade, imovelDto.Estado);
            Imovel imovel = ImovelFabrica.CriarInstancia(imovelDto.IdImovel, endereco, imovelDto.InscricaoMunicipal, imovelDto.ValorImovel);

            Proposta proposta = PropostaFabrica.CriarInstancia(propostaDto.IdProposta, imovel, listaProponentes, propostaDto.ValorEntrada, propostaDto.PrazoFinanciamento, propostaDto.TaxaJurosAnual, propostaDto.IsPropostaAprovada, propostaDto.ValorPrimeiraParcela, propostaDto.RendaBrutaMinima, propostaDto.RendaTotalProponentes, propostaDto.DataAnaliseProposta, propostaDto.MotivoRecusaProposta );
            
            return proposta;
        }


        public void Atualizar(Proposta proposta)
        {
            PropostaDTO propostaToUpdate = Context.Proposta.Where(c => c.IdProposta == proposta.IdProposta).FirstOrDefault();

            // propostaToUpdate -> proposta antiga (banco)
            // proposta         -> proposta nova (recebida pela função)

            //verifica se a qtd de proponentes no Banco é menor do que a qtd de proponentes na proposta recebida para atualizar
            //se for menor no banco quer dizer que foi adicionado um novo proponente, que ainda não existe no banco
            if(propostaToUpdate.Proponentes.Count() < proposta.Proponentes.Count())
            {
                //percorre toda a lista nova, convertendo todos pra DTO e depois insere cada um no banco
                foreach (var item in proposta.Proponentes)
                {
                    //se algum proponente (novo) não existir no banco, insere no banco.
                    if(proposta.Proponentes.LastOrDefault().Equals(item))
                    {
                    //converte para DTO 
                        ProponenteDTO proponenteDto = new ProponenteDTO();
                            proponenteDto.IdProponente = item.IdProponente;
                            proponenteDto.IdProposta = proposta.IdProposta; //relacionamento
                            proponenteDto.NomeCompleto = item.NomeCompleto;
                            proponenteDto.Cpf = item.Cpf;
                            proponenteDto.DataNascimento = item.DataNascimento;
                            proponenteDto.EstadoCivil = item.EstadoCivil;
                            proponenteDto.RendaBruta = item.RendaBruta;
                            proponenteDto.PossuiAlgumaDoencaGrave = item.PossuiAlgumaDoencaGrave;
                            proponenteDto.PossuiRestricao = item.PossuiRestricao;
                            proponenteDto.Restricao = item.Restricao;

                        foreach (var doc in item.Documentos)
                        {
                            DocumentoDTO documentoDto = new DocumentoDTO();
                                documentoDto.IdDocumento = doc.IdDocumento;
                                documentoDto.IdProponente = proponenteDto.IdProponente; // relacionamento
                                documentoDto.Nome = doc.Nome;
                                documentoDto.Descricao = doc.Descricao;
                                documentoDto.CaminhoArquivo = doc.CaminhoArquivo;
                                documentoDto.IsDocumentoAprovado = doc.IsDocumentoAprovado;
                                documentoDto.MotivoRecusaAprovacao = doc.MotivoRecusaAprovacao;

                                Context.Documentos.Add(documentoDto);

                                Context.SaveChanges();
                        }

                        Context.Proponentes.Add(proponenteDto);

                        Context.SaveChanges();
                    }
                }
            }

            //verifica se a qtd de proponentes no Banco é maior do que a qtd de proponentes na proposta recebida para atualizar
            //se for maior no banco quer dizer que foi removido um proponente, existente no banco 
            if(propostaToUpdate.Proponentes.Count() > proposta.Proponentes.Count())
            {
                foreach (var proponenteBanco in propostaToUpdate.Proponentes)
                {
                    foreach (var doc in proponenteBanco.Documentos)
                    {
                        Context.Documentos.Remove(doc);
                    }

                    Context.Proponentes.Remove(proponenteBanco);
                }
                
                Context.SaveChanges();

                foreach (var proponenteProposta in proposta.Proponentes)
                {
                    ProponenteDTO proponenteDto = new ProponenteDTO();
                            proponenteDto.IdProponente = proponenteProposta.IdProponente;
                            proponenteDto.IdProposta = proposta.IdProposta; //relacionamento
                            proponenteDto.NomeCompleto = proponenteProposta.NomeCompleto;
                            proponenteDto.Cpf = proponenteProposta.Cpf;
                            proponenteDto.DataNascimento = proponenteProposta.DataNascimento;
                            proponenteDto.EstadoCivil = proponenteProposta.EstadoCivil;
                            proponenteDto.RendaBruta = proponenteProposta.RendaBruta;
                            proponenteDto.PossuiAlgumaDoencaGrave = proponenteProposta.PossuiAlgumaDoencaGrave;
                            proponenteDto.PossuiRestricao = proponenteProposta.PossuiRestricao;
                            proponenteDto.Restricao = proponenteProposta.Restricao;

                        foreach (var doc in proponenteProposta.Documentos)
                        {
                            DocumentoDTO documentoDto = new DocumentoDTO();
                                documentoDto.IdDocumento = doc.IdDocumento;
                                documentoDto.IdProponente = proponenteDto.IdProponente; // relacionamento
                                documentoDto.Nome = doc.Nome;
                                documentoDto.Descricao = doc.Descricao;
                                documentoDto.CaminhoArquivo = doc.CaminhoArquivo;
                                documentoDto.IsDocumentoAprovado = doc.IsDocumentoAprovado;
                                documentoDto.MotivoRecusaAprovacao = doc.MotivoRecusaAprovacao;

                                Context.Documentos.Add(documentoDto);

                                Context.SaveChanges();
                        }

                        Context.Proponentes.Add(proponenteDto);

                        Context.SaveChanges();
                }
            }   
                        
            propostaToUpdate.IdProposta = proposta.IdProposta;
            propostaToUpdate.ValorEntrada = proposta.ValorEntrada;
            propostaToUpdate.PrazoFinanciamento = proposta.PrazoFinanciamento;
            propostaToUpdate.TaxaJurosAnual = proposta.TaxaJurosAnual;
            propostaToUpdate.IsPropostaAprovada = proposta.IsPropostaAprovada;
            propostaToUpdate.ValorPrimeiraParcela = proposta.ValorPrimeiraParcela;
            propostaToUpdate.RendaBrutaMinima = proposta.RendaBrutaMinima;
            propostaToUpdate.RendaTotalProponentes = proposta.RendaTotalProponentes;
            propostaToUpdate.DataAnaliseProposta = proposta.DataAnaliseProposta;
            propostaToUpdate.MotivoRecusaProposta = proposta.MotivoRecusaProposta;

            Context.Proposta.Update(propostaToUpdate);

            Context.SaveChanges();
        }


        //Converter o proponente recebido em Proponente Dto e inserir no repositorio
        public void InserirProponente(Proponente proponente)
        {
            ProponenteDTO proponenteDto = new ProponenteDTO();
                proponenteDto.IdProponente = proponente.IdProponente;
                proponenteDto.NomeCompleto = proponente.NomeCompleto;
                proponenteDto.Cpf = proponente.Cpf;
                proponenteDto.DataNascimento = proponente.DataNascimento;
                proponenteDto.EstadoCivil = proponente.EstadoCivil;
                proponenteDto.RendaBruta = proponente.RendaBruta;
                proponenteDto.PossuiAlgumaDoencaGrave = proponente.PossuiAlgumaDoencaGrave;
                proponenteDto.PossuiRestricao = proponente.PossuiRestricao;
                proponenteDto.Restricao = proponente.Restricao;

            Context.Proponentes.Add(proponenteDto);

            Context.SaveChanges();
        }

        //Converter para Dto e salvar no repositorio
        public void InserirProposta(Proposta proposta)
        {
            //ImovelDto
            ImovelDTO imovelDto = new ImovelDTO();
                imovelDto.IdImovel = proposta.Imovel.IdImovel;
                imovelDto.InscricaoMunicipal = proposta.Imovel.InscricaoMunicipal;
                imovelDto.ValorImovel = proposta.Imovel.ValorImovel;
                imovelDto.Logradouro = proposta.Imovel.Endereco.Logradouro;
                imovelDto.Numero = proposta.Imovel.Endereco.Numero;
                imovelDto.Complemento = proposta.Imovel.Endereco.Complemento;
                imovelDto.Cep = proposta.Imovel.Endereco.Cep;
                imovelDto.Bairro = proposta.Imovel.Endereco.Bairro;
                imovelDto.Cidade = proposta.Imovel.Endereco.Cidade;
                imovelDto.Estado = proposta.Imovel.Endereco.Estado;
                
                //Relacionamento Imovel <-> Proposta
                imovelDto.IdProposta = proposta.IdProposta;

                Context.Imovel.Add(imovelDto);

            //ProponenteDto
            List<DocumentoDTO> listaDocumentoDto = new List<DocumentoDTO>();

                foreach (var prop in proposta.Proponentes)
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

                        //Relacionamento Proponente <-> Proposta
                        proponenteDto.IdProposta = proposta.IdProposta;

                        Context.Proponentes.Add(proponenteDto);
                        //listaProponenteDto.Add(proponenteDto);

                    //Documento
                    foreach (var item in prop.Documentos)
                    {
                        DocumentoDTO documentoDto = new DocumentoDTO();
                            documentoDto.IdDocumento = Guid.NewGuid().ToString(); 
                            documentoDto.Nome = item.Nome; 
                            documentoDto.Descricao = item.Descricao;
                            documentoDto.CaminhoArquivo = item.CaminhoArquivo;
                            documentoDto.IsDocumentoAprovado = item.IsDocumentoAprovado;
                            documentoDto.MotivoRecusaAprovacao = item.MotivoRecusaAprovacao;
                            
                            //Relacionamento Documento <-> Proponente
                            documentoDto.IdProponente = proponenteDto.IdProponente;

                            Context.Documentos.Add(documentoDto);
                            //listaDocumentoDto.Add(documentoDto);
                    }
                }

            //Proposta
            PropostaDTO propostaDto = new PropostaDTO();
                propostaDto.IdProposta = proposta.IdProposta;
                propostaDto.ValorEntrada = proposta.ValorEntrada;
                propostaDto.PrazoFinanciamento = proposta.PrazoFinanciamento;
                propostaDto.TaxaJurosAnual = proposta.TaxaJurosAnual;
                propostaDto.IsPropostaAprovada = proposta.IsPropostaAprovada;
                propostaDto.ValorPrimeiraParcela = proposta.ValorPrimeiraParcela;
                propostaDto.RendaBrutaMinima = proposta.RendaBrutaMinima;
                propostaDto.RendaTotalProponentes = proposta.RendaTotalProponentes;
                propostaDto.DataAnaliseProposta = proposta.DataAnaliseProposta;
                propostaDto.MotivoRecusaProposta = proposta.MotivoRecusaProposta;

                //Relacionamento Imovel <-> Proposta
                propostaDto.IdImovel = proposta.Imovel.IdImovel;

            Context.Proposta.Add(propostaDto);
            //listaProponenteDto.ForEach(c=>Context.Proponentes.Add(c));
            //listaDocumentoDto.ForEach(c=>Context.Documentos.Add(c));
            Context.SaveChanges();            
        }
    }
}