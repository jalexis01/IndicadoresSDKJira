using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbHeaderMessage
{
    public long IdHeaderMessage { get; set; }

    public int IdMessageType { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime FechaPrimerIntento { get; set; }

    public bool? EstadoEnvio { get; set; }

    public bool? EstadoEnvioManatee { get; set; }

    public string? Idmanatee { get; set; }

    public string? Trama { get; set; }

    public DateTime? FechaHoraEnvio { get; set; }

    public long? Id { get; set; }

    public virtual ICollection<TbCommand> TbCommands { get; set; } = new List<TbCommand>();

    public virtual ICollection<TbLogHistoryMessageIn> TbLogHistoryMessageIns { get; set; } = new List<TbLogHistoryMessageIn>();

    public virtual ICollection<TbMessage> TbMessages { get; set; } = new List<TbMessage>();
}
