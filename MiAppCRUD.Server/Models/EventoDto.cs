using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MiAppCRUD.Server.Models
{
    public class EventoDto
    {
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public List<int>? ProductoIds { get; set; }
        public List<int>? CategoriaIds { get; set; }

        [JsonPropertyName("idsProducto")]
        public List<int> IdsProducto { get; set; } = new();

        public double? DescuentoPorcentaje { get; set; }
    }
}
