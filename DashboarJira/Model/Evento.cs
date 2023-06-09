using System.Text.Json.Nodes;

namespace DashboarJira.Model
{
    public class Evento
    {



        public String? versionTrama { get; set; }

        public String? idRegistro { get; set; }

        public String? idOperador { get; set; }
        public String? idEstacion { get; set; }
        public String? idVagon { get; set; }

        public String? idPuerta { get; set; }

        public String? codigoPuerta { get; set; }
        public DateTime? fechaHoraLecturaDato { get; set; }
        public DateTime? fechaHoraEnvioDato { get; set; }

        public int? tipoTrama { get; set; }

        public String? tramaRetransmitida { get; set; }

        public String? codigoEvento { get; set; }

        public String? estadoAperturaCierrePuertas { get; set; }
        public int? usoBotonmanual { get; set; }
        public bool? estadoBotonManual { get; set; }
        public bool? estadoErrorCritico { get; set; }
        public float? porcentajeCargaBaterias { get; set; }
        public int? ciclosApertura { get; set; }
        public int? horasServicios { get; set; }
        public int? tipoEnergizacion { get; set; }
        public float? velocidadMotor { get; set; }
        public float? fuerzaMotor { get; set; }

        public int? modoOperacion { get; set; }
        public string? idVehiculo { get; set; }

        public String? placaVehiculo { get; set; }

        public string? tipologiaVehiculo { get; set; }
        public string? numeroParada { get; set; }
        public string? nombreEstacion { get; set; }
        public string? nombreVagon { get; set; }
        public String? tipoTramaBusEstacion { get; set; }

        public String? codigoAlarma { get; set; }

        public String? codigoNivelAlarma { get; set; }
        public int? tiempoApertura { get; set; }






        public override string ToString()
        {
            return
                   $"versionTrama: {versionTrama}\n" +
                   $"idRegistro: {idRegistro}\n" +
                   $"idOperador: {idOperador}\n" +
                   $"idEstacion: {idEstacion}\n" +
                   $"idVagon: {idVagon}\n" +
                   $"idPuerta: {idPuerta}\n" +
                   $"codigoPuerta: {codigoPuerta}\n" +
                   $"fechaHoraLecturaDato: {fechaHoraLecturaDato}\n" +
                   $"fechaHoraEnvioDato: {fechaHoraEnvioDato}\n" +
                   $"tipoTrama: {tipoTrama}\n" +
                   $"tramaRetransmitida: {tramaRetransmitida}\n" +
                   $"codigoEvento: {codigoEvento}\n" +
                   $"estadoAperturaCierrePuertas: {estadoAperturaCierrePuertas}\n" +
                   $"usoBotonmanual: {usoBotonmanual}\n" +
                   $"estadoBotonManual: {estadoBotonManual}\n" +
                   $"estadoErrorCritico: {estadoErrorCritico}\n" +
                   $"porcentajeCargaBaterias: {porcentajeCargaBaterias}\n" +
                   $"ciclosApertura: {ciclosApertura}\n" +
                   $"horasServicios: {horasServicios}\n" +
                   $"tipoenergizacion: {tipoEnergizacion}\n" +
                   $"velocidadMotor: {velocidadMotor}\n" +
                   $"fuerzaMotor: {fuerzaMotor}\n" +
                   $"modoOperacion: {modoOperacion}\n" +
                   $"idVehiculo: {idVehiculo}\n" +
                   $"placaVehiculo: {placaVehiculo}\n" +
                   $"tipologiaVehiculo: {tipologiaVehiculo}\n" +
                   $"numeroParada: {numeroParada}\n" +
                   $"nombreEstacion: {nombreEstacion}\n" +
                   $"nombreVagon: {nombreVagon}\n" +
                   $"tipoTramaBusEstacion: {tipoTramaBusEstacion}\n" +
                   $"codigoAlarma: {codigoAlarma}\n" +
                   $"codigoNivelAlarma: {codigoNivelAlarma}\n" +
                   $"tiempoApertura: {tiempoApertura}";
        }

        public JsonObject convertToJson()
        {
            JsonObject msg = new JsonObject();
            msg.Add("versionTrama", versionTrama);
            msg.Add("idRegistro", idRegistro);
            msg.Add("idOperador", idOperador);
            msg.Add("idEstacion", idEstacion);
            msg.Add("idVagon", idVagon);
            msg.Add("idPuerta", idPuerta);
            msg.Add("codigoPuerta", codigoPuerta);
            msg.Add("fechaHoraLecturaDato", fechaHoraLecturaDato);
            msg.Add("fechaHoraEnvioDato", fechaHoraEnvioDato);
            msg.Add("tipoTrama", tipoTrama);
            msg.Add("tramaRetransmitida", tramaRetransmitida);
            msg.Add("codigoEvento", codigoEvento);
            msg.Add("estadoAperturaCierrePuertas", estadoAperturaCierrePuertas);
            msg.Add("usoBotonmanual", usoBotonmanual);
            msg.Add("estadoBotonManual", estadoBotonManual);
            msg.Add("estadoErrorCritico", estadoErrorCritico);
            msg.Add("porcentajeCargaBaterias", porcentajeCargaBaterias);
            msg.Add("ciclosApertura", ciclosApertura);
            msg.Add("horasServicios", horasServicios);
            msg.Add("tipoenergizacion", tipoEnergizacion);
            msg.Add("velocidadMotor", velocidadMotor);
            msg.Add("fuerzaMotor", fuerzaMotor);
            msg.Add("modoOperacion", modoOperacion);
            msg.Add("idVehiculo", idVehiculo);
            msg.Add("placaVehiculo", placaVehiculo);
            msg.Add("tipologiaVehiculo", tipologiaVehiculo);
            msg.Add("numeroParada", numeroParada);
            msg.Add("nombreEstacion", nombreEstacion);
            msg.Add("nombreVagon", nombreVagon);
            msg.Add("tipoTramaBusEstacion", tipoTramaBusEstacion);
            msg.Add("codigoAlarma", codigoAlarma);
            msg.Add("codigoNivelAlarma", codigoNivelAlarma);
            msg.Add("tiempoApertura", tiempoApertura);



            //JsonArray ceiJsonArray = new JsonArray(CEIList);
            //msg.Add("CEI LIST",  );

            return msg;
        }

    }
}
