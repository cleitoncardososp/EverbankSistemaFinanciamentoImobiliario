using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestrutura.Repositorios.Dtos
{
    [Table("Tb_Proponente")]
    public class ProponenteDTO
    {
        [Key]
        [MaxLength(50)]
        public string IdProponente {get; set;}

        [Required]
        [MaxLength(50)]
        public string IdProposta{get;set;}

        [Required]
        [MaxLength(50)]
        public string NomeCompleto {get; set;}

        [Required]
        [MaxLength(50)]
        public string Cpf {get; set;}
        
        [Required]
        [MaxLength(50)]
        public DateTime DataNascimento {get; set;}
        
        [Required]
        [MaxLength(50)]
        public string EstadoCivil { get; set;}

        [Required]
        [MaxLength(255)]
        public decimal RendaBruta {get; set;}

        public bool PossuiAlgumaDoencaGrave {get; set;}

        public bool PossuiRestricao{get; set;}

        [MaxLength(255)]
        public string Restricao{get; set;}

        [ForeignKey("IdProposta")]
        public virtual PropostaDTO Proposta{get;set;}

        [ForeignKey("IdDocumentos")]
        public virtual List<DocumentoDTO> Documentos{get;set;}
    }
}