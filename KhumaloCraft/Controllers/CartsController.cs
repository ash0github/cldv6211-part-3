using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KhumaloCraft.Data;
using KhumaloCraft.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace KhumaloCraft.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CartsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Carts
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Cart.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserID == user.Id && !c.OrderConfirmed);

            return View(cart);
        }

        // POST: AddToCart
        [Authorize(Roles = "Consumer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart([FromForm] int ProductId, [FromForm] bool Available, int quantity = 1)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserID == user.Id && !c.OrderConfirmed);

            if (cart == null)
            {
                cart = new Cart { UserID = user.Id, OrderConfirmed = false };
                _context.Cart.Add(cart);
                await _context.SaveChangesAsync();
            }

            if (ProductId == 0)
            {
                return NotFound();
            }

            if (!Available)
            {
                return Content("<script>alert('The product is not available.'); window.location.href = '/ProductInformations/Index';</script>", "text/html");
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == ProductId);

            if (cartItem == null)
            {
                cartItem = new CartItem { ProductID = ProductId, Quantity = quantity };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            TempData["ItemAddedToCart"] = true;
            return RedirectToAction("Index", "ProductInformations");
        }

        // POST: Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Cart.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefaultAsync(c => c.UserID == user.Id && !c.OrderConfirmed);

            if (cart == null || !cart.CartItems.Any())
            {
                return Content("<script>alert('There are no items in the cart'); window.location.href = '/Carts/Index';</script>", "text/html");
            }
            else
            {
                cart.OrderConfirmed = true;
                cart.OrderDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                TempData["Checkout"] = true;

                return RedirectToAction(nameof(OrderHistory));
            }
        }

        // POST: ClearCart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _context.Cart.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserID == user.Id && !c.OrderConfirmed);

            if (cart != null)
            {
                _context.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // GET: OrderHistory
        public async Task<IActionResult> OrderHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _context.Cart.Include(c => c.CartItems).ThenInclude(ci => ci.Product).Where(c => c.UserID == user.Id && c.OrderConfirmed).ToListAsync();

            return View(orders);
        }

        // POST: ClearOrders
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var orders = await _context.Cart.Include(c => c.CartItems).ThenInclude(ci => ci.Product).Where(c => c.UserID == user.Id && c.OrderConfirmed).ToListAsync();

            if (orders != null)
            {
                _context.RemoveRange(orders);
                await _context.SaveChangesAsync();
            }
            return View(orders);
        }
    }
}
