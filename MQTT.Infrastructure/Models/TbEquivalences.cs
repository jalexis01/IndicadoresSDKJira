using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbEquivalences
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int IdEquivalenceType { get; set; }
        public DateTime CrerationDate { get; set; }
        public int UserId { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? UserIdUpdate { get; set; }

        public virtual TbEquivalenceTypes IdEquivalenceTypeNavigation { get; set; }
    }
}
