using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Model
{
    public class Estacion
    {
        public int Id { get; set; }
        public int idEstacion { get; set; }
        public string nombreEstacion { get; set; }
        public string? Vagones { get; set; }
        public List<ComponenteHV> Componentes { get; set; }
        public DateTime? InicioOperacion { get; set; }
        public DateTime? FinOperacion { get; set; }
        public DateTime? UltimaConexion { get; set; }
    }
}
