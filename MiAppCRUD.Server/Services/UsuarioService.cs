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

        public async Task<Usuario> CrearUsuario(Usuario usuario, string claveAdmin = null)
        {
            var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == usuario.Correo);
            if (usuarioExistente != null)
                throw new Exception("El correo electrónico ya está registrado");

            if (!UbicacionHelper.ValidarCiudadProvincia(usuario.Provincia, usuario.Ciudad))
                throw new Exception("La ciudad no pertenece a la provincia seleccionada");

            // Comprobación para determinar si es admin
            bool esAdmin = usuario.Correo.ToLower().EndsWith("@admin.com");

            if (esAdmin)
            {
                // Generar clave única aleatoria
                string nuevaClave = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

                // Crear y asociar clave al correo del admin
                var claveAdminGenerada = new ClaveAdmin
                {
                    Correo = usuario.Correo,
                    Clave = nuevaClave,
                    Usada = true // se marca como activa y lista para usar
                };

                _context.ClavesAdmin.Add(claveAdminGenerada);
                usuario.Rol = "ADMIN";
            }
            else
            {
                usuario.Rol = "USUARIO";
            }

            // usuario.Contrasena = MD5Helper.EncriptarMD5(usuario.Contrasena);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> VerificarLogin(string correo, string contrasena)
        {
            correo = correo.Trim();
            contrasena = contrasena.Trim();

            Console.WriteLine(">>> LOGIN CORREO RECIBIDO: " + correo);
            Console.WriteLine(">>> LOGIN CONTRASEÑA RECIBIDA: " + contrasena);

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

            if (user != null)
            {
                Console.WriteLine(">>> USUARIO ENCONTRADO. CONTRASEÑA EN BD: " + user.Contrasena);
                Console.WriteLine(">>> ¿Coincide? " + (user.Contrasena == contrasena));
                return user.Contrasena == contrasena;
            }

            Console.WriteLine(">>> USUARIO NO ENCONTRADO");
            return false;
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
            usuarioExistente.Contrasena = usuario.Contrasena;


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

        public async Task<Usuario?> ObtenerUsuarioAdmin(string correo, string contrasena, string claveAdmin)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.Correo == correo && u.Contrasena == contrasena && u.Rol == "ADMIN");

            var claveValida = await _context.ClavesAdmin.FirstOrDefaultAsync(c =>
                c.Correo == correo && c.Clave == claveAdmin && c.Usada);

            if (usuario != null && claveValida != null)
                return usuario;

            return null;
        }


        public async Task<string?> ObtenerClaveAdminPorCorreo(string correo)
        {
            var clave = await _context.ClavesAdmin.FirstOrDefaultAsync(c => c.Correo == correo);
            return clave?.Clave;
        }

    }
}