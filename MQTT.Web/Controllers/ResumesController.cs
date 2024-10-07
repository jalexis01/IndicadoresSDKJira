using DashboarJira.Model;
using DashboarJira.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using MQTT.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IO;

namespace MQTT.Web.Controllers
{
    [Authorize]
    public class ResumesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly EYSIntegrationContext _context; // Inyección del contexto
        private JiraAccess jiraAccess;
        private DbConnector dbConnector;
        public ResumesController(IConfiguration configuration, EYSIntegrationContext context)
        {
            _configuration = configuration;
            _context = context;
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
        public async Task<IActionResult> TestDatabase()
        {
            try
            {
                // Realizar una consulta de prueba para verificar la existencia de la tabla
                var logActions = await _context.LogActions.ToListAsync();
                return Json(logActions);
            }
            catch (Exception ex)
            {
                // Registrar el error y devolver una respuesta adecuada
                return StatusCode(500, $"Error al consultar la base de datos: {ex.Message}");
            }
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

            // Obtener el valor de EnvironmentType desde appsettings
            ViewBag.EnvironmentType = _configuration["EnvironmentType"];
            return View();
        }

        int start = 0;

        public List<Ticket> getTickets(string startDate, string endDate, int max, string serial, string tipoMantenimiento, bool cerrados, bool estado)
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
                string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
                List<Ticket> tickets = new List<Ticket>();
                max = 0;
                if (tipoMantenimiento == "'Mantenimiento Preventivo'")
                {
                    tickets = jiraAccess.GetTiketsMTO(start, max, formattedStartDate, formattedEndDate, serial, "", estado);
                }
                else
                {
                    tickets = jiraAccess.GetTikets(start, max, formattedStartDate, formattedEndDate, serial, tipoMantenimiento, cerrados, estado);
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

        public IActionResult GetComponenteHV(string idComponente)
        {
            try
            {
                var componente = dbConnector.GetComponenteHV(idComponente);
                if (componente == null)
                {
                    return NotFound($"Componente con IdComponente '{idComponente}' no encontrado");
                }
                return Json(componente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult DownloadExcelAjax(string idComponente)
        {
            try
            {
                jiraAccess.DownloadExcel(idComponente);

                return Json(new { success = true, message = "Descarga exitosa" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
