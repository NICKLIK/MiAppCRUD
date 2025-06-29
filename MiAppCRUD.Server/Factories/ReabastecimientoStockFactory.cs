using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Factories
{
    public class ReabastecimientoStockFactory : IReabastecimientoStockFactory
    {
        public ReabastecimientoStock CrearDesdeDto(ReabastecimientoDto dto)
        {
            return new ReabastecimientoStock
            {
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                FechaEntrega = DateTime.SpecifyKind(dto.FechaEntrega, DateTimeKind.Utc),
                Estado = "En proceso",
                FechaSolicitud = DateTime.UtcNow
            };
        }
    }
}
