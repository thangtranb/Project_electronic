using BTL_WEBBH.Models;
using BTL_WEBBH.Models.Repository;
using BTL_WEBBH.Models.Repository.Md5;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Session09.Models;

namespace BTL_WEBBH.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(DataContext dataContext, IWebHostEnvironment webHostEnvironment) {
            
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Users.OrderBy(u => u.Id).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {

            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                var userName = await _dataContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
                if (userName != null)
                {
                    ModelState.AddModelError("", "User name already in database");
                    return View(user);
                }

                if (user.AvatarUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/avatar");
                    string imageName = Guid.NewGuid().ToString() + "_" + user.AvatarUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await user.AvatarUpload.CopyToAsync(fs);
                    fs.Close();
                    user.Avatar = imageName;

                }
                // Mã hóa mật khẩu
                user.Password = Cipher.GenerateMD5(user.Password);

                _dataContext.Add(user);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Add user success!";
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
                return View(user);
            }


        }

        public async Task<IActionResult> Update(int Id)
        {
            User product = await _dataContext.Users.FindAsync(Id);

            return View(product);
        }


        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, User user, string passWord)
        {
            

            var existed_user = _dataContext.Users.Find(user.Id); //tìm sp theo id

            var existed_pass = _dataContext.Users.FirstOrDefault(x=> x.Password == user.Password); //tìm sp theo id

            if (ModelState.IsValid)
            {
                //code thêm dữ liệu
                *//*var name = await _dataContext.Products.FirstOrDefaultAsync(p => p.ProName == product.ProName);
                if (name != null)
                {
                    ModelState.AddModelError("", "Product already in database");
                    return View(product);
                }*//*

                if (user.AvatarUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/avatar");
                    string imageName = Guid.NewGuid().ToString() + "_" + user.AvatarUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    //xóa ảnh cũ
                    string oldFilePath = Path.Combine(uploadsDir, existed_user.Avatar);
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
                    await user.AvatarUpload.CopyToAsync(fs);
                    fs.Close();
                    existed_user.Avatar = imageName;


                }


                

                //cập nhật các giá trị khác sp
                existed_user.FullName = user.FullName;
                existed_user.Username = user.Username;
                existed_user.Password = user.Password;
                existed_user.Phone = user.Phone;
                existed_user.Address = user.Address;
                existed_user.Email = user.Email;

                // Nếu người dùng muốn cập nhật mật khẩu
                if (existed_pass != null)
                {
                    // Mã hóa mật khẩu
                    user.Password = PasswordHelper.HashPassword(user.Password);
                }

                _dataContext.Update(existed_user);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Update product success!";
                return RedirectToAction("Index");
            }
            else
            {
                *//*TempData["error"] = "Models error";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);*//*
                return View(user);
            }


        }*/

        public async Task<IActionResult> Delete(int Id)
        {

            User user = await _dataContext.Users.FindAsync(Id);
            if (user == null)
            {
                return NotFound(); //Handle product not found
            }

            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/avatar");
            string oldFileImg = Path.Combine(uploadsDir, user.Avatar);
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

            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Delete product success!";
            return RedirectToAction("Index");
        }
    }
}
