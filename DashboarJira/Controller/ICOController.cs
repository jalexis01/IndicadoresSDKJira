using DashboarJira.Model;
using DashboarJira.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DashboarJira.Controller
{
    internal class ICOController
    {
        const string PETICIONEVP8 = "WHERE fechaHoraEnvioDato >= '{0}' AND fechaHoraEnvioDato <= '{1}' AND codigoEvento = 'EVP8' ORDER BY fechaHoraEnvioDato ASC";
        const string PETICIONEVP9 = "WHERE fechaHoraEnvioDato >= '{0}' AND fechaHoraEnvioDato <= '{1}' AND codigoEvento = 'EVP9' ORDER BY fechaHoraEnvioDato ASC";
        const string JQL_AIO = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND status = Cerrado AND 'Clase de fallo' = AIO AND 'Tipo de componente' = Puerta ORDER BY key DESC, 'Time to resolution' ASC";
        const string JQL_ANIO = "created >= {0} AND created <= {1} AND issuetype = 'Solicitud de Mantenimiento' AND status = Cerrado AND 'Clase de fallo' = ANIO AND 'Tipo de componente' = Puerta ORDER BY key DESC, 'Time to resolution' ASC";
        JiraAccess jira;
        DbConnector connector;
        public ICOController(JiraAccess jira, DbConnector connector)
        {
            this.jira = jira;
            this.connector = connector;
        }

        public List<EstacionEntity> calcularTTOP(List<JsonObject> estaciones, string startDate, string endDate)
        {
            string peticionEVP8 = string.Format(PETICIONEVP8, startDate, endDate);
            string peticionEVP9 = string.Format(PETICIONEVP9, startDate, endDate);
            List<EstacionEntity> ITTS_todas_estaciones = new List<EstacionEntity>();
            List<Evento> EVP8 = connector.GetEventos(peticionEVP8);
            List<Evento> EVP9 = connector.GetEventos(peticionEVP9);
            foreach (JsonObject estacion in estaciones)
            {
                List<Evento> evp8Estacion = EVP8.Where(e => e.idEstacion == estacion["idEstacion"].GetValue<string>()).ToList();
                List<Evento> evp9Estacion = EVP9.Where(e => e.idEstacion == estacion["idEstacion"].GetValue<string>()).ToList();
                int puertas = estacion["puertas"].GetValue<int>();
                ITTS_todas_estaciones.AddRange(calcularTTOPPorEstacion(evp8Estacion, evp9Estacion, puertas, startDate, endDate));

            }
            return ITTS_todas_estaciones;
        }

        public List<EstacionEntity> calcularTTOPPorEstacion(List<Evento> evp8Estacion, List<Evento> evp9Estacion, int cantidadPuertas, string startDate, string endDate)
        {
            List<EstacionEntity> ITTS_todas_estaciones = new List<EstacionEntity>();
            DateTime start = DateTime.Parse(startDate); // Fecha de inicio
            DateTime end = DateTime.Parse(endDate); // Fecha de fin

            List<DateTime> listaDias = new List<DateTime>();

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                listaDias.Add(date);
            }

            foreach (DateTime date in listaDias)
            {
                List<Evento> evp8PorDia = evp8Estacion.Where(e => e.fechaHoraLecturaDato.Value.Date == date.Date).ToList();
                List<Evento> evp9PorDia = evp9Estacion.Where(e => e.fechaHoraLecturaDato.Value.Date == date.AddDays(1).Date).ToList();
                EstacionEntity iTTSPorDia = new EstacionEntity(evp8PorDia, evp9PorDia, date, date.AddDays(1), cantidadPuertas);
                ITTS_todas_estaciones.Add(iTTSPorDia);
            }
            return ITTS_todas_estaciones;

        }


        public double tiempoTickets_AIO(string start, string end)
        {
            double result = 0;
            string jql = string.Format(JQL_AIO, start, end);
            List<Ticket> total_tickets = jira.GetTiketsIndicadores(jql);
            foreach (Ticket ticket in total_tickets)
            {
                if (ticket.fecha_apertura.HasValue && ticket.fecha_cierre.HasValue)
                {
                    result += (ticket.fecha_cierre.Value - ticket.fecha_apertura.Value).TotalHours;

                }
            }
            return result;
        }
        public double tiempoTickets_ANIO(string start, string end)
        {
            double result = 0;
            string jql = string.Format(JQL_ANIO, start, end);
            List<Ticket> total_tickets = jira.GetTiketsIndicadores(jql);
            foreach (Ticket ticket in total_tickets)
            {
                if (ticket.fecha_apertura.HasValue && ticket.fecha_cierre.HasValue)
                {
                    result += (ticket.fecha_cierre.Value - ticket.fecha_apertura.Value).TotalHours;

                }
            }
            return result;
        }

    }
}
