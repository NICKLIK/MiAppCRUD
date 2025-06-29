using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Repositories
{
    public class ReabastecimientoStockRepository : IReabastecimientoStockRepository
    {
        private readonly AppDbContext _context;

        public ReabastecimientoStockRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReabastecimientoStock>> GetAllWithProducto()
        {
            return await _context.ReabastecimientosStock
                .Include(r => r.Producto)
                .ToListAsync();
        }

        public async Task<ReabastecimientoStock?> GetByIdWithProducto(int id)
        {
            return await _context.ReabastecimientosStock
                .Include(r => r.Producto)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ReabastecimientoStock?> GetById(int id)
        {
            return await _context.ReabastecimientosStock
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ReabastecimientoStock>> ObtenerPendientesHastaFecha(DateTime fecha)
        {
            return await _context.ReabastecimientosStock
                .Include(r => r.Producto)
                .Where(r => r.Estado == "En proceso" && r.FechaEntrega.Date <= fecha.Date)
                .ToListAsync();
        }


        public async Task Create(ReabastecimientoStock reabastecimiento)
        {
            _context.ReabastecimientosStock.Add(reabastecimiento);
            await Save();
        }

        public async Task Update(ReabastecimientoStock reabastecimiento)
        {
            _context.ReabastecimientosStock.Update(reabastecimiento);
            await Save();
        }

        public async Task Delete(ReabastecimientoStock reabastecimiento)
        {
            _context.ReabastecimientosStock.Remove(reabastecimiento);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
