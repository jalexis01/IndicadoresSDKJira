using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    public class TiempoTotalOperacion
    {
        public List<Evento> evp8PorDia { get; set; }
        public List<Evento> evp9PorDia { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int cantidadPuertas { get; set; }
        public double TTOP { get; set; }

        public double cantidadEVP8 { get; set; }
        public double cantidadEVP9 { get; set; }
        public double IOR { get; set; }

        public TiempoTotalOperacion(List<Evento> evp8PorDia, List<Evento> evp9PorDia, DateTime startDate, DateTime endDate, int cantidadPuertas)
        {
            this.evp8PorDia = evp8PorDia;
            this.evp9PorDia = evp9PorDia;
            this.startDate = startDate;
            this.endDate = endDate;
            this.cantidadPuertas = cantidadPuertas;
            calcularTTOP();
        }
        public TiempoTotalOperacion(List<Evento> evp8PorDia, List<Evento> evp9PorDia, DateTime startDate, DateTime endDate)
        {
            this.evp8PorDia = evp8PorDia;
            this.evp9PorDia = evp9PorDia;
            this.startDate = startDate;
            this.endDate = endDate;
            this.cantidadPuertas = cantidadPuertas;
            cantidadEVP8 = evp8PorDia.Count;
            cantidadEVP9 = evp9PorDia.Count;
            calcularIOR();

        }
        public void calcularTTOP()
        {
            Evento evp8 = new Evento();
            evp8.fechaHoraEnvioDato = startDate.Date.AddHours(4).AddMinutes(30);
            Evento evp9 = new Evento();
            evp9.fechaHoraEnvioDato = endDate.Date.AddMinutes(30);
            if (evp8PorDia.Count>0) {
                evp8 = evp8PorDia[0];
            }
            if (evp9PorDia.Count > 0) {
                evp9 = evp9PorDia[0];
            }
            double diferencia_de_horas = (evp9.fechaHoraEnvioDato - evp8.fechaHoraEnvioDato).Value.TotalHours;
            TTOP = diferencia_de_horas * (double)cantidadPuertas;
        }
        public void calcularIOR() {
            IOR = (double)cantidadEVP8 / (double)cantidadEVP9;
        
        }
        public string ConvertirAJson()
        {
            var objeto = new
            {
                evp8PorDia,
                evp9PorDia,
                startDate,
                endDate,
                cantidadPuertas,
                TTOP
            };

            var opcionesJson = new JsonSerializerOptions
            {
                WriteIndented = true // Para que el JSON tenga formato legible
            };

            string json = JsonSerializer.Serialize(objeto, opcionesJson);
            return json;
        }
        public string ConvertirAJsonIOR()
        {
            var objeto = new
            {
                cantidadEVP8,
                cantidadEVP9,
                startDate,
                endDate,
                IOR
            };

            var opcionesJson = new JsonSerializerOptions
            {
                WriteIndented = true // Para que el JSON tenga formato legible
            };

            string json = JsonSerializer.Serialize(objeto, opcionesJson);
            return json;
        }
    }

}
