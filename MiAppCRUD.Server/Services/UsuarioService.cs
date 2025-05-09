using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Helpers;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetUsuarios() => await _context.Usuarios.ToListAsync();

        public async Task<Usuario> GetUsuarioByCorreo(string correo) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

        public async Task<Usuario> GetUsuarioById(int id) =>
            await _context.Usuarios.FindAsync(id);

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
            // Validación de correo existente (Back-End)
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == usuario.Correo);
            if (usuarioExistente != null)
                throw new Exception("El correo electrónico ya está registrado");

            // Validación de ubicación
            if (!UbicacionHelper.ValidarCiudadProvincia(usuario.Provincia, usuario.Ciudad))
                throw new Exception("La ciudad no pertenece a la provincia seleccionada");

            usuario.Contrasena = MD5Helper.EncriptarMD5(usuario.Contrasena);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> VerificarLogin(string correo, string contrasena)
        {
            var hash = MD5Helper.EncriptarMD5(contrasena);
            return await _context.Usuarios.AnyAsync(u => u.Correo == correo && u.Contrasena == hash);
        }

        public async Task<Usuario> ActualizarUsuario(int id, Usuario usuario)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null) return null;

            // Validación de correo existente (Back-End)
            if (usuarioExistente.Correo != usuario.Correo)
            {
                var correoExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == usuario.Correo);
                if (correoExistente != null)
                    throw new Exception("El correo electrónico ya está registrado");
            }

            // Validación de ubicación
            if (!UbicacionHelper.ValidarCiudadProvincia(usuario.Provincia, usuario.Ciudad))
                throw new Exception("La ciudad no pertenece a la provincia seleccionada");

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.Edad = usuario.Edad;
            usuarioExistente.Genero = usuario.Genero;
            usuarioExistente.Correo = usuario.Correo;
            usuarioExistente.Provincia = usuario.Provincia;
            usuarioExistente.Ciudad = usuario.Ciudad;
            usuarioExistente.Contrasena = MD5Helper.EncriptarMD5(usuario.Contrasena);

            await _context.SaveChangesAsync();
            return usuarioExistente;
        }

        public async Task<Usuario> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return null;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        // Nuevo método para obtener provincias
        public List<string> GetProvincias()
        {
            return UbicacionHelper.ProvinciaCiudades.Keys.ToList();
        }
            
        // Nuevo método para obtener ciudades por provincia
        public List<string> GetCiudadesPorProvincia(string provincia)
        {
            if (UbicacionHelper.ProvinciaCiudades.ContainsKey(provincia))
            {
                return UbicacionHelper.ProvinciaCiudades[provincia];
            }
            return new List<string>();
        }
    }
}