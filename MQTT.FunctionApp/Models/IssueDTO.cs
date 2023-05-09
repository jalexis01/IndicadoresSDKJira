using System;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class IssueDTO
    {
        [JsonPropertyName("id_ticket")]
        public string id_ticket { get; set; }

        [JsonPropertyName("idEstacion")]
        public string idEstacion { get; set; }
        [JsonPropertyName("idVagon")]
        public string idVagon { get; set; }

        [JsonPropertyName("Puerta")]
        public string Puerta { get; set; }

        [JsonPropertyName("Componente")]
        public string Componente { get; set; }

        [JsonPropertyName("Identificacion")]
        public string Identificacion { get; set; }

        //[JsonPropertyName("ubicacion")]
        //public string Location { get; set; }

        [JsonPropertyName("tipo_mantenimiento")]
        public string tipo_mantenimiento { get; set; }

        [JsonPropertyName("nivel_falla")]
        public string nivel_falla { get; set; }

        [JsonPropertyName("codigo_falla")]
        public string codigo_falla { get; set; }

        [JsonPropertyName("fecha_apertura")]
        public DateTime? Fecha_apertura { get; set; }

        [JsonPropertyName("fecha_cierre")]
        public DateTime? fecha_cierre { get; set; }

        [JsonPropertyName("fecha_arribo_locacion")]
        public DateTime? fecha_arribo_locacion { get; set; }

        [JsonPropertyName("Componente_Parte")]
        public string Componente_Parte { get; set; }

        [JsonPropertyName("Tipo_reparacion")]
        public string Tipo_reparacion { get; set; }

        [JsonPropertyName("Tipo_ajuste_configuracion")]
        public string Tipo_ajuste_configuracion { get; set; }

        [JsonPropertyName("Descripcion_reparacion")]
        public string Descripcion_reparacion { get; set; }

        [JsonPropertyName("Diagnostico_causa")]
        public string Diagnostico_causa { get; set; }

        //[JsonPropertyName("fecha_inclusion_registro")]
        //public DateTime RecordInclusionDate { get; set; }

        [JsonPropertyName("Estado_ticket")]
        public string Estado_ticket { get; set; }


    }
}
