using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eshop.Data;
using eshop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace eshop.Controllers
{
    [Authorize] // ✅ Samo prijavljeni korisnici mogu vidjeti košaricu i koristiti je
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Prikaz košarice
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.CartItems.Include(c => c.Product).ToListAsync();
            return View(cartItems);
        }

        // ✅ Dodavanje proizvoda u košaricu
        [HttpGet]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return RedirectToAction("Index", "Product");

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    Quantity = 1
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ✅ Povećavanje količine proizvoda u košarici
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                cartItem.Quantity++;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // ✅ Smanjivanje količine proizvoda (ako je 1, briše se iz košarice)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    _context.CartItems.Remove(cartItem);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // ✅ Brisanje proizvoda iz košarice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // ✅ Dovršavanje kupnje (prazni košaricu)
        public async Task<IActionResult> Checkout()
        {
            var cartItems = await _context.CartItems.Include(c => c.Product).ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Error"] = "Košarica je prazna! Dodajte proizvode prije dovršavanja kupnje.";
                return RedirectToAction("Index");
            }

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Kupnja uspješno dovršena! Hvala na kupnji.";
            return RedirectToAction("Confirmation");
        }

        // ✅ Stranica potvrde kupnje
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
