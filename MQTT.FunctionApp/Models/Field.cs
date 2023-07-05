using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class Field
    {
        [JsonPropertyName("customfield_10057")]
        public Category customfield_10057 { get; set; }

        [JsonPropertyName("customfield_10058")]
        public Category customfield_10058 { get; set; }

        [JsonPropertyName("customfield_10060")]
        public string customfield_10060 { get; set; }

        [JsonPropertyName("customfield_10088")]
        public Category customfield_10088 { get; set; }

        [JsonPropertyName("customfield_10059")]
        public string customfield_10059 { get; set; }

        [JsonPropertyName("customfield_10061")]
        public Category customfield_10061 { get; set; }

        [JsonPropertyName("customfield_10064")]
        public Category customfield_10064 { get; set; }

        [JsonPropertyName("customfield_10069")]
        public List<Category> customfield_10069 { get; set; }

        [JsonPropertyName("created")]
        public DateTime created { get; set; }

        [JsonPropertyName("statuscategorychangedate")]
        public DateTime statuscategorychangedate { get; set; }

        [JsonPropertyName("customfield_10071")]
        public DateTime? customfield_10071 { get; set; }

        [JsonPropertyName("customfield_10072")]
        public List<Category> customfield_10072 { get; set; }

        [JsonPropertyName("customfield_10081")]
        public List<Category> customfield_10081 { get; set; }

        [JsonPropertyName("customfield_10075")]
        public List<Category> customfield_10075 { get; set; }

        [JsonPropertyName("customfield_10076")]
        public List<Category> customfield_10076 { get; set; }

        [JsonPropertyName("customfield_10077")]
        public List<Category> customfield_10077 { get; set; }

        [JsonPropertyName("customfield_10078")]
        public List<Category> customfield_10078 { get; set; }

        [JsonPropertyName("customfield_10086")]
        public List<Category> customfield_10086 { get; set; }

        [JsonPropertyName("customfield_10105")]
        public string customfield_10105 { get; set; }

        [JsonPropertyName("customfield_10104")]
        public string customfield_10104 { get; set; }

        [JsonPropertyName("customfield_10067")]
        public Category customfield_10067 { get; set; }
        [JsonPropertyName("customfield_10101")]
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
