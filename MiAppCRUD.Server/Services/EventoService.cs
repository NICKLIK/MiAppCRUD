using MiAppCRUD.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiAppCRUD.Server.Services
{
    public interface EventoService
    {
        Task<List<Evento>> GetEventos();
        Task<Evento?> GetEventoById(int id);
        Task<Evento> CrearEvento(EventoDto dto);
        Task<Evento?> ActualizarEvento(int id, EventoDto dto);
        Task<bool> EliminarEvento(int id);

        // Ahora devuelven DTOs
        Task<List<ProductoDto>> GetProductosConStockMayorA50();
        Task<List<CategoriaProductoDto>> GetCategoriasConTodosProductosConStockMayorA50();

        Task<bool> ValidarProductosEvento(List<int> idsProductos);
        Task<bool> ValidarCategoriasEvento(List<int> idsCategorias);
    }
}
