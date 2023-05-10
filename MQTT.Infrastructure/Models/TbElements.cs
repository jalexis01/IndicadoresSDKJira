using System;
using System.Collections.Generic;

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
        public string Code { get; set; }
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
