using MiAppCRUD.Server.Data;
using MiAppCRUD.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MiAppCRUD.Server.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetAll()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario?> GetByCorreo(string correo)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task Create(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await Save();
        }

        public async Task Update(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await Save();
        }

        public async Task Delete(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            await Save();
        }

        public async Task<ClaveAdmin?> GetClaveAdminByCorreo(string correo)
        {
            return await _context.ClavesAdmin.FirstOrDefaultAsync(c => c.Correo == correo);
        }

        public async Task<ClaveAdmin?> GenerarNuevaClaveAdmin(string correo)
        {
            var nuevaClave = new ClaveAdmin
            {
                Correo = correo,
                Clave = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Usada = true
            };

            _context.ClavesAdmin.Add(nuevaClave);
            await Save();
            return nuevaClave;
        }

        public async Task<ClaveAdmin?> GetClaveAdminValidada(string correo, string clave)
        {
            return await _context.ClavesAdmin.FirstOrDefaultAsync(c => c.Correo == correo && c.Clave == clave && c.Usada);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
