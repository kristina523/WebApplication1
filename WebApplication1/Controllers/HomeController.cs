using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClothingStoreeContext _context;

        public HomeController(ClothingStoreeContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            var catolog = _context.Catalogs.ToList();
            return View(catolog);
        }
        public IActionResult CreateCatalog()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCatalog(Catalog catolog)
        {
            _context.Catalogs.Add(catolog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DetailsCatalog(int? id)
        {
            if (id != null)
            {
                Catalog catolog = await _context.Catalogs.FirstOrDefaultAsync(p => p.IdProduct == id);
                if (catolog != null)
                    return View(catolog);
            }
            return NotFound();
        }
        public async Task<IActionResult> EditCatalog(int? id)
        {
            if (id != null)
            {
                Catalog catolog = await _context.Catalogs.FirstOrDefaultAsync(p => p.IdProduct == id);
                if (catolog != null)
                    return View(catolog);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditCatalog(Catalog catolog)
        {
            _context.Catalogs.Update(catolog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Catalog catolog = await _context.Catalogs.FirstOrDefaultAsync(p => p.IdProduct == id);
                if (catolog != null)
                    return View(catolog);
            }
            return NotFound();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var catolog = await _context.Catalogs.FindAsync(id);
            if (catolog == null)
            {
                return NotFound();
            }

            _context.Catalogs.Remove(catolog);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Cart()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Пользователь не найден.");
            }

            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            decimal totalSum = cartItems.Sum(ci => ci.Price);

            ViewBag.TotalSum = totalSum;

            return View(cartItems);
        }
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Пользователь не аутентифицирован.");
            }

            var product = await _context.Catalogs.FindAsync(productId);
            if (product == null)
            {
                return NotFound("Продукт не найден.");
            }

            quantity = quantity > 0 ? quantity : 1;

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

            if (cartItem == null)
            {
                cartItem = new CartItems
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = (decimal)(product.Price * quantity),
                   UserId = userId
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                cartItem.Price = (decimal)(cartItem.Quantity * product.Price);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Cart");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCart(int cartItemId, int quantity)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Пользователь не аутентифицирован.");
            }

            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId && ci.UserId == userId);

            if (cartItem == null)
            {
                return NotFound("Товар не найден в корзине.");
            }

            if (cartItem.Product == null)
            {
                return NotFound("Продукт не найден.");
            }

            cartItem.Quantity = quantity;
            cartItem.Price = (decimal)(cartItem.Quantity * cartItem.Product.Price);
            await _context.SaveChangesAsync();

            return RedirectToAction("Cart");
        }
        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var products = await _context.Catalogs
                .Where(p => p.Name.Contains(searchTerm))
                .ToListAsync();

            return View("Index", products);
        }
    }
}