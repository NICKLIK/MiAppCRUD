using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Services
{
    public interface ICategoriaProductoService
    {
        Task<List<CategoriaProducto>> ObtenerTodas();
        Task<CategoriaProducto?> ObtenerPorId(int id);
        Task<CategoriaProducto> Crear(CategoriaProducto categoria);
        Task<CategoriaProducto?> Actualizar(int id, CategoriaProducto categoria);
        Task<bool> Eliminar(int id);
    }
}
