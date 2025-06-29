using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiAppCRUD.Server.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime FechaInicio { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime FechaFin { get; set; }

        public double? DescuentoPorcentaje { get; set; }

        public List<EventoProducto> Productos { get; set; } = new();
    }
}
