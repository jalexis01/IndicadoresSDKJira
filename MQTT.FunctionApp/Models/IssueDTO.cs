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

        [JsonPropertyName("tipoComponente")]
        public string tipoComponente { get; set; }

        [JsonPropertyName("idComponente")]
        public string idComponente { get; set; }

        [JsonPropertyName("Identificacion")]
        public string identificacion { get; set; }

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

        [JsonPropertyName("componenteParte")]
        public string componenteParte { get; set; }

        [JsonPropertyName("tipoReparacion")]
        public string tipoReparacion { get; set; }

        [JsonPropertyName("tipoAjusteConfiguracion")]
        public string tipoAjusteConfiguracion { get; set; }

        [JsonPropertyName("descripcionReparacion")]
        public string descripcionReparacion { get; set; }

        [JsonPropertyName("diagnosticoCausa")]
        public string diagnosticoCausa { get; set; }

        [JsonPropertyName("tipoCausa")]
        public string tipoCausa { get; set; }

        //[JsonPropertyName("fecha_inclusion_registro")]
        //public DateTime RecordInclusionDate { get; set; }

        [JsonPropertyName("estadoTicket")]
        public string estadoTicket { get; set; }


    }
}
