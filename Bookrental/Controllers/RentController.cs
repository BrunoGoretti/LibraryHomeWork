using Microsoft.AspNetCore.Mvc;

namespace Bookrental.Controllers
{
    public class RentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
