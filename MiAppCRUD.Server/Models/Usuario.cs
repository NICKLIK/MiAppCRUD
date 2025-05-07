using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiAppCRUD.Server.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string NombreUsuario { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Contrasena { get; set; }
    }
}