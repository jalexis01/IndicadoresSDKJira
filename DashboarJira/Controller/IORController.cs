using DashboarJira.Model;
using DashboarJira.Services;
using System.Text.Json.Nodes;

namespace DashboarJira.Controller
{
    internal class IORController
    {
        const string PETICIONEVP8 = "WHERE fechaHoraEnvioDato >= '{0}' AND fechaHoraEnvioDato <= '{1}' AND codigoEvento = 'EVP8' ORDER BY fechaHoraEnvioDato ASC";
        const string PETICIONEVP9 = "WHERE fechaHoraEnvioDato >= '{0}' AND fechaHoraEnvioDato <= '{1}' AND codigoEvento = 'EVP9' ORDER BY fechaHoraEnvioDato ASC";
        DbConnector connector;
        public IORController( DbConnector connector)
        {
            this.connector = connector;
        }

        public List<TiempoTotalOperacion> calcularIOR(List<JsonObject> estaciones, string startDate, string endDate)
        {
            string peticionEVP8 = string.Format(PETICIONEVP8, startDate, endDate);
            string peticionEVP9 = string.Format(PETICIONEVP9, startDate, endDate);
            DateTime start = DateTime.Parse(startDate); // Fecha de inicio
            DateTime end = DateTime.Parse(endDate);
            List<TiempoTotalOperacion> ITTS_todas_estaciones = new List<TiempoTotalOperacion>();
            List<Evento> EVP8 = connector.GetEventos(peticionEVP8);
            List<Evento> EVP9 = connector.GetEventos(peticionEVP9);
            foreach (JsonObject estacion in estaciones)
            {
                List<Evento> evp8Estacion = EVP8.Where(e => e.idEstacion == estacion["idEstacion"].GetValue<string>()).ToList();
                List<Evento> evp9Estacion = EVP9.Where(e => e.idEstacion == estacion["idEstacion"].GetValue<string>()).ToList();
                
                ITTS_todas_estaciones.Add(new TiempoTotalOperacion(evp8Estacion, evp9Estacion,start,end));

            }
            return ITTS_todas_estaciones;
        }
    }
}
