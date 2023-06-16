using MQTT.Infrastructure.DAL;
using MQTT.Infrastructure.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MQTT.Processor.BL
{
    public class ProcessorBL
    {
        private readonly string _connectionString = AppSettings.Instance.Configuration["connectionString"].ToString();
        private readonly List<MessageTypeDTO> _messagesType;
        private readonly List<HeaderFieldDTO> _headerFields;
        private readonly List<SettingDTO> _settings;
        private General _objGeneral;
        private General DBAccess { get => _objGeneral; set => _objGeneral = value; }

        public ProcessorBL()
        {
            DBAccess = new General(_connectionString);
            _messagesType = MessagesDAL.GetAllMessageTypes(DBAccess);
            _headerFields = MessagesDAL.GetHeaderFields(DBAccess);
            _settings = General.GetSettings(DBAccess);
        }

        private List<LogMessageDTO> GetLogMessagePending()
        {
            try
            {
                return LogMessagesDAL.GetLogMessagePending(DBAccess);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ProcessMessages()
        {
            DateTime initDate = DateTime.UtcNow;
            DateTime endDate;
            long? idLogMessageInit = null;
            long? idLogMessageEnd = null;
            string msgError = null;

            try
            {
                bool firtsTime = true;
                Console.WriteLine($">>>>>>> Obteniendo mensajes a procesar...");
                var lstMessagePending = GetLogMessagePending();
                Console.WriteLine($">>>>>>> Mensajes a procesar: {lstMessagePending.Count}");

                foreach (var item in lstMessagePending)
                {
                    try
                    {
                        idLogMessageEnd = item.Id;
                        if (firtsTime)
                        {
                            idLogMessageInit = item.Id;
                            firtsTime = false;
                        }
                        Console.WriteLine($">>>>> IdLogMessage: {item.Id}");
                        MessageTypeDTO currentMessageType;
                        dynamic dataJson = JsonConvert.DeserializeObject(item.Message);
                        dynamic dataWeft;
                        long? idHeaderMessage = null;

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

                        item.Processed = true;
                        item.IdProcessed = 2;
                        item.IdHeaderMessage = idHeaderMessage;                       
                    }
                    catch (Exception exItem)
                    {
                        item.Processed = true;
                        item.IdProcessed = 1;
                        item.Observations = $"{exItem.Message} {exItem.InnerException}";
                        Console.WriteLine($">>>>> Error al procesar Log: {item.Observations}");
                    }
                    UpdateLogMessageIn(item);
                    Console.WriteLine($">>>>> LogMessage Procesado.");
                    Console.WriteLine($"====================================================");
                }
            }
            catch (Exception ex)
            {
                msgError = $"{ex.Message} {ex.InnerException}";
            }
            finally
            {
                endDate = DateTime.UtcNow;
                Console.WriteLine($">>>>> Agregando registro de ejecución.");
                AddLogExecution(idLogMessageInit, idLogMessageEnd, initDate, endDate, msgError);
            }
        }

        private void AddLogExecution(long? idLogMessageInInit, long? idLogMessageInEnd, DateTime dtInit, DateTime dtEnd, string observations = null)
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
        private void GetMessageTypeAndWeft(dynamic dataJson, out MessageTypeDTO messageType, out dynamic dataWeft)
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

        private Dictionary<string, string> GetHeaderFromJson(dynamic dataJson, string fieldWeft)
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

        private int AddWeftMessage(dynamic dataJSon, MessageTypeDTO messageType, List<MessageTypeFieldDTO> lstMessageTypeField, long idHeaderMessage)
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

        private Dictionary<string, string> GetDataFromJson(dynamic dataJson)
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

        private long? CheckHeaderMessage(MessageTypeDTO messageType, string fieldWeft, dynamic dataJson, out bool add, string formatDate = "dd/MM/yyyy")
        {
            try
            {
                add = false;
                Dictionary<string, string> dctDataFields = GetHeaderFromJson(dataJson, fieldWeft);

                var id = dctDataFields[messageType.FieldIdentifierMessage];
                long? idHeaderMessage = MessagesDAL.GetHeaderMessageById(DBAccess, _headerFields, messageType.FieldIdentifierMessage, id);

                if (idHeaderMessage.HasValue)
                {
                    dctDataFields.Remove(messageType.FieldIdentifierMessage);
                    MessagesDAL.UpdateHeaderMessage(DBAccess, _headerFields, dctDataFields, idHeaderMessage.Value);
                }
                else
                {
                    idHeaderMessage = MessagesDAL.AddHeaderMessage(DBAccess, _headerFields, messageType.Id, dctDataFields);
                    add = true;
                }
                return idHeaderMessage.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<ValidFieldDTO> GetFieldsJSON(dynamic dataJson)
        {
            try
            {
                List<ValidFieldDTO> lstValidFieldDTO = new List<ValidFieldDTO>();
                //var dataEvent = EventsDAL.GetEventData(DBAccess, eventCode);

                foreach (var item in dataJson)
                {
                    try
                    {
                        var value = item.Value.ToString();
                        if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                            (value.StartsWith("[") && value.EndsWith("]"))) //For array
                        {
                            dynamic jsonVal = JsonConvert.DeserializeObject(value);
                            lstValidFieldDTO = GetFieldsJSON(jsonVal);
                        }
                    }
                    catch { }

                    ValidFieldDTO validFieldDTO = new ValidFieldDTO() { Name = item.Name };
                    lstValidFieldDTO.Add(validFieldDTO);
                }

                return lstValidFieldDTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void AddMessageIn(MessageInDTO messageInDTO)
        {
            try
            {
                MessagesDAL.AddMessageIn(DBAccess, messageInDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void UpdateLogMessageIn(LogMessageDTO logMessageDTO)
        {
            try
            {
                LogMessagesDAL.Update(DBAccess, logMessageDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReprocessData()
        {
            try
            {
                MessagesDAL.GetAllMessage(DBAccess);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
