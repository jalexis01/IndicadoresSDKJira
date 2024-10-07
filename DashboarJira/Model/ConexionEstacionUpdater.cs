using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT.FunctionApp.Models
{
    public class ConexionEstacionUpdater
    {
        public string Serial { get; set; }
        public DateTime UltimaConexion { get; set; }
        public string Evento { get; set; }
        public bool EstadoApertura { get; set; }
        public bool EstadoErrorCritico { get; set; }
        public int IdEstacion { get; set; }

    }
}
