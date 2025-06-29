using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _context;

        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> GetAllWithCategoria()
        {
            return await _context.Productos.Include(p => p.Categoria).ToListAsync();
        }

        public async Task<Producto?> GetByIdWithCategoria(int id)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ReabastecimientoStock>> GetReabastecimientosPendientes(DateTime fecha)
        {
            return await _context.ReabastecimientosStock
                .Where(r => r.Estado == "En proceso" && r.FechaEntrega <= fecha)
                .ToListAsync();
        }

        public async Task<CategoriaProducto?> GetCategoriaById(int id)
        {
            return await _context.CategoriasProducto.FindAsync(id);
        }

        public async Task<Producto?> GetById(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task Create(Producto producto)
        {
            _context.Productos.Add(producto);
            await SaveChanges();
        }

        public void Delete(Producto producto)
        {
            _context.Productos.Remove(producto);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
