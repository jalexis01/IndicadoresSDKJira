using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    public class ComponenteHV
    {
        public int IdComponente { get; set; }
        public string Serial { get; set; }
        public int AnioFabricacion { get; set; }
        public string Modelo { get; set; }
        public DateTime FechaInicio { get; set; }
    }

}
