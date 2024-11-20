using BTL_WEBBH.Models;
using BTL_WEBBH.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BTL_WEBBH.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(DataContext dataContext,ILogger<HomeController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            
            return View(await _dataContext.Products.Include(p => p.Category).Include(p => p.Brand).ToListAsync());

        }

        public IActionResult About() {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public async Task<IActionResult> Detail(string Id)
        {
            if(Id == null)
            {
                
                return RedirectToAction("Index");
            }

            var productsById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
            return View(productsById);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("NotFound");
            }
            else {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            } 
            
        }
    }
}
