using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class Issue
    {
        [JsonPropertyName("issues")]
        public List<Data> Issues { get; set; }
        public int Total { get; internal set; }
    }

    public class Data
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("fields")]
        public Field Fields { get; set; }
    }

}
