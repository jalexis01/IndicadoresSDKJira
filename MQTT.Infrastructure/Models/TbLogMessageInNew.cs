using System;

namespace MQTT.Infrastructure.Models
{
    public partial class TbLogMessageInNew
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
