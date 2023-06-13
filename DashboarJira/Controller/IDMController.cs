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
    public class IDMController
    {
        const string PETICIONEVP10 = "WHERE fechaHoraEnvioDato >= '{0}' AND fechaHoraEnvioDato <= '{1}' AND codigoEvento = 'EVP10' ORDER BY fechaHoraEnvioDato ASC";
        const string PETICIONEVP11 = "WHERE fechaHoraEnvioDato >= '{0}' AND fechaHoraEnvioDato <= '{1}' AND codigoEvento = 'EVP11' ORDER BY fechaHoraEnvioDato ASC";
        const string PETICIONEVP14 = "WHERE fechaHoraEnvioDato >= '{0}' AND fechaHoraEnvioDato <= '{1}' AND codigoEvento = 'EVP14' ORDER BY fechaHoraEnvioDato ASC";
        DbConnector connector;
        public IDMController(DbConnector connector)
        {
            this.connector = connector;
        }

        public List<EstacionEntity> calcularIDM(List<JsonObject> estaciones, string startDate, string endDate)
        {
            string peticionEVP10 = string.Format(PETICIONEVP10, startDate, endDate);
            string peticionEVP11 = string.Format(PETICIONEVP11, startDate, endDate);
            string peticionEVP14 = string.Format(PETICIONEVP14, startDate, endDate);
            DateTime start = DateTime.Parse(startDate); // Fecha de inicio
            DateTime end = DateTime.Parse(endDate);
            List<EstacionEntity> ITTS_todas_estaciones = new List<EstacionEntity>();
            List<Evento> EVP10 = connector.GetEventos(peticionEVP10);
            List<Evento> EVP11 = connector.GetEventos(peticionEVP11);
            List<Evento> EVP14 = connector.GetEventos(peticionEVP14);
            foreach (JsonObject estacion in estaciones)
            {
                List<Evento> evp10Estacion = EVP10.Where(e => e.idEstacion == estacion["idEstacion"].GetValue<string>()).ToList();
                List<Evento> evp11Estacion = EVP11.Where(e => e.idEstacion == estacion["idEstacion"].GetValue<string>()).ToList();
                List<Evento> evp14Estacion = EVP14.Where(e => e.idEstacion == estacion["idEstacion"].GetValue<string>()).ToList();

                ITTS_todas_estaciones.Add(new EstacionEntity(evp10Estacion, evp11Estacion, evp14Estacion, start, end));

            }
            return ITTS_todas_estaciones;
        }
    }
}
