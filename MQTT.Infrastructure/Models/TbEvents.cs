namespace MQTT.Infrastructure.Models
{
    public partial class TbEvents
    {
        public long Id { get; set; }
        public long IdHeaderMessage { get; set; }
        public string VersionTrama { get; set; }
        public string IdRegistro { get; set; }
        public string IdOperador { get; set; }
        public string IdEstacion { get; set; }
        public string IdVagon { get; set; }
        public string IdPuerta { get; set; }
        public string CodigoPuerta { get; set; }
        public string FechaHoraLecturaDato { get; set; }
        public string FechaHoraEnvioDato { get; set; }
        public int? TipoTrama { get; set; }
        public string TramaRetransmitida { get; set; }
        public string CodigoEvento { get; set; }
        public string EstadoAperturaCierrePuertas { get; set; }

        public virtual TbHeaderMessage IdHeaderMessageNavigation { get; set; }
    }
}
