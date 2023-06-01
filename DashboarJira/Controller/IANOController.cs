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
    public class IANOController
    {
        private readonly double TOTAL_PUERTAS = 146.0;
        const string JQL_GENERAL = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND status = Cerrado AND 'Clase de fallo' = ANIO AND 'Tipo de componente' = Puerta ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Tipo de causa' = 'A cargo del contratista' AND status = Cerrado AND 'Clase de fallo' = ANIO AND 'Tipo de componente' = Puerta ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_NO_CONTRATISTA = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND 'Tipo de causa' != 'A cargo del contratista' AND status = Cerrado AND 'Clase de fallo' = ANIO AND 'Tipo de componente' = Puerta ORDER BY key DESC, 'Time to resolution' ASC";
        JiraAccess jiraAccess;
        public IANOController()
        {
            jiraAccess = new JiraAccess();
        }
        
        public IANOEntity IANOGeneral(string start, string end) {
            string jql = string.Format(JQL_GENERAL, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new IANOEntity(ANIO_POR_PUERTA(total_tickets),TOTAL_PUERTAS);
        }
        public IANOEntity IANOContratista(string start, string end)
        {
            string jql = string.Format(JQL_CONTRATISTA, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new IANOEntity(ANIO_POR_PUERTA(total_tickets), TOTAL_PUERTAS);
        }

        public IANOEntity IANO_NO_Contratista(string start, string end)
        {
            string jql = string.Format(JQL_NO_CONTRATISTA, start, end);
            List<Ticket> total_tickets = jiraAccess.GetTiketsIndicadores(jql);
            return new IANOEntity(ANIO_POR_PUERTA(total_tickets), TOTAL_PUERTAS);
        }

        public List<List<Ticket>> ANIO_POR_PUERTA(List<Ticket> Ticket)
        {
            List<List<Ticket>> gruposPuertasAIO = new List<List<Ticket>>();
            List<Ticket> auxiliar = new List<Ticket>();
            var ticketANIOPuertaGroup = Ticket.GroupBy(ticket => ticket.id_puerta);


            foreach (var group in ticketANIOPuertaGroup)
            {

                foreach (var ticket in group)
                {
                    auxiliar.Add(ticket);
                }
                gruposPuertasAIO.Add(auxiliar);
            }
            return gruposPuertasAIO;
        }
    }
}
