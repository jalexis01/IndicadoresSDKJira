using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTT.Infrastructure.Models;
using System;


namespace MQTT.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurar el contexto de base de datos
            services.AddDbContext<EYSIntegrationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityContextConnection")));

            services.AddControllersWithViews();
            services.AddRazorPages();

            // Configurar la expiración de la sesión por inactividad
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(720);  // Expiración de sesión después de 30 minutos de inactividad
                options.SlidingExpiration = true;  // Renueva la sesión si el usuario sigue activo
                options.LoginPath = "/Identity/Account/Login";  // Ruta para redirigir al login
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            // Configurar Serialización para JSON
            services.AddControllersWithViews().AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Middleware para capturar los logs y detectar expiración de sesión
            app.Use(async (context, next) =>
            {
                if (context.User.Identity.IsAuthenticated == false && context.Request.Path != "/Identity/Account/Login")
                {
                    var userEmail = context.User.Identity.Name;

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        // Registrar el log de la sesión expirada
                        var logAction = new LogActions
                        {
                            Usuario = userEmail,
                            Accion = "Sesión expirada",
                            FechaAccion = DateTime.UtcNow
                        };

                        // Guardar en base de datos
                        var dbContext = context.RequestServices.GetService<EYSIntegrationContext>();
                        dbContext.LogActions.Add(logAction);
                        await dbContext.SaveChangesAsync();
                    }
                }

                // Continuar con el siguiente middleware
                await next();
            });

            // Registrar cada solicitud
            app.Use(async (context, next) =>
            {
                logger.LogInformation($"Request {context.Request.Method} {context.Request.Path}");
                await next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Tickets}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
