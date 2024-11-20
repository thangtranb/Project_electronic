using BTL_WEBBH.Models;
using BTL_WEBBH.Models.Repository;
using BTL_WEBBH.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BTL_WEBBH.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            List<CartItem> cartItems = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItemView cartVm = new ()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price),
            };
            return View(cartVm);
        }

        public async Task<IActionResult> Add(string Id)
        {
            Product product = await _dataContext.Products.FindAsync(Id);
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItem cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItems == null)
            {
                cart.Add(new CartItem(product));
            }
            else {
                cartItems.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);
            TempData["success"] = "Add product to cart success!";
            return Redirect(Request.Headers["Referer"].ToString());
        }

       
        public async Task<IActionResult> Decrease(string Id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItem.Quantity > 1) {
                --cartItem.Quantity;
            } else
            {
                cart.RemoveAll(p=>p.ProductId == Id);
            }

            if (cart.Count == 0) {
                HttpContext.Session.Remove("Cart");
            } else
            {
                HttpContext.Session.SetJson("Cart", cart);
                
            }
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(string Id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
            if (cartItem.Quantity >= 1)
            {
                ++cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == Id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);

            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(string Id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(p=>p.ProductId == Id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["success"] = "Delete product in cart success!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            
            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Clear cart success!";
            return RedirectToAction("Index");
        }
    }
}
