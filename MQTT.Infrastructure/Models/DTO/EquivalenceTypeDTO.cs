using System;
using System.Collections.Generic;
using System.Text;

namespace MQTT.Infrastructure.Models.DTO
{
    public class EquivalenceTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TitleController { get; set; }
        public string NameController { get; set; }
        public string MenuController { get; set; }
    }
}
