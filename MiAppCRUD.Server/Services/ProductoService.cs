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

        public async Task<List<ProductoRespuestaDto>> GetProductos()
        {
            var productos = await _context.Productos.Include(p => p.Categoria).ToListAsync();

            return productos.Select(p => new ProductoRespuestaDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                ImagenUrl = p.ImagenUrl,
                EcuniPoints = p.EcuniPoints,
                CategoriaProductoId = p.CategoriaProductoId,
                CategoriaNombre = p.Categoria?.Nombre
            }).ToList();
        }

        public async Task<ProductoRespuestaDto?> GetProductoById(int id)
        {
            var producto = await _context.Productos.Include(p => p.Categoria)
                                                   .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null) return null;

            return new ProductoRespuestaDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                ImagenUrl = producto.ImagenUrl,
                EcuniPoints = producto.EcuniPoints,
                CategoriaProductoId = producto.CategoriaProductoId,
                CategoriaNombre = producto.Categoria?.Nombre
            };
        }

        public async Task<ProductoRespuestaDto> CrearProducto(ProductoDto dto)
        {
            var categoria = await _context.CategoriasProducto.FindAsync(dto.CategoriaProductoId);
            if (categoria == null)
                throw new Exception("La categoría seleccionada no existe");

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                ImagenUrl = dto.ImagenUrl,
                EcuniPoints = dto.EcuniPoints,
                CategoriaProductoId = dto.CategoriaProductoId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return new ProductoRespuestaDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                ImagenUrl = producto.ImagenUrl,
                EcuniPoints = producto.EcuniPoints,
                CategoriaProductoId = producto.CategoriaProductoId,
                CategoriaNombre = categoria.Nombre
            };
        }


        public async Task<Producto?> ActualizarProducto(int id, Producto producto)
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
            productoExistente.ImagenUrl = producto.ImagenUrl;
            productoExistente.EcuniPoints = producto.EcuniPoints;
            productoExistente.CategoriaProductoId = producto.CategoriaProductoId;

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
