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
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios() =>
            Ok(await _usuarioService.GetUsuarios());

        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register(Usuario usuario)
        {
            var usuarioExistente = await _usuarioService.GetUsuarioByNombre(usuario.NombreUsuario);
            if (usuarioExistente != null)
                return Conflict("Usuario ya existe.");

            var nuevoUsuario = await _usuarioService.CrearUsuario(usuario);
            return CreatedAtAction(nameof(Register), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Usuario usuario)
        {
            bool loginExitoso = await _usuarioService.VerificarLogin(usuario.NombreUsuario, usuario.Contrasena);
            if (!loginExitoso)
                return Unauthorized();

            
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            var result = await _usuarioService.ActualizarUsuario(id, usuario);
            return result == null ? NotFound() : NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var result = await _usuarioService.EliminarUsuario(id);
            return result == null ? NotFound() : NoContent();
        }
    }

}
