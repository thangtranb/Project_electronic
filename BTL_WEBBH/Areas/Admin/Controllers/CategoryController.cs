using BTL_WEBBH.Models;
using BTL_WEBBH.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BTL_WEBBH.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        
        private readonly DataContext _dataContext;

        public CategoryController(DataContext dataContext) {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Categories.OrderBy(c => c.CategoryId).ToListAsync());
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
 
            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                var name = await _dataContext.Categories.FirstOrDefaultAsync(p => p.CategoryName == category.CategoryName);
                if (name != null)
                {
                    ModelState.AddModelError("", "Product already in database");
                    return View(category);
                }

                _dataContext.Add(category);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add Category success!";
                return RedirectToAction("Index");
            }
            else
            {
                
                return View(category);
            }


        }

        public async Task<IActionResult> Delete(int Id)
        {

            Category category = await _dataContext.Categories.FindAsync(Id);
            if (category == null)
            {
                return NotFound(); //Handle product not found
            }

            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete Category success!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int Id)
        {
            Category category = await _dataContext.Categories.FindAsync(Id);
           

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id,Category category)
        {
            
            if (ModelState.IsValid)
            { 

                    _dataContext.Update(category);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Update category success!";
                    return RedirectToAction("Index");
            }

                return View(category);
            


        }
    }
}
