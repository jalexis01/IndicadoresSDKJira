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
            string urlJira = Environment.GetEnvironmentVariable("TimeZone", EnvironmentVariableTarget.Process).ToString();
            //var connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDB;User Id=administrador;Password=2022/M4n4t334zur3;";
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

                Models.IssueDTO issueDTO = new Models.IssueDTO
                {
                    idTicket = item.Key,
                    idEstacion = fields.Estacion == null ? null : fields.Estacion.Value,
                    idVagon = fields.Vagon == null ? null : fields.Vagon.Value,
                    idPuerta = fields.IdentificacionComponente != null ? fields.IdentificacionComponente : null,
                    tipoComponente = fields.TipoDeComponente == null ? null : fields.TipoDeComponente.Value,
                    idComponente = fields.IdentificacionComponente != null ? fields.IdentificacionComponente : null,
                    identificacion = fields.IdentificacionSerial != null ? fields.IdentificacionSerial : null,
                    tipoMantenimiento = fields.TipoDeServicio == null ? null : fields.TipoDeServicio.Value,
                    nivelFalla = fields.ClaseDeFallo == null ? null : fields.ClaseDeFallo.Value,
                    codigoFalla = (fields.DescripcionDeFallo == null || fields.DescripcionDeFallo[0] == null) ? null : fields.DescripcionDeFallo[0].Value,
                    fechaApertura = fields.created != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.created), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
                    fechaCierre = fields.FechaSolucion != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.FechaSolucion), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
                    fechaArriboLocacion = fields.FechayHoraDeLlegadaAEstacion != null ? TimeZoneInfo.ConvertTime(Convert.ToDateTime(fields.FechayHoraDeLlegadaAEstacion), TimeZoneInfo.FindSystemTimeZoneById(timeZone)) : (DateTime?)null,
                    componenteParte = (fields.DescripcionRepuesto == null || fields.DescripcionRepuesto[0] == null) ? null : fields.DescripcionRepuesto[0].Value,
                    tipoReparacion = (fields.TipoReparacion == null || fields.TipoReparacion[0] == null) ? null : fields.TipoReparacion[0].Value,
                    tipoAjusteConfiguracion = $"{typeSettingConfiguration}{typeSettingConfiguration2}{typeSettingConfiguration3}{typeSettingConfiguration4}{typeSettingConfiguration5}",
                    descripcionReparacion = fields.DescripcionReparacion != null ? fields.DescripcionReparacion : null,
                    tipoCausa = fields.TipoCausa != null ? fields.TipoCausa.Value : null,
                    diagnosticoCausa = fields.DiagnosticoCausa != null ? fields.DiagnosticoCausa : null,
                    estadoTicket = fields.status == null ? string.Empty : fields.status.name
                };

                var equivalence = equivalenceServiceType.Where(e => e.Name == issueDTO.tipoMantenimiento).Select(e => e.Value).FirstOrDefault();
                issueDTO.tipoMantenimiento = string.IsNullOrEmpty(equivalence) ? issueDTO.tipoMantenimiento : equivalence;

                equivalence = string.Empty;
                equivalence = equivalenceServiceType.Where(e => e.Name == issueDTO.estadoTicket).Select(e => e.Value).FirstOrDefault();
                issueDTO.estadoTicket = string.IsNullOrEmpty(equivalence) ? "Abierta" : equivalence;
                if (issueDTO.descripcionReparacion != null)
                    result.Add(issueDTO);
            }
            return result;
        }

		public static List<Models.IssueDTO> getTicketsFromJira(int start, int max, string token, Models.Filters filters, string timeZone, ILogger log, Guid guid, string msgError) {
            string uri = Environment.GetEnvironmentVariable("urljira", EnvironmentVariableTarget.Process);
            //string uri = "https://assaabloymda.atlassian.net/rest/api/2/search";
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
            if ( max + start < total) {
                result = result.Concat( getTicketsFromJira(start + max, max, token, filters, timeZone, log, guid, msgError)).ToList();
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

        public static Data ConvertJsonInData(JObject dataObject) {
            Data data = new Data();
            data.Key = dataObject["key"].Value<string>();
            var dataList = dataObject["fields"].Value<JObject>();
            data.Fields = ConverJsonInField(dataList);
            return data;
        }

        public static Field ConverJsonInField(JObject fieldObject) {
            Field field = new Field();

            if (fieldObject.ContainsKey("customfield_10052") && fieldObject["customfield_10052"]["value"].Value<string>() != null)
            {
                Category estacion = new Category();
                estacion.Value = fieldObject["customfield_10052"]["value"].Value<string>();
                field.Estacion = estacion;
            }
            else
            {
                field.Estacion = null;
            }

            if (fieldObject.ContainsKey("customfield_10073") && fieldObject["customfield_10073"]["value"].Value<string>() != null)
            {
                Category Vagon = new Category();
                Vagon.Value = fieldObject["customfield_10073"]["value"].Value<string>();
                field.Vagon = Vagon;
            }else
            {
                field.Vagon = null;
            }

            if (fieldObject.ContainsKey("customfield_10057") && fieldObject["customfield_10057"].Value<string>() != null)
            {
                field.IdentificacionComponente = fieldObject["customfield_10057"].Value<string>();
            }
            else
            {
                field.IdentificacionComponente= null;
            }


            //Category tipoDeComponente = fieldObject["customfield_10070"].Value<Category>();
            if (fieldObject.ContainsKey("customfield_10070") && fieldObject["customfield_10070"]["value"].Value<string>() != null)
            {
                Category tipoDeComponente = new Category();
                tipoDeComponente.Value = fieldObject["customfield_10070"]["value"].Value<string>();
                field.TipoDeComponente = tipoDeComponente;
            }
            else
            {
                field.TipoDeComponente = null;
            }

            if (fieldObject.ContainsKey("customfield_10058") && fieldObject["customfield_10058"].Value<string>() != null)
            {
                field.IdentificacionSerial = fieldObject["customfield_10058"].Value<string>();
            }
            else
            {
                field.IdentificacionSerial = null;
            }

            if (fieldObject["customfield_10072"].Contains("value") && fieldObject["customfield_10072"]["value"].Value<string>() != null)
            {
                Category tipoDeServicio = new Category();
                tipoDeServicio.Value = fieldObject["customfield_10072"]["value"].Value<string>();
                field.TipoDeServicio = tipoDeServicio;
            }
            else
            {
                field.TipoDeServicio= null;
            }

            if (fieldObject["customfield_10046"].Contains("value") && fieldObject["customfield_10046"]["value"].Value<string>() != null)
            {
                Category claseDeFallo = new Category();
                claseDeFallo.Value = fieldObject["customfield_10046"]["value"].Value<string>();
                field.ClaseDeFallo = claseDeFallo;
            }
            else
            {
                field.ClaseDeFallo = null;
            }

            if (fieldObject["customfield_10048"].Value<JArray>() != null)
            {
                var descripcionDeFalloArray = fieldObject["customfield_10048"].Value<JArray>();
                List<Category> descripcionDeFallo = new List<Category>();
                foreach (JObject dataItem in descripcionDeFalloArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    descripcionDeFallo.Add(temp);
                }
                field.DescripcionDeFallo = descripcionDeFallo;
            }
            else
            {
                field.DescripcionDeFallo = null;
            }

            if (fieldObject["created"].Value<DateTime>() != null)
            {
                DateTime created = fieldObject["created"].Value<DateTime>();
                field.created = created;
            }
            else
            {
                field.created = null;
            }

            if (fieldObject["statuscategorychangedate"].Value<DateTime>() != null)
            {
                DateTime statuscategorychangedate = fieldObject["statuscategorychangedate"].Value<DateTime>();
                field.statuscategorychangedate = statuscategorychangedate;
            }
            else
            {
                field.statuscategorychangedate = null;
            }
            if (fieldObject.ContainsKey("customfield_10056") && fieldObject.TryGetValue("customfield_10056", out JToken customField10056) && customField10056.Type != JTokenType.Null)
            {
                DateTime FechayHoraDeLlegadaAEstacion = fieldObject["customfield_10056"].Value<DateTime>();
                field.FechayHoraDeLlegadaAEstacion = FechayHoraDeLlegadaAEstacion;
            }
            else
            {
                field.FechayHoraDeLlegadaAEstacion = null;
            }

            if (fieldObject["customfield_10050"].Value<JArray>() != null)
            {
                var DescripcionRepuestoArray = fieldObject["customfield_10050"].Value<JArray>();
                List<Category> DescripcionRepuesto = new List<Category>();
                foreach (JObject dataItem in DescripcionRepuestoArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    DescripcionRepuesto.Add(temp);
                }
                field.DescripcionRepuesto = DescripcionRepuesto;
            }
            else
            {
                field.DescripcionRepuesto = null;
            }

            if (fieldObject["customfield_10071"].Value<JArray>() != null)
            {
                var TipoReparacionArray = fieldObject["customfield_10071"].Value<JArray>();
                List<Category> TipoReparacion = new List<Category>();
                foreach (JObject dataItem in TipoReparacionArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    TipoReparacion.Add(temp);
                }
                field.TipoReparacion = TipoReparacion;
            }
            else
            {
                field.TipoReparacion = null;
            }

            if (fieldObject["customfield_10060"].Value<JArray>() != null)
            {
                var ListadoAjustesPuertaArray = fieldObject["customfield_10060"].Value<JArray>();
                List<Category> ListadoAjustesPuerta = new List<Category>();
                foreach (JObject dataItem in ListadoAjustesPuertaArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoAjustesPuerta.Add(temp);
                }
                field.ListadoAjustesPuerta = ListadoAjustesPuerta;
            }
            else
            {
                field.ListadoConfiguracionPuerta = null;
            }

            if (fieldObject["customfield_10063"].Value<JArray>() != null)
            {
                var ListadoConfiguracionPuertaArray = fieldObject["customfield_10063"].Value<JArray>();
                List<Category> ListadoConfiguracionPuerta = new List<Category>();
                foreach (JObject dataItem in ListadoConfiguracionPuertaArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoConfiguracionPuerta.Add(temp);
                }
                field.ListadoConfiguracionPuerta = ListadoConfiguracionPuerta;
            }
            else
            {
                field.ListadoConfiguracionPuerta = null;
            }

            if (fieldObject["customfield_10059"].Value<JArray>() != null)
            {
                var ListadoAjustesITSArray = fieldObject["customfield_10059"].Value<JArray>();
                List<Category> ListadoAjustesITS = new List<Category>();
                foreach (JObject dataItem in ListadoAjustesITSArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoAjustesITS.Add(temp);
                }
                field.ListadoAjustesITS = ListadoAjustesITS;
            }
            else
            {
                field.ListadoAjustesITS = null;
            }

            if (fieldObject["customfield_10062"].Value<JArray>() != null)
            {
                var ListadoConfiguracionITSArray = fieldObject["customfield_10062"].Value<JArray>();
                List<Category> ListadoConfiguracionITS = new List<Category>();
                foreach (JObject dataItem in ListadoConfiguracionITSArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoConfiguracionITS.Add(temp);
                }
                field.ListadoConfiguracionITS = ListadoConfiguracionITS;
            }
            else
            {
                field.ListadoConfiguracionITS = null;
            }

            if (fieldObject["customfield_10064"].Value<JArray>() != null)
            {
                var ListadoConfiguracionRFIDArray = fieldObject["customfield_10064"].Value<JArray>();
                List<Category> ListadoConfiguracionRFID = new List<Category>();
                foreach (JObject dataItem in ListadoConfiguracionRFIDArray)
                {
                    Category temp = new Category();
                    temp.Value = dataItem["value"].Value<string>();
                    ListadoConfiguracionRFID.Add(temp);
                }
                field.ListadoConfiguracionRFID = ListadoConfiguracionRFID;
            }
            else
            {
                field.ListadoConfiguracionRFID = null;
            }

            if (fieldObject["customfield_10087"].Value<string>() != null)
            {
                field.DescripcionReparacion = fieldObject["customfield_10087"].Value<string>();
            }
            else
            {
                field.DescripcionReparacion = null;
            }

            if (fieldObject["customfield_10088"].Value<string>() != null)
            {
                field.DiagnosticoCausa = fieldObject["customfield_10088"].Value<string>();
            }
            else
            {
                field.DiagnosticoCausa = null;
            }
            if (fieldObject["customfield_10051"].Contains("value")&&fieldObject["customfield_10051"]["value"].Value<string>() != null)
            {
                Category TipoCausa = new Category();
                TipoCausa.Value = fieldObject["customfield_10051"]["value"].Value<string>();
                field.TipoCausa = TipoCausa;
            }
            else
            {
                field.TipoCausa = null;
            }

            if (fieldObject.TryGetValue("customfield_10055", out JToken customfield_10055) && customfield_10055.Type != JTokenType.Null)
            {
                DateTime FechaSolucion = fieldObject["customfield_10055"].Value<DateTime>();
                field.FechaSolucion = FechaSolucion;
            }
            else
            {
                field.FechaSolucion = null;
            }

            if (fieldObject["status"]["name"].Value<string>() != null)
            {
                Status status = new Status();
                status.name = fieldObject["status"]["name"].Value<string>();
                field.status = status;
            }
            else
            {
                field.status = null;
            }
            return field;
        }
    }
}
