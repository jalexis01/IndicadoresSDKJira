using Atlassian.Jira;
using DashboarJira.Model;
using System.Text;
using System.Linq;

namespace DashboarJira.Services
{
    public class JiraAccess
    {
        //string jiraUrl = "https://manateecc.atlassian.net/";
        //string username = "desarrollocc@manateeingenieria.com";
        //string password = "ATATT3xFfGF0ZRHIEZTEJVRnhNKviH0CGed6QXqCDMj5bCmKSEbO00UUjHUb3yDcaA4YD1SHohyDr4qnwRx2x4Tu_S_QW_xlGIcIUDvL7CFKEg47_Jcy4Dmq6YzO0dvqB3qeT-EVWfwJ2jJ-9vEUfsqXavD0IIGA7DAZHGCtIWhxgwKIbAWsmeA=038B810D";
        string jiraUrl = "https://assaabloymda.atlassian.net/";
        string username = "desarrollocc@manateeingenieria.com";
        string password = "ATATT3xFfGF0ZRHIEZTEJVRnhNKviH0CGed6QXqCDMj5bCmKSEbO00UUjHUb3yDcaA4YD1SHohyDr4qnwRx2x4Tu_S_QW_xlGIcIUDvL7CFKEg47_Jcy4Dmq6YzO0dvqB3qeT-EVWfwJ2jJ-9vEUfsqXavD0IIGA7DAZHGCtIWhxgwKIbAWsmeA=038B810D";

        Jira jira;

        public JiraAccess()
        {
            jira = Jira.CreateRestClient(jiraUrl, username, password);
        }

