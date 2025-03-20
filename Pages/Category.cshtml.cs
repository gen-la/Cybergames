using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cybergames.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cybergames.Data;
using Microsoft.AspNetCore.Authorization;

namespace Cybergames.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly ApplicationDbContext _context; // Databasens kontext
        private readonly CartService _cartService; // Tjänst för att hantera kundvagnen

        // Konstruktor för att injicera databaskontext och kundvagnstjänst
        public CategoryModel(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public string CategoryName { get; set; } // Namn på den valda kategorin
        public IList<Game> Games { get; set; } = new List<Game>(); // Lista över spel i kategorin

        // Hämtar alla spel i en viss kategori
        public async Task<IActionResult> OnGetAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToPage("/Index"); // Om ingen kategori anges, omdirigera till startsidan
            }

            CategoryName = category;

            // Hämta alla spel som matchar den valda kategorin
            Games = await _context.Games
                .Where(g => g.Category == category)
                .ToListAsync();

            return Page(); // Rendera sidan med de hämtade spelen
        }

        [Authorize] // Användaren måste vara inloggad för att lägga till spel i kundvagnen
        public IActionResult OnPostAddToCart(int id, string title, decimal price, int quantity, string category)
        {
            var cart = _cartService.GetCart();

            // Lägg till spelet i kundvagnen
            cart.AddItem(new CartItem
            {
                Id = id,
                Name = title,
                Price = price,
                Quantity = quantity
            });

            _cartService.SaveCart(cart); // Spara den uppdaterade kundvagnen

            // Omdirigera tillbaka till kategorisidan med vald kategori
            return RedirectToPage(new { category = category });
        }
    }
}
