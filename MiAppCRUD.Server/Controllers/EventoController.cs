using Microsoft.AspNetCore.Mvc;
using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Services;

namespace MiAppCRUD.Server.Controllers
{
    [ApiController]
    [Route("api/evento")]
    public class EventoController : ControllerBase
    {
        private readonly EventoService _eventoService;

        public EventoController(EventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Evento>>> GetEventos()
        {
            return await _eventoService.GetEventos();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEventoById(int id)
        {
            var evento = await _eventoService.GetEventoById(id);
            if (evento == null) return NotFound();
            return evento;
        }

        [HttpPost]
        public async Task<ActionResult<Evento>> CrearEvento([FromBody] EventoDto dto)
        {
            var esValido = await _eventoService.ValidarProductosEvento(dto.IdsProducto);
            if (!esValido)
            {
                return BadRequest("Uno o más productos no tienen el stock requerido (mayor a 50)");
            }

            var evento = await _eventoService.CrearEvento(dto);
            return CreatedAtAction(nameof(GetEventoById), new { id = evento.Id }, evento);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Evento>> ActualizarEvento(int id, [FromBody] EventoDto dto)
        {
            var esValido = await _eventoService.ValidarProductosEvento(dto.IdsProducto);
            if (!esValido)
            {
                return BadRequest("Uno o más productos no tienen el stock requerido (mayor a 50)");
            }

            var eventoActualizado = await _eventoService.ActualizarEvento(id, dto);
            if (eventoActualizado == null) return NotFound();
            return eventoActualizado;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEvento(int id)
        {
            var eliminado = await _eventoService.EliminarEvento(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }

        [HttpGet("productos-stock-50")]
        public async Task<ActionResult<List<ProductoDto>>> GetProductosConStockMayorA50()
        {
            var productos = await _eventoService.GetProductosConStockMayorA50();
            return Ok(productos);
        }

        [HttpGet("categorias-stock-50")]
        public async Task<ActionResult<List<CategoriaProductoDto>>> GetCategoriasConTodosProductosConStockMayorA50()
        {
            var categorias = await _eventoService.GetCategoriasConTodosProductosConStockMayorA50();
            return Ok(categorias);
        }

        [HttpPost("validar-productos")]
        public async Task<ActionResult<bool>> ValidarProductos([FromBody] List<int> ids)
        {
            var esValido = await _eventoService.ValidarProductosEvento(ids);
            return Ok(esValido);
        }

        [HttpPost("validar-categorias")]
        public async Task<ActionResult<bool>> ValidarCategorias([FromBody] List<int> ids)
        {
            var esValido = await _eventoService.ValidarCategoriasEvento(ids);
            return Ok(esValido);
        }


    }
}