        /*TODO*/
        public List<Ticket> GetTikets(int start, int max, string startDate, string endDate, string idComponente)
        {
            try
            {
                var jql = "";
                //created >= 2023-04-04 AND created <= 2023-04-13 AND issuetype = "Solicitud de Mantenimiento" AND resolution = Unresolved AND "Clase de fallo" = AIO AND "Identificacion componente" ~ 9119-WA-OR-1 ORDER BY key DESC, "Time to resolution" ASC
                if (jiraUrl == "https://assaabloymda.atlassian.net/")
                {
                    jql = "(project = 'Mesa de Ayuda' OR project = 'Mtto Preventivo') and issuetype = 'Solicitud de Mantenimiento'";
                }
                else
                {
                    jql = "project = 'Centro de Control' and issuetype = 'Solicitud de Mantenimiento'";
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
                    result = result.Concat(GetTikets(start + max, max, startDate, endDate, idComponente)).ToList();
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

            return converIssueInTicket(ticket);
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

            List<Ticket> result = new List<Ticket>();
            foreach (var issue in issues.Result)
            {

                result.Add(converIssueInTicket(issue));
            }
            return result;

        }

        public Ticket converIssueInTicket(Issue issue)
        {
            Ticket temp = new Ticket();
            temp.id_ticket = issue.Key.Value;

            temp.id_estacion = (issue.CustomFields["Estacion"] != null ? issue.CustomFields["Estacion"].Values[0] : "");


            temp.id_vagon = (issue.CustomFields["Vagon"] != null ? issue.CustomFields["Vagon"].Values[0] : "");


            temp.tipoComponente = (issue.CustomFields["Tipo de componente"] != null ? issue.CustomFields["Tipo de componente"].Values[0] : "");


            temp.id_puerta = (temp.tipoComponente == "Puerta" && temp.tipoComponente != "null" && issue.CustomFields["Identificacion componente"] != null && issue.CustomFields["Identificacion componente"].Values[0] != null ? issue.CustomFields["Identificacion componente"].Values[0] : "");


            temp.id_componente = (issue.CustomFields["Identificacion componente"] != null ? issue.CustomFields["Identificacion componente"].Values[0] : "");


            temp.identificacion = (issue.CustomFields["Identificacion (serial)"] != null ? issue.CustomFields["Identificacion (serial)"].Values[0] : "");


            temp.tipo_mantenimiento = (issue.CustomFields["Tipo de servicio"] != null ? (issue.CustomFields["Tipo de servicio"].Values[0] == "Mantenimiento Preventivo" ? "Preventivo" : "Correctivo") : "");


            temp.nivel_falla = (issue.CustomFields["Clase de fallo"] != null ? issue.CustomFields["Clase de fallo"].Values[0] : "");


            temp.codigo_falla = (issue.CustomFields["Descripcion de fallo"] != null ? issue.CustomFields["Descripcion de fallo"].Values[0] : "");


            temp.fecha_apertura = (issue.CustomFields["Fecha de creacion"] != null ? DateTime.Parse(issue.CustomFields["Fecha de creacion"].Values[0]) : null); ;


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
            temp.codigo_plan_mantenimiento = (issue.CustomFields["Codigo plan de MTTO"] != null ? issue.CustomFields["Codigo plan de MTTO"].Values[0] : "");
            temp.descripcion_actividad_mantenimiento = (issue.CustomFields["Descripcion de la actividad de MTTO"] != null ? issue.CustomFields["Descripcion de la actividad de MTTO"].Values[0] : "");
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
        /*
        public List<byte[]> GetAttachmentImages(string id)
        {
            var issue = jira.Issues.GetIssueAsync(id).Result;
            var attachments = issue.GetAttachmentsAsync().Result;
            List<byte[]> imageList = new List<byte[]>();

            using (HttpClient client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                foreach (var attachment in attachments)
                {
                    string imageUrl = $"{jiraUrl}/rest/api/2/attachment/content/{attachment.Id}";

                    byte[] imageBytes = client.GetByteArrayAsync(imageUrl).Result;
                    imageList.Add(imageBytes);
                }
            }

            return imageList;
        }
        */
        public List<byte[]> GetAttachmentImages(string id)
        {
            var issue = jira.Issues.GetIssueAsync(id).Result;
            var attachments = issue.GetAttachmentsAsync().Result;
            List<byte[]> imageList = new List<byte[]>();

            using (HttpClient client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                foreach (var attachment in attachments)
                {
                    if (attachment.MimeType == "image/jpeg" ||
                        attachment.MimeType == "image/png" ||
                        attachment.MimeType == "image/gif" ||
                        attachment.MimeType == "image/bmp" ||
                        attachment.MimeType == "image/webp" ||
                        attachment.MimeType == "image/tiff" ||
                        attachment.MimeType == "image/x-icon" ||
                        attachment.MimeType == "image/svg+xml" ||
                        attachment.MimeType == "image/apng" ||
                        attachment.MimeType == "image/avif" ||
                        attachment.MimeType == "image/cgm" ||
                        attachment.MimeType == "image/ief" ||
                        attachment.MimeType == "image/jp2" ||
                        attachment.MimeType == "image/jpm" ||
                        attachment.MimeType == "image/jpx" ||
                        attachment.MimeType == "image/ktx" ||
                        attachment.MimeType == "image/ovtf" ||
                        attachment.MimeType == "image/vnd.adobe.photoshop" ||
                        attachment.MimeType == "image/vnd.microsoft.icon" ||
                        attachment.MimeType == "image/vnd.radiance" ||
                        attachment.MimeType == "image/x-cmu-raster" ||
                        attachment.MimeType == "image/x-portable-anymap" ||
                        attachment.MimeType == "image/x-portable-bitmap" ||
                        attachment.MimeType == "image/x-portable-graymap" ||
                        attachment.MimeType == "image/x-portable-pixmap" ||
                        attachment.MimeType == "image/x-rgb" ||
                        attachment.MimeType == "image/x-xbitmap" ||
                        attachment.MimeType == "image/x-xpixmap" ||
                        attachment.MimeType == "image/x-xwindowdump")
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

            using (HttpClient client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                foreach (var attachment in attachments)
                {
                    // Check if the MIME type is one of the desired video types
                    if (attachment.MimeType == "video/mp4" ||
                       attachment.MimeType == "video/webm" ||
                       attachment.MimeType == "video/avi" ||
                       attachment.MimeType == "video/quicktime" ||  // For MOV files
                       attachment.MimeType == "video/mpeg" ||
                       attachment.MimeType == "video/x-mpeg" ||
                       attachment.MimeType == "video/x-mpeg2a" ||    // For MPG/MPEG files
                       attachment.MimeType == "video/3gpp" ||       // For 3GP files
                       attachment.MimeType == "video/x-flv" ||      // For FLV files
                       attachment.MimeType == "video/x-ms-wmv" ||   // For WMV files
                       attachment.MimeType == "video/x-msvideo" ||  // For AVI files
                       attachment.MimeType == "video/x-matroska" || // For MKV files
                       attachment.MimeType == "video/x-theora" ||   // For OGV files
                       attachment.MimeType == "video/vnd.rn-realvideo" || // For RMVB files
                       attachment.MimeType == "video/ogg" ||           // For OGG files
                       attachment.MimeType == "video/x-ms-asf" ||     // For ASF files
                       attachment.MimeType == "video/3gpp2" ||        // For 3G2 files
                       attachment.MimeType == "video/vnd.dvb.file" || // For DVB files
                       attachment.MimeType == "video/x-f4v" ||        // For F4V files
                       attachment.MimeType == "video/x-fli" ||        // For FLI files
                       attachment.MimeType == "video/x-flv" ||        // For FLV files
                       attachment.MimeType == "video/x-m4v" ||        // For M4V files
                       attachment.MimeType == "video/x-ms-vob" ||     // For VOB files
                       attachment.MimeType == "video/x-ms-wm" ||      // For WM files
                       attachment.MimeType == "video/x-ms-wmv" ||     // For WMV files
                       attachment.MimeType == "video/x-ms-wmx" ||     // For WMX files
                       attachment.MimeType == "video/x-ms-wvx" ||     // For WVX files
                       attachment.MimeType == "video/x-msvideo" ||    // For AVI files
                       attachment.MimeType == "video/x-sgi-movie" ||  // For Movie files
                       attachment.MimeType == "video/x-smv" ||        // For SMV files
                       attachment.MimeType == "video/x-flv")          // For FLV files
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
    }
}