using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MQTT.Infrastructure.DAL;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;
using MQTT.FunctionApp.Models;
using MQTT.Infrastructure.Models.Enums;

namespace MQTT.FunctionApp
{
    public static class SetSerial
    {
        [FunctionName("SetSerial")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var guid = Guid.NewGuid();
            log.LogInformation($"{guid}====== START PROCESS ======");
            string msgError = string.Empty;
            string uri = string.Empty;
            string body = string.Empty;
            var logRequestIn = new Infrastructure.Models.DTO.LogRequestInDTO();
            logRequestIn.IdEndPoint = (int)EndPointEnum.SetSerial;

            var connectionString = Environment.GetEnvironmentVariable("ConnectionStringDB", EnvironmentVariableTarget.Process);
            string token = Environment.GetEnvironmentVariable("TokenJira", EnvironmentVariableTarget.Process).ToString();
            //var connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDB;User Id=administrador;Password=2022/M4n4t334zur3;";
            //string token = "ZGVzYXJyb2xsb2NjQG1hbmF0ZWVpbmdlbmllcmlhLmNvbTpoZlV0Z1o5UkZHb1F5MlNmSDdzQ0Y5QTY=";
            General DBAccess = new General(connectionString);

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                log.LogInformation($"{guid}=== Log request in...");
                logRequestIn.DataQuery = string.IsNullOrEmpty(req.QueryString.ToString()) ? null : req.QueryString.ToString();
                logRequestIn.DataBody = string.IsNullOrEmpty(requestBody) ? null : requestBody;
                LogRequestInDAL.Add(DBAccess, ref logRequestIn);
                log.LogInformation($"{guid}=== IdRequestIn: {logRequestIn.Id}");

                log.LogInformation($"{guid}=== dataSource: {connectionString}");
                log.LogInformation($"{guid}=== token: {token}");
                log.LogInformation($"{guid}=== body: {requestBody}");

                JObject data = JObject.Parse(requestBody);
                string field = (string)data["issue"]["fields"][Constantes.IdentificacionComponente];
                //string field = (string)data["issue"]["fields"][Constantes.IdentificacionComponente];
                string key = (string)data["issue"]["key"];
                log.LogInformation($"{guid}=== issue Key: {key}");
                log.LogInformation($"{guid}=== field to search: {field}");

                var door = field != null ? field : null;
                log.LogInformation($"{guid}=== Getting data...");
                var dataElement = ElementsDAL.GetElementByCode(DBAccess, door);

                if (dataElement == null)
                {
                    msgError = $"Element *{door}* not found in database.";
                    throw new Exception(msgError);
                }
               
               
                JObject dataIssueJson = new JObject();
                JObject fieldJson = new JObject();
                fieldJson.Add(Constantes.IdentificacionSerial, dataElement.Value);
                JObject tipoComponente = new JObject();
                tipoComponente.Add("value", dataElement.NameElementType);
                fieldJson.Add(Constantes.TipoDeComponente, tipoComponente);
                dataIssueJson.Add("fields", fieldJson);

                Console.WriteLine(dataIssueJson);
                log.LogInformation($"{guid}=== Element Name: {dataIssueJson}");
                body = dataIssueJson.ToString();
                uri = Constantes.URI + $"/issue/{key}";

                log.LogInformation($"{guid}=== Request to Jira...");
                var response = MQTT.Infrastructure.BL.Requests.GetResponse(uri, "PUT", token, body);
                logRequestIn.Processed = true;
                log.LogInformation($"{guid}=== Serial set successfully");
                log.LogInformation($"{guid}==== END PROCESS ======");
                return new OkObjectResult("Serial set successfully");
            }
            catch (Exception ex)
            {
                msgError = $"{ex.Message} {ex.InnerException}|Uri:{uri}|Body:{body}";
                log.LogError($"{guid}=== ERROR:{msgError}");
                logRequestIn.Observations = msgError;
                return new ConflictObjectResult(msgError);
            }
            finally
            {
                LogRequestInDAL.Update(DBAccess, logRequestIn);
            }
        }
    }
}
