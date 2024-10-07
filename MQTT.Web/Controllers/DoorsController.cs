using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace MQTT.Web.Controllers
{
    public class DoorsController : Controller
    {
        private DbConnector dbConnector;
        public DoorsController()
        {

            init();
        }
        public ActionResult Index()
        {

            var estaciones = dbConnector.GetEstacionesV();
            foreach (var estacion in estaciones)
            {
                estacion.Componentes = dbConnector.GetPuertasByEstacionId(estacion.idEstacion);
            }
            return View(estaciones);
        }

        public void init()
        {
            string url = "https://manateecc.atlassian.net/";
            string connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDB;User Id=administrador;Password=2022/M4n4t334zur3";
            string user = "desarrollocc@manateeingenieria.com";
            string token = "ATATT3xFfGF0ZRHIEZTEJVRnhNKviH0CGed6QXqCDMj5bCmKSEbO00UUjHUb3yDcaA4YD1SHohyDr4qnwRx2x4Tu_S_QW_xlGIcIUDvL7CFKEg47_Jcy4Dmq6YzO0dvqB3qeT-EVWfwJ2jJ-9vEUfsqXavD0IIGA7DAZHGCtIWhxgwKIbAWsmeA=038B810D";


            //string url = "https://assaabloymda.atlassian.net/";
            //string connectionString = "Server=manatee.database.windows.net;Database=PuertasTransmilenioDBAssaabloy;User Id=administrador;Password=2022/M4n4t334zur3";          
            //string user = "desarrollocc@manateeingenieria.com";
            //string token = "ATATT3xFfGF0ZRHIEZTEJVRnhNKviH0CGed6QXqCDMj5bCmKSEbO00UUjHUb3yDcaA4YD1SHohyDr4qnwRx2x4Tu_S_QW_xlGIcIUDvL7CFKEg47_Jcy4Dmq6YzO0dvqB3qeT-EVWfwJ2jJ-9vEUfsqXavD0IIGA7DAZHGCtIWhxgwKIbAWsmeA=038B810D";


            dbConnector = new DbConnector(connectionString);
        }

        public IActionResult GetEstacionesConComponentes()
        {
            try
            {
                var estaciones = dbConnector.GetEstacionesV();

                foreach (var estacion in estaciones)
                {
                    estacion.Componentes = dbConnector.GetPuertasByEstacionId(estacion.idEstacion) ?? new List<ComponenteHV>();
                }

                return Json(estaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult GetEstaciones()
        {
            var estaciones = dbConnector.GetEstacionesV();
            return Json(estaciones);
        }

        public IActionResult GetPuertasByEstacionId(int estacionId)
        {
            try
            {
                var componentes = dbConnector.GetPuertasByEstacionId(estacionId);
                if (componentes == null || componentes.Count == 0)
                {
                    return NotFound($"No se encontraron componentes para la estación con Id '{estacionId}'");
                }
                return Json(componentes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
