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
    public class RANOController
    {
        const string JQL_GENERAL = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Clase de fallo' = ANIO ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Clase de fallo' = ANIO AND 'Tipo de causa' = 'A cargo del contratista' ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_NO_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Clase de fallo' = ANIO AND 'Tipo de causa' != 'A cargo del contratista' ORDER BY key DESC, 'Time to resolution' ASC";
        public const int HORAS_MAXIMAS_A_TIEMPO = 24;
        JiraAccess jiraAccess;
        public RANOController() {
            jiraAccess = new JiraAccess();
        }
        public RAIOEntity RAIOContratista(string start, string end) {

            string jql = string.Format(JQL_CONTRATISTA, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            List<Ticket> tickets_cerrados = ANIO_CERRADO_A_TIEMPO(total_tickets);
            //Console.WriteLine(jql);
            return new RAIOEntity(tickets_cerrados, total_tickets);
        
        }

        public RAIOEntity RAIONoContratista(string start, string end)
        {

            string jql = string.Format(JQL_NO_CONTRATISTA, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            List<Ticket> tickets_cerrados = ANIO_CERRADO_A_TIEMPO(total_tickets);
            //Console.WriteLine(jql);
            return new RAIOEntity(tickets_cerrados, total_tickets);

        }
        public RAIOEntity RAIOGeneral(string start, string end)
        {

            string jql = string.Format(JQL_GENERAL, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            List<Ticket> tickets_cerrados = ANIO_CERRADO_A_TIEMPO(total_tickets);
            return new RAIOEntity(tickets_cerrados, total_tickets);

        }

        public List<Ticket> ANIO_CERRADO_A_TIEMPO(List<Ticket> Ticket)
        {
            var ticketGroups = Ticket.Where(ticket => ticket.fecha_apertura != null &&
                ((ticket.fecha_cierre != null && (ticket.fecha_cierre.Value - ticket.fecha_apertura.Value).TotalHours <= HORAS_MAXIMAS_A_TIEMPO) ||
                (ticket.fecha_cierre == null && (DateTime.Now - ticket.fecha_apertura.Value).TotalHours <= HORAS_MAXIMAS_A_TIEMPO)) &&
                !ticket.estado_ticket.Equals("null") &&
                ticket.estado_ticket.Equals("Cerrado"))
                .GroupBy(x => x).ToList();
            List<Ticket> Ticketc = new List<Ticket>();
            foreach (var group in ticketGroups)
            {
                foreach (var ticket in group)
                {

                    Ticketc.Add(ticket);

                }
            }


            return Ticketc;
        }
    }
}
