using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MQTT.Web.Controllers
{
    [Authorize]
    public class SerialesController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}