using System;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class IssueDTO
    {
        [JsonPropertyName("id_ticket")]
        public string IdTicket { get; set; }

        [JsonPropertyName("idEstacion")]
        public string Station { get; set; }
        [JsonPropertyName("idVagon")]
        public string Wagon { get; set; }

        [JsonPropertyName("idPuerta")]
        public string Door { get; set; }

        [JsonPropertyName("Componente")]
        public string Component { get; set; }

        [JsonPropertyName("Identificacion")]
        public string Serial { get; set; }

        //[JsonPropertyName("ubicacion")]
        //public string Location { get; set; }

        [JsonPropertyName("tipo_mantenimiento")]
        public string MaintenanceType { get; set; }

        [JsonPropertyName("nivel_falla")]
        public string FailureLevel { get; set; }

        [JsonPropertyName("codigo_falla")]
        public string FaultCode { get; set; }

        [JsonPropertyName("fecha_apertura")]
        public DateTime CreationDate {get; set; }

        [JsonPropertyName("fecha_cierre")]
        public DateTime ResolutionDate { get; set; }

        [JsonPropertyName("fecha_arribo_locacion")]
        public DateTime LocationDate { get; set; }

        [JsonPropertyName("Componente_parte")]
        public string ComponentITS { get; set; }

        [JsonPropertyName("Tipo_reparacion")]
        public string TypeOfRepair { get; set; }

        [JsonPropertyName("Tipo_ajuste_configuracion")]
        public string TypeSettingConfiguration { get; set; }

        [JsonPropertyName("Descripcion_reparacion")]
        public string DescriptionRepair { get; set; }

        [JsonPropertyName("Diagnostico_causa")]
        public string DiagnosisCause { get; set; }

        //[JsonPropertyName("fecha_inclusion_registro")]
        //public DateTime RecordInclusionDate { get; set; }

        [JsonPropertyName("estado_ticket")]
        public string Status { get; set; }


    }
}
