using Domain.Entidades;

namespace Aplicacao.Interfaces
{
    public interface IImovelRepositorio
    {
        Imovel Consultar(string id);
    }
}
