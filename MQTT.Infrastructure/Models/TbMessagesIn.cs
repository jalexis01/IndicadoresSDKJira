using System;
using System.Collections.Generic;

namespace MQTT.Infrastructure.Models
{
    public partial class TbMessagesIn
    {
        public TbMessagesIn()
        {
            TbMessageInFields = new HashSet<TbMessageInFields>();
        }

        public long Id { get; set; }
        public int IdMessage { get; set; }
        public long IdLogMessageIn { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual TbLogMessageIn IdLogMessageInNavigation { get; set; }
        public virtual TbMessages IdMessageNavigation { get; set; }
        public virtual ICollection<TbMessageInFields> TbMessageInFields { get; set; }
    }
}
