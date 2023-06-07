using Atlassian.Jira.Linq;
using DashboarJira.Model;
using DashboarJira.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Controller
{
    internal class IRFController
    {
        const string JQL_GENERAL = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND status = Cerrado AND 'Clase de fallo' in (ANIO, AIO) AND 'Tipo de componente' = Puerta ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Tipo de causa' = 'A cargo del contratista' AND status = Cerrado AND 'Clase de fallo' in (ANIO, AIO) AND 'Tipo de componente' = Puerta ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_NO_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Tipo de causa' != 'A cargo del contratista' AND status = Cerrado AND 'Clase de fallo' in (ANIO, AIO) AND 'Tipo de componente' = Puerta ORDER BY key DESC, 'Time to resolution' ASC";
        private const string ESTADO = "Cerrado";
        private const double TOTAL_PUERTAS = 146.0;
        private const string COMPONENTE = "Puerta";
        JiraAccess jiraAccess;
        public IRFController(JiraAccess jira)
        {
            jiraAccess = jira;
        }
        public IRFEntity IRFContratista(string start, string end) {

            string jql = string.Format(JQL_CONTRATISTA, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            
            Console.WriteLine(jql);
            IRFEntity irf = new IRFEntity(ContarFallasPorPuerta(total_tickets), TOTAL_PUERTAS, total_tickets);
            return irf;

        }

        public IRFEntity IRFNoContratista(string start, string end)
        {

            string jql = string.Format(JQL_NO_CONTRATISTA, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);

            Console.WriteLine(jql);
            IRFEntity irf = new IRFEntity(ContarFallasPorPuerta(total_tickets), TOTAL_PUERTAS, total_tickets);
            return irf;

        }
        public IRFEntity IRFGeneral(string start, string end)
        {

            string jql = string.Format(JQL_GENERAL, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);

            Console.WriteLine(jql);
            IRFEntity irf = new IRFEntity(ContarFallasPorPuerta(total_tickets), TOTAL_PUERTAS, total_tickets);
            return irf;

        }

        public List<ReporteFallasPorPuerta> ContarFallasPorPuerta(List<Ticket> tickets)
        {
            
            // Separar los tickets por puerta
            var ticketsPorPuerta = tickets
                .GroupBy(ticket => ticket.id_puerta);

            // Contar las fallas repetidas por cada una de las fallas en cada puerta cerrada
            var reportesFallasPorPuerta = new List<ReporteFallasPorPuerta>();
            foreach (var grupo in ticketsPorPuerta)
            {
                var fallasPorPuertaEnGrupo = grupo
                    .Where(ticket => ticket.codigo_falla != null 
                    )
                    .GroupBy(ticket => ticket.codigo_falla)
                    .Select(grupoFallas => new FallaPorPuerta
                    {
                        CodigoFalla = grupoFallas.Key,
                        Cantidad = grupoFallas.Count()
                    })
                    .ToList();

                var reporteFallasPorPuerta = new ReporteFallasPorPuerta
                {
                    Puerta = grupo.Key,
                    Fallas = fallasPorPuertaEnGrupo
                };

                reportesFallasPorPuerta.Add(reporteFallasPorPuerta);
            }

            return reportesFallasPorPuerta;
        }
    }
}
