using MiAppCRUD.Server.Models;
using MiAppCRUD.Server.Repositories;

namespace MiAppCRUD.Server.Services
{
    public class CategoriaProductoService : ICategoriaProductoService
    {
        private readonly ICategoriaProductoRepository _repository;

        public CategoriaProductoService(ICategoriaProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoriaProducto>> ObtenerTodas()
        {
            return await _repository.GetAll();
        }

        public async Task<CategoriaProducto?> ObtenerPorId(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<CategoriaProducto> Crear(CategoriaProducto categoria)
        {
            await _repository.Create(categoria);
            return categoria;
        }

        public async Task<CategoriaProducto?> Actualizar(int id, CategoriaProducto categoria)
        {
            var existente = await _repository.GetById(id);
            if (existente == null)
                return null;

            existente.Nombre = categoria.Nombre;
            await _repository.Update(existente);
            return existente;
        }

        public async Task<bool> Eliminar(int id)
        {
            var existente = await _repository.GetById(id);
            if (existente == null)
                return false;

            await _repository.Delete(existente);
            return true;
        }
    }
}
