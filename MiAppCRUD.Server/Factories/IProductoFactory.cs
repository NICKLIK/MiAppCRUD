using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Factories
{
    public interface IProductoFactory
    {
        Producto CrearProducto(ProductoDto dto);
    }
}
