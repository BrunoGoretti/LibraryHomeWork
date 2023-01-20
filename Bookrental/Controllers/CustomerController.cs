using Microsoft.AspNetCore.Mvc;

namespace Bookrental.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
