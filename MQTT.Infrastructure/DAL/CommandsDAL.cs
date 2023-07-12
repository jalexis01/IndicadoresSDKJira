using Microsoft.EntityFrameworkCore;
using MQTT.Infrastructure.Models.DTO;
using System;
using System.Data;

namespace MQTT.Infrastructure.DAL
{
    public class CommandsDAL
    {
        private const string _formateDate = "dd/MM/yyyy HH:mm:ss.fff";
        public static DataTable SearchMessages(General objContext, DateTime dtInit, DateTime dtEnd, MessageTypeFieldDTO messageField, string value)
        {
            try
            {
                string sentence = "SELECT M.fechaHoraLecturaDato, M.fechaHoraEnvioDato, DATEADD(HOUR,-5,HM.CreationDate) [CreationDateLocal], M.idEstacion, M.idVagon, M.idPuerta, M.codigoPuerta, M.codigoMensaje, M.mensaje, HM.fechaPrimerIntento, HM.estadoEnvio, " +
                    "HM.estadoEnvioManatee, HM.fechaHoraEnvio, M.versionTrama, M.tipoTrama, HM.trama, M.IdHeaderMessage, HM.IdMessageType, M.idRegistro, M.idOperador, M.Id " +
                    "FROM [Operation].[tbCommands] M " +
                    "INNER JOIN [Operation].tbHeaderMessage HM ON M.IdHeaderMessage = HM.IdHeaderMessage WHERE ";
                string where = $"CreationDate BETWEEN '{dtInit}' AND '{dtEnd}' ";

                if (messageField.Name != null && !string.IsNullOrEmpty(value))
                {
                    where += $"AND {messageField.Name} = {General.GetValueFromFields(messageField.DataType, value, _formateDate)}";
                }


                sentence += where;
                sentence = sentence.Remove(sentence.Length - 1);
                DataTable dt = new DataTable();
                using (var DBContext = objContext.DBConnection())
                {
                    using (var command = DBContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = sentence;
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

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
