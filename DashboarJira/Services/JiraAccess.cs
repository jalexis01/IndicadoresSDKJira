using Atlassian.Jira;
using DashboarJira.Model;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Data;

namespace DashboarJira.Services
{
    public class JiraAccess
    {
        string jiraUrl = "https://manateecc.atlassian.net/";
        string username = "desarrollocc@manateeingenieria.com";
        string password = "ATATT3xFfGF0ZRHIEZTEJVRnhNKviH0CGed6QXqCDMj5bCmKSEbO00UUjHUb3yDcaA4YD1SHohyDr4qnwRx2x4Tu_S_QW_xlGIcIUDvL7CFKEg47_Jcy4Dmq6YzO0dvqB3qeT-EVWfwJ2jJ-9vEUfsqXavD0IIGA7DAZHGCtIWhxgwKIbAWsmeA=038B810D";
        //string jiraUrl = "https://assaabloymda.atlassian.net/";
        //string username = "desarrollocc@manateeingenieria.com";
        //string password = "ATATT3xFfGF0ZRHIEZTEJVRnhNKviH0CGed6QXqCDMj5bCmKSEbO00UUjHUb3yDcaA4YD1SHohyDr4qnwRx2x4Tu_S_QW_xlGIcIUDvL7CFKEg47_Jcy4Dmq6YzO0dvqB3qeT-EVWfwJ2jJ-9vEUfsqXavD0IIGA7DAZHGCtIWhxgwKIbAWsmeA=038B810D";

        Jira jira;
        private DbConnector connector;

        public JiraAccess()
        {
            connector = new DbConnector();
            jira = Jira.CreateRestClient(jiraUrl, username, password);
        }
        const string proyectAssa = "project = 'Mesa de Ayuda'";
        const string proyectAssaMTO = "project = 'Mesa de Ayuda-MP'";
        //const string proytect = "(project = 'Mesa de Ayuda' OR project = 'Mtto Preventivo')";
        const string proyectManatee = "project = 'Centro de Control'";

