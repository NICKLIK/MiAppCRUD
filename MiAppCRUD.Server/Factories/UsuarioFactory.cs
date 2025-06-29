using MiAppCRUD.Server.Models;

namespace MiAppCRUD.Server.Factories
{
    public class UsuarioFactory : IUsuarioFactory
    {
        public Usuario CrearUsuario(Usuario usuarioDatos, bool esAdmin, string? claveGenerada = null)
        {
            var nuevoUsuario = new Usuario
            {
                Nombre = usuarioDatos.Nombre,
                Apellido = usuarioDatos.Apellido,
                Edad = usuarioDatos.Edad,
                Genero = usuarioDatos.Genero,
                Correo = usuarioDatos.Correo,
                Provincia = usuarioDatos.Provincia,
                Ciudad = usuarioDatos.Ciudad,
                Contrasena = usuarioDatos.Contrasena,
                Rol = esAdmin ? "ADMIN" : "USUARIO"
            };

            return nuevoUsuario;
        }
    }
}
