using MiAppCRUD.Server.Factories;
using MiAppCRUD.Server.Helpers;
using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Repositories;

namespace MiAppCRUD.Server.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IUsuarioFactory _factory;

        public UsuarioService(IUsuarioRepository repository, IUsuarioFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        public async Task<Usuario> CrearUsuario(Usuario usuario, string? claveAdmin = null)
        {
            var usuarioExistente = await _repository.GetByCorreo(usuario.Correo);
            if (usuarioExistente != null)
                throw new Exception("El correo electrónico ya está registrado");

            if (!UbicacionHelper.ValidarCiudadProvincia(usuario.Provincia, usuario.Ciudad))
                throw new Exception("La ciudad no pertenece a la provincia seleccionada");

            bool esAdmin = usuario.Correo.ToLower().EndsWith("@admin.com");
            string? claveGenerada = null;

            if (esAdmin)
            {
                var nuevaClave = await _repository.GenerarNuevaClaveAdmin(usuario.Correo);
                claveGenerada = nuevaClave.Clave;
            }

            var nuevoUsuario = _factory.CrearUsuario(usuario, esAdmin, claveGenerada);

            await _repository.Create(nuevoUsuario);
            return nuevoUsuario;
        }

        public async Task<bool> VerificarLogin(string correo, string contrasena)
        {
            var user = await _repository.GetByCorreo(correo.Trim());
            if (user != null)
                return user.Contrasena == contrasena.Trim();

            return false;
        }

        public async Task<Usuario?> ActualizarUsuario(int id, Usuario usuario)
        {
            var usuarioExistente = await _repository.GetById(id);
            if (usuarioExistente == null) return null;

            if (usuarioExistente.Correo != usuario.Correo)
            {
                var correoExistente = await _repository.GetByCorreo(usuario.Correo);
                if (correoExistente != null)
                    throw new Exception("El correo electrónico ya está registrado");
            }

            if (!UbicacionHelper.ValidarCiudadProvincia(usuario.Provincia, usuario.Ciudad))
                throw new Exception("La ciudad no pertenece a la provincia seleccionada");

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.Edad = usuario.Edad;
            usuarioExistente.Genero = usuario.Genero;
            usuarioExistente.Correo = usuario.Correo;
            usuarioExistente.Provincia = usuario.Provincia;
            usuarioExistente.Ciudad = usuario.Ciudad;
            usuarioExistente.Contrasena = usuario.Contrasena;

            await _repository.Update(usuarioExistente);
            return usuarioExistente;
        }

        public async Task<Usuario?> EliminarUsuario(int id)
        {
            var usuario = await _repository.GetById(id);
            if (usuario == null) return null;

            await _repository.Delete(usuario);
            return usuario;
        }

        public List<string> GetProvincias()
        {
            return UbicacionHelper.ProvinciaCiudades.Keys.ToList();
        }

        public List<string> GetCiudadesPorProvincia(string provincia)
        {
            if (UbicacionHelper.ProvinciaCiudades.ContainsKey(provincia))
                return UbicacionHelper.ProvinciaCiudades[provincia];

            return new List<string>();
        }

        public async Task<Usuario?> ObtenerUsuarioAdmin(string correo, string contrasena, string claveAdmin)
        {
            var usuario = await _repository.GetByCorreo(correo);
            if (usuario == null || usuario.Contrasena != contrasena || usuario.Rol != "ADMIN")
                return null;

            var claveValida = await _repository.GetClaveAdminValidada(correo, claveAdmin);
            if (claveValida == null)
                return null;

            return usuario;
        }

        public async Task<string?> ObtenerClaveAdminPorCorreo(string correo)
        {
            var clave = await _repository.GetClaveAdminByCorreo(correo);
            return clave?.Clave;
        }


        public async Task<List<Usuario>> GetUsuarios()
        {
            return await _repository.GetAll();
        }

        public async Task<Usuario?> GetUsuarioByCorreo(string correo)
        {
            return await _repository.GetByCorreo(correo);
        }

        public async Task<Usuario?> GetUsuarioById(int id)
        {
            return await _repository.GetById(id);
        }

    }
}
