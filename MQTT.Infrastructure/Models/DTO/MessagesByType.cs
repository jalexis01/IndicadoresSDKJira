using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MQTT.Infrastructure.Models.DTO
{
    public class MessagesByType
    {
        public int IdMessageType { get; set; }
        public string TableName { get; set; }
        public DataTable Messages { get; set; }
    }
}
