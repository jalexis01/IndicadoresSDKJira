using Microsoft.AspNetCore.Mvc;
using MQTT.Infrastructure.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace MQTT.Web.Controllers
{
    public class LogActionsController : Controller
    {
        private readonly EYSIntegrationContext _context;

        public LogActionsController(EYSIntegrationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Cargar los datos de la base de datos
                var logActions = await _context.LogActions.ToListAsync();
                
                return View(logActions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLogActions(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var query = _context.LogActions.AsQueryable();

                if (startDate.HasValue)
                {
                    query = query.Where(x => x.FechaAccion >= startDate.Value);
                }

                if (endDate.HasValue)
                {
                    endDate = endDate.Value.Date.AddDays(1).AddTicks(-1); // Esto establece la hora en 23:59:59.9999999
                    query = query.Where(x => x.FechaAccion <= endDate.Value);
                }

                var logActions = await query.Select(log => new
                {
                    Usuario = log.Usuario,
                    Accion = log.Accion,
                    //FechaAccion = log.FechaAccion.ToString("yyyy-MM-dd HH:mm:ss")
                    FechaAccion = log.FechaAccion.AddHours(-5).ToString("yyyy-MM-dd HH:mm:ss")
                }).ToListAsync();

                return Json(logActions); // Devolvemos los resultados como JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }
    }
}
