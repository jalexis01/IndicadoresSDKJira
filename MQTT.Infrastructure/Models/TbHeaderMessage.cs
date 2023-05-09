using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MQTT.Infrastructure.Models
{
    public partial class TbHeaderMessage
    {
        public TbHeaderMessage()
        {
            TbCommands = new HashSet<TbCommands>();
            TbMessages = new HashSet<TbMessages>();
        }

        public long IdHeaderMessage { get; set; }
        public int IdMessageType { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime FechaPrimerIntento { get; set; }
        public bool? EstadoEnvio { get; set; }
        public bool? EstadoEnvioManatee { get; set; }
        public string Idmanatee { get; set; }
        public string Trama { get; set; }
        public DateTime? FechaHoraEnvio { get; set; }
        public long? Id { get; set; }

        public virtual TbMessageTypes IdMessageTypeNavigation { get; set; }
        public virtual ICollection<TbCommands> TbCommands { get; set; }
        public virtual ICollection<TbMessages> TbMessages { get; set; }
    }
}
