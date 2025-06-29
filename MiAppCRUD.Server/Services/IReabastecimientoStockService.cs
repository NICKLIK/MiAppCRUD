using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Services
{
    public interface IReabastecimientoStockService
    {
        Task<List<ReabastecimientoStock>> ObtenerTodos();
        Task<ReabastecimientoStock?> ObtenerPorId(int id);
        Task<ReabastecimientoStock> Crear(ReabastecimientoDto dto);
        Task<ReabastecimientoStock?> Actualizar(int id, ReabastecimientoDto dto);
        Task<bool> Eliminar(int id);
    }
}
