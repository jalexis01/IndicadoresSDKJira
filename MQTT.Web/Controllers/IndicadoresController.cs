using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using DashboarJira.Model;
using DashboarJira.Services;
using DashboarJira.Controller;

namespace MQTT.Web.Controllers
{
    public class IndicadoresController : Controller
    {
        public IActionResult Index(int max, string componente)
        {
            Indicadores indicadores = ObtenerIndicadores(); // Llama al método que obtiene los indicadores
            return View(indicadores);
        }


        int start = 0;

        public List<Ticket> getTickets(string startDate, string endDate, int max, string componente)
        {
            try
            {
                string formattedStartDate;
                string formattedEndDate;

                if (startDate != null || endDate != null)
                {
                    //max = 10;
                    DateTime startDateTime = DateTime.Parse(startDate);
                    DateTime endDateTime = DateTime.Parse(endDate).AddDays(1).AddSeconds(-1); //agrega 1 día y resta 1 segundo para obtener el final del día

                    formattedStartDate = startDateTime.ToString("yyyy-MM-dd");
                    formattedEndDate = endDateTime.ToString("yyyy-MM-dd");
                }
                else
                {
                    formattedStartDate = startDate;
                    formattedEndDate = endDate;
                }
                JiraAccess jiraAccess = new JiraAccess();
                return jiraAccess.GetTikets(start, max, formattedStartDate, formattedEndDate, componente);
                Indicadores indicadores = new Indicadores();
                return indicadores.indicadores("2023-01-01", "2023-06-01");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Indicadores ObtenerIndicadores()
        {
            Indicadores indicadores = new Indicadores();
            return indicadores.indicadores("2023-01-01", "2023-06-01");
        }

        public IActionResult consultarIndicadores()
        {
            try
            {
                Indicadores indicadores = new Indicadores();
                Console.WriteLine(indicadores.indicadores("2023-01-01", "2023-06-01"));

                return Ok(indicadores);
            }
            catch (Exception ex)
            {
                // Manejar el error de alguna forma si lo deseas
                throw ex;
            }
        }
    }
}
