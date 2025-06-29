using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Factories
{
    public interface IReabastecimientoStockFactory
    {
        ReabastecimientoStock CrearDesdeDto(ReabastecimientoDto dto);
    }
}
