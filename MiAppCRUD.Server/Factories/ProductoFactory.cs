using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Factories
{
    public class ProductoFactory : IProductoFactory
    {
        public Producto CrearProducto(ProductoDto dto)
        {
            return new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                ImagenUrl = dto.ImagenUrl,
                EcuniPoints = dto.EcuniPoints,
                CategoriaProductoId = dto.CategoriaProductoId
            };
        }
    }
}
