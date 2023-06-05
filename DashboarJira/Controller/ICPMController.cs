using DashboarJira.Model;
using DashboarJira.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Controller
{
    public class ICPMController
    {
        const string JQL_MTTO = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento'  AND 'Tipo de servicio' = 'Mantenimiento Preventivo' ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_PUERTAS = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Tipo de servicio' = 'Mantenimiento Preventivo' AND 'Tipo de componente' = Puerta AND 'Tipo de servicio' = 'Mantenimiento Preventivo' ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_ITTS = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Tipo de servicio' = 'Mantenimiento Preventivo' AND 'Tipo de componente' = 'Componente ITS' AND 'Tipo de servicio' = 'Mantenimiento Preventivo' ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_RFID = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Tipo de servicio' = 'Mantenimiento Preventivo' AND 'Tipo de componente' = 'Componente RFID' AND 'Tipo de servicio' = 'Mantenimiento Preventivo' ORDER BY key DESC, 'Time to resolution' ASC";
        JiraAccess jiraAccess;
        public ICPMController(JiraAccess jira)
        {
            jiraAccess = jira;
        }
        
        public ICPMEntity ICPM_MTTO(string start, string end) {
            string jql = string.Format(JQL_MTTO, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new ICPMEntity(total_tickets, ObtenerTICKETSCerrados(total_tickets));
        }
        public ICPMEntity ICPM_PUERTAS(string start, string end)
        {
            string jql = string.Format(JQL_PUERTAS, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new ICPMEntity(total_tickets, ObtenerTICKETSCerrados(total_tickets));
        }
        public ICPMEntity ICPM_ITTS(string start, string end)
        {
            string jql = string.Format(JQL_ITTS, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new ICPMEntity(total_tickets, ObtenerTICKETSCerrados(total_tickets));
        }
        public ICPMEntity ICPM_RFID(string start, string end)
        {
            string jql = string.Format(JQL_RFID, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new ICPMEntity(total_tickets, ObtenerTICKETSCerrados(total_tickets));
        }

        public List<Ticket> ObtenerTICKETSCerrados(List<Ticket> Ticket)
        {
            var ticketAPEGroup = Ticket.Where(ticket => ticket.estado_ticket!= null && ticket.estado_ticket != "null" && ticket.estado_ticket == "Cerrado"
             ).GroupBy(ticket => ticket);
            List<Ticket> Ticketc = new List<Ticket>();
            foreach (var group in ticketAPEGroup)
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
