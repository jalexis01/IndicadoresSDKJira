using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MQTT.Web.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly IConfiguration _configuration;
        private JiraAccess jiraAccess;
        private DbConnector dbConnector;
        public TicketsController(IConfiguration configuration)
        {
            _configuration = configuration;
            init();
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


            // Aquí puedes usar las variables como desees
            jiraAccess = new JiraAccess
            (
                url,
                user,
                token,
                connectionString
            );
            dbConnector = new DbConnector(connectionString);
        }
        public IActionResult Index(int max, string componente)
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
            /* string startDate = startDateTime.ToString("yyyy-MM-dd");
             string endDate = currentDateTime.ToString("yyyy-MM-dd");
             max = 0;
             List<Ticket> tickets = getTickets(startDate, endDate, max, componente);
             */
            // Obtener el valor de EnvironmentType desde appsettings
            ViewBag.EnvironmentType = _configuration["EnvironmentType"];
            return View();
        }


        int start = 0;

        public List<Ticket> getTickets(string startDate, string endDate, int max, string componente, string tipoMantenimiento, bool cerrados, bool estado)
        {
            try
            {
                string formattedStartDate;
                string formattedEndDate;

                if (startDate != null || endDate != null)
                {
                    //max = 10;
                    DateTime startDateTime = DateTime.Parse(startDate);
                    DateTime endDateTime = DateTime.Parse(endDate);//.AddDays(1); //agrega 1 día y resta 1 segundo para obtener el final del día

                    formattedStartDate = startDateTime.ToString("yyyy-MM-dd 00:00");
                    formattedEndDate = endDateTime.ToString("yyyy-MM-dd 23:59");
                }
                else
                {
                    formattedStartDate = startDate;
                    formattedEndDate = endDate;
                }

                List<Ticket> tickets = new List<Ticket>();
                max = 0;
                if (tipoMantenimiento == "'Mantenimiento Preventivo'")
                {
                    tickets = jiraAccess.GetTiketsMTO(start, max, formattedStartDate, formattedEndDate, componente, "", estado);
                }
                else
                {
                    tickets = jiraAccess.GetTikets(start, max, formattedStartDate, formattedEndDate, componente, tipoMantenimiento, cerrados, estado);
                }

                return tickets;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public IActionResult consultarTicket(string idTicket)
        {
            try
            {

                IssueJira ticket = jiraAccess.getIssueJira(idTicket);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GetImageTicket(string idTicket)
        {
            try
            {

                List<byte[]> images = jiraAccess.GetAttachmentImages(idTicket);
                Console.WriteLine("la cantidad de imagenes del " + idTicket + " son : " + images.Count);
                if (images.Count > 0)
                {
                    List<string> base64Images = new List<string>();

                    foreach (byte[] imageData in images)
                    {
                        string base64Image = Convert.ToBase64String(imageData);
                        base64Images.Add(base64Image);
                    }

                    return Ok(base64Images);
                }
                else
                {
                    return NotFound(); // or return some appropriate response when no images are found
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // or handle the exception in an appropriate way
            }
        }

        public IActionResult getContadorImagenes(string idTicket)
        {
            try
            {

                var cantidadImagenes = 0;
                List<byte[]> images = jiraAccess.GetAttachmentImages(idTicket);
                Console.WriteLine("la cantidad de imágenes del " + idTicket + " es : " + images.Count);
                cantidadImagenes = images.Count;
                return Json(cantidadImagenes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public IActionResult getContadorAdjuntos(string idTicket)
        {
            try
            {

                var cantidadImagenes = 0;
                var cantidadVideos = 0;
                Tuple<List<byte[]>, List<byte[]>> adjuntos = jiraAccess.GetAttachmentAdjuntos(idTicket);
                Console.WriteLine("la cantidad de imágenes del " + idTicket + " son : " + adjuntos.Item1.Count);
                Console.WriteLine("la cantidad de Videos del " + idTicket + " son : " + adjuntos.Item2.Count);
                cantidadImagenes = adjuntos.Item1.Count;
                cantidadVideos = adjuntos.Item2.Count;

                var cantAdjuntos = new
                {
                    CantidadImagenes = cantidadImagenes,
                    CantidadVideos = cantidadVideos
                };
                return Json(cantAdjuntos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public IActionResult getContadorVideos(string idTicket)
        {
            try
            {

                var cantidadVideos = 0;
                List<byte[]> videos = jiraAccess.GetAttachmentVideos(idTicket);
                //Console.WriteLine("la cantidad de videos del " + idTicket + " es : " + videos.Count);
                cantidadVideos = videos.Count;
                return Json(cantidadVideos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        public IActionResult GetVideoTicket(string idTicket)
        {
            try
            {
                List<byte[]> videos = jiraAccess.GetAttachmentVideos(idTicket);

                if (videos.Count > 0)
                {
                    List<string> base64Videos = new List<string>();

                    foreach (byte[] videoData in videos)
                    {
                        string base64Video = Convert.ToBase64String(videoData);
                        base64Videos.Add(base64Video);
                    }

                    return Ok(base64Videos);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }

}