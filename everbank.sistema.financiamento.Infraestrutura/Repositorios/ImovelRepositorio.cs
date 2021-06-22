using System;
using System.Linq;
using Aplicacao.Interfaces;
using Domain.Entidades;
using Dominio.Fabricas;
using Dominio.ObjetosValor;
using Everbank.SistemaFinanciamento.Infraestrutura.Repositorios.Dtos;

namespace Infraestrutura.Repositorios
{
    public class ImovelRepositorio : IImovelRepositorio
    {
        public ApplicationContext Context{get;set;}
        public IImovelFabrica ImovelFabrica{get; set;}
        public IEnderecoFabrica EnderecoFabrica{get; set;}

        //Recebe o id de imovel, pesquisa no repositorio, converte o imovelDTO para uma instancia de Endereco do domínio e Imovel do domínio e retorna o imovel
        public Imovel Consultar(string idImovel)
        {
            ImovelDTO imovelDto = Context.Imovel.Where(c => c.IdImovel == idImovel).FirstOrDefault();
            
            Endereco endereco = EnderecoFabrica.CriarInstancia(imovelDto.Logradouro, imovelDto.Numero, imovelDto.Complemento, imovelDto.Cep, imovelDto.Bairro, imovelDto.Cidade, imovelDto.Estado);
            Imovel imovel = ImovelFabrica.CriarInstancia(imovelDto.IdImovel, endereco, imovelDto.InscricaoMunicipal, imovelDto.ValorImovel);
            
            return imovel;
        }
    }
}
