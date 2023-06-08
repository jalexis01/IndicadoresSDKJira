using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MQTT.Infrastructure.DAL;
using MQTT.Web.Models;
using System;
using System.Diagnostics;

namespace MQTT.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString = AppSettings.Instance.Configuration["connectionString"].ToString();
        private General _objGeneral;
        private General DBAccess { get => _objGeneral; set => _objGeneral = value; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            DBAccess = new General(_connectionString);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetEquivalenceTypes()
        {
            try
            {
                var json = Json(
                    new
                    {
                        data = EquivalencesDAL.GetEquivalenceTypes(DBAccess)
                    });

                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
