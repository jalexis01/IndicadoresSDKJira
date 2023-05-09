using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbElements
    {
        public TbElements()
        {
            InverseIdElementFatherNavigation = new HashSet<TbElements>();
            TbLogElements = new HashSet<TbLogElements>();
        }

        public int Id { get; set; }
        public int IdElementType { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? IdElementFather { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreationUser { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? UpdateUser { get; set; }
        public bool? Enable { get; set; }

        public virtual TbElements IdElementFatherNavigation { get; set; }
        public virtual ICollection<TbElements> InverseIdElementFatherNavigation { get; set; }
        public virtual ICollection<TbLogElements> TbLogElements { get; set; }
    }
}
