using System;
using System.Collections.Generic;

namespace MQTT.Infrastructure.Models
{
    public partial class TbParameters
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
