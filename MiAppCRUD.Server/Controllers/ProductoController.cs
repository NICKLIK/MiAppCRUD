using Microsoft.AspNetCore.Mvc;
using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoRespuestaDto>>> GetProductos()
        {
            var productos = await _productoService.GetProductos();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoRespuestaDto>> GetProductoById(int id)
        {
            var producto = await _productoService.GetProductoById(id);
            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoRespuestaDto>> CreateProducto([FromBody] ProductoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var dtoRespuesta = await _productoService.CrearProducto(dto);
                return CreatedAtAction(nameof(GetProductoById), new { id = dtoRespuesta.Id }, dtoRespuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] ProductoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = new Producto
            {
                Id = id,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                ImagenUrl = dto.ImagenUrl,
                EcuniPoints = dto.EcuniPoints,
                CategoriaProductoId = dto.CategoriaProductoId
            };

            var actualizado = await _productoService.ActualizarProducto(id, producto);
            if (actualizado == null)
                return NotFound();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var eliminado = await _productoService.EliminarProducto(id);
            if (!eliminado)
                return NotFound();

            return NoContent();
        }

        [HttpPut("descontar-stock/{id}")]
        public async Task<IActionResult> DescontarStock(int id, [FromQuery] int cantidad)
        {
            var producto = await _productoService.GetProductoEntityById(id);
            if (producto == null)
                return NotFound();

            if (producto.Stock < cantidad)
                return BadRequest(new { mensaje = "Stock insuficiente para descontar." });

            producto.Stock -= cantidad;
            await _productoService.GuardarCambios();

            return Ok(new { mensaje = "Stock actualizado exitosamente." });
        }

    }
}
