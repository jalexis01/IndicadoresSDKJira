using Atlassian.Jira;
using DashboarJira.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Services
{
    public class JiraAccess
    {
        string jiraUrl = "https://manateecc.atlassian.net/";
        string username = "desarrollocc@manateeingenieria.com";
        string password = "ATATT3xFfGF0ZRHIEZTEJVRnhNKviH0CGed6QXqCDMj5bCmKSEbO00UUjHUb3yDcaA4YD1SHohyDr4qnwRx2x4Tu_S_QW_xlGIcIUDvL7CFKEg47_Jcy4Dmq6YzO0dvqB3qeT-EVWfwJ2jJ-9vEUfsqXavD0IIGA7DAZHGCtIWhxgwKIbAWsmeA=038B810D";

        Jira jira;

        public JiraAccess() {
            jira = Jira.CreateRestClient(jiraUrl, username, password);
        }
        public List<Ticket> GetTikets(int start, int max, string startDate, string endDate, string idComponente)
        {
            //created >= 2023-04-04 AND created <= 2023-04-13 AND issuetype = "Solicitud de Mantenimiento" AND resolution = Unresolved AND "Clase de fallo" = AIO AND "Identificacion componente" ~ 9119-WA-OR-1 ORDER BY key DESC, "Time to resolution" ASC
            var jql = "issuetype = 'Solicitud de Mantenimiento'";
            if (startDate != null && endDate != null) {
                jql += " AND " +"created >= " + startDate + " AND " + "created <= " + endDate ;
            }
            if (idComponente != null) {

                jql += " AND " + "'Identificacion componente' ~ " + idComponente;
            }
            jql += " ORDER BY key DESC, 'Time to resolution' ASC";
            Task<IPagedQueryResult<Issue>> issues = null;
            if (max != null && start != null)
            {
                issues = jira.Issues.GetIssuesFromJqlAsync(jql, max, start);
            }
            else if (max == null && start == null) {
                issues = jira.Issues.GetIssuesFromJqlAsync(jql);
            }

                return ConvertIssusInTickets(issues);
        }

        public List<Ticket> GetTiketsIndicadores(string query)
        {
            var jql = query;
            Console.WriteLine(jql);
            var issues =  jira.Issues.GetIssuesFromJqlAsync(jql);
            

            return ConvertIssusInTickets(issues);
        }

        public List<byte[]> getTicket() {
            var ticket = jira.Issues.GetIssueAsync("TICKET-92");
            var attachments = ticket.Result.GetAttachmentsAsync();
            List<byte[]> imagenes = new List<byte[]>();
            Console.WriteLine(attachments.Result.ToList().Count);
            foreach (var attachment in attachments.Result)
            {
                attachment.Download(attachment.FileName);
                Console.WriteLine(attachment.FileName);
                var imagen = attachment.DownloadData;
                imagenes.Add(imagen.Invoke());
                Console.WriteLine(imagen.ToString);
            }
            return imagenes;
        }



        public List<Ticket> ConvertIssusInTickets(Task<IPagedQueryResult<Issue>> issues) {
            List<Ticket> result = new List<Ticket>();

            foreach (var issue in issues.Result)
            {
                Ticket temp = new Ticket();
                temp.id_ticket = issue.Key.Value;

                temp.id_estacion = (issue.CustomFields["Estacion"] != null ? issue.CustomFields["Estacion"].Values[0] : "null");
                

                temp.id_vagon = (issue.CustomFields["Vagon"] != null ? issue.CustomFields["Vagon"].Values[0] : "null");


                temp.tipoComponente = (issue.CustomFields["Tipo de componente"] != null ? issue.CustomFields["Tipo de componente"].Values[0] : "null");


                temp.id_puerta = ( temp.tipoComponente == "Puerta" && temp.tipoComponente != "null" && issue.CustomFields["Identificacion componente"]!= null && issue.CustomFields["Identificacion componente"].Values[0] !=null   ? issue.CustomFields["Identificacion componente"].Values[0] : "null");
                
                
                temp.id_componente = (issue.CustomFields["Identificacion componente"] != null ? issue.CustomFields["Identificacion componente"].Values[0] : "null");
                
                
                temp.identificacion = (issue.CustomFields["Identificacion (serial)"] != null ? issue.CustomFields["Identificacion (serial)"].Values[0] : "null");
                
                
                temp.tipo_mantenimiento = (issue.CustomFields["Tipo de servicio"] != null ? (issue.CustomFields["Tipo de servicio"].Values[0]  == "Mantenimiento Preventivo" ? "Preventivo" : "Correctivo") : "null");
                
                
                temp.nivel_falla = (issue.CustomFields["Clase de fallo"] != null ? issue.CustomFields["Clase de fallo"].Values[0] : "null");
                
                
                temp.codigo_falla = (issue.CustomFields["Descripcion de fallo"] != null ? issue.CustomFields["Descripcion de fallo"].Values[0] : "null");
                
                
                temp.fecha_apertura = (issue.Created != null ? issue.Created.Value : null);
                
                
                temp.fecha_arribo_locacion = (issue.CustomFields["Fecha y Hora de Llegada a Estacion"] != null ? DateTime.Parse(issue.CustomFields["Fecha y Hora de Llegada a Estacion"].Values[0]) : null);
                
                
                temp.fecha_cierre = (issue.CustomFields["Fecha de solucion"] != null ? DateTime.Parse(issue.CustomFields["Fecha de solucion"].Values[0]) : null);
                
                
                temp.componente_Parte = (issue.CustomFields["Descripcion de repuesto"] != null ? issue.CustomFields["Descripcion de repuesto"].Values[0] : " ");
                
                
                temp.tipo_reparacion = (issue.CustomFields["Tipo de reparacion"] != null ? issue.CustomFields["Tipo de reparacion"].Values[0] : " ");
                
                
                temp.tipo_ajuste_configuracion = (issue.CustomFields["Listado de ajustes ITS"] != null ? issue.CustomFields["Listado de ajustes ITS"].Values[0] + "\n" : "");
                
                
                temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de configuracion ITS"] != null ? issue.CustomFields["Listado de configuracion ITS"].Values[0] + "\n" : "");
                
                
                temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de ajustes Puerta"] != null ? issue.CustomFields["Listado de ajustes Puerta"].Values[0] + "\n" : "");
                
                
                temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de configuracion Puerta"] != null ? issue.CustomFields["Listado de configuracion Puerta"].Values[0] + "\n" : "");
                
                
                temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de ajustes RFID"] != null ? issue.CustomFields["Listado de ajustes RFID"].Values[0] + "\n" : "");
                
                
                temp.tipo_ajuste_configuracion += (issue.CustomFields["Listado de configuracion RFID"] != null ? issue.CustomFields["Listado de configuracion RFID"].Values[0] + "\n" : "");
                
                
                temp.descripcion_reparacion = (issue.CustomFields["Descripcion de la reparacion"] != null ? issue.CustomFields["Descripcion de la reparacion"].Values[0] : "null");
                
                
                temp.diagnostico_causa = (issue.CustomFields["Tipo de causa"] != null ? issue.CustomFields["Tipo de causa"].Values[0] : "null");
                
                
                temp.tipo_causa = (issue.CustomFields["Tipo de causa"] != null ? issue.CustomFields["Tipo de causa"].Values[0] : "null");
                
                
                temp.estado_ticket = (issue.Status != null ? issue.Status.Name : "null");
                
                
                Console.WriteLine(issue.Key);

                result.Add(temp);
            }
            return result;

        }
    }
}
