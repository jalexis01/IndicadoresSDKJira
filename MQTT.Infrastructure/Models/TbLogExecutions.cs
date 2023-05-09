using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbLogExecutions
    {
        public long Id { get; set; }
        public DateTime InitDateTime { get; set; }
        public string Observations { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
