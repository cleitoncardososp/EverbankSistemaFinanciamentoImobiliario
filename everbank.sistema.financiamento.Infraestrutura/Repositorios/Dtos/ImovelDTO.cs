using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infraestrutura.Repositorios.Dtos;

namespace Everbank.SistemaFinanciamento.Infraestrutura.Repositorios.Dtos
{
    [Table("Tb_Imovel")]
    public class ImovelDTO
    {   
        [Key]
        [MaxLength(50)]
        public string IdImovel {get; set;}

        [Required]
        [MaxLength(50)]
        public string IdProposta {get; set;}
        
        [Required]
        [MaxLength(50)]
        public int InscricaoMunicipal {get; set;}
        
        [Required]
        [MaxLength(50)]
        public decimal ValorImovel {get; set;}

        [Required]
        [MaxLength(50)]
        public string Logradouro {get; set;}

        [Required]
        [MaxLength(50)]
        public string Numero {get; set;}
        
        [MaxLength(255)]
        public string Complemento {get; set;}
        
        [Required]
        [MaxLength(50)]
        public string Cep {get; set;}
        
        [Required]
        [MaxLength(255)]
        public string Bairro {get; set;}
        
        [Required]
        [MaxLength(255)]
        public string Cidade {get; set;}
        
        [Required]
        [MaxLength(50)]
        public string Estado {get; set;}

        [ForeignKey("IdProposta")]
        public virtual PropostaDTO Proposta{get;set;}

    }
}
