using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbLogMessageInSummaryDay
    {
        public long Id { get; set; }
        public long IdLogMessageIn { get; set; }
        public DateTime DateDay { get; set; }
    }
}
