namespace MiAppCRUD.Server.Models
{
    public class EventoProducto
    {
        public int Id { get; set; }

        public int EventoId { get; set; }
        public Evento Evento { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
    }
}
