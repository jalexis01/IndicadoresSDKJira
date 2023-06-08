using System.Collections.Generic;

namespace MQTT.Infrastructure.Models
{
    public partial class TbMessageInFields
    {
        public TbMessageInFields()
        {
            InverseIdParentNavigation = new HashSet<TbMessageInFields>();
        }

        public long Id { get; set; }
        public long IdMessageIn { get; set; }
        public int IdMessageField { get; set; }
        public string Value { get; set; }
        public long? IdParent { get; set; }

        public virtual TbMessageFields IdMessageFieldNavigation { get; set; }
        public virtual TbMessagesIn IdMessageInNavigation { get; set; }
        public virtual TbMessageInFields IdParentNavigation { get; set; }
        public virtual ICollection<TbMessageInFields> InverseIdParentNavigation { get; set; }
    }
}
