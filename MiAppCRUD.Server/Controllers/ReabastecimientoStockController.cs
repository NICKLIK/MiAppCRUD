using Microsoft.AspNetCore.Mvc;
using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Services;

namespace MiAppCRUD.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReabastecimientoStockController : ControllerBase
    {
        private readonly IReabastecimientoStockService _service;

        public ReabastecimientoStockController(IReabastecimientoStockService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReabastecimientoStock>>> GetAll()
        {
            var lista = await _service.ObtenerTodos();
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReabastecimientoStock>> GetById(int id)
        {
            var item = await _service.ObtenerPorId(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ReabastecimientoStock>> Create([FromBody] ReabastecimientoDto dto)
        {
            try
            {
                var nueva = await _service.Crear(dto);
                return CreatedAtAction(nameof(GetById), new { id = nueva.Id }, nueva);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReabastecimientoDto dto)
        {
            try
            {
                var actualizado = await _service.Actualizar(id, dto);
                if (actualizado == null) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.Eliminar(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }
    }
}
