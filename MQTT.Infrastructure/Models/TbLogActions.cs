using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT.Infrastructure.Models
{
    [Table("LogActions")]
    public partial class LogActions
    {        
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public DateTime FechaAccion { get; set; }
    }
}
