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
using MQTT.FunctionApp.Models;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

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

            var connectionString = Environment.GetEnvironmentVariable("ConnectionStringDB", EnvironmentVariableTarget.Process);
            string token = Environment.GetEnvironmentVariable("TokenJira", EnvironmentVariableTarget.Process).ToString();
            string timeZone = Environment.GetEnvironmentVariable("TimeZone", EnvironmentVariableTarget.Process).ToString();
            //var connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDBAssaabloy;User Id=administrador;Password=2022/M4n4t334zur3;";
            //string token = "ZGVzYXJyb2xsb2NjQG1hbmF0ZWVpbmdlbmllcmlhLmNvbTpoZlV0Z1o5UkZHb1F5MlNmSDdzQ0Y5QTY=";
            //string timeZone = "SA Pacific Standard Time";
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
                int start = 0;
                int max = 100;
                List<Models.IssueDTO> result = getTicketsFromJira(start, max, token, filters, timeZone, log, guid, msgError);
                result = result.Concat(getTicketsFromJiraMTO(start, max, token, filters, timeZone, log, guid, msgError)).ToList().OrderByDescending(issue => issue.fechaApertura).ToList();

                log.LogInformation($"{guid}==== END PROCESS ======");
				logRequestIn.Processed = true;
                if( result.Count == 1 )
                {
                    return new OkObjectResult(result[0]);
                }
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

		public static List<Models.IssueDTO> convertIssueInTicket(Issue issues, Models.Filters filters, string timeZone) {
            var equivalenceServiceType = filters.equivalenceServiceType;
            List<Models.IssueDTO> result = new List<Models.IssueDTO>();
            foreach (var item in issues.Issues)
            {
                var fields = item.Fields;
                
                var typeSettingConfiguration = (fields.ListadoAjustesPuerta == null || fields.ListadoAjustesPuerta[0] == null) ? string.Empty : fields.ListadoAjustesPuerta[0].Value;
                var typeSettingConfiguration2 = (fields.ListadoConfiguracionPuerta == null || fields.ListadoConfiguracionPuerta[0] == null) ? string.Empty : fields.ListadoConfiguracionPuerta[0].Value;
                var typeSettingConfiguration3 = (fields.ListadoAjustesITS == null || fields.ListadoAjustesITS[0] == null) ? string.Empty : fields.ListadoAjustesITS[0].Value;
                var typeSettingConfiguration4 = (fields.ListadoConfiguracionITS == null || fields.ListadoConfiguracionITS[0] == null) ? string.Empty : fields.ListadoConfiguracionITS[0].Value;
                var typeSettingConfiguration5 = (fields.ListadoConfiguracionRFID == null || fields.ListadoConfiguracionRFID[0] == null) ? string.Empty : fields.ListadoConfiguracionRFID[0].Value;
                var typeSettingConfiguration6 = (fields.ListadoAjusteRFID == null || fields.ListadoAjusteRFID[0] == null) ? string.Empty : fields.ListadoAjusteRFID[0].Value;
                Console.WriteLine(item.Key);
                Console.WriteLine(fields.ListadoAjustesITS);
                Console.WriteLine(fields.ListadoConfiguracionITS);
                Console.WriteLine(fields.ListadoAjustesPuerta);
                Console.WriteLine(fields.ListadoConfiguracionPuerta);
                Console.WriteLine(fields.ListadoConfiguracionRFID);

                
                Models.IssueDTO issueDTO = new Models.IssueDTO
                {
                    idTicket = item.Key,
                    idEstacion = fields.Estacion == null || fields.Estacion.Equals(string.Empty) ? null : fields.Estacion.Value,
                    idVagon = fields.Vagon == null || fields.Vagon.Equals(string.Empty) ? null : fields.Vagon.Value,
                    idPuerta = fields.IdentificacionComponente == null || fields.IdentificacionComponente.Equals(string.Empty) ? null : fields.IdentificacionComponente,
                    tipoComponente = fields.TipoDeComponente == null || fields.TipoDeComponente.Equals(string.Empty) ? null : fields.TipoDeComponente.Value,
                    idComponente = fields.IdentificacionComponente == null || fields.IdentificacionComponente.Equals(string.Empty) ? null : fields.IdentificacionComponente,
                    identificacion = fields.IdentificacionSerial == null || fields.IdentificacionSerial.Equals(string.Empty)  ? null : fields.IdentificacionSerial,
                    tipoMantenimiento = fields.TipoDeServicio == null || fields.TipoDeServicio.Equals(string.Empty) ? null : fields.TipoDeServicio.Value,
                    nivelFalla = fields.ClaseDeFallo == null || fields.ClaseDeFallo.Equals(string.Empty) ? null : fields.ClaseDeFallo.Value,
                    codigoFalla = (fields.DescripcionDeFallo == null || fields.DescripcionDeFallo[0] == null)|| fields.DescripcionDeFallo.Equals(string.Empty) ? null : fields.DescripcionDeFallo[0].Value,
                    fechaApertura = fields.created != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.created), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
                    fechaCierre = fields.FechaSolucion != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.FechaSolucion), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
                    fechaArriboLocacion = fields.FechayHoraDeLlegadaAEstacion != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.FechayHoraDeLlegadaAEstacion), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
                    componenteParte = (fields.DescripcionRepuesto == null || fields.DescripcionRepuesto[0] == null) || fields.DescripcionRepuesto.Equals(string.Empty) ? null : fields.DescripcionRepuesto[0].Value,
                    tipoReparacion = (fields.TipoReparacion == null || fields.TipoReparacion[0] == null) || fields.TipoReparacion.Equals(string.Empty) ? null : fields.TipoReparacion[0].Value,
                    tipoAjusteConfiguracion = $"{typeSettingConfiguration}{typeSettingConfiguration2}{typeSettingConfiguration3}{typeSettingConfiguration4}{typeSettingConfiguration5}{typeSettingConfiguration6}".Equals(string.Empty)? null: $"{typeSettingConfiguration}{typeSettingConfiguration2}{typeSettingConfiguration3}{typeSettingConfiguration4}{typeSettingConfiguration5}{typeSettingConfiguration6}",
                    descripcionReparacion = fields.DescripcionReparacion == null || fields.DescripcionReparacion.Equals(string.Empty) ? null : fields.DescripcionReparacion,
                    tipoCausa = fields.TipoCausa == null || fields.TipoCausa.Equals(string.Empty) ? null : fields.TipoCausa.Value,
                    diagnosticoCausa = fields.DiagnosticoCausa == null || fields.DiagnosticoCausa.Equals(string.Empty) ? null : fields.DiagnosticoCausa,
                    estadoTicket = fields.status == null ? null : fields.status.name
                };

                var equivalence = equivalenceServiceType.Where(e => e.Name == issueDTO.tipoMantenimiento).Select(e => e.Value).FirstOrDefault();
                issueDTO.tipoMantenimiento = string.IsNullOrEmpty(equivalence) ? issueDTO.tipoMantenimiento : equivalence;

                equivalence = string.Empty;
                Console.WriteLine($"Valor de issueDTO.estadoTicket antes de la consulta: {issueDTO.estadoTicket}");
                equivalence = equivalenceServiceType.Where(e => e.Name == issueDTO.estadoTicket).Select(e => e.Value).FirstOrDefault();
                Console.WriteLine($"Valor de equivalence después de la consulta: {equivalence}");
                issueDTO.estadoTicket = fields.status.name != "Cerrado" && fields.status.name != "DESCARTADO" ? "Abierto" : fields.status.name == "Cerrado" ? "Cerrado" : "DESCARTADO";
                if (issueDTO.tipoMantenimiento != null)
                    result.Add(issueDTO);
            }
            return result;
        }
        public static List<Models.IssueDTO> convertIssueInTicketMTO(Issue issues, Models.Filters filters, string timeZone)
        {
            var equivalenceServiceType = filters.equivalenceServiceType;
            List<Models.IssueDTO> result = new List<Models.IssueDTO>();
            foreach (var item in issues.Issues)
            {
                var fields = item.Fields;

                var typeSettingConfiguration = (fields.ListadoAjustesPuerta == null || fields.ListadoAjustesPuerta[0] == null) ? string.Empty : fields.ListadoAjustesPuerta[0].Value;
                var typeSettingConfiguration2 = (fields.ListadoConfiguracionPuerta == null || fields.ListadoConfiguracionPuerta[0] == null) ? string.Empty : fields.ListadoConfiguracionPuerta[0].Value;
                var typeSettingConfiguration3 = (fields.ListadoAjustesITS == null || fields.ListadoAjustesITS[0] == null) ? string.Empty : fields.ListadoAjustesITS[0].Value;
                var typeSettingConfiguration4 = (fields.ListadoConfiguracionITS == null || fields.ListadoConfiguracionITS[0] == null) ? string.Empty : fields.ListadoConfiguracionITS[0].Value;
                var typeSettingConfiguration5 = (fields.ListadoConfiguracionRFID == null || fields.ListadoConfiguracionRFID[0] == null) ? string.Empty : fields.ListadoConfiguracionRFID[0].Value;
                var typeSettingConfiguration6 = (fields.ListadoAjusteRFID == null || fields.ListadoAjusteRFID[0] == null) ? string.Empty : fields.ListadoAjusteRFID[0].Value;
                Console.WriteLine(item.Key);
                Console.WriteLine(fields.ListadoAjustesITS);
                Console.WriteLine(fields.ListadoConfiguracionITS);
                Console.WriteLine(fields.ListadoAjustesPuerta);
                Console.WriteLine(fields.ListadoConfiguracionPuerta);
                Console.WriteLine(fields.ListadoConfiguracionRFID);


                Models.IssueDTO issueDTO = new Models.IssueDTO
                {
                    idTicket = item.Key,
                    idEstacion = fields.Estacion == null || fields.Estacion.Equals(string.Empty) ? null : fields.Estacion.Value,
                    idVagon = fields.Vagon == null || fields.Vagon.Equals(string.Empty) ? null : fields.Vagon.Value,
                    idPuerta = fields.IdentificacionComponente == null || fields.IdentificacionComponente.Equals(string.Empty) ? null : fields.IdentificacionComponente,
                    tipoComponente = fields.TipoDeComponente == null || fields.TipoDeComponente.Equals(string.Empty) ? null : fields.TipoDeComponente.Value,
                    idComponente = fields.IdentificacionComponente == null || fields.IdentificacionComponente.Equals(string.Empty) ? null : fields.IdentificacionComponente,
                    identificacion = fields.IdentificacionSerial == null || fields.IdentificacionSerial.Equals(string.Empty) ? null : fields.IdentificacionSerial,
                    tipoMantenimiento = fields.TipoDeServicio == null || fields.TipoDeServicio.Equals(string.Empty) ? null : fields.TipoDeServicio.Value,
                    nivelFalla = fields.ClaseDeFallo == null || fields.ClaseDeFallo.Equals(string.Empty) ? null : fields.ClaseDeFallo.Value,
                    codigoFalla = (fields.DescripcionDeFallo == null || fields.DescripcionDeFallo[0] == null) || fields.DescripcionDeFallo.Equals(string.Empty) ? null : fields.DescripcionDeFallo[0].Value,
                    fechaApertura = fields.created != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.created), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
                    fechaCierre = fields.FechaSolucion != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.FechaSolucion), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
                    fechaArriboLocacion = fields.FechayHoraDeLlegadaAEstacion != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.FechayHoraDeLlegadaAEstacion), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
                    componenteParte = (fields.DescripcionRepuesto == null || fields.DescripcionRepuesto[0] == null) || fields.DescripcionRepuesto.Equals(string.Empty) ? null : fields.DescripcionRepuesto[0].Value,
                    tipoReparacion = (fields.TipoReparacion == null || fields.TipoReparacion[0] == null) || fields.TipoReparacion.Equals(string.Empty) ? null : fields.TipoReparacion[0].Value,
                    tipoAjusteConfiguracion = $"{typeSettingConfiguration}{typeSettingConfiguration2}{typeSettingConfiguration3}{typeSettingConfiguration4}{typeSettingConfiguration5}{typeSettingConfiguration6}".Equals(string.Empty) ? null : $"{typeSettingConfiguration}{typeSettingConfiguration2}{typeSettingConfiguration3}{typeSettingConfiguration4}{typeSettingConfiguration5}{typeSettingConfiguration6}",
                    descripcionReparacion = fields.DescripcionReparacion == null || fields.DescripcionReparacion.Equals(string.Empty) ? null : fields.DescripcionReparacion,
                    tipoCausa = fields.TipoCausa == null || fields.TipoCausa.Equals(string.Empty) ? null : fields.TipoCausa.Value,
                    diagnosticoCausa = fields.DiagnosticoCausa == null || fields.DiagnosticoCausa.Equals(string.Empty) ? null : fields.DiagnosticoCausa,
                    estadoTicket = fields.status == null ? null : fields.status.name
                };

                var equivalence = equivalenceServiceType.Where(e => e.Name == issueDTO.tipoMantenimiento).Select(e => e.Value).FirstOrDefault();
                issueDTO.tipoMantenimiento = string.IsNullOrEmpty(equivalence) ? issueDTO.tipoMantenimiento : equivalence;

                equivalence = string.Empty;
                Console.WriteLine($"Valor de issueDTO.estadoTicket antes de la consulta: {issueDTO.estadoTicket}");
                equivalence = equivalenceServiceType.Where(e => e.Name == issueDTO.estadoTicket).Select(e => e.Value).FirstOrDefault();
                Console.WriteLine($"Valor de equivalence después de la consulta: {equivalence}");
                issueDTO.estadoTicket = fields.status.name != "Cerrado" && fields.status.name != "DESCARTADO" ? "Abierto" : fields.status.name == "Cerrado" ? "Cerrado" : "DESCARTADO";
                if (issueDTO.tipoMantenimiento != null)
                    result.Add(issueDTO);
            }
            return result;
        }

        public static List<Models.IssueDTO> getTicketsFromJira(int start, int max, string token, Models.Filters filters, string timeZone, ILogger log, Guid guid, string msgError) {
            //string uri = Environment.GetEnvironmentVariable("urljira", EnvironmentVariableTarget.Process);
            //string uri = "https://assaabloymda.atlassian.net/rest/api/2/search";
            string uri = Constantes.URI + "/search";
            string resultJira;
            uri = $"{uri}?{filters.resultQuery}" + "&maxResults=" + max + "&startAt=" + start;

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

            JObject jObject = JObject.Parse(resultJira);
            string propertyValue = jObject["total"].Value<string>();
            int total = int.Parse(propertyValue);
            log.LogInformation($"{guid}=== Total: {propertyValue}");
            var json = ConverJSONInIssue(jObject);
            log.LogInformation($"{guid}=== Total Issues: {json.Issues.Count}");
            List<Models.IssueDTO> result = convertIssueInTicket(json, filters, timeZone);
            if (max + start < total) {
                result = result.Concat(getTicketsFromJira(start + max, max, token, filters, timeZone, log, guid, msgError)).ToList();
            }
            return result;

        }

        public static List<Models.IssueDTO> getTicketsFromJiraMTO(int start, int max, string token, Models.Filters filters, string timeZone, ILogger log, Guid guid, string msgError)
        {
            //string uri = Environment.GetEnvironmentVariable("urljira", EnvironmentVariableTarget.Process);
            //string uri = "https://assaabloymda.atlassian.net/rest/api/2/search";
            string uri = Constantes.URI + "/search";
            string resultJira;
            uri = $"{uri}?{filters.resultQueryMTO}" + "&maxResults=" + max + "&startAt=" + start;

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

            JObject jObject = JObject.Parse(resultJira);
            string propertyValue = jObject["total"].Value<string>();
            int total = int.Parse(propertyValue);
            log.LogInformation($"{guid}=== Total: {propertyValue}");
            var json = ConverJSONInIssueMTO(jObject);
            log.LogInformation($"{guid}=== Total Issues: {json.Issues.Count}");
            List<Models.IssueDTO> result = convertIssueInTicketMTO(json, filters, timeZone);
            if (max + start < total)
            {
                result = result.Concat(getTicketsFromJiraMTO(start + max, max, token, filters, timeZone, log, guid, msgError)).ToList();
            }
            return result;

        }

        public static Issue ConverJSONInIssue(JObject resultJira) {
            Issue issue = new Issue();
            issue.Total = resultJira["total"].Value<int>();
            var dataList = resultJira["issues"].Value<JArray>();
            List<Data> data = new List<Data>();
            foreach (JObject dataItem in dataList) {
                data.Add(ConvertJsonInData(dataItem));
            }
            issue.Issues = data;
            return issue;
        }
        public static Issue ConverJSONInIssueMTO(JObject resultJira)
        {
            Issue issue = new Issue();
            issue.Total = resultJira["total"].Value<int>();
            var dataList = resultJira["issues"].Value<JArray>();
            List<Data> data = new List<Data>();
            foreach (JObject dataItem in dataList)
            {
                data.Add(ConvertJsonInDataMTO(dataItem));
            }
            issue.Issues = data;
            return issue;
        }

        public static Data ConvertJsonInData(JObject dataObject) {
            Data data = new Data();
            data.Key = dataObject["key"].Value<string>();
            var dataList = dataObject["fields"].Value<JObject>();
            data.Fields = Field.ConverJsonInField(dataList);
            return data;
        }
        public static Data ConvertJsonInDataMTO(JObject dataObject)
        {
            Data data = new Data();
            data.Key = dataObject["key"].Value<string>();
            var dataList = dataObject["fields"].Value<JObject>();
            data.Fields = Field.ConverJsonInFieldMTO(dataList);
            return data;
        }

        
    }
}
