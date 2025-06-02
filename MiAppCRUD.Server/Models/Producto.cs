using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiAppCRUD.Server.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; }

        [Column(TypeName = "text")]
        public string Descripcion { get; set; }

        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ImagenUrl { get; set; }  

        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal EcuniPoints { get; set; }

       
        public int CategoriaProductoId { get; set; }

        [ForeignKey("CategoriaProductoId")]
        public CategoriaProducto Categoria { get; set; }
    }
}
