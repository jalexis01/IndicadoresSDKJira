using System;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class IssueDTO
    {
        [JsonPropertyName("id_ticket")]
        public string id_ticket { get; set; }

        [JsonPropertyName("idEstacion")]
        public string id_estacion { get; set; }
        [JsonPropertyName("idVagon")]
        public string id_vagon { get; set; }

        [JsonPropertyName("Puerta")]
        public string id_puerta { get; set; }

        [JsonPropertyName("Componente")]
        public string tipo_componente { get; set; }

        [JsonPropertyName("Componente")]
        public string id_componente { get; set; }

        [JsonPropertyName("Identificacion")]
        public string identificacion { get; set; }

        //[JsonPropertyName("ubicacion")]
        //public string Location { get; set; }

        [JsonPropertyName("tipo_mantenimiento")]
        public string tipo_mantenimiento { get; set; }

        [JsonPropertyName("nivel_falla")]
        public string nivel_falla { get; set; }

        [JsonPropertyName("codigo_falla")]
        public string codigo_falla { get; set; }

        [JsonPropertyName("fecha_apertura")]
        public DateTime? fecha_apertura { get; set; }

        [JsonPropertyName("fecha_cierre")]
        public DateTime? fecha_cierre { get; set; }

        [JsonPropertyName("fecha_arribo_locacion")]
        public DateTime? fecha_arribo_locacion { get; set; }

        [JsonPropertyName("Componente_Parte")]
        public string componente_parte { get; set; }

        [JsonPropertyName("Tipo_reparacion")]
        public string tipo_reparacion { get; set; }

        [JsonPropertyName("Tipo_ajuste_configuracion")]
        public string tipo_ajuste_configuracion { get; set; }

        [JsonPropertyName("Descripcion_reparacion")]
        public string descripcion_reparacion { get; set; }

        [JsonPropertyName("Diagnostico_causa")]
        public string diagnostico_causa { get; set; }

        [JsonPropertyName("tipoCausa")]
        public string tipo_causa { get; set; }

        //[JsonPropertyName("fecha_inclusion_registro")]
        //public DateTime RecordInclusionDate { get; set; }

        [JsonPropertyName("Estado_ticket")]
        public string estado_ticket { get; set; }


    }
}
