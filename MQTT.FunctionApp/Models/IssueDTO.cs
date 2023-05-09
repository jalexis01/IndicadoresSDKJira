using System;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class IssueDTO
    {
        [JsonPropertyName("idTicket")]
        public string idTicket { get; set; }

        [JsonPropertyName("idEstacion")]
        public string idEstacion { get; set; }
        [JsonPropertyName("idVagon")]
        public string idVagon { get; set; }

        [JsonPropertyName("idPuerta")]
        public string idPuerta { get; set; }

        [JsonPropertyName("Componente")]
        public string Componente { get; set; }

        [JsonPropertyName("Identificacion")]
        public string Identificacion { get; set; }

        //[JsonPropertyName("ubicacion")]
        //public string Location { get; set; }

        [JsonPropertyName("tipoMantenimiento")]
        public string tipoMantenimiento { get; set; }

        [JsonPropertyName("nivelFalla")]
        public string nivelFalla { get; set; }

        [JsonPropertyName("codigoFalla")]
        public string codigoFalla { get; set; }

        [JsonPropertyName("fechaApertura")]
        public DateTime? fechaApertura { get; set; }

        [JsonPropertyName("fechaCierre")]
        public DateTime? fechaCierre { get; set; }

        [JsonPropertyName("fechaArriboLocacion")]
        public DateTime? fechaArriboLocacion { get; set; }

        [JsonPropertyName("ComponenteParte")]
        public string ComponenteParte { get; set; }

        [JsonPropertyName("tipoReparacion")]
        public string tipoReparacion { get; set; }

        [JsonPropertyName("TipoAjusteConfiguracion")]
        public string TipoAjusteConfiguracion { get; set; }

        [JsonPropertyName("DescripcionReparacion")]
        public string DescripcionReparacion { get; set; }

        [JsonPropertyName("DiagnosticoCausa")]
        public string DiagnosticoCausa { get; set; }

        //[JsonPropertyName("fecha_inclusion_registro")]
        //public DateTime RecordInclusionDate { get; set; }

        [JsonPropertyName("EstadoTicket")]
        public string EstadoTicket { get; set; }


    }
}
