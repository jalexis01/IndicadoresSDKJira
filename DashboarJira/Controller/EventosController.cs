using DashboarJira.Model;
using DashboarJira.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboarJira.Controller
{
    internal class EventosController
    {
        public RANOEntity RANOGeneral(string start, string end)
        {

            string jql = string.Format(JQL_GENERAL, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            List<Ticket> tickets_cerrados = ANIO_CERRADO_A_TIEMPO(total_tickets);
            return new RANOEntity(tickets_cerrados, total_tickets);

        }
    }
}
