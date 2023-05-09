using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbMessageTypes
    {
        public TbMessageTypes()
        {
            TbHeaderMessage = new HashSet<TbHeaderMessage>();
            TbMessageTypeFields = new HashSet<TbMessageTypeFields>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TableName { get; set; }
        public bool? Enable { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string FieldCode { get; set; }
        public string FieldWeft { get; set; }
        public string FieldIdentifierMessage { get; set; }

        public virtual ICollection<TbHeaderMessage> TbHeaderMessage { get; set; }
        public virtual ICollection<TbMessageTypeFields> TbMessageTypeFields { get; set; }
    }
}
