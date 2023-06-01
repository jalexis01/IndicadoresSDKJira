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

        public JiraAccess()
        {
            jira = Jira.CreateRestClient(jiraUrl, username, password);
        }
        public List<Ticket> GetTikets(int start, int max, string startDate, string endDate, string idComponente)
        {
            try
            {
                //created >= 2023-04-04 AND created <= 2023-04-13 AND issuetype = "Solicitud de Mantenimiento" AND resolution = Unresolved AND "Clase de fallo" = AIO AND "Identificacion componente" ~ 9119-WA-OR-1 ORDER BY key DESC, "Time to resolution" ASC
                var jql = "project = 'Centro de Control' and issuetype = 'Solicitud de Mantenimiento'";
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

                return ConvertIssusInTickets(issues);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            return null;
        }
        public List<IssueJira> GetIssuesJira(int start, int max, string startDate, string endDate, string idComponente)
        {
            try
            {
                //created >= 2023-04-04 AND created <= 2023-04-13 AND issuetype = "Solicitud de Mantenimiento" AND resolution = Unresolved AND "Clase de fallo" = AIO AND "Identificacion componente" ~ 9119-WA-OR-1 ORDER BY key DESC, "Time to resolution" ASC
                var jql = "project = 'Centro de Control' and issuetype = 'Solicitud de Mantenimiento'";
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

                return ConvertIssusInIssuesJira(issues);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }
            return null;
        }

        public List<Ticket> GetTiketsIndicadores(string query)
        {
            var jql = query;
            //Console.WriteLine(jql);
            var issues = jira.Issues.GetIssuesFromJqlAsync(jql, int.MaxValue, 0);


            return ConvertIssusInTickets(issues);
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


        public List<Ticket> ConvertIssusInTickets(Task<IPagedQueryResult<Issue>> issues)
        {
            List<Ticket> result = new List<Ticket>();

            foreach (var issue in issues.Result)
            {
                //Console.WriteLine(issue.Key);

                result.Add(converIssueInTicket(issue));
            }
            return result;

        }

        public Ticket converIssueInTicket(Issue issue)
        {
            Ticket temp = new Ticket();
            temp.id_ticket = issue.Key.Value;

            temp.id_estacion = (issue.CustomFields["Estacion"] != null ? issue.CustomFields["Estacion"].Values[0] : "null");


            temp.id_vagon = (issue.CustomFields["Vagon"] != null ? issue.CustomFields["Vagon"].Values[0] : "null");


            temp.tipoComponente = (issue.CustomFields["Tipo de componente"] != null ? issue.CustomFields["Tipo de componente"].Values[0] : "null");


            temp.id_puerta = (temp.tipoComponente == "Puerta" && temp.tipoComponente != "null" && issue.CustomFields["Identificacion componente"] != null && issue.CustomFields["Identificacion componente"].Values[0] != null ? issue.CustomFields["Identificacion componente"].Values[0] : "null");


            temp.id_componente = (issue.CustomFields["Identificacion componente"] != null ? issue.CustomFields["Identificacion componente"].Values[0] : "null");


            temp.identificacion = (issue.CustomFields["Identificacion (serial)"] != null ? issue.CustomFields["Identificacion (serial)"].Values[0] : "null");


            temp.tipo_mantenimiento = (issue.CustomFields["Tipo de servicio"] != null ? (issue.CustomFields["Tipo de servicio"].Values[0] == "Mantenimiento Preventivo" ? "Preventivo" : "Correctivo") : "null");


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

            return temp;

        }

        public IssueJira getIssueJira(string id)
        {
            var issue = jira.Issues.GetIssueAsync(id).Result;

            return convertIssueInIssueJira(issue);

        }

        public IssueJira convertIssueInIssueJira(Issue issue)
        {
            IssueJira temp = new IssueJira();
            temp.Id = issue.Key.Value;
            temp.FechaCreacion = issue.Created;
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
            temp.TiempoResolucionAnio = issue.CustomFields["Tiempo a resolucion ANIO"] != null ? issue.CustomFields["Tiempo a resolucion ANIO"].Values[0] : null;
            temp.TiempoResolucionAIO = issue.CustomFields["Tiempo a resolución AIO"] != null ? issue.CustomFields["Tiempo a resolución AIO"].Values[0] : null;
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
            //Cantidad(es) repuesto(s) utilizado(s)
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