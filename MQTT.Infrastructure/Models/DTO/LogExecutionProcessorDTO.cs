using System;
using System.Collections.Generic;
using System.Text;

namespace MQTT.Infrastructure.Models.DTO
{
    public class LogExecutionProcessorDTO : LogExecutionDTO
    {
        public DateTime CreationDate { get; set; }
        public long? IdLogMessageInInit { get; set; }
        public long? IdLogMessageInEnd { get; set; }

    }
}
