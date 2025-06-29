namespace MiAppCRUD.Server.Models
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string ImagenUrl { get; set; } 
        public decimal EcuniPoints { get; set; }
        public int CategoriaProductoId { get; set; }
    }

}
