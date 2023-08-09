using System.Text.Json;

namespace DashboarJira.Model
{
    public class EstacionEntity
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


        public List<Evento> evp14PorDia { get; set; }
        public List<Evento> evp11PorDia { get; set; }
        public List<Evento> evp10PorDia { get; set; }

        public double IDM { get; set; }

        public EstacionEntity(List<Evento> evp10PorDia, List<Evento> evp11PorDia, List<Evento> evp14PorDia, DateTime startDate, DateTime endDate)
        {
            this.endDate = endDate;
            this.startDate = startDate;
            this.evp14PorDia = evp14PorDia;
            this.evp11PorDia = evp11PorDia;
            this.evp10PorDia = evp10PorDia;
            calcularIDM();
        }

        private void calcularIDM()
        {
            double tmr = evp10PorDia.Count + evp11PorDia.Count + evp14PorDia.Count;
            if (tmr != 0)
                IDM = tmr / tmr;
            else IDM = 1;

        }

        public EstacionEntity(List<Evento> evp8PorDia, List<Evento> evp9PorDia, DateTime startDate, DateTime endDate, int cantidadPuertas)
        {
            this.evp8PorDia = evp8PorDia;
            this.evp9PorDia = evp9PorDia;
            this.startDate = startDate;
            this.endDate = endDate;
            this.cantidadPuertas = cantidadPuertas;
            calcularTTOP();
        }



        public EstacionEntity(List<Evento> evp8PorDia, List<Evento> evp9PorDia, DateTime startDate, DateTime endDate)
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
            if (evp8PorDia.Count > 0)
            {
                evp8 = evp8PorDia[0];
            }
            if (evp9PorDia.Count > 0)
            {
                evp9 = evp9PorDia[0];
            }
            double diferencia_de_horas = (evp9.fechaHoraEnvioDato - evp8.fechaHoraEnvioDato).Value.TotalHours;
            TTOP = diferencia_de_horas * (double)cantidadPuertas;
        }
        public void calcularIOR()
        {

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

        public string ConvertirAJsonIDM()
        {
            int evp10 = evp10PorDia.Count;
            int evp11 = evp11PorDia.Count;
            int evp14 = evp14PorDia.Count;
            var objeto = new
            {
                evp10,
                evp11,
                evp14,
                startDate,
                endDate,
                IDM
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
