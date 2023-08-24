using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace MQTT.Web.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
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
            */return View();
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
                    DateTime endDateTime = DateTime.Parse(endDate).AddDays(1); //agrega 1 día y resta 1 segundo para obtener el final del día

                    formattedStartDate = startDateTime.ToString("yyyy-MM-dd");
                    formattedEndDate = endDateTime.ToString("yyyy-MM-dd");
                }
                else
                {
                    formattedStartDate = startDate;
                    formattedEndDate = endDate;
                }

                JiraAccess jiraAccess = new JiraAccess();
                max = 0;
                return jiraAccess.GetTikets(start, max, formattedStartDate, formattedEndDate, componente);
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
                JiraAccess jira = new JiraAccess();
                IssueJira ticket = jira.getIssueJira(idTicket);
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
                JiraAccess jira = new JiraAccess();
                List<byte[]> images = jira.GetAttachmentImages(idTicket);
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
                JiraAccess jira = new JiraAccess();
                var cantidadImagenes = 0;
                List<byte[]> images = jira.GetAttachmentImages(idTicket);
                Console.WriteLine("la cantidad de imágenes del " + idTicket + " es : " + images.Count);
                cantidadImagenes = images.Count;
                return Json(cantidadImagenes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //public IActionResult getContadorImagenes(string idTicket)
        //{
        //    try
        //    {
        //        JiraAccess jira = new JiraAccess();
        //        var cantidadImagenes = 0;
        //        var cantidadVideos = 0;
        //        Tuple<List<byte[]>, List<byte[]>> adjuntos = jira.GetAttachmentAdjuntos(idTicket);
        //        Console.WriteLine("la cantidad de imágenes del " + idTicket + " son : " + adjuntos.Item1.Count);
        //        Console.WriteLine("la cantidad de Videos del " + idTicket + " son : " + adjuntos.Item2.Count);
        //        cantidadImagenes = adjuntos.Item1.Count;
        //        cantidadVideos = adjuntos.Item2.Count;
                
        //        var cantAdjuntos = new
        //        {
        //            CantidadImagenes = cantidadImagenes,
        //            CantidadVideos = cantidadVideos
        //        };
        //        return Json(cantAdjuntos);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        public IActionResult getContadorVideos(string idTicket)
        {
            try
            {
                JiraAccess jira = new JiraAccess();
                var cantidadVideos = 0;
                List<byte[]> videos = jira.GetAttachmentVideos(idTicket);
                Console.WriteLine("la cantidad de videos del " + idTicket + " es : " + videos.Count);
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
                JiraAccess jira = new JiraAccess();
                List<byte[]> videos = jira.GetAttachmentVideos(idTicket);

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