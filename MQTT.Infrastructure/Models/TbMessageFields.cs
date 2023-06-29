using System;
using System.Collections.Generic;

namespace MQTT.Infrastructure.Models
{
    public partial class TbMessageFields
    {
        public TbMessageFields()
        {
            TbMessageInFields = new HashSet<TbMessageInFields>();
        }

        public int Id { get; set; }
        public int IdMessage { get; set; }
        public int IdValidField { get; set; }
        public string CustomName { get; set; }
        public bool? Enable { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual TbMessages IdMessageNavigation { get; set; }
        public virtual TbValidFields IdValidFieldNavigation { get; set; }
        public virtual ICollection<TbMessageInFields> TbMessageInFields { get; set; }
    }
}
