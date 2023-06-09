using DashboarJira.Model;
using DashboarJira.Services;

namespace DashboarJira.Controller
{
    public class IEPMController
    {
        const string JQL_GENERAL = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND status = Cerrado AND 'Falla externa' = EMPTY AND 'Tipo de componente' = Puerta AND 'Tipo de servicio' in ('Falla ITS', 'Falla Puerta', 'Falla RFID', 'Mantenimiento Preventivo') ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento'AND status = Cerrado AND 'Falla externa' = EMPTY AND 'Tipo de componente' = Puerta AND 'Tipo de servicio' in ('Falla ITS', 'Falla Puerta', 'Falla RFID', 'Mantenimiento Preventivo') AND 'Tipo de causa' = 'A cargo del contratista' ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_NO_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Falla externa' = EMPTY AND 'Tipo de componente' = Puerta AND status = Cerrado AND 'Tipo de servicio' in ('Falla ITS', 'Falla Puerta', 'Falla RFID', 'Mantenimiento Preventivo')  AND 'Tipo de causa' != 'A cargo del contratista' ORDER BY key DESC, 'Time to resolution' ASC";
        JiraAccess jiraAccess;

        public IEPMController(JiraAccess jira)
        {
            jiraAccess = jira;
        }

        public IEPMEntity IEPM_GENERAL(string start, string end)
        {
            string jql = string.Format(JQL_GENERAL, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new IEPMEntity(ObtenerANP(total_tickets), ObtenerTICKETSCerrados(total_tickets));
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
            var ticketAPEGroup = Ticket.Where(ticket => ticket.estado_ticket != null && ticket.estado_ticket != "null" && ticket.estado_ticket == "Cerrado" && ticket.tipo_mantenimiento == "Correctivo" || ticket.tipo_mantenimiento == "Preventivo"
             ).GroupBy(ticket => ticket);
            List<Ticket> Ticketc = new List<Ticket>();
            //Console.WriteLine("AME");
            foreach (var group in ticketAPEGroup)
            {
                foreach (var ticket in group)
                {
                    //Console.WriteLine(ticket.id_ticket);
                    Ticketc.Add(ticket);
                }
            }

            return Ticketc;

        }

        public List<Ticket> ObtenerANP(List<Ticket> Ticket)
        {
            var ticketAPEGroup = Ticket.Where(ticket => ticket.estado_ticket != null && ticket.estado_ticket != "null" && ticket.estado_ticket == "Cerrado" && ticket.tipo_mantenimiento == "Correctivo"
             ).GroupBy(ticket => ticket);
            List<Ticket> Ticketc = new List<Ticket>();
            //Console.WriteLine("ANP");
            foreach (var group in ticketAPEGroup)
            {
                foreach (var ticket in group)
                {
                    //Console.WriteLine(ticket.id_ticket);
                    Ticketc.Add(ticket);
                }
            }

            return Ticketc;

        }

    }
}
