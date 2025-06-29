using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAll();
        Task<Usuario?> GetById(int id);
        Task<Usuario?> GetByCorreo(string correo);
        Task Create(Usuario usuario);
        Task Update(Usuario usuario);
        Task Delete(Usuario usuario);
        Task<ClaveAdmin?> GetClaveAdminByCorreo(string correo);
        Task<ClaveAdmin?> GenerarNuevaClaveAdmin(string correo);
        Task<ClaveAdmin?> GetClaveAdminValidada(string correo, string clave);
        Task Save();
    }
}
