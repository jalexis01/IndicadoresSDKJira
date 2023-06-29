using System;
using System.Collections.Generic;

namespace MQTT.Infrastructure.Models
{
    public partial class TbEndPoints
    {
        public TbEndPoints()
        {
            TbLogRequestsIn = new HashSet<TbLogRequestsIn>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public DateTime CreationDate { get; set; }
        public string AzureLocation { get; set; }

        public virtual ICollection<TbLogRequestsIn> TbLogRequestsIn { get; set; }
    }
}
