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

namespace MQTT.FunctionApp
{
    public static class GetIssuesJira
    {
        [FunctionName("GetIssuesJira")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var dateIni = req.Query["InitialDate"];
            var dateEnd = req.Query["EndDate"];
            string uri = "https://manateecc.atlassian.net/rest/api/2/search";
            string parameters = $"jql=created >= {dateIni} AND created <= {dateEnd} order by created DESC";
            string token = "anVhbl9rXzk2MkBob3RtYWlsLmNvbTpxcDlJdHBjVVhOY2VaUHhlRGg3ZjkwOTk=";
            string resultJira;

            uri = $"{uri}?{parameters}";

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
                throw new Exception($"Error: {ex.Message} {ex.InnerException}. {uri}.");
            }

            var json = JsonConvert.DeserializeObject<Models.Issue>(resultJira);
            List<Models.IssueDTO> result = new List<Models.IssueDTO>();
            foreach (var item in json.Issues)
            {
                var fields = item.Fields;

                var typeSettingConfiguration = (fields.customfield_10076 == null || fields.customfield_10076[0] == null) ? string.Empty : fields.customfield_10076[0].Value;
                var typeSettingConfiguration2 = (fields.customfield_10078 == null || fields.customfield_10078[0] == null) ? string.Empty : fields.customfield_10078[0].Value;
                var typeSettingConfiguration3 = (fields.customfield_10075 == null || fields.customfield_10075[0] == null) ? string.Empty : fields.customfield_10075[0].Value;
                var typeSettingConfiguration4 = (fields.customfield_10077 == null || fields.customfield_10077[0] == null) ? string.Empty : fields.customfield_10077[0].Value;

                Models.IssueDTO issueDTO = new Models.IssueDTO
                {
                    IdTicket = item.Key,
                    Station = fields.customfield_10057 != null ? fields.customfield_10057.Value : null,
                    Wagon = fields.customfield_10058 != null ? fields.customfield_10058.Value : null,
                    Door = fields.customfield_10059 != null ? fields.customfield_10059 : null,
                    Component = (fields.customfield_10072 == null || fields.customfield_10072[0] == null) ? null : fields.customfield_10072[0].Value,
                    Serial = fields.customfield_10059 != null ? fields.customfield_10059 : null,
                    //Location = fields.customfield_10060 != null ? fields.customfield_10060 : null,
                    MaintenanceType = fields.customfield_10061 != null ? fields.customfield_10061.Value : null,
                    FailureLevel = fields.customfield_10064 != null ? fields.customfield_10064.Value : null,
                    FaultCode = (fields.customfield_10069 == null || fields.customfield_10069[0] == null) ? null : fields.customfield_10069[0].Value,
                    CreationDate = fields.created != null ? fields.created : new DateTime(),
                    ResolutionDate = (fields.customfield_10010 == null || fields.customfield_10010.currentStatus == null || fields.customfield_10010.currentStatus.statusDate == null) ? new DateTime() : DateTimeOffset.FromUnixTimeMilliseconds(fields.customfield_10010.currentStatus.statusDate.epochMillis).DateTime,
                    LocationDate = fields.customfield_10071 != null ? fields.customfield_10071 : new DateTime(),
                    ComponentITS = (fields.customfield_10072 == null || fields.customfield_10072[0] == null) ? null : fields.customfield_10072[0].Value,
                    TypeOfRepair = (fields.customfield_10081 == null || fields.customfield_10081[0] == null) ? null : fields.customfield_10081[0].Value,
                    TypeSettingConfiguration = $"{typeSettingConfiguration}{typeSettingConfiguration2}{typeSettingConfiguration3}{typeSettingConfiguration4}",
                    DescriptionRepair = fields.description != null ? fields.description : null,
                    DiagnosisCause = fields.customfield_10067 != null ? fields.customfield_10067.Value : null,
                    //RecordInclusionDate = fields.created != null ? fields.created : new DateTime(),
                    Status = (fields.customfield_10010 == null || fields.customfield_10010.currentStatus == null || fields.customfield_10010.currentStatus.status == null) ? string.Empty : fields.customfield_10010.currentStatus.status
                };

                result.Add(issueDTO);
            }
            return new OkObjectResult(result);
        }
    }
}
