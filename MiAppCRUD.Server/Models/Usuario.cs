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
        public string Nombre { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Apellido { get; set; }

        [Required]
        [Range(1, 120, ErrorMessage = "La edad debe estar entre 1 y 120")]
        public int Edad { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Genero { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
        public string Correo { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Provincia { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Ciudad { get; set; }

        [Required]
        [Column(TypeName = "text")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un símbolo")]
        public string Contrasena { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Rol { get; set; } = "Usuario";  

    }
}