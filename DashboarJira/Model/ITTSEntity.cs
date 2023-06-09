using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public TiempoTotalOperacion(List<Evento> evp8PorDia, List<Evento> evp9PorDia, DateTime startDate, DateTime endDate, int cantidadPuertas)
        {
            this.evp8PorDia = evp8PorDia;
            this.evp9PorDia = evp9PorDia;
            this.startDate = startDate;
            this.endDate = endDate;
            this.cantidadPuertas = cantidadPuertas;
            calcularTTOP();
        }

        public void calcularTTOP()
        {
            Evento evp8 = new Evento();
            evp8.fechaHoraEnvioDato = startDate.Date.AddHours(4).AddMinutes(30);
            Evento evp9 = new Evento();
            evp9.fechaHoraEnvioDato = startDate.Date.AddMinutes(30);
            if (evp8PorDia.Count>0) {
                evp8 = evp8PorDia[0];
            }
            if (evp9PorDia.Count > 0) {
                evp9 = evp9PorDia[0];
            }
            double diferencia_de_horas = (evp8.fechaHoraEnvioDato - evp9.fechaHoraEnvioDato).Value.TotalHours;
            TTOP = diferencia_de_horas * (double)cantidadPuertas;
        }

        
    }

}
