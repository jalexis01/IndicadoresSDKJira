using System;

namespace MQTT.Infrastructure.Models.DTO
{
    public class LogExecutionDTO
    {
        public long Id { get; set; }
        public DateTime Init { get; set; }
        public DateTime End { get; set; }
        public string Observation { get; set; }
    }
}
