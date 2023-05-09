using System;
using System.Collections.Generic;
using System.Text;

namespace MQTT.Infrastructure.Models.DTO
{
    public class ElementDTO
    {
        public int Id { get; set; }
        public int IdElementType { get; set; }
        public string NameElementType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Enable { get; set; }
        public int? IdElementFather { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreationUser { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? UpdateUser { get; set; }
        public List<ElementDTO> SubElements { get; set; }
    }
}
