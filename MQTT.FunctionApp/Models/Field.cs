using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class Field
    {
        [JsonPropertyName("customfield_10052")]
        public Category? Estacion { get; set; }

        [JsonPropertyName("customfield_10073")]
        public Category? Vagon { get; set; }

        [JsonPropertyName("customfield_10057")]
        public string? IdentificacionComponente { get; set; }

        [JsonPropertyName("customfield_10070")]
        public Category? TipoDeComponente { get; set; }

        [JsonPropertyName("customfield_10058")]
        public string? IdentificacionSerial { get; set; }

        [JsonPropertyName("customfield_10072")]
        public Category? TipoDeServicio { get; set; }

        [JsonPropertyName("customfield_10046")]
        public Category? ClaseDeFallo { get; set; }

        [JsonPropertyName("customfield_10048")]
        public List<Category>? DescripcionDeFallo { get; set; }

        [JsonPropertyName("created")]
        public DateTime? created { get; set; }

        [JsonPropertyName("statuscategorychangedate")]
        public DateTime? statuscategorychangedate { get; set; }

        [JsonPropertyName("customfield_10056")]
        public DateTime? FechayHoraDeLlegadaAEstacion { get; set; }

        [JsonPropertyName("customfield_10050")]
        public List<Category>? DescripcionRepuesto { get; set; }

        [JsonPropertyName("customfield_10071")]
        public List<Category>? TipoReparacion    { get; set; }

        [JsonPropertyName("customfield_10060")]
        public List<Category>? ListadoAjustesPuerta { get; set; }

        [JsonPropertyName("customfield_10063")]
        public List<Category>? ListadoConfiguracionPuerta { get; set; }

        [JsonPropertyName("customfield_10059")]
        public List<Category>? ListadoAjustesITS { get; set; }

        [JsonPropertyName("customfield_10062")]
        public List<Category>? ListadoConfiguracionITS { get; set; }

        [JsonPropertyName("customfield_10064")]
        public List<Category>? ListadoConfiguracionRFID { get; set; }

       /* [JsonPropertyName("customfield_10061")] //Se supone que funciona con Manatee
        public List<Category>? ListadoAjusteRFID { get; set; }

        */

        [JsonPropertyName("customfield_10061")] //FUNCIONA SOLO CON ASSAABLOY
        public List<Category>? ListadoAjusteRFID { get; set; }

        [JsonPropertyName("customfield_10087")]
        public string? DescripcionReparacion { get; set; }

        [JsonPropertyName("customfield_10088")]
        public string? DiagnosticoCausa { get; set; }

        [JsonPropertyName("customfield_10051")]
        public Category? TipoCausa { get; set; }

        [JsonPropertyName("customfield_10055")]
        public DateTime? FechaSolucion { get; set; }

        [JsonPropertyName("Status")]
        public Status? status { get; set; }

    }

    public class Category
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class Status
    {
        [JsonPropertyName("name")]
        public string name { get; set; }
    }
}
