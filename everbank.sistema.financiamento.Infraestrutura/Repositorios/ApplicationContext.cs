using System;
using Everbank.SistemaFinanciamento.Infraestrutura.Repositorios.Dtos;
using Infraestrutura.Repositorios.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<DocumentoDTO> Documentos{get;set;}

        public DbSet<ImovelDTO> Imovel {get; set;}

        public DbSet<ProponenteDTO> Proponentes{get; set;}

        public DbSet<PropostaDTO> Proposta {get; set;}

    }
}
