using Microsoft.AspNetCore.Mvc;
using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Services;

namespace MiAppCRUD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaProductoController : ControllerBase
    {
        private readonly ICategoriaProductoService _categoriaService;

        public CategoriaProductoController(ICategoriaProductoService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaProductoDto>>> GetCategorias()
        {
            var categorias = await _categoriaService.ObtenerTodas();

            var resultado = categorias.Select(c => new CategoriaProductoDto
            {
                Id = c.Id,
                Nombre = c.Nombre
            }).ToList();

            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaProducto>> GetCategoriaById(int id)
        {
            var categoria = await _categoriaService.ObtenerPorId(id);
            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaProducto>> CreateCategoria([FromBody] CategoriaProductoDto dto)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest(new { message = "El nombre de la categoría es obligatorio." });

            var nueva = await _categoriaService.Crear(new CategoriaProducto
            {
                Nombre = dto.Nombre
            });

            return CreatedAtAction(nameof(GetCategoriaById), new { id = nueva.Id }, nueva);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] CategoriaProductoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return BadRequest(new { message = "El nombre de la categoría es obligatorio." });

            var actualizada = await _categoriaService.Actualizar(id, new CategoriaProducto
            {
                Id = id,
                Nombre = dto.Nombre
            });

            if (actualizada == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var eliminado = await _categoriaService.Eliminar(id);
            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}
