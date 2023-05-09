using System;
using System.Collections.Generic;

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
        public string Description { get; set; }
        public DateTime? CreationDate { get; set; }

        public virtual ICollection<TbEquivalences> TbEquivalences { get; set; }
    }
}
