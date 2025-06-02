using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Services
{
    public class ReabastecimientoStockService
    {
        private readonly AppDbContext _context;

        public ReabastecimientoStockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReabastecimientoStock>> ObtenerTodos()
        {
            var hoy = DateTime.UtcNow.Date;

            var solicitudes = await _context.ReabastecimientosStock
                .Include(r => r.Producto)
                .ToListAsync();

            var cambios = false;

            foreach (var solicitud in solicitudes)
            {
                if (solicitud.Estado == "En proceso" && solicitud.FechaEntrega.Date <= hoy)
                {
                    var producto = solicitud.Producto;

                    if (producto != null)
                    {
                        producto.Stock += solicitud.Cantidad;
                        solicitud.Estado = "Finalizado";
                        cambios = true;
                    }
                }
            }

            if (cambios)
            {
                await _context.SaveChangesAsync();
            }

            return solicitudes;
        }


        public async Task<ReabastecimientoStock> ObtenerPorId(int id)
        {
            return await _context.ReabastecimientosStock
                .Include(r => r.Producto)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ReabastecimientoStock> Crear(ReabastecimientoDto dto)
        {
            try
            {
                var solicitud = new ReabastecimientoStock
                {
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad,
                    FechaEntrega = DateTime.SpecifyKind(dto.FechaEntrega, DateTimeKind.Utc),
                    Estado = "En proceso",
                    FechaSolicitud = DateTime.UtcNow
                };

                _context.ReabastecimientosStock.Add(solicitud);
                await _context.SaveChangesAsync();
                return solicitud;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR al guardar solicitud:");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner: " + ex.InnerException.Message);
                throw;
            }
        }


        public async Task<ReabastecimientoStock> Actualizar(int id, ReabastecimientoDto dto)
        {
            var existente = await _context.ReabastecimientosStock.FindAsync(id);
            if (existente == null) return null;

            existente.ProductoId = dto.ProductoId;
            existente.Cantidad = dto.Cantidad;
            existente.FechaEntrega = DateTime.SpecifyKind(dto.FechaEntrega, DateTimeKind.Utc);
            

            await _context.SaveChangesAsync();
            return existente;
        }


        public async Task<bool> Eliminar(int id)
        {
            var existente = await _context.ReabastecimientosStock.FindAsync(id);
            if (existente == null) return false;

            _context.ReabastecimientosStock.Remove(existente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
