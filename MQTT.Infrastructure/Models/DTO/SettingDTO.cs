using System;
using System.Collections.Generic;
using System.Text;

namespace MQTT.Infrastructure.Models.DTO
{
    public class SettingDTO
    {
        public int Id  { get; set; }
        public int? IdParent { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
