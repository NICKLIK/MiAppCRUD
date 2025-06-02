using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Services
{
    public class CategoriaProductoService
    {
        private readonly AppDbContext _context;

        public CategoriaProductoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaProducto>> GetCategorias()
        {
            return await _context.CategoriasProducto
                                 .AsNoTracking() 
                                 .ToListAsync();
        }


        public async Task<CategoriaProducto> GetCategoriaById(int id)
        {
            return await _context.CategoriasProducto
                                 .Include(c => c.Productos)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CategoriaProducto> CrearCategoria(CategoriaProducto categoria)
        {
            _context.CategoriasProducto.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<CategoriaProducto> ActualizarCategoria(int id, CategoriaProducto categoria)
        {
            var existente = await _context.CategoriasProducto.FindAsync(id);
            if (existente == null) return null;

            existente.Nombre = categoria.Nombre;
            await _context.SaveChangesAsync();
            return existente;
        }

        public async Task<bool> EliminarCategoria(int id)
        {
            var categoria = await _context.CategoriasProducto.FindAsync(id);
            if (categoria == null) return false;

            _context.CategoriasProducto.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
