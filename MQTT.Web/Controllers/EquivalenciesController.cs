using Microsoft.AspNetCore.Mvc;

namespace MQTT.Web.Controllers
{
    public class EquivalenciesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
