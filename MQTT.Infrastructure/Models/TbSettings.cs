using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbSettings
    {
        public TbSettings()
        {
            InverseIdParentNavigation = new HashSet<TbSettings>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? IdParent { get; set; }
        public string Description { get; set; }

        public virtual TbSettings IdParentNavigation { get; set; }
        public virtual ICollection<TbSettings> InverseIdParentNavigation { get; set; }
    }
}
