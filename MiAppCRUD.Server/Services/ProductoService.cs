using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace MiAppCRUD.Server.Services
{
    public class ProductoService
    {
        private readonly AppDbContext _context;

        public ProductoService(AppDbContext context)
        {
            _context = context;
        }

    
        public async Task<List<Producto>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }

       
        public async Task<Producto> GetProductoById(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        
        public async Task<Producto> CrearProducto(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        
        public async Task<Producto> ActualizarProducto(int id, Producto producto)
        {
            var productoExistente = await _context.Productos.FindAsync(id);
            if (productoExistente == null)
            {
                return null;
            }

            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;

            await _context.SaveChangesAsync();
            return productoExistente;
        }

        
        public async Task<bool> EliminarProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return false;
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
