using System;

namespace MQTT.Infrastructure.Models.DTO
{
    public class LogRequestInDTO
    {
        public long Id { get; set; }
        public int IdEndPoint { get; set; }
        public string DataQuery { get; set; }
        public string DataBody { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Processed { get; set; }
        public string Observations { get; set; }
    }
}
