using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class Issue
    {
        [JsonPropertyName("issues")]
        public List<Data> Issues { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("fields")]
        public Field Fields { get; set; }

        //[JsonPropertyName("key")] TODO: Hacer la entidad de Jira, para serializar el texto del hgttpresult en la función.
    }

    public class Field
    {
        [JsonPropertyName("Station")]
        public Category customfield_10057 { get; set; }

        [JsonPropertyName("Wagon")]
        public Category customfield_10058 { get; set; }

        [JsonPropertyName("Door")]
        public string customfield_10060 { get; set; }

        [JsonPropertyName("Component")]
        public List<Category> customfield_10088 { get; set; }

        [JsonPropertyName("Serial")]
        public List<Category> customfield_10059 { get; set; }

        //[JsonPropertyName("Location")]
        //public string customfield_10060 { get; set; }

        [JsonPropertyName("MaintenanceType")]
        public Category customfield_10061 { get; set; }

        [JsonPropertyName("FailureLevel")]
        public Category customfield_10064 { get; set; }

        [JsonPropertyName("FaultCode")]
        public List<Category> customfield_10069 { get; set; }

        [JsonPropertyName("CreationDate")]
        public DateTime created { get; set; }

        [JsonPropertyName("ResolutionDate")]
        public CurrentStatus statuscategorychangedate { get; set; }

        [JsonPropertyName("LocationDate")]
        public DateTime customfield_10071 { get; set; }

        [JsonPropertyName("TypeOfRepair")]
        public List<Category> customfield_10081 { get; set; }

        [JsonPropertyName("TypeSettingConfiguration")]
        public List<Category> customfield_10076 { get; set; }

        [JsonPropertyName("TypeSettingConfiguration2")]
        public List<Category> customfield_10078 { get; set; }

        [JsonPropertyName("TypeSettingConfiguration3")]
        public List<Category> customfield_10075 { get; set; }

        [JsonPropertyName("TypeSettingConfiguration4")]
        public List<Category> customfield_10077 { get; set; }

        [JsonPropertyName("DescriptionRepair")]
        public string description { get; set; }

        [JsonPropertyName("DiagnosisCause")]
        public Category customfield_10067 { get; set; }

        //[JsonPropertyName("RecordInclusionDate")] //TODO: Change this
        //public string customfield_10081 { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class CurrentStatus
    {
        [JsonPropertyName("currentStatus")]
        public DataStatus currentStatus { get; set; }
    }

    public class DataStatus
    {

        [JsonPropertyName("status")]
        public string status { get; set; }
        public SubData statusDate { get; set; }
    }

    public class SubData
    {
        [JsonPropertyName("epochMillis")]
        public long epochMillis { get; set; }

    }
}
