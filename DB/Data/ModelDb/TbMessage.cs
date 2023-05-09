using System;
using System.Collections.Generic;

namespace DB.Data.ModelDB;

public partial class TbMessage
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

    public DateTime? FechaHoraLecturaDato { get; set; }

    public DateTime? FechaHoraEnvioDato { get; set; }

    public int? TipoTrama { get; set; }

    public string? TramaRetransmitida { get; set; }

    public string? CodigoEvento { get; set; }

    public string? EstadoAperturaCierrePuertas { get; set; }

    public int? UsoBotonManual { get; set; }

    public bool? EstadoBotonManual { get; set; }

    public bool? EstadoErrorCritico { get; set; }

    public double? PorcentajeCargaBaterias { get; set; }

    public int? CiclosApertura { get; set; }

    public int? HorasServicio { get; set; }

    public int? TipoEnergizacion { get; set; }

    public double? VelocidaMotor { get; set; }

    public double? FuerzaMotor { get; set; }

    public int? ModoOperacion { get; set; }

    public int? NumeroEventoBusEstacion { get; set; }

    public string? IdVehiculo { get; set; }

    public string? PlacaVehiculo { get; set; }

    public string? TipologiaVehiculo { get; set; }

    public string? NumeroParada { get; set; }

    public string? NombreEstacion { get; set; }

    public string? NombreVagon { get; set; }

    public string? TipoTramaBusEstacion { get; set; }

    public string? CodigoAlarma { get; set; }

    public string? CodigoNivelAlarma { get; set; }

    public int? TiempoApertura { get; set; }

    public virtual TbHeaderMessage IdHeaderMessageNavigation { get; set; } = null!;
}
