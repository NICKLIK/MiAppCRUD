using System;
using System.Collections.Generic;

namespace MiAppCRUD.Server.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // Opcional: descuento sobre precio (solo aplica si se desea)
        public double? DescuentoPorcentaje { get; set; }

        // Relación: uno a muchos con EventoProducto
        public List<EventoProducto> Productos { get; set; } = new();
    }
}
