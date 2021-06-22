using System;
using Domain.Entidades;
using Dominio.ObjetosValor;

namespace Dominio.Fabricas
{
    public interface IImovelFabrica
    {
        Imovel CriarInstancia(string IdImovel, Endereco Endereco, int InscricaoMunicipal, decimal ValorImovel);
    }

    public class ImovelFabrica : IImovelFabrica
    {
        public Imovel CriarInstancia(string idImovel, Endereco endereco, int inscricaoMunicipal, decimal valorImovel)
        {
            return new Imovel(idImovel, endereco, inscricaoMunicipal, valorImovel);
        }

    }
}
