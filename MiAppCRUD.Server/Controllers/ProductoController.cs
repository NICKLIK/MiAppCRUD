using Microsoft.AspNetCore.Mvc;
using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Services; 
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _productoService.GetProductos(); 
            return Ok(productos);  
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProductoById(int id)
        {
            var producto = await _productoService.GetProductoById(id);  

            if (producto == null)
            {
                return NotFound();  
            }

            return Ok(producto);  
        }

        
        [HttpPost]
        public async Task<ActionResult<Producto>> CreateProducto(Producto producto)
        {
            var nuevoProducto = await _productoService.CrearProducto(producto);  
            return CreatedAtAction(nameof(GetProductoById), new { id = nuevoProducto.Id }, nuevoProducto);  
        }

       
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();  
            }

            var updatedProducto = await _productoService.ActualizarProducto(id, producto);  

            if (updatedProducto == null)
            {
                return NotFound();  
            }

            return NoContent();  
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var deletedProducto = await _productoService.EliminarProducto(id);  

            if (deletedProducto == null)
            {
                return NotFound();  
            }

            return NoContent();  
        }
    }
}
