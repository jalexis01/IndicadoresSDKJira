using System;
using System.Collections.Generic;
using MQTT.Infrastructure.Models.DTO;

namespace MQTT.Infrastructure.Models.DTO
{
    public class MessageTypeDTO
    {
        public MessageTypeDTO()
        {
            Fields = new List<MessageTypeFieldDTO>();
            CreationDate = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string TableName { get; set; }
        public string FieldCode { get; set; }
        public string FieldWeft { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enable { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string FieldIdentifierMessage { get; set; }
        public List<MessageTypeFieldDTO> Fields { get; set; }
    }
}
