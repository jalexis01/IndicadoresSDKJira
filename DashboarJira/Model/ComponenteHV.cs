using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    public class ComponenteHV
    {
        public string? IdComponente { get; set; }
        public string? Serial { get; set; }
        public int? AnioFabricacion { get; set; }
        public string? Modelo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public double? horasDeOperacion { get; set; }
        public int? descargado { get; set; }
        public string? tipoComponente { get; set; }


        public void CalcularHorasDeOperacion()
        {
            DateTime fechaActual = DateTime.Now;

            // Verificar si FechaInicio es la fecha por defecto (1 de enero de 1900)
            if (!this.FechaInicio.HasValue || this.FechaInicio.Value.Year == 1900)
            {
                // Si FechaInicio es la fecha por defecto, establecer horasDeOperacion como -1
                this.horasDeOperacion = -1;
            }
            else
            {
                // Establecer las horas, minutos y segundos a cero para ambas fechas
                DateTime fechaInicioSinHoras = this.FechaInicio.Value.Date;
                DateTime fechaActualSinHoras = fechaActual.Date;

                // Calcular la diferencia sin las horas
                TimeSpan diferencia = fechaActualSinHoras - fechaInicioSinHoras;

                // Asignar el resultado a la propiedad horasDeOperacion
                this.horasDeOperacion = diferencia.TotalHours;
            }
        }
        public string GetTemplateFileName(string marca)
        {
            // Implementa un switch para asignar el nombre de la plantilla según el modelo
            switch (marca)
            {
                case "https://assaabloymda.atlassian.net/":
                    return "Plantilla " + this.Modelo + " Assa.xlsx"; 
                case "https://manateecc.atlassian.net/":
                   
                    return "Plantilla "+ this.Modelo + " Nautilus.xlsx";

                default:
                    // Si el modelo no coincide con ninguno de los casos anteriores, usa una plantilla predeterminada
                    return "Plantilla_Predeterminada.xlsx";
            }
        }

    }

}
