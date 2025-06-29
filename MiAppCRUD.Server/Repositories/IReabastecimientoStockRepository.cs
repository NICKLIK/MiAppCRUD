using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Repositories
{
    public interface IReabastecimientoStockRepository
    {
        Task<List<ReabastecimientoStock>> GetAllWithProducto();
        Task<ReabastecimientoStock?> GetByIdWithProducto(int id);
        Task<ReabastecimientoStock?> GetById(int id);
        Task Create(ReabastecimientoStock reabastecimiento);
        Task Update(ReabastecimientoStock reabastecimiento);
        Task Delete(ReabastecimientoStock reabastecimiento);
        Task Save();
        Task<List<ReabastecimientoStock>> ObtenerPendientesHastaFecha(DateTime fecha);
    }
}



