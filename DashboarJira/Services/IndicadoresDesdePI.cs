using MQTT.Infrastructure.DAL;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MQTT.Infrastructure.Models;

namespace DashboarJira.Services
{
    public class IndicadoresDesdePI
    {

        public void SearchMessages(General objContext, DateTime dtInit, DateTime dtEnd)
        {
            try
            {
                string formattedDtInit = dtInit.ToString("yyyy-MM-dd HH:mm:ss");
                string formattedDtEnd = dtEnd.ToString("yyyy-MM-dd HH:mm:ss");

                string sentence = "SELECT  M.fechaHoraLecturaDato, M.fechaHoraEnvioDato, DATEADD(HOUR,-5,HM.CreationDate) [CreationDateLocal], M.idEstacion, " +
                    "M.idVagon, M.idPuerta, M.codigoEvento, M.estadoAperturaCierrePuertas, M.numeroParada, M.idVehiculo, M.placaVehiculo, M.tipologiaVehiculo, " +
                    "M.estadoErrorCritico, M.nombreEstacion, M.nombreVagon, M.codigoPuerta, M.numeroEventoBusEstacion, M.tipoTramaBusEstacion, HM.fechaPrimerIntento, " +
                    "HM.estadoEnvio, HM.estadoEnvioManatee, HM.fechaHoraEnvio, M.versionTrama, M.tipoTrama, M.tramaRetransmitida, HM.trama, M.usoBotonManual, " +
                    "M.estadoBotonManual, M.porcentajeCargaBaterias, M.ciclosApertura, M.horasServicio, M.tipoEnergizacion, M.velocidaMotor, M.fuerzaMotor, " +
                    "M.modoOperacion, M.codigoAlarma, M.codigoNivelAlarma, M.tiempoApertura, M.IdHeaderMessage, HM.IdMessageType, M.idRegistro, M.idOperador, M.Id " +
                    "FROM [Operation].[tbMessages] M INNER JOIN [Operation].tbHeaderMessage HM ON M.IdHeaderMessage = HM.IdHeaderMessage";
                string where = $" WHERE M.fechaHoraLecturaDato BETWEEN '{formattedDtInit}' AND '{formattedDtEnd}' ";

                if (dtInit == dtEnd)
                {
                    sentence = "SELECT TOP(1000) M.fechaHoraLecturaDato, M.fechaHoraEnvioDato, DATEADD(HOUR,-5,HM.CreationDate) [CreationDateLocal], M.idEstacion, " +
                    "M.idVagon, M.idPuerta, M.codigoEvento, M.estadoAperturaCierrePuertas, M.numeroParada, M.idVehiculo, M.placaVehiculo, M.tipologiaVehiculo, " +
                    "M.estadoErrorCritico, M.nombreEstacion, M.nombreVagon, M.codigoPuerta, M.numeroEventoBusEstacion, M.tipoTramaBusEstacion, HM.fechaPrimerIntento, " +
                    "HM.estadoEnvio, HM.estadoEnvioManatee, HM.fechaHoraEnvio, M.versionTrama, M.tipoTrama, M.tramaRetransmitida, HM.trama, M.usoBotonManual, " +
                    "M.estadoBotonManual, M.porcentajeCargaBaterias, M.ciclosApertura, M.horasServicio, M.tipoEnergizacion, M.velocidaMotor, M.fuerzaMotor, " +
                    "M.modoOperacion, M.codigoAlarma, M.codigoNivelAlarma, M.tiempoApertura, M.IdHeaderMessage, HM.IdMessageType, M.idRegistro, M.idOperador, M.Id " +
                    "FROM [Operation].[tbMessages] M INNER JOIN [Operation].tbHeaderMessage HM ON M.IdHeaderMessage = HM.IdHeaderMessage";
                    where = "";
                }
                sentence += where;
                sentence += " ORDER BY M.fechaHoraLecturaDato DESC;";
                DataTable dt = new DataTable();
                using (var DBContext = objContext.DBConnection())
                {
                    using (var command = DBContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = sentence;
                        command.CommandTimeout = 600000;
                        DBContext.Database.OpenConnection();
                        using (var readerResult = command.ExecuteReader())
                        {
                            if (readerResult.HasRows)
                            {
                                dt.Load(readerResult);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
