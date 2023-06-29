using System;
using System.Collections.Generic;
using System.Text;

namespace MQTT.Infrastructure.Models.DTO
{
    public class HeaderFieldDTO :ValidFieldDTO
    {
        public HeaderFieldDTO()
        {
            CreationDate = DateTime.UtcNow;
        }
        public string CustomName { get; set; }
        public bool Enable { get; set; }
    }
}
