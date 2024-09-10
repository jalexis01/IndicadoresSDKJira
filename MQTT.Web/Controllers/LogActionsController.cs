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

                // Pasar los datos a la vista
                return View(logActions);
            }
            catch (Exception ex)
            {
                // Captura el error y devuélvelo a la vista para que puedas verlo
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetLogActions(DateTime? startDate, DateTime? endDate, string searchUser)
        {
            var query = _context.LogActions.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(x => x.FechaAccion >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(x => x.FechaAccion <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(searchUser))
            {
                query = query.Where(x => x.Usuario.Contains(searchUser));
            }

            var logActions = await query.ToListAsync();

            // Devuelve la vista parcial o el HTML para el Grid
            return PartialView("_LogActionsGrid", logActions); // Puedes cambiar "_LogActionsGrid" por una vista parcial
        }
    }
}
