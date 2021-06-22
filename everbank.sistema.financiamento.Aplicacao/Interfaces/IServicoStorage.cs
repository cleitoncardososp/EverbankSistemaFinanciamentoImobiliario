using System;
using System.Collections.Generic;
using Dominio.Entidades;

namespace Aplicacao.Interfaces
{
    public interface IServicoStorage
    {
        String SalvarArquivo(String nomeArquivo, byte[] conteudoArquivo);
    }
}
