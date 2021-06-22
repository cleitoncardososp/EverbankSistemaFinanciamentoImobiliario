
using Dominio.ObjetosValor;

namespace Dominio.Fabricas
{
    public interface IEnderecoFabrica
    {
        Endereco CriarInstancia(string Logradouro, string Numero, string Complemento, string Cep, string Bairro, string Cidade, string Estado);
    }

    public class EnderecoFabrica : IEnderecoFabrica
    {
        public Endereco CriarInstancia(string logradouro, string numero, string complemento, string cep, string bairro, string cidade, string estado)
        {
            return new Endereco(logradouro, numero, complemento, cep, bairro, cidade, estado);
        }
    }
}
