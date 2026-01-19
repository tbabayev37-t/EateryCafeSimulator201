using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EateryCafeSimulator201.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
