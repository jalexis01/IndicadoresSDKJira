using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MQTT.FunctionApp.Models
{
    public class IssueEdit
    {
        [JsonPropertyName("fields")]
        public Field Fields { get; set; }
    }
}
