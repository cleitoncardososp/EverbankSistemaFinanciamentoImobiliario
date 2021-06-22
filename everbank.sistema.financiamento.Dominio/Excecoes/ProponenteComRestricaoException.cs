using System;

namespace Dominio.Excecoes
{
    public class ProponenteComRestricaoException : Exception
    {
        public String NomeProponente{get;set;}

        public ProponenteComRestricaoException(string nomeProponente)
        {
            NomeProponente = nomeProponente;
        }
    }
}
