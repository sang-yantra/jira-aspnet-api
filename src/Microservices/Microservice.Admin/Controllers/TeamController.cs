using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
