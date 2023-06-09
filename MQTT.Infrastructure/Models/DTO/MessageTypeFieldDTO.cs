using System;

namespace MQTT.Infrastructure.Models.DTO
{
    public class MessageTypeFieldDTO : ValidFieldDTO
    {
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public int IdValidField { get; set; }
        public string CustomName { get; set; }
        public bool Enable { get; set; }

    }
}
