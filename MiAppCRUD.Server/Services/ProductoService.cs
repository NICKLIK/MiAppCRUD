using MiAppCRUD.Server.Factories;
using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repository;
        private readonly IProductoFactory _productoFactory;

        public ProductoService(IProductoRepository repository, IProductoFactory productoFactory)
        {
            _repository = repository;
            _productoFactory = productoFactory;
        }

        public async Task<List<ProductoRespuestaDto>> GetProductos()
        {
            var productos = await _repository.GetAllWithCategoria();
            var hoy = DateTime.UtcNow;

            var reabastecimientosPendientes = await _repository.GetReabastecimientosPendientes(hoy);

            foreach (var r in reabastecimientosPendientes)
            {
                var producto = productos.FirstOrDefault(p => p.Id == r.ProductoId);
                if (producto != null)
                {
                    producto.Stock += r.Cantidad;
                    r.Estado = "Finalizado";
                }
            }

            if (reabastecimientosPendientes.Any())
            {
                await _repository.SaveChanges();
            }

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
            var producto = await _repository.GetByIdWithCategoria(id);
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
            var categoria = await _repository.GetCategoriaById(dto.CategoriaProductoId);
            if (categoria == null)
                throw new Exception("La categoría seleccionada no existe");

            var producto = _productoFactory.CrearProducto(dto);

            await _repository.Create(producto);

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
            var productoExistente = await _repository.GetById(id);
            if (productoExistente == null)
                return null;

            productoExistente.Nombre = producto.Nombre;
            productoExistente.Descripcion = producto.Descripcion;
            productoExistente.Precio = producto.Precio;
            productoExistente.Stock = producto.Stock;
            productoExistente.ImagenUrl = producto.ImagenUrl;
            productoExistente.EcuniPoints = producto.EcuniPoints;
            productoExistente.CategoriaProductoId = producto.CategoriaProductoId;

            await _repository.SaveChanges();
            return productoExistente;
        }

        public async Task<bool> EliminarProducto(int id)
        {
            var producto = await _repository.GetById(id);
            if (producto == null)
                return false;

            _repository.Delete(producto);
            await _repository.SaveChanges();
            return true;
        }

        public async Task<Producto?> GetProductoEntityById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task GuardarCambios()
        {
            await _repository.SaveChanges();
        }
    }
}
