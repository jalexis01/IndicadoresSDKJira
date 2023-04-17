using System;
using System.Collections.Generic;
using System.Text;

namespace MQTT.DTO
{
    public class LogMessageDTO
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime DateIn { get; set; }
    }
}
