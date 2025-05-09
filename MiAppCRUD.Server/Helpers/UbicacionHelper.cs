using System.Collections.Generic;
using System.Linq;

namespace MiAppCRUD.Server.Helpers
{
    public static class UbicacionHelper
    {
        public static Dictionary<string, List<string>> ProvinciaCiudades = new Dictionary<string, List<string>>()
        {
                {"Esmeraldas", new List<string>{"Esmeraldas", "Atacames", "Muisne"}},
                {"Manabí", new List<string>{"Manta", "Portoviejo", "Chone"}},
                {"Los Ríos", new List<string>{"Babahoyo", "Quevedo", "Ventanas"}},
                {"Guayas", new List<string>{"Guayaquil", "Daule", "Milagro", "Samborondón"}},
                {"El Oro", new List<string>{"Machala", "Huaquillas", "Santa Rosa"}},
                {"Santa Elena", new List<string>{"Santa Elena", "Salinas", "La Libertad"}},
                {"Santo Domingo de los Tsáchilas", new List<string>{"Santo Domingo"}},

            // Sierra
                {"Carchi", new List<string>{"Tulcán", "Mira"}},
                {"Imbabura", new List<string>{"Ibarra", "Otavalo", "Atuntaqui"}},
                {"Pichincha", new List<string>{"Quito", "Sangolquí", "Cayambe"}},
                {"Cotopaxi", new List<string>{"Latacunga", "Saquisilí"}},
                {"Tungurahua", new List<string>{"Ambato", "Pelileo"}},
                {"Chimborazo", new List<string>{"Riobamba", "Guano"}},
                {"Bolívar", new List<string>{"Guaranda", "San Miguel"}},
                {"Cañar", new List<string>{"Azogues", "La Troncal"}},
                {"Azuay", new List<string>{"Cuenca", "Gualaceo", "Chordeleg"}},
                {"Loja", new List<string>{"Loja", "Zapotillo"}},

            // Amazonía
                {"Napo", new List<string>{"Tena", "Archidona"}},
                {"Orellana", new List<string>{"Puerto Francisco de Orellana", "Coca"}},
                {"Pastaza", new List<string>{"Puyo", "Mera"}},
                {"Sucumbíos", new List<string>{"Nueva Loja", "Shushufindi"}},
                {"Morona Santiago", new List<string>{"Macas", "Gualaquiza"}},
                {"Zamora Chinchipe", new List<string>{"Zamora", "Yantzaza"}}
        };

        public static bool ValidarCiudadProvincia(string provincia, string ciudad)
        {
            if (string.IsNullOrEmpty(provincia) || string.IsNullOrEmpty(ciudad))
                return false;

            return ProvinciaCiudades.ContainsKey(provincia) &&
                   ProvinciaCiudades[provincia].Contains(ciudad);
        }

        public static List<string> GetProvincias()
        {
            return ProvinciaCiudades.Keys.ToList();
        }

        public static List<string> GetCiudadesPorProvincia(string provincia)
        {
            if (ProvinciaCiudades.ContainsKey(provincia))
            {
                return ProvinciaCiudades[provincia];
            }
            return new List<string>();
        }
    }
}