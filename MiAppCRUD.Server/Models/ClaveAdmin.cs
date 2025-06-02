using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiAppCRUD.Server.Models
{
    public class ClaveAdmin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Correo { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Clave { get; set; }

        [Required]
        public bool Usada { get; set; } = false;
    }
}
