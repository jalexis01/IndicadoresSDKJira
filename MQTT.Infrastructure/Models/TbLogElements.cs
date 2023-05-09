using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbLogElements
    {
        public long Id { get; set; }
        public int IdElement { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int? OldFatherId { get; set; }
        public int? NewFatherId { get; set; }
        public bool Enable { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreationUser { get; set; }

        public virtual TbElements IdElementNavigation { get; set; }
    }
}
