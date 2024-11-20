using Microsoft.AspNetCore.Mvc;

namespace BTL_WEBBH.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
