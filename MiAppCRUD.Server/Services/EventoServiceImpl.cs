using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Services
{
    public class EventoServiceImpl : EventoService
    {
        private readonly AppDbContext _context;

        public EventoServiceImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Evento>> GetEventos()
        {
            return await _context.Eventos
                .Include(e => e.Productos)
                .ThenInclude(ep => ep.Producto)
                .ToListAsync();
        }

        public async Task<Evento?> GetEventoById(int id)
        {
            return await _context.Eventos
                .Include(e => e.Productos)
                .ThenInclude(ep => ep.Producto)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Evento> CrearEvento(EventoDto dto)
        {
            var nuevoEvento = new Evento
            {
                Nombre = dto.Nombre,
                FechaInicio = DateTime.SpecifyKind(dto.FechaInicio, DateTimeKind.Utc),
                FechaFin = DateTime.SpecifyKind(dto.FechaFin, DateTimeKind.Utc),
                DescuentoPorcentaje = dto.DescuentoPorcentaje
            };

            _context.Eventos.Add(nuevoEvento);
            await _context.SaveChangesAsync();

            foreach (var idProducto in dto.IdsProducto)
            {
                var producto = await _context.Productos.FindAsync(idProducto);
                if (producto != null)
                {
                    var relacion = new EventoProducto
                    {
                        EventoId = nuevoEvento.Id,
                        ProductoId = producto.Id
                    };
                    _context.EventosProductos.Add(relacion);
                }
            }

            await _context.SaveChangesAsync();
            return nuevoEvento;
        }

        public async Task<Evento?> ActualizarEvento(int id, EventoDto dto)
        {
            var eventoExistente = await _context.Eventos
                .Include(e => e.Productos)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventoExistente == null)
                return null;

            eventoExistente.Nombre = dto.Nombre;
            eventoExistente.FechaInicio = DateTime.SpecifyKind(dto.FechaInicio, DateTimeKind.Utc);
            eventoExistente.FechaFin = DateTime.SpecifyKind(dto.FechaFin, DateTimeKind.Utc);
            eventoExistente.DescuentoPorcentaje = dto.DescuentoPorcentaje;

            _context.EventosProductos.RemoveRange(eventoExistente.Productos);

            foreach (var idProducto in dto.IdsProducto)
            {
                _context.EventosProductos.Add(new EventoProducto
                {
                    EventoId = id,
                    ProductoId = idProducto
                });
            }

            await _context.SaveChangesAsync();
            return eventoExistente;
        }

        public async Task<bool> EliminarEvento(int id)
        {
            var evento = await _context.Eventos
                .Include(e => e.Productos)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null) return false;

            _context.EventosProductos.RemoveRange(evento.Productos);
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductoDto>> GetProductosConStockMayorA50()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Stock > 50)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Stock = p.Stock,
                    ImagenUrl = p.ImagenUrl,
                    EcuniPoints = p.EcuniPoints,
                    CategoriaProductoId = p.CategoriaProductoId
                })
                .ToListAsync();
        }

        public async Task<List<CategoriaProductoDto>> GetCategoriasConTodosProductosConStockMayorA50()
        {
            return await _context.CategoriasProducto
                .Include(c => c.Productos)
                .Where(c => c.Productos.Any() && c.Productos.All(p => p.Stock > 50))
                .Select(c => new CategoriaProductoDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre
                })
                .ToListAsync();
        }

        public async Task<bool> ValidarProductosEvento(List<int> idsProductos)
        {
            return await _context.Productos
                .Where(p => idsProductos.Contains(p.Id))
                .AllAsync(p => p.Stock > 50);
        }

        public async Task<bool> ValidarCategoriasEvento(List<int> idsCategorias)
        {
            return await _context.Productos
                .Where(p => idsCategorias.Contains(p.CategoriaProductoId))
                .GroupBy(p => p.CategoriaProductoId)
                .AllAsync(g => g.All(p => p.Stock > 50));
        }
    }
}
