﻿using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


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
             */
            return View();
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

                //if (images.Count > 0)
                //{
                //    byte[] imageData = images[0]; // Assuming you want to return the first image
                //    string base64Image = Convert.ToBase64String(imageData);
                //    return Ok(base64Image);
                //}

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
        public IActionResult GetVideoTicket(string idTicket)
        {
            try
            {
                JiraAccess jira = new JiraAccess();
                List<byte[]> videos = jira.GetAttachmentVideos(idTicket); // Modifica este método para obtener los bytes de video

                if (videos.Count > 0)
                {
                    List<string> base64Videos = new List<string>();

                    foreach (byte[] videoData in videos)
                    {
                        // Convierte los bytes de video a una cadena base64
                        string base64Video = Convert.ToBase64String(videoData);
                        base64Videos.Add(base64Video);
                    }

                    return Ok(base64Videos);
                }
                else
                {
                    return NotFound(); // o devuelve una respuesta adecuada cuando no se encuentren videos
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // o maneja la excepción de manera apropiada
            }
        }


    }

}