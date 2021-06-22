using System;

namespace Dominio.Excecoes
{
    public class ExcecaoDominio : Exception
    {
        public ExcecaoDominio(string message) : base(message)
        {
        }

        public static void LancarQuando(Func<bool> expressao, String mensagem)
        {
            if(expressao.Invoke()==true)
            {
                throw new ExcecaoDominio(mensagem);
            }
        }
    }
}
