using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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
