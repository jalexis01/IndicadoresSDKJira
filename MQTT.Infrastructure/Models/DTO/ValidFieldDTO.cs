using System;

namespace MQTT.Infrastructure.Models.DTO
{
    public class ValidFieldDTO
    {
        public ValidFieldDTO()
        {
            CreationDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string? Description { get; set; }
        public bool SearchType { get; set; }
        public bool PrimaryType { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
