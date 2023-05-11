using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    public class IssueJira
    {
        public string? Id { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? Resumen { get; set; }
        public string? QuienRequiereServicio { get; set; }
        public string? CanalComunicacion { get; set; }
        public string? IdentificacionComponente { get; set; }
        public string? TipoComponente { get; set; }
        public string? IdentificacionSerial { get; set; }
        public string? OperadorMA { get; set; }
        public string? TecnicoAsignado { get; set; }
        public DateTime? FechaHoraLlegadaEstacion { get; set; }
        public string? TipoServicio { get; set; }
        public string? ClaseFallo { get; set; }
        public string? DescripcionFallo { get; set; }
        public string? TipoCausa { get; set; }
        public string? DiagnosticoCausa { get; set; }
        public string? TipoReparacion { get; set; }
        public string? DescripcionReparacion { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaSolucion { get; set; }
        public string? Estacion { get; set; }
        public string? Vagon { get; set; }
        public string? TiempoResolucionAnio { get; set; }
        public string? TiempoResolucionAIO { get; set; }
        public string? AjustesPuerta { get; set; }
        public string? ConfiguracionPuerta { get; set; }
        public string? AjustesITS { get; set; }
        public string? ConfiguracionITS { get; set; }
        public string? AjustesRFID { get; set; }
        public string? ConfiguracionRFID { get; set; }
        public string? DescripcionRepuesto { get; set; }
        public string? CantidadRepuestosUtilizados { get; set; }
        public string? CodigoPlanMantenimiento { get; set; }
        public string? DescripcionActividadMantenimiento { get; set; }
        public string? MotivoAtraso { get; set; }
        public string? OtroMotivoAtraso { get; set; }


    }
}
