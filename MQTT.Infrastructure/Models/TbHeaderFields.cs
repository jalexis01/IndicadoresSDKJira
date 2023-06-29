using System;
using System.Collections.Generic;

namespace MQTT.Infrastructure.Models
{
    public partial class TbHeaderFields
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public string CustomName { get; set; }
        public bool? Enabled { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool SearchType { get; set; }
        public bool PrimaryData { get; set; }
    }
}
