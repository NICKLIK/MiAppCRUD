using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Repositories
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAllWithCategoria();
        Task<List<ReabastecimientoStock>> GetReabastecimientosPendientes(DateTime fecha);
        Task<Producto?> GetByIdWithCategoria(int id);
        Task<Producto?> GetById(int id);
        Task<CategoriaProducto?> GetCategoriaById(int id);
        Task Create(Producto producto);
        void Delete(Producto producto);
        Task SaveChanges();
    }
}
