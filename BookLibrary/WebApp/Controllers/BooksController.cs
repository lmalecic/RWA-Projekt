using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
