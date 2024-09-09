using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MQTT.Infrastructure.Models;
using MQTT.Web.Areas.Identity.Data;
using System.Threading.Tasks;
using MQTT.Infrastructure.Models;
using System;

namespace MQTT.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly EYSIntegrationContext _context;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, EYSIntegrationContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Obtiene el usuario actual
            var userEmail = User.Identity?.Name;

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            // Registrar el cierre de sesión
            if (!string.IsNullOrEmpty(userEmail))
            {
                var logAction = new LogActions
                {
                    Usuario = userEmail,
                    Accion = "Cierre de sesión",
                    FechaAccion = DateTime.UtcNow
                };

                _context.LogActions.Add(logAction);
                await _context.SaveChangesAsync();
            }

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
