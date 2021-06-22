using System;

namespace Dominio.Excecoes
{
    public class DocumentoPendenteException : Exception
    {
        public string NomeDocumento {get; set;}

        public DocumentoPendenteException(string nomeDocumento)
        {
            NomeDocumento = nomeDocumento;
        }
    }
}
