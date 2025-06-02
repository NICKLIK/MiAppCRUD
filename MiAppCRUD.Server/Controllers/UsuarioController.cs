using MiAppCRUD.Server.Helpers;
using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetUsuarios();
            return Ok(usuarios);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuarioById(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] Usuario usuario)
        {
            try
            {
                var nuevoUsuario = await _usuarioService.CrearUsuario(usuario);

                if (nuevoUsuario.Rol == "ADMIN")
                {
                    var clave = await _usuarioService.ObtenerClaveAdminPorCorreo(nuevoUsuario.Correo);
                    return Ok(new { mensaje = "Registro exitoso", claveAdmin = clave });
                }

                return Ok(new { mensaje = "Registro exitoso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredencialesDto credenciales)
        {
            try
            {
                bool loginExitoso = await _usuarioService.VerificarLogin(credenciales.Correo, credenciales.Contrasena);
                if (!loginExitoso)
                {
                    return Unauthorized("Credenciales incorrectas");
                }

                return Ok(new { mensaje = "Inicio de sesión exitoso" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            try
            {

                if (!UbicacionHelper.ValidarCiudadProvincia(usuario.Provincia, usuario.Ciudad))
                {
                    return BadRequest("La ciudad no pertenece a la provincia seleccionada");
                }

                var result = await _usuarioService.ActualizarUsuario(id, usuario);
                if (result == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var result = await _usuarioService.EliminarUsuario(id);
            if (result == null)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpGet("provincias")]
        public ActionResult<List<string>> GetProvincias()
        {
            var provincias = UbicacionHelper.GetProvincias();
            return Ok(provincias);
        }


        [HttpGet("ciudades/{provincia}")]
        public ActionResult<List<string>> GetCiudades(string provincia)
        {
            var ciudades = UbicacionHelper.GetCiudadesPorProvincia(provincia);
            if (ciudades.Count == 0)
            {
                return NotFound($"No se encontraron ciudades para la provincia {provincia}");
            }
            return Ok(ciudades);
        }


        [HttpGet("validar-correo/{correo}")]
        public async Task<ActionResult<bool>> ValidarCorreo(string correo)
        {
            var existe = await _usuarioService.GetUsuarioByCorreo(correo);
            return Ok(existe == null);
        }


        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginAdminRequest request)
        {
            var usuario = await _usuarioService.ObtenerUsuarioAdmin(request.Correo, request.Contrasena, request.ClaveAdmin);
            if (usuario == null)
                return Unauthorized(new { message = "Credenciales o clave incorrectas" });

            return Ok(new
            {
                correo = usuario.Correo,
                rol = usuario.Rol
            });
        }


    }
}
