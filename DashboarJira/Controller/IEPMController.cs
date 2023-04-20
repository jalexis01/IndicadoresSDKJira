using DashboarJira.Model;
using DashboarJira.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Controller
{
    public class IEPMController
    {
        // AND 'Tipo de servicio' in ('Falla ITS', 'Falla Puerta', 'Falla RFID', 'Mantenimiento Preventivo')
        const string JQL_GENERAL = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND status = Cerrado AND 'Tipo de servicio' in ('Falla ITS', 'Falla Puerta', 'Falla RFID', 'Mantenimiento Preventivo') ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento'AND status = Cerrado AND 'Tipo de servicio' in ('Falla ITS', 'Falla Puerta', 'Falla RFID', 'Mantenimiento Preventivo') AND 'Tipo de causa' = 'A cargo del contratista' ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_NO_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND status = Cerrado AND 'Tipo de servicio' in ('Falla ITS', 'Falla Puerta', 'Falla RFID', 'Mantenimiento Preventivo')  AND 'Tipo de causa' != 'A cargo del contratista' ORDER BY key DESC, 'Time to resolution' ASC";
        JiraAccess jiraAccess;

        public IEPMController()
        {
            jiraAccess = new JiraAccess();
        }

        public IEPMEntity IEPM_GENERAL(string start, string end)
        {
            string jql = string.Format(JQL_GENERAL, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new IEPMEntity(total_tickets, ObtenerTICKETSCerrados(total_tickets));
        }

        public IEPMEntity IEPM_CONTRATISTA(string start, string end)
        {
            string jql = string.Format(JQL_CONTRATISTA, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new IEPMEntity(total_tickets, ObtenerTICKETSCerrados(total_tickets));
        }

        public IEPMEntity IEPM_NO_CONTRATISTA(string start, string end)
        {
            string jql = string.Format(JQL_NO_CONTRATISTA, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new IEPMEntity(total_tickets, ObtenerTICKETSCerrados(total_tickets));
        }




        public List<Ticket> ObtenerTICKETSCerrados(List<Ticket> Ticket)
        {
            var ticketAPEGroup = Ticket.Where(ticket => ticket.estado_ticket != null && ticket.estado_ticket != "null" && ticket.estado_ticket == "Cerrado"
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
