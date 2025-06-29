using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Services
{
    public interface IProductoService
    {
        Task<List<ProductoRespuestaDto>> GetProductos();
        Task<ProductoRespuestaDto?> GetProductoById(int id);
        Task<ProductoRespuestaDto> CrearProducto(ProductoDto dto);
        Task<Producto?> ActualizarProducto(int id, Producto producto);
        Task<bool> EliminarProducto(int id);
        Task<Producto?> GetProductoEntityById(int id);
        Task GuardarCambios();
    }
}
