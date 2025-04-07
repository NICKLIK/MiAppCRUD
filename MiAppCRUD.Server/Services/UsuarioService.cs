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

        public async Task<Usuario> GetUsuarioByNombre(string nombre) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombre);

        public async Task<Usuario> GetUsuarioById(int id) =>
            await _context.Usuarios.FindAsync(id);

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
            usuario.Contrasena = MD5Helper.EncriptarMD5(usuario.Contrasena);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> VerificarLogin(string nombre, string contrasena)
        {
            var hash = MD5Helper.EncriptarMD5(contrasena);
            return await _context.Usuarios.AnyAsync(u => u.NombreUsuario == nombre && u.Contrasena == hash);
        }

        public async Task<Usuario> ActualizarUsuario(int id, Usuario usuario)
        {
            var usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null) return null;

            usuarioExistente.NombreUsuario = usuario.NombreUsuario;
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
    }
}
