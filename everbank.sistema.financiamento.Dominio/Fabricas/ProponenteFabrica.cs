using System;
using System.Collections.Generic;
using Dominio.Entidades;

namespace Dominio.Fabricas
{
    public interface IProponenteFabrica
    {
        Proponente CriarInstancia(string idProponente, List<Documento> documentos, string nomeCompleto, string cpf, DateTime dataNascimento, string estadoCivil, decimal rendaBruta, bool possuiAlgumaDoencaGrave, bool possuiRestricao, string restricao);
    }

    public class ProponenteFabrica : IProponenteFabrica
    {
        public Proponente CriarInstancia(string idProponente, List<Documento> documentos, string nomeCompleto, string cpf, DateTime dataNascimento, string estadoCivil, decimal rendaBruta, bool possuiAlgumaDoencaGrave, bool possuiRestricao, string restricao)
        {
            return new Proponente(idProponente, documentos, nomeCompleto, cpf, dataNascimento, estadoCivil, rendaBruta, possuiAlgumaDoencaGrave, possuiRestricao, restricao);
        }
    }
}
