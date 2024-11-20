using BTL_WEBBH.Models;
using BTL_WEBBH.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Session09.Models;
using System.Drawing.Drawing2D;

namespace BTL_WEBBH.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _dataContext;
        public UserController(DataContext dataContext)
        {

        _dataContext = dataContext; 
        }
    
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            var name = await _dataContext.Users.FirstOrDefaultAsync(p => p.Username == user.Username);
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng đã tồn tại chưa
                if (name != null)
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View();
                }

                // Mã hóa mật khẩu
                user.Password = Cipher.GenerateMD5(user.Password);

                // Lưu vào CSDL
                _dataContext.Users.Add(user);
               await _dataContext.SaveChangesAsync();
                TempData["success"] = "Register success!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Model fail";
                return View(user);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            var passmd5 = Cipher.GenerateMD5(password);
            if (user == null)
            {
                TempData["error"] = "Login failed";
                return View("Login");
            }
            else
            {
                if (!user.Password.Equals(passmd5))
                {
                    TempData["error"] = "Login failed";
                    return View("Login");
                }
                else
                {
                    HttpContext.Session.SetString("fullname", user.FullName);
                    HttpContext.Session.SetString("username", user.Username);
                    HttpContext.Session.SetString("picture", "admin.jpg");
                }
            }
            TempData["success"] = "Login success!";
            return RedirectToAction("Index","Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}
