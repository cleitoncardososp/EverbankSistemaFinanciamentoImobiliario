using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infraestrutura.Repositorios.Dtos
{
    [Table("Tb_Documento")]
    public class DocumentoDTO
    {
        [Key]
        [MaxLength(50)]
        public string IdDocumento { get; set; }

        [Required]
        [MaxLength(50)]
        public string IdProponente{get;set;}

        [MaxLength(255)]
        [Required]
        public string Nome { get; set; }
        
        [MaxLength(255)]
        public string Descricao { get; set; }

        [MaxLength(1024)]
        public string CaminhoArquivo { get; set; }

        public bool IsDocumentoAprovado { get; set; }

        [MaxLength(1024)]
        public string MotivoRecusaAprovacao{get; set; }

        [ForeignKey("IdProponente")]
        public ProponenteDTO Proponente{get;set;}
    }
}
