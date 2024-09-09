﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MQTT.Web.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MQTT.Infrastructure.Models;

namespace MQTT.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly EYSIntegrationContext _context; // Usar EYSIntegrationContext

        public LoginModel(SignInManager<ApplicationUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            EYSIntegrationContext context)  // Inyectar el EYSIntegrationContext
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;  // Asignar el contexto a la variable
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //returnUrl = Url.Content("~/Tickets/Index");
            //returnUrl = User.IsInRole("Administrador") ? Url.Content("~/Tickets/Index") : Url.Content("~/Messages/Index");
            //returnUrl = Input.Email.Equals("admin@admin.com", StringComparison.OrdinalIgnoreCase) ? Url.Content("~/Tickets/Index") : Url.Content("~/Messages/Index");
            returnUrl = Input.Email.Equals("admin@admin.com", StringComparison.OrdinalIgnoreCase) ? Url.Content("~/Messages/Index") : Url.Content("~/Tickets/Index");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);                

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    // Crear un nuevo registro de la acción del usuario
                    var logAction = new LogActions
                    {
                        Usuario = Input.Email,
                        Accion = "Inicio de sesión",
                        FechaAccion = DateTime.UtcNow
                    };

                    // Guardar el registro en la base de datos
                    _context.LogActions.Add(logAction);
                    await _context.SaveChangesAsync();

                    return LocalRedirect(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Usuario bloqueado.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario incorrecto.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
