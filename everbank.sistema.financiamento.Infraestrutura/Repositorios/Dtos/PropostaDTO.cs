using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Everbank.SistemaFinanciamento.Infraestrutura.Repositorios.Dtos;

namespace Infraestrutura.Repositorios.Dtos
{
    [Table("Tb_Proposta")]
    public class PropostaDTO
    {
        [Key]
        [MaxLength(50)]
        public string IdProposta {get; set;}

        [Required]
        [MaxLength(50)]
        public string IdImovel {get; set;}

        [Required]
        [MaxLength(50)]
        public decimal ValorEntrada{ get; set; }

        [Required]
        [MaxLength(50)]
        public int PrazoFinanciamento { get; set; }

        [Required]
        [MaxLength(50)]
        public decimal TaxaJurosAnual {get; set;}

        [Required]
        [MaxLength(50)]
        public bool IsPropostaAprovada{get; set;}

        [Required]
        [MaxLength(50)]
        public decimal ValorPrimeiraParcela {get; set;}

        [Required]
        [MaxLength(50)]
        public decimal RendaBrutaMinima {get; set;}

        [Required]
        [MaxLength(50)]
        public decimal RendaTotalProponentes {get; set;}

        public DateTime? DataAnaliseProposta{get; set;}

        [MaxLength(255)]
        public String MotivoRecusaProposta{get; set;}

        public virtual List<ProponenteDTO> Proponentes{get;set;}

        [ForeignKey("IdImovel")]
        public virtual ImovelDTO Imovel {get; set;}
    }
}

