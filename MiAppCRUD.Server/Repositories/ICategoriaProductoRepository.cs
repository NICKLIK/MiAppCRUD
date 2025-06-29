using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Repositories
{
    public interface ICategoriaProductoRepository
    {
        Task<List<CategoriaProducto>> GetAll();
        Task<CategoriaProducto?> GetById(int id);
        Task Create(CategoriaProducto categoria);
        Task Update(CategoriaProducto categoria);
        Task Delete(CategoriaProducto categoria);
        Task Save();
    }
}