        const string proyectManateeMTO = "(project = 'TICKETMP' OR project = 'TICKETDRV')";
        /*TODO*/
        public List<Ticket> GetTikets(int start, int max, string startDate, string endDate, string idComponente)
        {
            try
            {
                List<Ticket> result = GetTiketsCC(start, max, startDate, endDate, idComponente) ?? new List<Ticket>();
                result = result.Concat(GetTiketsMTO(start, max, startDate, endDate, idComponente) ?? new List<Ticket>()).ToList().OrderByDescending(issue => issue.fecha_apertura).ToList();
                return result;
            }
            catch (Exception ex) { return new List<Ticket>(); }

        }
        public List<Ticket> GetTiketsCC(int start, int max, string startDate, string endDate, string idComponente)
        {
            try
            {
                var jql = "";
                //created >= 2023-04-04 AND created <= 2023-04-13 AND issuetype = "Solicitud de Mantenimiento" AND resolution = Unresolved AND "Clase de fallo" = AIO AND "Identificacion componente" ~ 9119-WA-OR-1 ORDER BY key DESC, "Time to resolution" ASC
                if (jiraUrl == "https://assaabloymda.atlassian.net/")
                {
                    jql = $"{proyectAssa} and issuetype = 'Solicitud de Mantenimiento'";
                }
                else
                {
                    jql = $"{proyectManatee} and issuetype = 'Solicitud de Mantenimiento'";
                }

                if (startDate != null && endDate != null)
                {
                    jql += " AND " + "created >= " + startDate + " AND " + "created <= " + endDate;
                }
                if (idComponente != null)
                {

                    jql += " AND " + "'Identificacion componente' ~ " + idComponente;
                }
                //jql += " AND 'Tipo de servicio' is not empty ";
                jql += " ORDER BY key DESC, 'Time to resolution' ASC";

                Task<IPagedQueryResult<Issue>> issues = null;

                if (max != 0)
                {
                    issues = jira.Issues.GetIssuesFromJqlAsync(jql, max, start);
                }
                else if (max == 0)
                {
                    max = 100;
                    issues = jira.Issues.GetIssuesFromJqlAsync(jql, max, start);
                }
                int total = totalITem(issues);
                List<Ticket> result = ConvertIssusInTickets(issues);
                if (total > max + start)
                {
                    result = result.Concat(GetTiketsCC(start + max, max, startDate, endDate, idComponente)).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            return null;
        }
        public List<Ticket> GetTiketsMTO(int start, int max, string startDate, string endDate, string idComponente)
        {
            try
            {
                var jql = "";
                //created >= 2023-04-04 AND created <= 2023-04-13 AND issuetype = "Solicitud de Mantenimiento" AND resolution = Unresolved AND "Clase de fallo" = AIO AND "Identificacion componente" ~ 9119-WA-OR-1 ORDER BY key DESC, "Time to resolution" ASC
                if (jiraUrl == "https://assaabloymda.atlassian.net/")
                {
                    jql = $"{proyectAssaMTO} and issuetype = 'Solicitud de Mantenimiento' and status = cerrado";
                }
                else
                {
                    jql = $"{proyectManateeMTO} and issuetype = 'Solicitud de Mantenimiento'";
                }

                if (startDate != null && endDate != null)
                {
                    jql += " AND " + "'Fecha de creacion' >= '" + startDate + "' AND " + "'Fecha de creacion' <= '" + endDate + "'";
                }
                if (idComponente != null)
                {

                    jql += " AND " + "'Identificacion componente' ~ " + idComponente;
                }
                //jql += " AND 'Tipo de servicio' is not empty ";
                jql += " ORDER BY key DESC, 'Time to resolution' ASC";

                Task<IPagedQueryResult<Issue>> issues = null;

                if (max != 0)
                {
                    issues = jira.Issues.GetIssuesFromJqlAsync(jql, max, start);
                }
                else if (max == 0)
                {
                    max = 100;
                    issues = jira.Issues.GetIssuesFromJqlAsync(jql, max, start);
                }
                int total = totalITem(issues);
                List<Ticket> result = ConvertIssusInTicketsMTO(issues);
                if (total > max + start)
                {
                    result = result.Concat(GetTiketsMTO(start + max, max, startDate, endDate, idComponente)).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            return null;
        }

        /*TODO*/
        public List<IssueJira> GetIssuesJira(int start, int max, string startDate, string endDate, string idComponente)
        {
            try
            {
                //created >= 2023-04-04 AND created <= 2023-04-13 AND issuetype = "Solicitud de Mantenimiento" AND resolution = Unresolved AND "Clase de fallo" = AIO AND "Identificacion componente" ~ 9119-WA-OR-1 ORDER BY key DESC, "Time to resolution" ASC
                var jql = "(project = 'Mesa de Ayuda' OR project = 'Mtto Preventivo') and issuetype = 'Solicitud de Mantenimiento'";
                if (startDate != null && endDate != null)
                {
                    jql += " AND " + "created >= " + startDate + " AND " + "created <= " + endDate;
                }
                if (idComponente != null)
                {

                    jql += " AND " + "'Identificacion componente' ~ " + idComponente;
                }
                jql += " ORDER BY key DESC, 'Time to resolution' ASC";
                Task<IPagedQueryResult<Issue>> issues = null;
                if (max != 0)
                {
                    issues = jira.Issues.GetIssuesFromJqlAsync(jql, max, start);
                }
                else if (max == 0)
                {
                    issues = jira.Issues.GetIssuesFromJqlAsync(jql, int.MaxValue, 0);
                }
                int total = totalITem(issues);
                List<IssueJira> result = ConvertIssusInIssuesJira(issues);
                if (total > max + start)
                {
                    result = result.Concat(GetIssuesJira(start + max, max, startDate, endDate, idComponente)).ToList();
                }
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            return null;
        }
        /*TODO*/
        public List<Ticket> GetTiketsIndicadores(string query)
        {
            var jql = query;
            var issues = jira.Issues.GetIssuesFromJqlAsync(jql, int.MaxValue, 0);
            int total = totalITem(issues);
            int max = 100;
            int actualPos = 0;
            List<Ticket> result = ConvertIssusInTickets(issues);
            for (int i = actualPos + max; i < total; i++)
            {
                issues = jira.Issues.GetIssuesFromJqlAsync(jql, max, actualPos + max);
                result = result.Concat(ConvertIssusInTickets(issues)).ToList();
            }
            return result.ToList();
        }

        public Ticket getTicket(string id)
        {
            var ticket = jira.Issues.GetIssueAsync(id).Result;
            var estacionMap = getEstaciones().AsEnumerable()
                .ToDictionary(row => row[1].ToString(), row => row[2].ToString());

            return converIssueInTicket(ticket, estacionMap);
        }

        public List<IssueJira> ConvertIssusInIssuesJira(Task<IPagedQueryResult<Issue>> issues)
        {
            List<IssueJira> result = new List<IssueJira>();
            foreach (var issue in issues.Result)
            {
                Console.WriteLine(issue.Key);

                result.Add(convertIssueInIssueJira(issue));
            }
            return result;

        }
        public int totalITem(Task<IPagedQueryResult<Issue>> issues)
        {
            int totalInt = issues.Result.TotalItems;

            return totalInt;

        }

        public List<Ticket> ConvertIssusInTickets(Task<IPagedQueryResult<Issue>> issues)
        {

            var result = new List<Ticket>();
            var estacionMap = getEstaciones().AsEnumerable()
                .ToDictionary(row => row[1].ToString(), row => row[2].ToString());

            foreach (var issue in issues.Result)
            {
                result.Add(converIssueInTicket(issue, estacionMap));
            }

            return result;

        }
        public List<Ticket> ConvertIssusInTicketsMTO(Task<IPagedQueryResult<Issue>> issues)
        {

            var result = new List<Ticket>();
            var estacionMap = getEstaciones().AsEnumerable()
                .ToDictionary(row => row[1].ToString(), row => row[2].ToString());

            foreach (var issue in issues.Result)
            {
                result.Add(converIssueInTicketMTO(issue, estacionMap));
            }

            return result;

        }

        public Ticket converIssueInTicket(Issue issue, Dictionary<string, string> estacionMap)
        {
            Ticket temp = new Ticket();

            temp.id_ticket = issue.Key.Value;

            temp.id_estacion = (issue.CustomFields["Estacion"] != null ? issue.CustomFields["Estacion"].Values[0] : "");

            string estacionValue = (issue.CustomFields["Estacion"] != null ? issue.CustomFields["Estacion"].Values[0] : "");

            if (estacionMap.ContainsKey(estacionValue))
            {
                temp.nombre_estacion = estacionMap[estacionValue];
            }
            else
            {
                temp.nombre_estacion = "Estación sin configurar";
            }

            temp.id_vagon = (issue.CustomFields["Vagon"] != null ? issue.CustomFields["Vagon"].Values[0] : "");


            temp.tipoComponente = (issue.CustomFields["Tipo de componente"] != null ? issue.CustomFields["Tipo de componente"].Values[0] : "");


            temp.id_puerta = (temp.tipoComponente == "Puerta" && temp.tipoComponente != "null" && issue.CustomFields["Identificacion componente"] != null && issue.CustomFields["Identificacion componente"].Values[0] != null ? issue.CustomFields["Identificacion componente"].Values[0] : "");


            temp.id_componente = (issue.CustomFields["Identificacion componente"] != null ? issue.CustomFields["Identificacion componente"].Values[0] : "");


            temp.identificacion = (issue.CustomFields["Identificacion (serial)"] != null ? issue.CustomFields["Identificacion (serial)"].Values[0] : "");


            temp.tipo_mantenimiento = (issue.CustomFields["Tipo de servicio"] != null ? (issue.CustomFields["Tipo de servicio"].Values[0] == "Mantenimiento Preventivo" ? "Preventivo" : "Correctivo") : "");


            temp.nivel_falla = (issue.CustomFields["Clase de fallo"] != null ? issue.CustomFields["Clase de fallo"].Values[0] : "");


            temp.codigo_falla = (issue.CustomFields["Descripcion de fallo"] != null ? issue.CustomFields["Descripcion de fallo"].Values[0] : "");

            //if (jiraUrl == "https://assaabloymda.atlassian.net/")
            //{                
            //    //temp.fecha_apertura = (issue.Created != null ? DateTime.Parse(issue.CustomFields["Fecha de creacion"].Values[0]) : null);
            //}
            //else
            //{
            //    temp.fecha_apertura = issue.Created == null ? null : DateTime.Parse(issue.Created.Value.ToString());
            //}


            temp.fecha_apertura = issue.Created == null ? null : DateTime.Parse(issue.Created.Value.ToString());

            temp.fecha_arribo_locacion = (issue.CustomFields["Fecha y Hora de Llegada a Estacion"] != null ? DateTime.Parse(issue.CustomFields["Fecha y Hora de Llegada a Estacion"].Values[0]) : null);

            temp.fecha_cierre = (issue.CustomFields["Fecha de solucion"] != null ? DateTime.Parse(issue.CustomFields["Fecha de solucion"].Values[0]) : null);

            temp.cantidad_repuesto_utilizado = (issue.CustomFields["Cantidad(es) repuesto(s) utilizado(s)"] != null ? issue.CustomFields["Cantidad(es) repuesto(s) utilizado(s)"].Values[0] : "");

            temp.componente_Parte = (issue.CustomFields["Descripcion de repuesto"] != null ? issue.CustomFields["Descripcion de repuesto"].Values[0] : "");


            temp.tipo_reparacion = (issue.CustomFields["Tipo de reparacion"] != null ? issue.CustomFields["Tipo de reparacion"].Values[0] : "");


            temp.tipo_ajuste_configuracion = (issue.CustomFields["Listado de ajustes ITS"] != null ? issue.CustomFields["Listado de ajustes ITS"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de configuracion ITS"] != null ? issue.CustomFields["Listado de configuracion ITS"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de ajustes Puerta"] != null ? issue.CustomFields["Listado de ajustes Puerta"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de configuracion Puerta"] != null ? issue.CustomFields["Listado de configuracion Puerta"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de ajustes RFID"] != null ? issue.CustomFields["Listado de ajustes RFID"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de configuracion RFID"] != null ? issue.CustomFields["Listado de configuracion RFID"].Values[0] + "\n" : "");


            temp.descripcion_reparacion = (issue.CustomFields["Descripcion de la reparacion"] != null ? issue.CustomFields["Descripcion de la reparacion"].Values[0] : "");


            temp.diagnostico_causa = (issue.CustomFields["Diagnostico de la causa"] != null ? issue.CustomFields["Diagnostico de la causa"].Values[0] : "");


            temp.tipo_causa = (issue.CustomFields["Tipo de causa"] != null ? issue.CustomFields["Tipo de causa"].Values[0] : "");


            //temp.estado_ticket = issue.Status.Name;

            if (issue.Status.Name == "Cerrado" || issue.Status.Name == "DESCARTADO")
            {
                temp.estado_ticket = issue.Status.Name;
            }
            else
            {
                temp.estado_ticket = "Abierto";
            }
            if (issue.Status == null)
                temp.estado_ticket = (issue.Status != null ? issue.Status.Name : "");

            temp.descripcion = (issue.Description != null ? issue.Description : "");

            //Se adciona nuevos campos

            temp.canal_comunicacion = (issue.CustomFields["Canal comunicacion"] != null ? issue.CustomFields["Canal comunicacion"].Values[0] : "");
            temp.quien_requiere_servicio = (issue.CustomFields["¿Quien requiere el servicio?"] != null ? issue.CustomFields["¿Quien requiere el servicio?"].Values[0] : "");
            //temp.operador_ma = (issue.CustomFields["Operador MA"] != null ? issue.CustomFields["Operador MA"].Values[0] : "");
            temp.codigo_plan_mantenimiento = (issue.CustomFields["Codigo plan de mantenimiento"] != null ? issue.CustomFields["Codigo plan de mantenimiento"].Values[0] : "");
            temp.descripcion_actividad_mantenimiento = (issue.CustomFields["Descripcion de la actividad de mantenimiento"] != null ? issue.CustomFields["Descripcion de la actividad de mantenimiento"].Values[0] : "");
            temp.tecnico_asignado = (issue.CustomFields["Tecnico Asignado"] != null ? issue.CustomFields["Tecnico Asignado"].Values[0] : "");
            temp.motivo_atraso = (issue.CustomFields["Motivo de atraso"] != null ? issue.CustomFields["Motivo de atraso"].Values[0] : "");
            temp.otro_motivo_atraso = (issue.CustomFields["Otro motivo de atraso"] != null ? issue.CustomFields["Otro motivo de atraso"].Values[0] : "");

            return temp;

        }
        public Ticket converIssueInTicketMTO(Issue issue, Dictionary<string, string> estacionMap)
        {
            Ticket temp = new Ticket();

            temp.id_ticket = issue.Key.Value;

            temp.id_estacion = (issue.CustomFields["Estacion"] != null ? issue.CustomFields["Estacion"].Values[0] : "");

            string estacionValue = (issue.CustomFields["Estacion"] != null ? issue.CustomFields["Estacion"].Values[0] : "");

            if (estacionMap.ContainsKey(estacionValue))
            {
                temp.nombre_estacion = estacionMap[estacionValue];
            }
            else
            {
                temp.nombre_estacion = "Estación sin configurar";
            }

            temp.id_vagon = (issue.CustomFields["Vagon"] != null ? issue.CustomFields["Vagon"].Values[0] : "");


            temp.tipoComponente = (issue.CustomFields["Tipo de componente"] != null ? issue.CustomFields["Tipo de componente"].Values[0] : "");


            temp.id_puerta = (temp.tipoComponente == "Puerta" && temp.tipoComponente != "null" && issue.CustomFields["Identificacion componente"] != null && issue.CustomFields["Identificacion componente"].Values[0] != null ? issue.CustomFields["Identificacion componente"].Values[0] : "");


            temp.id_componente = (issue.CustomFields["Identificacion componente"] != null ? issue.CustomFields["Identificacion componente"].Values[0] : "");


            temp.identificacion = (issue.CustomFields["Identificacion (serial)"] != null ? issue.CustomFields["Identificacion (serial)"].Values[0] : "");


            temp.tipo_mantenimiento = (issue.CustomFields["Tipo de servicio"] != null ? (issue.CustomFields["Tipo de servicio"].Values[0] == "Mantenimiento Preventivo" ? "Preventivo" : "Correctivo") : "");


            temp.nivel_falla = (issue.CustomFields["Clase de fallo"] != null ? issue.CustomFields["Clase de fallo"].Values[0] : "");


            temp.codigo_falla = (issue.CustomFields["Descripcion de fallo"] != null ? issue.CustomFields["Descripcion de fallo"].Values[0] : "");

            //if (jiraUrl == "https://assaabloymda.atlassian.net/")
            //{                
            //    //temp.fecha_apertura = (issue.Created != null ? DateTime.Parse(issue.CustomFields["Fecha de creacion"].Values[0]) : null);
            //}
            //else
            //{
            //    temp.fecha_apertura = issue.Created == null ? null : DateTime.Parse(issue.Created.Value.ToString());
            //}


            temp.fecha_apertura = issue.CustomFields["Fecha de creacion"] == null ? null : DateTime.Parse(issue.CustomFields["Fecha de creacion"].Values[0].ToString());

            temp.fecha_arribo_locacion = (issue.CustomFields["Fecha y Hora de Llegada a Estacion"] != null ? DateTime.Parse(issue.CustomFields["Fecha y Hora de Llegada a Estacion"].Values[0]) : null);

            temp.fecha_cierre = (issue.CustomFields["Fecha de solucion"] != null ? DateTime.Parse(issue.CustomFields["Fecha de solucion"].Values[0]) : null);

            temp.cantidad_repuesto_utilizado = (issue.CustomFields["Cantidad(es) repuesto(s) utilizado(s)"] != null ? issue.CustomFields["Cantidad(es) repuesto(s) utilizado(s)"].Values[0] : "");

            temp.componente_Parte = (issue.CustomFields["Descripcion de repuesto"] != null ? issue.CustomFields["Descripcion de repuesto"].Values[0] : "");


            temp.tipo_reparacion = (issue.CustomFields["Tipo de reparacion"] != null ? issue.CustomFields["Tipo de reparacion"].Values[0] : "");


            temp.tipo_ajuste_configuracion = (issue.CustomFields["Listado de ajustes ITS"] != null ? issue.CustomFields["Listado de ajustes ITS"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de configuracion ITS"] != null ? issue.CustomFields["Listado de configuracion ITS"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de ajustes Puerta"] != null ? issue.CustomFields["Listado de ajustes Puerta"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de configuracion Puerta"] != null ? issue.CustomFields["Listado de configuracion Puerta"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de ajustes RFID"] != null ? issue.CustomFields["Listado de ajustes RFID"].Values[0] + "\n" : "");


            temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de configuracion RFID"] != null ? issue.CustomFields["Listado de configuracion RFID"].Values[0] + "\n" : "");


            temp.descripcion_reparacion = (issue.CustomFields["Descripcion de la reparacion"] != null ? issue.CustomFields["Descripcion de la reparacion"].Values[0] : "");


            temp.diagnostico_causa = (issue.CustomFields["Diagnostico de la causa"] != null ? issue.CustomFields["Diagnostico de la causa"].Values[0] : "");


            temp.tipo_causa = (issue.CustomFields["Tipo de causa"] != null ? issue.CustomFields["Tipo de causa"].Values[0] : "");


            //temp.estado_ticket = issue.Status.Name;

            if (issue.Status.Name == "Cerrado" || issue.Status.Name == "DESCARTADO")
            {
                temp.estado_ticket = issue.Status.Name;
            }
            else
            {
                temp.estado_ticket = "Abierto";
            }
            if (issue.Status == null)
                temp.estado_ticket = (issue.Status != null ? issue.Status.Name : "");

            temp.descripcion = (issue.Description != null ? issue.Description : "");

            //Se adciona nuevos campos

            temp.canal_comunicacion = (issue.CustomFields["Canal comunicacion"] != null ? issue.CustomFields["Canal comunicacion"].Values[0] : "");
            temp.quien_requiere_servicio = (issue.CustomFields["¿Quien requiere el servicio?"] != null ? issue.CustomFields["¿Quien requiere el servicio?"].Values[0] : "");
            //temp.operador_ma = (issue.CustomFields["Operador MA"] != null ? issue.CustomFields["Operador MA"].Values[0] : "");
            temp.codigo_plan_mantenimiento = (issue.CustomFields["Codigo plan de mantenimiento"] != null ? issue.CustomFields["Codigo plan de mantenimiento"].Values[0] : "");
            temp.descripcion_actividad_mantenimiento = (issue.CustomFields["Descripcion de la actividad de mantenimiento"] != null ? issue.CustomFields["Descripcion de la actividad de mantenimiento"].Values[0] : "");
            temp.tecnico_asignado = (issue.CustomFields["Tecnico Asignado"] != null ? issue.CustomFields["Tecnico Asignado"].Values[0] : "");
            temp.motivo_atraso = (issue.CustomFields["Motivo de atraso"] != null ? issue.CustomFields["Motivo de atraso"].Values[0] : "");
            temp.otro_motivo_atraso = (issue.CustomFields["Otro motivo de atraso"] != null ? issue.CustomFields["Otro motivo de atraso"].Values[0] : "");

            return temp;

        }
        public IssueJira getIssueJira(string id)
        {
            var issue = jira.Issues.GetIssueAsync(id).Result;
            var attachments = issue.GetAttachmentsAsync().Result.FirstOrDefault();
            Console.WriteLine(attachments.Id);

            var tempFile = Path.GetTempFileName();
            return convertIssueInIssueJira(issue);

        }


        public Tuple<List<byte[]>, List<byte[]>> GetAttachmentAdjuntos(string id)
        {
            var issue = jira.Issues.GetIssueAsync(id).Result;
            var attachments = issue.GetAttachmentsAsync().Result;
            List<byte[]> imageList = new List<byte[]>();
            List<byte[]> videoList = new List<byte[]>();

            var imageExtensions = new List<string>
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff", // ... (add more as needed)
    };

            var videoExtensions = new List<string>
    {
        ".mp4", ".webm", ".avi", ".mov", // ... (add more as needed)
    };

            using (HttpClient client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                foreach (var attachment in attachments)
                {
                    string attachmentUrl = $"{jiraUrl}/rest/api/2/attachment/content/{attachment.Id}";
                    byte[] attachmentBytes = client.GetByteArrayAsync(attachmentUrl).Result;

                    if (IsExtensionSupported(attachment.FileName, imageExtensions))
                    {
                        imageList.Add(attachmentBytes);
                    }
                    else if (IsExtensionSupported(attachment.FileName, videoExtensions))
                    {
                        videoList.Add(attachmentBytes);
                    }
                }
            }
            return new Tuple<List<byte[]>, List<byte[]>>(imageList, videoList);
        }

        private bool IsExtensionSupported(string fileName, List<string> supportedExtensions)
        {
            string extension = Path.GetExtension(fileName)?.ToLower();
            return !string.IsNullOrEmpty(extension) && supportedExtensions.Contains(extension);
        }



        public List<byte[]> GetAttachmentImages(string id)
        {
            var issue = jira.Issues.GetIssueAsync(id).Result;
            var attachments = issue.GetAttachmentsAsync().Result;
            List<byte[]> imageList = new List<byte[]>();

            var imageExtensions = new List<string>
    {
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tiff", // ... (add more as needed)
    };

            using (HttpClient client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                foreach (var attachment in attachments)
                {
                    if (IsExtensionSupported(attachment.FileName, imageExtensions))
                    {
                        string imageUrl = $"{jiraUrl}/rest/api/2/attachment/content/{attachment.Id}";

                        byte[] imageBytes = client.GetByteArrayAsync(imageUrl).Result;
                        imageList.Add(imageBytes);
                    }
                }
            }

            return imageList;
        }

        public List<byte[]> GetAttachmentVideos(string id)
        {
            var issue = jira.Issues.GetIssueAsync(id).Result;
            var attachments = issue.GetAttachmentsAsync().Result;
            List<byte[]> videoList = new List<byte[]>();

            var videoExtensions = new List<string>
    {
        ".mp4", ".webm", ".avi", ".mov", // ... (add more as needed)
    };

            using (HttpClient client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                foreach (var attachment in attachments)
                {
                    if (IsExtensionSupported(attachment.FileName, videoExtensions))
                    {
                        string videoUrl = $"{jiraUrl}/rest/api/2/attachment/content/{attachment.Id}";

                        byte[] videoBytes = client.GetByteArrayAsync(videoUrl).Result;
                        videoList.Add(videoBytes);
                    }
                }
            }

            return videoList;
        }
        public IssueJira convertIssueInIssueJira(Issue issue)
        {
            IssueJira temp = new IssueJira();
            temp.Id = issue.Key.Value;
            temp.Archivos = (issue.GetAttachmentsAsync() != null ? issue.GetAttachmentsAsync().Result.First().DownloadData() : null);
            temp.FechaCreacion = (issue.CustomFields["Fecha de creacion"] != null ? DateTime.Parse(issue.CustomFields["Fecha de creacion"].Values[0]) : null);
            temp.Descripcion = (issue.Description != null ? issue.Description : "null");
            temp.Resumen = issue.Summary;
            temp.QuienRequiereServicio = (issue.CustomFields["¿Quién requiere el servicio?"] != null ? issue.CustomFields["¿Quién requiere el servicio?"].Values[0] : null);
            temp.CanalComunicacion = issue.CustomFields["Canal comunicacion"] != null ? issue.CustomFields["Canal comunicacion"].Values[0] : null;
            temp.IdentificacionComponente = issue.CustomFields["Identificacion componente"] != null ? issue.CustomFields["Identificacion componente"].Values[0] : null;
            temp.TipoComponente = issue.CustomFields["Tipo de componente"] != null ? issue.CustomFields["Tipo de componente"].Values[0] : null;
            temp.IdentificacionSerial = issue.CustomFields["Identificacion (serial)"] != null ? issue.CustomFields["Identificacion (serial)"].Values[0] : null;
            temp.OperadorMA = issue.CustomFields["Operador MA"] != null ? issue.CustomFields["Operador MA"].Values[0] : null;
            temp.TecnicoAsignado = issue.CustomFields["Tecnico Asignado"] != null ? issue.CustomFields["Tecnico Asignado"].Values[0] : null;
            temp.FechaHoraLlegadaEstacion = issue.CustomFields["Fecha y Hora de Llegada a Estacion"] != null ? DateTime.Parse(issue.CustomFields["Fecha y Hora de Llegada a Estacion"].Values[0]) : null;
            temp.TipoServicio = issue.CustomFields["Tipo de servicio"] != null ? issue.CustomFields["Tipo de servicio"].Values[0] : null;
            temp.ClaseFallo = issue.CustomFields["Clase de fallo"] != null ? issue.CustomFields["Clase de fallo"].Values[0] : null;
            temp.DescripcionFallo = issue.CustomFields["Descripcion de fallo"] != null ? issue.CustomFields["Descripcion de fallo"].Values[0] : null;
            temp.TipoCausa = issue.CustomFields["Tipo de causa"] != null ? issue.CustomFields["Tipo de causa"].Values[0] : null;
            temp.DiagnosticoCausa = issue.CustomFields["Diagnostico de la causa"] != null ? issue.CustomFields["Diagnostico de la causa"].Values[0] : null;
            temp.TipoReparacion = issue.CustomFields["Tipo de reparacion"] != null ? issue.CustomFields["Tipo de reparacion"].Values[0] : null;
            temp.DescripcionReparacion = issue.CustomFields["Descripcion de la reparacion"] != null ? issue.CustomFields["Descripcion de la reparacion"].Values[0] : null;
            temp.Estado = issue.Status.Name;
            temp.FechaSolucion = issue.CustomFields["Fecha de solucion"] != null ? DateTime.Parse(issue.CustomFields["Fecha de solucion"].Values[0]) : null;
            temp.Estacion = issue.CustomFields["Estacion"] != null ? issue.CustomFields["Estacion"].Values[0] : null;
            temp.Vagon = issue.CustomFields["Vagon"] != null ? issue.CustomFields["Vagon"].Values[0] : null;
            //temp.TiempoResolucionAnio = issue.CustomFields["Tiempo a resolucion ANIO"] != null ? issue.CustomFields["Tiempo a resolucion ANIO"].Values[0] : null;
            //temp.TiempoResolucionAIO = issue.CustomFields["Tiempo a resolución AIO"] != null ? issue.CustomFields["Tiempo a resolución AIO"].Values[0] : null;
            if (issue.CustomFields["Listado de ajustes puerta"] != null)
            {
                foreach (var lista in issue.CustomFields["Listado de ajustes puerta"].Values.ToList())
                {
                    temp.AjustesPuerta += lista + "\n";

                }
            }
            if (issue.CustomFields["Listado de configuracion puerta"] != null)
            {
                foreach (var lista in issue.CustomFields["Listado de configuracion puerta"].Values.ToList())
                {
                    temp.ConfiguracionPuerta += lista + "\n";

                }
            }
            if (issue.CustomFields["Listado de ajustes ITS"] != null)
            {
                foreach (var lista in issue.CustomFields["Listado de ajustes ITS"].Values.ToList())
                {
                    temp.AjustesITS += lista + "\n";

                }
            }
            if (issue.CustomFields["Listado de configuracion ITS"] != null)
            {
                foreach (var lista in issue.CustomFields["Listado de configuracion ITS"].Values.ToList())
                {
                    temp.ConfiguracionITS += lista + "\n";

                }
            }
            if (issue.CustomFields["Listado de ajustes RFID"] != null)
            {
                foreach (var lista in issue.CustomFields["Listado de ajsutes RFID"].Values.ToList())
                {
                    temp.AjustesRFID += lista + "\n";

                }
            }
            if (issue.CustomFields["Listado de configuracion RFID"] != null)
            {
                foreach (var lista in issue.CustomFields["Listado de configuracion RFID"].Values.ToList())
                {
                    temp.ConfiguracionRFID += lista + "\n";

                }
            }
            if (issue.CustomFields["Descripcion de repuesto"] != null)
            {
                foreach (var lista in issue.CustomFields["Descripcion de repuesto"].Values.ToList())
                {
                    temp.DescripcionRepuesto += lista + "\n";

                }
            }
            if (issue.CustomFields["Cantidad(es) repuesto(s) utilizado(s)"] != null)
            {
                foreach (var lista in issue.CustomFields["Cantidad(es) repuesto(s) utilizado(s)"].Values.ToList())
                {
                    temp.CantidadRepuestosUtilizados += lista + "\n";

                }
            }
            temp.CodigoPlanMantenimiento = issue.CustomFields["Codigo plan de mantenimiento"] != null ? issue.CustomFields["Codigo plan de mantenimiento"].Values[0] : null;
            if (issue.CustomFields["Descripcion de la actividad de mantenimiento"] != null)
            {
                foreach (var lista in issue.CustomFields["Descripcion de la actividad de mantenimiento"].Values.ToList())
                {
                    temp.DescripcionActividadMantenimiento += lista + "\n";

                }
            }
            temp.MotivoAtraso = issue.CustomFields["Motivo de atraso"] != null ? issue.CustomFields["Motivo de atraso"].Values[0] : null;
            temp.OtroMotivoAtraso = issue.CustomFields["otro motivo de atraso"] != null ? issue.CustomFields["otro motivo de atraso"].Values[0] : null;
            Console.WriteLine(temp.CantidadRepuestosUtilizados);
            return temp;
        }
        public DataTable getEstaciones()
        {

            return connector.GetEstaciones();

        }
    }
}