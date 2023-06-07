using MQTT.Infrastructure.Models;
using MQTT.Infrastructure.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace MQTT.Infrastructure.DAL
{
    public class MessagesDAL
    {
        private const string _formateDate = "yyyy/MM/dd HH:mm:ss.fff";
        public static MessageTypeDTO GetConfigurationMessage(General objContext, string code)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var currentMessage = GetMessageDTO(objContext, code);

                    if (currentMessage == null)
                    {
                        return null;
                    }
                    var fields = (from eventfield in DBContext.TbMessageTypeFields
                                  join field in DBContext.TbValidFields
                                    on eventfield.IdValidField equals field.Id
                                  where eventfield.IdMessageType == currentMessage.Id
                                  select new MessageTypeFieldDTO
                                  {
                                      Id = eventfield.Id,
                                      IdValidField = field.Id,
                                      Name = field.Name,
                                      Description = field.Description,
                                      DataType = field.DataType,
                                      Enable = eventfield.Enable.Value,
                                      CreationDate = field.CreationDate,
                                      CustomName = eventfield.CustomName,
                                      UpdateDate = field.UpdateDate
                                  }).ToList();

                    currentMessage.Fields = new List<MessageTypeFieldDTO>();
                    currentMessage.Fields = fields;

                    return currentMessage;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MessageTypeFieldDTO> GetFieldsMessage(General objContext, int id)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = (from eventfield in DBContext.TbMessageTypeFields
                                  join field in DBContext.TbValidFields
                                    on eventfield.IdValidField equals field.Id
                                  where eventfield.IdMessageType == id
                                  select new MessageTypeFieldDTO
                                  {
                                      Id = eventfield.Id,
                                      IdValidField = field.Id,
                                      Name = field.Name,
                                      Description = field.Description,
                                      DataType = field.DataType,
                                      Enable = eventfield.Enable.Value,
                                      CreationDate = field.CreationDate,
                                      CustomName = eventfield.CustomName,
                                      UpdateDate = field.UpdateDate
                                  }).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static MessageTypeDTO GetMessageDTO(General objContext, string code)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {

                    var currentMessage = (from ev in DBContext.TbMessageTypes
                                          where ev.Code.Equals(code)
                                          select new MessageTypeDTO
                                          {
                                              Id = ev.Id,
                                              Name = ev.Name,
                                              Description = ev.Description,
                                              Enable = ev.Enable.Value,
                                              Code = ev.Code,
                                              FieldCode = ev.FieldCode,
                                              TableName = ev.TableName,
                                              CreationDate = ev.CreationDate,
                                              UpdateDate = ev.UpdateDate
                                          }).FirstOrDefault();

                    return currentMessage;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MessageTypeDTO> GetAllMessageTypes(General objContext)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {

                    var result = (from ev in DBContext.TbMessageTypes
                                  select new MessageTypeDTO
                                  {
                                      Id = ev.Id,
                                      Name = ev.Name,
                                      Description = ev.Description,
                                      Enable = ev.Enable.Value,
                                      Code = ev.Code,
                                      FieldCode = ev.FieldCode,
                                      TableName = ev.TableName,
                                      FieldWeft = ev.FieldWeft,
                                      CreationDate = ev.CreationDate,
                                      UpdateDate = ev.UpdateDate,
                                      FieldIdentifierMessage = ev.FieldIdentifierMessage
                                  }).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddMessageIn(General objContext, MessageInDTO messageInDTO)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    TbMessagesIn tbMessagesIn = new TbMessagesIn()
                    {
                        IdLogMessageIn = messageInDTO.IgLogMessageIn,
                        IdMessage = messageInDTO.IdMessage
                    };
                    DBContext.Add(tbMessagesIn);
                    DBContext.SaveChanges();

                    foreach (var item in messageInDTO.Fields)
                    {
                        TbMessageInFields tbMessageInFields = new TbMessageInFields()
                        {
                            IdMessageIn = tbMessagesIn.Id,
                            IdMessageField = item.IdMessageField,
                            Value = item.Value
                        };

                        DBContext.Add(tbMessageInFields);
                    }

                    DBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<HeaderFieldDTO> GetHeaderFields(General objContext)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = DBContext.TbHeaderFields.Select(p =>
                    new HeaderFieldDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        DataType = p.DataType,
                        CustomName = p.CustomName,
                        Enable = p.Enabled.Value,
                        CreationDate = p.CreationDate,
                        Description = p.Description,
                        UpdateDate = p.UpdateDate
                    }).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static long AddHeaderMessage(General objContext, List<HeaderFieldDTO> lstHeaderFields, int idMessageType, Dictionary<string, string> dctDataFields, string formatDate = _formateDate)
        {
            try
            {
                string sentence = "INSERT INTO [Operation].[tbHeaderMessage] (IdMessageType, CreationDate,";
                string value = $"VALUES ({idMessageType},'{DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss.fff")}',";
                long idHeaderMessage = 0;

                foreach (var item in dctDataFields)
                {
                    string dataType = string.Empty;
                    if (item.Key == "fechaHoraEnvio")
                    {
                        dataType = lstHeaderFields.Where(f => f.Name.Equals(item.Key)).Select(f => f.DataType).FirstOrDefault();

                        value += General.GetValueFromFields(dataType, item.Value, formatDate);
                        sentence += $"fechaPrimerIntento,";
                    }

                    dataType = lstHeaderFields.Where(f => f.Name.Equals(item.Key)).Select(f => f.DataType).FirstOrDefault();

                    value += General.GetValueFromFields(dataType, item.Value, formatDate);
                    sentence += $"{item.Key},";
                }

                sentence = sentence.Remove(sentence.Length - 1);
                value = value.Remove(value.Length - 1);
                sentence += ")";
                value += ")";

                sentence += $" {value} SELECT SCOPE_IDENTITY() [Id]";

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
                                while (readerResult.Read())
                                {
                                    idHeaderMessage = Convert.ToInt64(readerResult["Id"]);
                                }
                            }
                        }
                    }
                }

                return idHeaderMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void UpdateHeaderMessage(General objContext, List<HeaderFieldDTO> lstHeaderFields, Dictionary<string, string> dctDataFields, long idHeaderMessage, string formatDate = _formateDate)
        {
            try
            {
                string sentence = "UPDATE Operation.tbHeaderMessage SET ";
                string value = string.Empty;

                foreach (var item in dctDataFields)
                {
                    var dataType = lstHeaderFields.Where(f => f.Name.Equals(item.Key)).Select(f => f.DataType).FirstOrDefault();
                    value = General.GetValueFromFields(dataType, item.Value, formatDate);
                    sentence += $"{item.Key} = {value}";
                }

                sentence = sentence.Remove(sentence.Length - 1);

                sentence += $" WHERE IdHeaderMessage = {idHeaderMessage}";

                using (var DBContext = objContext.DBConnection())
                {
                    FormattableString sql = $@"{sentence}";
                    DBContext.Database.ExecuteSqlInterpolated(sql);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static long? GetHeaderMessageById(General objContext, List<HeaderFieldDTO> lstHeaderFields, string fieldName, string fieldValue, string formatDate = _formateDate)
        {
            try
            {
                var dataType = lstHeaderFields.Where(f => f.Name.Equals(fieldName)).Select(f => f.DataType).FirstOrDefault();
                var value = General.GetValueFromFields(dataType, fieldValue, formatDate);
                value = value.Remove(value.Length - 1);
                string sentence = $"SELECT IdHeaderMessage FROM [Operation].[tbHeaderMessage] WHERE {fieldName} = {value}";
                long? idHeaderMessage = null;
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
                                while (readerResult.Read())
                                {
                                    try
                                    {
                                        idHeaderMessage = Convert.ToInt64(readerResult["IdHeaderMessage"]);
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                    }
                }

                return idHeaderMessage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void GetHeaderMessageDuplicated(General objContext)
        {
            try
            {
                using (var dbContext = objContext.DBConnection())
                {
                    string sentence = "SELECT O.IDManatee, COUNT(O.IDManatee) FROM [Operation].[tbHeaderMessage] O GROUP BY O.IDManatee HAVING COUNT(O.IDManatee) > 1";
                    List<string> fields = new List<string>();
                    List<TbHeaderMessage> headersPending = new List<TbHeaderMessage>();
                    using (var command = dbContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = sentence;
                        dbContext.Database.OpenConnection();
                        using (var readerResult = command.ExecuteReader())
                        {
                            if (readerResult.HasRows)
                            {
                                while (readerResult.Read())
                                {
                                    fields.Add(readerResult["IDManatee"].ToString());
                                }
                            }
                        }
                    }

                    foreach (var item in fields)
                    {
                        try
                        {

                            var record = dbContext.TbHeaderMessage.Where(f => f.Idmanatee == item).ToList();

                            if (record.Count > 2)
                            {
                                headersPending.AddRange(record);
                            }
                            if (record.Count == 1)
                            {
                                continue;
                            }
                            var headerMessageUpdate = record.OrderBy(f => f.CreationDate).First();
                            var headerMessageDelete = record.OrderBy(f => f.CreationDate).Last();

                            var log = dbContext.TbLogMessageIn.Where(f => f.IdHeaderMessage == headerMessageDelete.IdHeaderMessage).ToList();
                            log.ForEach(f => f.IdHeaderMessage = headerMessageUpdate.IdHeaderMessage);
                            headerMessageUpdate.FechaHoraEnvio = headerMessageDelete.FechaHoraEnvio;

                            var messageToDelete = dbContext.TbMessages.Where(f => f.IdHeaderMessage.Equals(headerMessageUpdate.IdHeaderMessage)).FirstOrDefault();
                            var messageToUpdate = dbContext.TbMessages.Where(f => f.IdHeaderMessage.Equals(headerMessageDelete.IdHeaderMessage)).FirstOrDefault();
                            messageToUpdate.IdHeaderMessage = headerMessageUpdate.IdHeaderMessage;
                            dbContext.Remove(messageToDelete);
                            dbContext.SaveChanges();
                            dbContext.Remove(headerMessageDelete);
                            dbContext.SaveChanges();
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int AddWeftMessage(General objContext, MessageTypeDTO messageType, List<MessageTypeFieldDTO> lstMessageTypeFields, long idHeaderMessage, Dictionary<string, string> dctDataFields, string formatDate = _formateDate)
        {
            try
            {
                string tableName = messageType.TableName;
                string sentence = $"INSERT INTO [Operation].tb{tableName} (IdHeaderMessage,";
                string value = $"VALUES ({idHeaderMessage},";
                int totalRows = 0;
                foreach (var item in dctDataFields)
                {
                    var dataType = lstMessageTypeFields.Where(f => f.Name.Equals(item.Key)).Select(f => f.DataType).FirstOrDefault();

                    value += General.GetValueFromFields(dataType, item.Value, formatDate);
                    sentence += $"{item.Key},";
                }

                sentence = sentence.Remove(sentence.Length - 1);
                value = value.Remove(value.Length - 1);
                sentence += ")";
                value += ")";

                sentence += $" {value}";

                using (var DBContext = objContext.DBConnection())
                {
                    using (var command = DBContext.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = sentence;
                        DBContext.Database.OpenConnection();
                        totalRows = command.ExecuteNonQuery();
                    }
                }
                return totalRows;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable SearchMessages(General objContext, DateTime dtInit, DateTime dtEnd)
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

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable GetLogMessageByIdHeader(General objContext, string idHeaderMessage)
        {
            try
            {
                string sentence = $"SELECT [Message] [Trama], Observations [Observaciones], DateProcessed [Fecha Procesado] FROM [Log].tbLogMessageIn LM WHERE LM.IdHeaderMessage = {idHeaderMessage}";

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

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddMessageType(General objContext, MessageTypeDTO messageTypeDTO)
        {
            try
            {
                var DBContext = objContext.DBConnection();

                TbMessageTypes tbMessageTypes = new TbMessageTypes
                {
                    Code = messageTypeDTO.Code,
                    Description = messageTypeDTO.Description,
                    FieldCode = messageTypeDTO.FieldCode,
                    FieldIdentifierMessage = messageTypeDTO.FieldIdentifierMessage,
                    FieldWeft = messageTypeDTO.FieldWeft,
                    Name = messageTypeDTO.Name,
                    TableName = messageTypeDTO.TableName,
                    CreationDate = DateTime.UtcNow,
                    Enable = true
                };

                DBContext.Add(tbMessageTypes);
                DBContext.SaveChanges();
                messageTypeDTO.Id = tbMessageTypes.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static MessageTypeDTO GetMessageType(General objContext, int id)
        {
            try
            {
                var DBContext = objContext.DBConnection();

                var result = DBContext.TbMessageTypes.Where(f => f.Id == id).FirstOrDefault();

                MessageTypeDTO messageTypeDTO = new MessageTypeDTO
                {
                    Id = result.Id,
                    Code = result.Code,
                    Description = result.Description,
                    FieldCode = result.FieldCode,
                    FieldIdentifierMessage = result.FieldIdentifierMessage,
                    FieldWeft = result.FieldWeft,
                    Name = result.Name,
                    TableName = result.TableName
                };

                return messageTypeDTO;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static List<ValidFieldDTO> GetValidFieldsByMessageTypeId(General objContext, int id)
        {
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = (from vf in DBContext.TbValidFields
                                  join mf in DBContext.TbMessageTypeFields
                                    on vf.Id equals mf.IdValidField
                                  where mf.IdMessageType == id
                                  select new ValidFieldDTO
                                  {
                                      CreationDate = vf.CreationDate,
                                      DataType = vf.DataType,
                                      Description = vf.Description,
                                      Name = vf.Name,
                                      UpdateDate = vf.UpdateDate,
                                      SearchType = vf.SearchType,
                                      PrimaryType = vf.PrimaryData.Value
                                  }).Distinct().ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void GetAllMessage(General objContext)
        {
            List<long> ids = new List<long>();
            try
            {
                using (var DBContext = objContext.DBConnection())
                {
                    var result = DBContext.TbMessages.Where(f => f.Id > -9223372036853705160).ToList();

                    int i = 0;
                    foreach (var item in result)
                    {
                        i++;
                        Console.WriteLine($"===========================");
                        Console.WriteLine($"item: {i}/{result.Count}");
                        Console.WriteLine($"Id {item.Id}");
                        Console.WriteLine($"FechaHoraEnvioDato: {item.FechaHoraEnvioDato}");
                        Console.WriteLine($"FechaHoraLecturaDato: {item.FechaHoraLecturaDato}");

                        if (string.IsNullOrEmpty(item.FechaHoraEnvioDato.ToString()))
                        {
                            item.FechaHoraEnvioDato = null;
                        }
                        else
                        {
                            //item.FechaHoraEnvioDato =Convert.ToDateTime(General.GetValueFromFields("DATETIME", item.FechaHoraEnvioDato.ToString(), _formateDate, true));
                        }

                        if (string.IsNullOrEmpty(item.FechaHoraLecturaDato.ToString()))
                        {
                            item.FechaHoraLecturaDato = null;
                        }
                        else
                        {
                            //item.FechaHoraLecturaDato = Convert.ToDateTime(General.GetValueFromFields("DATETIME", item.FechaHoraLecturaDato.ToString(), _formateDate, true));
                        }

                        ids.Add(item.Id);
                        DBContext.Update(item);
                    }

                    DBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}