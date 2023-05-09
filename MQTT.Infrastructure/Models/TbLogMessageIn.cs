using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbLogMessageIn
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public string Observations { get; set; }
        public bool Processed { get; set; }
        public int IdProcessed { get; set; }
        public DateTime? DateProcessed { get; set; }
        public long? IdHeaderMessage { get; set; }
    }
}
