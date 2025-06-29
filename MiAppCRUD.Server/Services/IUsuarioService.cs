using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetUsuarios();  // Para el GET /api/Usuario
        Task<Usuario?> GetUsuarioByCorreo(string correo);  // Para validar correo y login admin
        Task<Usuario?> GetUsuarioById(int id);  // Para el GET por ID

        Task<Usuario> CrearUsuario(Usuario usuario, string? claveAdmin = null);  // POST /register
        Task<bool> VerificarLogin(string correo, string contrasena);  // POST /login
        Task<Usuario?> ActualizarUsuario(int id, Usuario usuario);  // PUT /{id}
        Task<Usuario?> EliminarUsuario(int id);  // DELETE /{id}

        List<string> GetProvincias();  // GET /provincias
        List<string> GetCiudadesPorProvincia(string provincia);  // GET /ciudades/{provincia}

        Task<Usuario?> ObtenerUsuarioAdmin(string correo, string contrasena, string claveAdmin);  // POST /login-admin
        Task<string?> ObtenerClaveAdminPorCorreo(string correo);  // Para mostrar clave luego de crear admin
    }
}
