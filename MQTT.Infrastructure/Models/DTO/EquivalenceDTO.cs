using System;

namespace MQTT.Infrastructure.Models.DTO
{
    public class EquivalenceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int IdEquivalenceType { get; set; }
        public DateTime CrerationDate { get; set; }
        public int UserId { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? UserIdUpdate { get; set; }
    }
}
