using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eshop.Data;
using eshop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace eshop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🛒 Prikaz košarice
        public async Task<IActionResult> Index()
        {
            var cartItems = await _context.CartItems
                .Include(c => c.Product) // ✅ Učitava povezani proizvod iz baze
                .ToListAsync();

            return View(cartItems);
        }

        // ✅ Metoda za dodavanje proizvoda u košaricu
        [HttpGet]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null) // Ako proizvod ne postoji, vrati se na listu proizvoda
            {
                return RedirectToAction("Index", "Product");
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity++; // Ako proizvod već postoji u košarici, povećaj količinu
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = product.Id,
                    Product = product, // Postavlja povezani proizvod
                    Quantity = 1
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // ✅ Metoda za uklanjanje proizvoda iz košarice
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
