using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace MQTT.Web.Controllers
{
    [Authorize]
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<IndicadoresEntity> getIndicadores(string startDate, string endDate)
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

                Indicadores indicadores = new Indicadores();

                return indicadores.ObtenerIndicadores(formattedStartDate, formattedEndDate);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

}