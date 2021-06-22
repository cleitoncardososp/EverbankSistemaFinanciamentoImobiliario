using System;
using System.Collections.Generic;
using Domain.Entidades;
using Dominio.Entidades;
using Dominio.RaizAgregacao;

namespace Dominio.Fabricas
{
    public interface IPropostaFabrica
    {
        Proposta CriarInstancia(string idProposta, Imovel imovel, List<Proponente> proponentes, decimal valorEntrada, int prazoFinanciamento, decimal taxaJurosAnual, bool isPropostaAprovada, decimal valorPrimeiraParcela, decimal rendaBrutaMinima, decimal rendaTotalProponentes, DateTime? dataAnaliseProposta, string motivoRecusaProposta);
    }

    public class PropostaFabrica : IPropostaFabrica
    {
        public Proposta CriarInstancia(string idProposta, Imovel imovel, List<Proponente> proponentes, decimal valorEntrada, int prazoFinanciamento, decimal taxaJurosAnual, bool isPropostaAprovada, decimal valorPrimeiraParcela, decimal rendaBrutaMinima, decimal rendaTotalProponentes, DateTime? dataAnaliseProposta, string motivoRecusaProposta)
        {
            return new Proposta(idProposta, imovel, proponentes, valorEntrada, prazoFinanciamento, taxaJurosAnual, isPropostaAprovada, valorPrimeiraParcela, rendaBrutaMinima, rendaTotalProponentes, dataAnaliseProposta, motivoRecusaProposta);
        }
    }
}
