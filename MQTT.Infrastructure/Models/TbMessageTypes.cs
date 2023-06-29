using System;
using System.Collections.Generic;

namespace MQTT.Infrastructure.Models
{
    public partial class TbMessageTypes
    {
        public TbMessageTypes()
        {
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

        public virtual ICollection<TbMessageTypeFields> TbMessageTypeFields { get; set; }
    }
}
