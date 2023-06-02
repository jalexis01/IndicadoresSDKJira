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
        public IActionResult Index()
        {
            // Obtiene la identidad del usuario actual
            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;

            // Verifica si el usuario tiene el rol de "Administrador"
            if (identity != null && identity.HasClaim(System.Security.Claims.ClaimTypes.Name, "admin@admin.com"))
            {
                ViewBag.Menu = "admin";
            }
            else
            {
                ViewBag.Menu = "user";
            }

            //return View();

            // Obtener la fecha actual
            DateTime currentDateTime = DateTime.Now;

            // Restar un mes a la fecha actual
            DateTime startDateTime = currentDateTime.AddMonths(-1);

            // Formatear las fechas en el formato deseado
            string startDate = startDateTime.ToString("yyyy-MM-dd");
            string endDate = currentDateTime.ToString("yyyy-MM-dd");

           //List<Ticket> tickets = getTickets(startDate, endDate, max, componente);

            Indicadores indicadores = new Indicadores();
            //Indicadores resultado = indicadores.indicadores("2023-01-01", "2023-06-01");            
            
            //dynamic resultados = indicadores.indicadores("2023-01-01", "2023-06-01");
            return View();
            //return View();
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
                //Indicadores indicadores = new Indicadores();
                //return indicadores.indicadores("2023-01-01", "2023-06-01");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }       
    }
}
