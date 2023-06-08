using System;

namespace MQTT.Infrastructure.Models.DTO
{
    public class HeaderFieldDTO : ValidFieldDTO
    {
        public HeaderFieldDTO()
        {
            CreationDate = DateTime.UtcNow;
        }
        public string CustomName { get; set; }
        public bool Enable { get; set; }
    }
}
