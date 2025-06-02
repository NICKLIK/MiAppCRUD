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
            return await _context.ReabastecimientosStock
                .Include(r => r.Producto)
                .ToListAsync();
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
                Console.WriteLine("❌ ERROR al guardar solicitud:");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("👉 Inner: " + ex.InnerException.Message);
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
            // Estado no se actualiza desde el frontend, se controla internamente

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
