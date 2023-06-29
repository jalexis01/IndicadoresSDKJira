using System;
using System.Collections.Generic;
using System.Text;

namespace MQTT.Infrastructure.Models.DTO
{
    public class MessageInFieldDTO
    {
        public long Id { get; set; }
        public long IdMessageIn { get; set; }
        public int IdMessageField { get; set; }
        public string Value { get; set; }
    }
}
