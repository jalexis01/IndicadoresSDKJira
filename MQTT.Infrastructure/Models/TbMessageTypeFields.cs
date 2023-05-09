using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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
