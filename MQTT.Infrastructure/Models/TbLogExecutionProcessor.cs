using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbLogExecutionProcessor
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime EndDate { get; set; }
        public long? IdLogMessageInInit { get; set; }
        public long? IdLogMessageInEnd { get; set; }
        public string Observations { get; set; }
    }
}
