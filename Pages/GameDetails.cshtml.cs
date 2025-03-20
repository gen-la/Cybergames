using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cybergames.Models;
using Cybergames.Data;
using Microsoft.EntityFrameworkCore;

namespace Cybergames.Pages
{
    public class GameDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context; // Databasens kontext
        private readonly CartService _cartService; // Tjänst för att hantera kundvagnen

        public Game Game { get; set; } // Spelet som visas på detaljsidan

        // Konstruktor för att injicera ApplicationDbContext och CartService
        public GameDetailsModel(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService; // Injicerar CartService
        }

        // Hämtar spelets detaljer baserat på ID
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Returnera NotFound om inget ID anges
            }

            // Hämtar spelet med det angivna ID från databasen
            Game = await _context.Games.FirstOrDefaultAsync(g => g.ID == id);

            if (Game == null)
            {
                return NotFound(); // Returnera NotFound om spelet inte finns i databasen
            }

            // 🛒 Uppdatera CartItemCount för badgen
            var cart = _cartService.GetCart(); 
            ViewData["CartItemCount"] = cart.Items.Sum(item => item.Quantity); // Skickar antalet varor i kundvagnen till vyn

            return Page(); // Rendera sidan med spelets detaljer
        }

        // 🛒 Lägg till spelet i kundvagnen och uppdatera CartItemCount
        public IActionResult OnPostAddToCart(int id, string title, decimal price, int quantity)
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

            // 🛒 Uppdatera CartItemCount efter att ett spel lagts till
            ViewData["CartItemCount"] = cart.Items.Sum(item => item.Quantity); // Skickar antalet varor i kundvagnen till vyn

            return RedirectToPage(new { id = id }); // Omdirigera tillbaka till samma sida med spelets detaljer
        }
    }
}
