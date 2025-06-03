using System;
using System.Collections.Generic;

namespace MiAppCRUD.Server.Models
{
    public class EventoDto
    {
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }


        // Si se usan categorías, estos deben tener valores; si se usan productos, el otro queda null.
        public List<int>? ProductoIds { get; set; }
        public List<int>? CategoriaIds { get; set; }

        public List<int> IdsProducto { get; set; } = new();

        public double? DescuentoPorcentaje { get; set; }

    }
}

