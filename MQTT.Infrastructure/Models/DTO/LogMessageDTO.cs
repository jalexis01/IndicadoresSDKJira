using System;

namespace MQTT.Infrastructure.Models.DTO
{
    public class LogMessageDTO
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime DateIn { get; set; }
        public string? Observations { get; set; }
        public bool Processed { get; set; }
        public int IdProcessed { get; set; }
        public long IdEventRecord { get; set; }
        public long? IdHeaderMessage { get; set; }
    }
}
