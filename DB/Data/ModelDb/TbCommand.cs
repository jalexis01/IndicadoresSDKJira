using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbCommand
{
    public long Id { get; set; }

    public long IdHeaderMessage { get; set; }

    public string? VersionTrama { get; set; }

    public string? IdRegistro { get; set; }

    public string? IdOperador { get; set; }

    public string? IdEstacion { get; set; }

    public string? IdVagon { get; set; }

    public string? IdPuerta { get; set; }

    public string? CodigoPuerta { get; set; }

    public string? FechaHoraLecturaDato { get; set; }

    public string? FechaHoraEnvioDato { get; set; }

    public int? TipoTrama { get; set; }

    public int? CodigoMensaje { get; set; }

    public string? Mensaje { get; set; }

    public virtual TbHeaderMessage IdHeaderMessageNavigation { get; set; } = null!;
}
