using BTL_WEBBH.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBBH.Controllers
{
    public class ShopController : Controller
    {
        private readonly DataContext _dataContext;
        public ShopController(DataContext dataContext) {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Products.OrderBy(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }
    }
}
