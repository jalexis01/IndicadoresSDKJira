using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MQTT.Infrastructure.DAL;
using MQTT.Infrastructure.Models.DTO;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using MQTT.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MQTT.FunctionApp
{
    public static class ProcessMessageMQTT
    {
        private static List<MessageTypeDTO> _messagesType;
        private static List<HeaderFieldDTO> _headerFields;
        private static List<SettingDTO> _settings;
        private static General DBAccess;

        [FunctionName("ProcessMessageMQTT")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            LogMessageDTO logMsg = new LogMessageDTO();
            DateTime dtIni = DateTime.UtcNow;
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStringDB", EnvironmentVariableTarget.Process);
                //var connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDBQA;User Id=administrador;Password=2022/M4n4t334zur3;";
                DBAccess = new General(connectionString);
                _messagesType = MessagesDAL.GetAllMessageTypes(DBAccess);
                _headerFields = MessagesDAL.GetHeaderFields(DBAccess);
                _settings = General.GetSettings(DBAccess);

                MessageTypeDTO currentMessageType;

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                logMsg = JsonConvert.DeserializeObject<LogMessageDTO>(requestBody);
                dynamic dataWeft;
                long? idHeaderMessage = null;
                dynamic dataJson = JsonConvert.DeserializeObject(logMsg.Message);

                GetMessageTypeAndWeft(dataJson, out currentMessageType, out dataWeft);
                Console.WriteLine($">>>>> IdMessageType: {currentMessageType.Id}");
                Console.WriteLine($">>>>> MessageCode: {currentMessageType.Code}");
                Console.WriteLine($">>>>> Agregando Header...");
                if (currentMessageType == null || currentMessageType.Id == 0)
                {
                    throw new Exception("Evento no configurado");
                }
                idHeaderMessage = CheckHeaderMessage(currentMessageType, currentMessageType.FieldWeft, dataJson, out bool add);
                Console.WriteLine($">>>>> IdHeader: {idHeaderMessage}");

                if (add)
                {
                    var lstMessageTypeFields = MessagesDAL.GetFieldsMessage(DBAccess, currentMessageType.Id);
                    Console.WriteLine($">>>>> Total de campos a insertar: {lstMessageTypeFields.Count}");
                    Console.WriteLine($">>>>> Guardando trama a la tabla: {currentMessageType.TableName}");
                    var totalRows = AddWeftMessage(dataWeft, currentMessageType, lstMessageTypeFields, idHeaderMessage.Value);
                    Console.WriteLine($">>>>> Total de filas: {totalRows}");
                }

                logMsg.Processed = true;
                logMsg.IdProcessed = 2;
                logMsg.IdHeaderMessage = idHeaderMessage;
            }
            catch (Exception ex)
            {
                logMsg.Processed = true;
                logMsg.IdProcessed = 1;
                logMsg.Observations = $"{ex.Message} {ex.InnerException}";
            }

            try
            {
                ProcessLogAsHistory(logMsg);
            }
            catch (Exception ex)
            {
                AddLogExecution(logMsg.Id, logMsg.Id, dtIni, DateTime.UtcNow, $"Error FX(ProcessMessageMQTT)-ProcessLogAsHistory: {ex.Message} {ex.InnerException}");
            }
            watch.Stop();

            log.LogInformation($"Execution Time: {watch.ElapsedMilliseconds} ms");
            return new OkObjectResult("oK");
        }
        private static void AddLogExecution(long? idLogMessageInInit, long? idLogMessageInEnd, DateTime dtInit, DateTime dtEnd, string observations = null)
        {
            try
            {
                var logExecutionProcessorDTO = new LogExecutionProcessorDTO
                {
                    Init = dtInit,
                    End = dtEnd,
                    IdLogMessageInInit = idLogMessageInInit,
                    IdLogMessageInEnd = idLogMessageInEnd,
                    Observation = observations
                };

                LogExecutionProcessorDAL.Add(DBAccess, logExecutionProcessorDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void ProcessLogAsHistory(LogMessageDTO logMessageDTO)
        {
            try
            {
                LogMessagesDAL.AddLogHistory(DBAccess, logMessageDTO);
                LogMessagesDAL.RemoveLogMsg(DBAccess, logMessageDTO.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static int AddWeftMessage(dynamic dataJSon, MessageTypeDTO messageType, List<MessageTypeFieldDTO> lstMessageTypeField, long idHeaderMessage)
        {
            try
            {
                Dictionary<string, string> dctFields = GetDataFromJson(dataJSon);
                return MessagesDAL.AddWeftMessage(DBAccess, messageType, lstMessageTypeField, idHeaderMessage, dctFields);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void GetMessageTypeAndWeft(dynamic dataJson, out MessageTypeDTO messageType, out dynamic dataWeft)
        {
            try
            {
                messageType = null;
                dataWeft = null;
                string codeMsg = null;

                foreach (var message in _messagesType)
                {
                    string weft = string.Empty;
                    try
                    {
                        weft = dataJson[message.FieldWeft];
                    }
                    catch (Exception) { }

                    if (string.IsNullOrEmpty(weft))
                    {
                        throw new Exception("Error, aqui se debe poner el LogMessage como no procesado.");
                    }

                    dataWeft = JsonConvert.DeserializeObject(weft);

                    try
                    {
                        codeMsg = dataWeft[message.FieldCode].ToString();

                        if (codeMsg == message.Code)
                        {
                            messageType = message;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                if (messageType is null || string.IsNullOrEmpty(messageType.Code))
                {
                    throw new Exception($"{codeMsg}, código de mensaje no configurado.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static long? CheckHeaderMessage(MessageTypeDTO messageType, string fieldWeft, dynamic dataJson, out bool add, string formatDate = "dd/MM/yyyy")
        {
            try
            {
                add = false;
                Dictionary<string, string> dctDataFields = GetHeaderFromJson(dataJson, fieldWeft);
                long? idHeaderMessage;

                try
                {
                    idHeaderMessage = MessagesDAL.AddHeaderMessage(DBAccess, _headerFields, messageType.Id, dctDataFields);
                    add = true;
                }
                catch (Exception)
                {
                    var id = dctDataFields[messageType.FieldIdentifierMessage];
                    idHeaderMessage = MessagesDAL.GetHeaderMessageById(DBAccess, _headerFields, messageType.FieldIdentifierMessage, id);
                    dctDataFields.Remove(messageType.FieldIdentifierMessage);
                    MessagesDAL.UpdateHeaderMessage(DBAccess, _headerFields, dctDataFields, idHeaderMessage.Value);

                }
                return idHeaderMessage.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static Dictionary<string, string> GetHeaderFromJson(dynamic dataJson, string fieldWeft)
        {
            try
            {
                Dictionary<string, string> dctFields = GetDataFromJson(dataJson);
                dctFields.Remove(fieldWeft);

                return dctFields;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static Dictionary<string, string> GetDataFromJson(dynamic dataJson)
        {
            try
            {
                Dictionary<string, string> dctFields = new Dictionary<string, string>();
                foreach (var item in dataJson)
                {
                    var fieldName = item.Name;
                    var fieldValue = item.Value.ToString();

                    dctFields.Add(fieldName, fieldValue);
                }

                return dctFields;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
