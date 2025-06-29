using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Repositories
{
    public class CategoriaProductoRepository : ICategoriaProductoRepository
    {
        private readonly AppDbContext _context;

        public CategoriaProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaProducto>> GetAll()
        {
            return await _context.CategoriasProducto
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<CategoriaProducto?> GetById(int id)
        {
            return await _context.CategoriasProducto
                                 .Include(c => c.Productos)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Create(CategoriaProducto categoria)
        {
            _context.CategoriasProducto.Add(categoria);
            await Save();
        }

        public async Task Update(CategoriaProducto categoria)
        {
            _context.CategoriasProducto.Update(categoria);
            await Save();
        }

        public async Task Delete(CategoriaProducto categoria)
        {
            _context.CategoriasProducto.Remove(categoria);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
