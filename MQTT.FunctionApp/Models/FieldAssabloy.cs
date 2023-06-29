using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class FieldAssabloy
    {
        //[JsonPropertyName("Station")]
        public Category customfield_10057 { get; set; }

        //[JsonPropertyName("Wagon")]
        public Category customfield_10058 { get; set; }

        //[JsonPropertyName("Door")]
        public string customfield_10060 { get; set; }

        //[JsonPropertyName("Component")]
        public Category customfield_10088 { get; set; }

        //[JsonPropertyName("Serial")]
        public string customfield_10059 { get; set; }

        //[JsonPropertyName("MaintenanceType")]
        public Category customfield_10061 { get; set; }

        //[JsonPropertyName("FailureLevel")]
        public Category customfield_10064 { get; set; }

        //[JsonPropertyName("FaultCode")]
        public List<Category> customfield_10069 { get; set; }

        //[JsonPropertyName("CreationDate")]
        public DateTime created { get; set; }

        //[JsonPropertyName("ResolutionDate")]
        public DateTime statuscategorychangedate { get; set; }

        //[JsonPropertyName("LocationDate")]
        public DateTime? customfield_10071 { get; set; }

        //[JsonPropertyName("SpareComponent")]
        public List<Category> customfield_10072 { get; set; }

        //[JsonPropertyName("TypeOfRepair")]
        public List<Category> customfield_10081 { get; set; }

        //[JsonPropertyName("TypeSettingConfiguration")]
        public List<Category> customfield_10075 { get; set; }

        //[JsonPropertyName("TypeSettingConfiguration2")]
        public List<Category> customfield_10076 { get; set; }

        //[JsonPropertyName("TypeSettingConfiguration3")]
        public List<Category> customfield_10077 { get; set; }

        //[JsonPropertyName("TypeSettingConfiguration4")]
        public List<Category> customfield_10078 { get; set; }

        //[JsonPropertyName("TypeSettingConfiguration5")]
        public List<Category> customfield_10086 { get; set; }

        //[JsonPropertyName("DescriptionRepair")]
        //public string description { get; set; }
        public string customfield_10105 { get; set; }

        //[JsonPropertyName("DiagnosisCause")]
        public string customfield_10104 { get; set; }

        //[JsonPropertyName("CauseType")]
        public Category customfield_10067 { get; set; }
        public DateTime? customfield_10101 { get; set; }

        [JsonPropertyName("Status")]
        public Status status { get; set; }

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
