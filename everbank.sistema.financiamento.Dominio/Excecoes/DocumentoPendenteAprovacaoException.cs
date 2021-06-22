using System;

namespace Dominio.Excecoes
{
    public class DocumentoPendenteAprovacaoException:Exception
    {
        public String NomeDocumento{get;set;}

        public DocumentoPendenteAprovacaoException(string nomeDocumento)
        {
            NomeDocumento = nomeDocumento;
        }
    }
}
