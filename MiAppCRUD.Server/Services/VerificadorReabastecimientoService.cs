using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Services
{
    public class VerificadorReabastecimientoService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VerificadorReabastecimientoService> _logger;

        public VerificadorReabastecimientoService(IServiceProvider serviceProvider, ILogger<VerificadorReabastecimientoService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var ahora = DateTime.UtcNow.Date;

                    var pendientes = await dbContext.ReabastecimientosStock
                        .Include(r => r.Producto)
                        .Where(r => r.Estado == "En proceso")
                        .ToListAsync();

                    var vencidos = pendientes
                        .Where(r => r.FechaEntrega.Date <= ahora)
                        .ToList();

                    foreach (var r in vencidos)
                    {
                        r.Estado = "Finalizado";
                        r.Producto.Stock += r.Cantidad;
                        _logger.LogInformation($"Stock actualizado para producto {r.Producto.Nombre}: +{r.Cantidad}");
                    }

                    if (vencidos.Any())
                    {
                        await dbContext.SaveChangesAsync();
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
