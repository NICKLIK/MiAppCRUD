using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiAppCRUD.Server.Models
{
    public class ReabastecimientoStock
    {
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Estado { get; set; } = "En proceso";

        [Required]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime FechaEntrega { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime FechaSolicitud { get; set; } // ya no pongas `.UtcNow` por defecto


    }
}
