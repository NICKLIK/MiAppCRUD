    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace MiAppCRUD.Server.Models
    {
        public class CategoriaProducto
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            [Column(TypeName = "varchar(100)")]     
            public string Nombre { get; set; }

            public ICollection<Producto> Productos { get; set; }
        }
    }
