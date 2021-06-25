using System;

namespace Dominio.Excecoes
{
    public class RendaInsuficienteException : Exception
    {
        public RendaInsuficienteException(string message) : base(message)
        {
        }
    }
}
