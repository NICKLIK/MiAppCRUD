using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Factories
{
    public interface IUsuarioFactory
    {
        Usuario CrearUsuario(Usuario usuarioDatos, bool esAdmin, string? claveGenerada = null);
    }
}
