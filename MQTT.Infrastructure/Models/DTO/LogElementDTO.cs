using System;

namespace MQTT.Infrastructure.Models.DTO
{
    public class LogElementDTO
    {
        public long Id { get; set; }
        public int IdElement { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
        public int? OldFatherId { get; set; }
        public int? NewFatherId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public bool Enable { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreationUser { get; set; }
    }
}
