using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using MQTT.Infrastructure.DAL;
using MQTT.Infrastructure.Models.DTO;
using MQTT.Web.Models;
using System.Data;
using System.Linq;

using DashboarJira.Model;
using DashboarJira.Services;

namespace MQTT.Web.Controllers
{
    public class TicketsController : Controller
    {
        public IActionResult Index(int max, string componente)
        {
            //return View();

            // Obtener la fecha actual
            DateTime currentDateTime = DateTime.Now;

            // Restar un mes a la fecha actual
            DateTime startDateTime = currentDateTime.AddMonths(-1);

            // Formatear las fechas en el formato deseado
            string startDate = startDateTime.ToString("yyyy-MM-dd");
            string endDate = currentDateTime.ToString("yyyy-MM-dd");

            List<Ticket> tickets = getTickets(startDate, endDate, max, componente);
            return View(tickets);
        }


        int start = 0;
                
        public List<Ticket> getTickets(string startDate, string endDate, int max, string componente)
        {
            try
            {
                string formattedStartDate;
                string formattedEndDate;

                if(startDate!=null|| endDate != null)
                {
                    max = 10;
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }

   
    /*{
        [Authorize]
        public IActionResult Index()
        {
            //return View();
            List<Ticket> tickets = getTickets();
            return View(tickets);
        }

        int start = 0;
        int max = 100;
        string startDate = null;
        string endDate = null;
        string idComponente = null;
        public List<Ticket> getTickets()
        {
            try
            {
                JiraAccess jiraAccess = new JiraAccess();
                return jiraAccess.GetTikets(start, max, startDate, endDate, idComponente);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }*/
}