using BTL_WEBBH.Models;
using BTL_WEBBH.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBBH.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {

            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public async Task<IActionResult> Index()

        {
           /* var products = from p in _dataContext.Products select p;
            if(!string.IsNullOrEmpty(name))
            {
                products = products.Where(x=>x.ProName.Contains(name));
            }*/
            return View(await _dataContext.Products.OrderBy(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "CategoryId", "CategoryName");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "BrandId", "BrandName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "BrandId", "BrandName", product.BrandId);


            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                var name = await _dataContext.Products.FirstOrDefaultAsync(p => p.ProName == product.ProName);
                if (name != null)
                {
                    ModelState.AddModelError("", "Product already in database");
                    return View(product);
                }

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/product");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;

                }

                _dataContext.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add product success!";
                return RedirectToAction("Index");
            }
            else
            {
                /*TempData["error"] = "Models error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);*/
                return View(product);
            }


        }

        public async Task<IActionResult> Update(string Id)
        {
            Product product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "CategoryId", "CategoryName");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "BrandId", "BrandName");

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string Id, Product product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "BrandId", "BrandName", product.BrandId);

            var existed_product = _dataContext.Products.Find(product.Id); //tìm sp theo id

            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                /*var name = await _dataContext.Products.FirstOrDefaultAsync(p => p.ProName == product.ProName);
                if (name != null)
                {
                    ModelState.AddModelError("", "Product already in database");
                    return View(product);
                }*/

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/product");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    //xóa ảnh cũ
                    string oldFilePath = Path.Combine(uploadsDir, existed_product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while deleting the product image");
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Image = imageName;

                    
                }

                //cập nhật các giá trị khác sp
                existed_product.ProName = product.ProName;
                existed_product.Description = product.Description;
                existed_product.Price = product.Price;
                existed_product.OldPrice = product.OldPrice;
                existed_product.BrandId = product.BrandId;
                existed_product.CategoryId = product.CategoryId;

                _dataContext.Update(existed_product);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update product success!";
                return RedirectToAction("Index");
            }
            else
            {
                /*TempData["error"] = "Models error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);*/
                return View(product);
            }


        }

        public async Task<IActionResult> Delete(string Id)
        {

            Product product = await _dataContext.Products.FindAsync(Id);
            if (product == null) {
                return NotFound(); //Handle product not found
            }

            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/product");
            string oldFileImg = Path.Combine(uploadsDir, product.Image);
            try
            {


                if (System.IO.File.Exists(oldFileImg))
                {
                    System.IO.File.Delete(oldFileImg);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while deleting the product image");
            }
            
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete product success!";
            return RedirectToAction("Index");
        }

        public IActionResult Search(string proName)
        {
            var products = from p in _dataContext.Products select p;
            if (!string.IsNullOrEmpty(proName))
            {
                products = products.Where(x => x.ProName.Contains(proName));
            }
            return View(products);
        }
    }
}
