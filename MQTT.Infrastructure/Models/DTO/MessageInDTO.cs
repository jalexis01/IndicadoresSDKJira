using System;
using System.Collections.Generic;

namespace MQTT.Infrastructure.Models.DTO
{
    public class MessageInDTO
    {
        public MessageInDTO()
        {
            Fields = new List<MessageInFieldDTO>();
            CreationDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public int IdMessage { get; set; }
        public long IgLogMessageIn { get; set; }
        public DateTime CreationDate { get; set; }
        public List<MessageInFieldDTO> Fields { get; set; }

    }
}
