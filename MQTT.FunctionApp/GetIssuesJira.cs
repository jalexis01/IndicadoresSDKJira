using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Generic;
using MQTT.Infrastructure.DAL;
using System.Linq;
using MQTT.Infrastructure.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MQTT.FunctionApp
{
	public static class GetIssuesJira
	{
		[FunctionName("GetIssuesJira")]
		public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] Models.Filters filters, HttpRequest req, ILogger log)
		{
            var guid = Guid.NewGuid();

			log.LogInformation($"{guid}====== START PROCESS ======");
			string msgError = string.Empty;
			string uri = string.Empty;
			var logRequestIn = new Infrastructure.Models.DTO.LogRequestInDTO();
			logRequestIn.IdEndPoint = (int)EndPointEnum.GetIssueJira;

			//var connectionString = Environment.GetEnvironmentVariable("ConnectionStringDB", EnvironmentVariableTarget.Process);
			//string token = Environment.GetEnvironmentVariable("TokenJira", EnvironmentVariableTarget.Process).ToString();
			//string timeZone = Environment.GetEnvironmentVariable("TimeZone", EnvironmentVariableTarget.Process).ToString();
			var connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDB;User Id=administrador;Password=2022/M4n4t334zur3;";
			string token = "ZGVzYXJyb2xsb2NjQG1hbmF0ZWVpbmdlbmllcmlhLmNvbTpoZlV0Z1o5UkZHb1F5MlNmSDdzQ0Y5QTY=";
			string timeZone = "SA Pacific Standard Time";
			General DBAccess = new General(connectionString);
			 
			try
			{
				log.LogInformation($"{guid}=== Log request in...");
				logRequestIn.DataQuery = req.QueryString.ToString();
				logRequestIn.DataBody = await new StreamReader(req.Body).ReadToEndAsync();
				logRequestIn.DataBody = string.IsNullOrEmpty(logRequestIn.DataBody) ? null : logRequestIn.DataBody;
				LogRequestInDAL.Add(DBAccess, ref logRequestIn);
				log.LogInformation($"{guid}=== IdRequestIn: {logRequestIn.Id}");
				log.LogInformation($"{guid}=== dataSource: {connectionString}");
				log.LogInformation($"{guid}=== token: {token}");

				log.LogInformation($"{guid}=== Getting equivalences...");
				filters.GetAllEquivalences(DBAccess);
				var equivalenceServiceType = filters.equivalenceServiceType;

                List<ValidationResult> resultValidationsModel = new List<ValidationResult>();
                ValidationContext context = new ValidationContext(filters, null, null);
                if (!Validator.TryValidateObject(filters, context, resultValidationsModel, true))
                {
					throw new Exception(string.Join(", ",resultValidationsModel));
                }

				uri = "https://manateecc.atlassian.net/rest/api/2/search";
				string resultJira;

				uri = $"{uri}?{filters.resultQuery}";

				log.LogInformation($"{guid}=== Endpoint Jira: {uri}");
				var request = (HttpWebRequest)WebRequest.Create(uri);
				request.Method = "GET";
				request.ContentType = "application/json";
				request.Accept = "application/json";
				request.Headers.Add("Authorization", $"Basic {token}");

				try
				{
					using (var response = request.GetResponse())
					{
						using (Stream strReader = response.GetResponseStream())
						{
							if (strReader == null) return null;

							using (StreamReader objReader = new StreamReader(strReader))
							{
								resultJira = objReader.ReadToEnd();
							}
						}
					}
				}
				catch (Exception ex)
				{
					msgError = $"Error at {uri} {ex.Message} {ex.InnerException}";
					throw new Exception(msgError);
				}

				log.LogInformation($"{guid}=== Response from Jira: {resultJira}");
				var json = JsonConvert.DeserializeObject<Models.Issue>(resultJira);

				log.LogInformation($"{guid}=== Total Issues: {json.Issues.Count}");
				List<Models.IssueDTO> result = new List<Models.IssueDTO>();
				foreach (var item in json.Issues)
				{
					var fields = item.Fields;

					var typeSettingConfiguration = (fields.customfield_10075 == null || fields.customfield_10075[0] == null) ? string.Empty : fields.customfield_10075[0].Value;
					var typeSettingConfiguration2 = (fields.customfield_10076 == null || fields.customfield_10076[0] == null) ? string.Empty : fields.customfield_10076[0].Value;
					var typeSettingConfiguration3 = (fields.customfield_10077 == null || fields.customfield_10077[0] == null) ? string.Empty : fields.customfield_10077[0].Value;
					var typeSettingConfiguration4 = (fields.customfield_10078 == null || fields.customfield_10078[0] == null) ? string.Empty : fields.customfield_10078[0].Value;
					var typeSettingConfiguration5 = (fields.customfield_10086 == null || fields.customfield_10086[0] == null) ? string.Empty : fields.customfield_10086[0].Value;

					Models.IssueDTO issueDTO = new Models.IssueDTO
					{
						idTicket = item.Key,
						idEstacion = fields.customfield_10057 == null ? null : fields.customfield_10057.Value,
						idVagon = fields.customfield_10058 == null ? null : fields.customfield_10058.Value,
						idPuerta = fields.customfield_10060 != null ? fields.customfield_10060 : null,
						tipoComponente = fields.customfield_10088 == null ? null : fields.customfield_10088.Value,
                        idComponente = fields.customfield_10060 != null ? fields.customfield_10060 : null,
                        identificacion = fields.customfield_10059 != null ? fields.customfield_10059 : null,
						tipoMantenimiento = fields.customfield_10061 == null ? null : fields.customfield_10061.Value,
						nivelFalla = fields.customfield_10064 == null ? null : fields.customfield_10064.Value,
						codigoFalla = (fields.customfield_10069 == null || fields.customfield_10069[0] == null) ? null : fields.customfield_10069[0].Value,
						fechaApertura = fields.created != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.created), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
						fechaCierre = fields.customfield_10101 != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.customfield_10101), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
						fechaArriboLocacion = fields.customfield_10071 != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.customfield_10071), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
						componenteParte = (fields.customfield_10072 == null || fields.customfield_10072[0] == null) ? null : fields.customfield_10072[0].Value,
						tipoReparacion = (fields.customfield_10081 == null || fields.customfield_10081[0] == null) ? null : fields.customfield_10081[0].Value,
						tipoAjusteConfiguracion = $"{typeSettingConfiguration}{typeSettingConfiguration2}{typeSettingConfiguration3}{typeSettingConfiguration4}{typeSettingConfiguration5}",
						descripcionReparacion = fields.customfield_10105 != null ? fields.customfield_10105 : null,
						tipoCausa = fields.customfield_10067 != null ? fields.customfield_10067.Value : null,
						diagnosticoCausa = fields.customfield_10104 != null ? fields.customfield_10104 : null,
						estadoTicket = fields.status == null ? string.Empty : fields.status.name
					};

					var equivalence = equivalenceServiceType.Where(e => e.Name == issueDTO.tipoMantenimiento).Select(e => e.Value).FirstOrDefault();
					issueDTO.tipoMantenimiento = string.IsNullOrEmpty(equivalence) ? issueDTO.tipoMantenimiento : equivalence;

					equivalence = string.Empty;
					equivalence = equivalenceServiceType.Where(e => e.Name == issueDTO.estadoTicket).Select(e => e.Value).FirstOrDefault();
					issueDTO.estadoTicket = string.IsNullOrEmpty(equivalence) ? "Abierta" : equivalence;
					if (issueDTO.descripcionReparacion!=null) 
					result.Add(issueDTO);
				}

				log.LogInformation($"{guid}==== END PROCESS ======");
				logRequestIn.Processed = true;
				return new OkObjectResult(result);
			}
			catch (Exception ex)
			{
				msgError = $"{ex.Message} {ex.InnerException}";
				msgError += string.IsNullOrEmpty(uri) ? "" : $"|Uri:{uri}";
				log.LogError($"{guid}===ERROR: {msgError}");
				logRequestIn.Observations = msgError;
				return new ConflictObjectResult(msgError);
			}
			finally {
				LogRequestInDAL.Update(DBAccess, logRequestIn);
			}
		}
	}
}
