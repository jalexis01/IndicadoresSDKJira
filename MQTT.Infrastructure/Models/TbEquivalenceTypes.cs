using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbEquivalenceTypes
    {
        public TbEquivalenceTypes()
        {
            TbEquivalences = new HashSet<TbEquivalences>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string TitleController { get; set; }
        public string NameController { get; set; }
        public string MenuController { get; set; }
        public bool? Enable { get; set; }

        public virtual ICollection<TbEquivalences> TbEquivalences { get; set; }
    }
}
