using System;
using Dominio.Entidades;

namespace Aplicacao.Interfaces
{
    public interface IProponenteRepositorio
    {
        Proponente Consultar(string IdProposta, string IdProponente);
        void Inserir(Proponente proponente);
        void Atualizar(Proponente proponente);
    }
}
