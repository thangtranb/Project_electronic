using BTL_WEBBH.Models;
using BTL_WEBBH.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBBH.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class BrandController : Controller
    {

        private readonly DataContext _dataContext;

        public BrandController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderBy(b => b.BrandId).ToListAsync());
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {

            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                var name = await _dataContext.Brands.FirstOrDefaultAsync(p => p.BrandName == brand.BrandName);
                if (name != null)
                {
                    ModelState.AddModelError("", "Brand name already in database");
                    return View(brand);
                }

                _dataContext.Add(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add brand success!";
                return RedirectToAction("Index");
            }
            else
            {

                return View(brand);
            }


        }

        public async Task<IActionResult> Delete(int Id)
        {

            Brand brand = await _dataContext.Brands.FindAsync(Id);
            if (brand == null)
            {
                return NotFound(); //Handle product not found
            }

            _dataContext.Brands.Remove(brand);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete brand success!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int Id)
        {
            Brand brand = await _dataContext.Brands.FindAsync(Id);


            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, Brand brand)
        {

            if (ModelState.IsValid)
            {

                _dataContext.Update(brand);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update brand success!";
                return RedirectToAction("Index");
            }

            return View(brand);



        }
    }
}
