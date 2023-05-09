using System;
using System.Collections.Generic;

namespace MQTT.Infrastructure.Models
{
    public partial class TbMessageTypeFields
    {
        public int Id { get; set; }
        public int IdMessageType { get; set; }
        public int IdValidField { get; set; }
        public string CustomName { get; set; }
        public bool? Enable { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual TbMessageTypes IdMessageTypeNavigation { get; set; }
        public virtual TbValidFields IdValidFieldNavigation { get; set; }
    }
}
