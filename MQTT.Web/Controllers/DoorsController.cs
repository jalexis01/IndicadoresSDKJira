using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MQTT.Web.Controllers
{
    public class DoorsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
    }
}
