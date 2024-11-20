using BTL_WEBBH.Models.Repository;
using BTL_WEBBH.Models.Repository.Md5;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Session09.Models;
using System.Security.Claims;

namespace BTL_WEBBH.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        public AccountController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, string? requestUrl)
        {
            string passmd5 = PasswordHelper.HashPassword(password);
            var account = _dataContext.Accounts.SingleOrDefault(x => x.Username == username && x.Password == passmd5 && x.IsAdmin);
            if (account == null)
            {
                ViewBag.error = "<div class='alert alert-danger'>Tên đăng nhập hoặc mật khẩu sai, hoặc bạn không có quyền</div>";
                return View();
            }
            else
            {
                var identity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim("accountid",account.AccountId),
                        new Claim("fullname",account.Fullname),
                        new Claim("picture",account.Picture),
                    }, "bkapschema"
                    );
                var pricipal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync("bkapschema", pricipal);
                if (requestUrl != null)
                    return Redirect(requestUrl);
                else
                    return RedirectToAction("Index","Product");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("bkapschema");
            return View("Login");
        }
    }
}
