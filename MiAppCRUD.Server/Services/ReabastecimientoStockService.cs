using MiAppCRUD.Server.Factories;
using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Repositories;

namespace MiAppCRUD.Server.Services
{
    public class ReabastecimientoStockService : IReabastecimientoStockService
    {
        private readonly IReabastecimientoStockRepository _repository;
        private readonly IReabastecimientoStockFactory _factory;

        public ReabastecimientoStockService(IReabastecimientoStockRepository repository, IReabastecimientoStockFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public async Task<List<ReabastecimientoStock>> ObtenerTodos()
        {
            var hoy = DateTime.UtcNow.Date;
            var solicitudes = await _repository.GetAllWithProducto();

            var cambios = false;

            foreach (var solicitud in solicitudes)
            {
                if (solicitud.Estado == "En proceso" && solicitud.FechaEntrega.Date <= hoy)
                {
                    if (solicitud.Producto != null)
                    {
                        solicitud.Producto.Stock += solicitud.Cantidad;
                        solicitud.Estado = "Finalizado";
                        cambios = true;
                    }
                }
            }

            if (cambios)
            {
                await _repository.Save();
            }

            return solicitudes;
        }

        public async Task<ReabastecimientoStock?> ObtenerPorId(int id)
        {
            return await _repository.GetByIdWithProducto(id);
        }

        public async Task<ReabastecimientoStock> Crear(ReabastecimientoDto dto)
        {
            var nueva = _factory.CrearDesdeDto(dto);
            await _repository.Create(nueva);
            return nueva;
        }

        public async Task<ReabastecimientoStock?> Actualizar(int id, ReabastecimientoDto dto)
        {
            var existente = await _repository.GetById(id);
            if (existente == null) return null;

            existente.ProductoId = dto.ProductoId;
            existente.Cantidad = dto.Cantidad;
            existente.FechaEntrega = DateTime.SpecifyKind(dto.FechaEntrega, DateTimeKind.Utc);

            await _repository.Update(existente);
            return existente;
        }

        public async Task<bool> Eliminar(int id)
        {
            var existente = await _repository.GetById(id);
            if (existente == null) return false;

            await _repository.Delete(existente);
            return true;
        }
    }
}
