using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cybergames.Models;
using Cybergames.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Cybergames.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger; // Logger för att logga händelser
        private readonly CartService _cartService; // Tjänst för att hantera kundvagnen
        private readonly ApplicationDbContext _context; // Databasens kontext
        private readonly SignInManager<ApplicationUser> _signInManager; // Hanterar användarinloggning

        public List<Game> Games { get; set; } = new(); // Lista för att hålla de spel som hämtas från databasen
        public int CurrentPage { get; set; } = 1; // Den aktuella sidan för sidnumrering
        public int TotalPages { get; set; } // Totalt antal sidor
        private const int PageSize = 12; // Antal spel per sida

        // Konstruktor för att injicera tjänster och databas
        public IndexModel(
            ILogger<IndexModel> logger, 
            CartService cartService, 
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _cartService = cartService;
            _context = context;
            _signInManager = signInManager;
        }

        // Hämtar spel från databasen baserat på sidnummer
        public async Task OnGetAsync(int pageIndex = 1)
        {
            CurrentPage = pageIndex;

            // Beräknar totalt antal spel och totalt antal sidor
            int totalGames = await _context.Games.CountAsync();
            TotalPages = (int)Math.Ceiling(totalGames / (double)PageSize);

            // Hämtar spelen för den aktuella sidan med sidindelning
            Games = await _context.Games
                .Skip((pageIndex - 1) * PageSize) // Hoppa över spelen på tidigare sidor
                .Take(PageSize) // Ta endast de spel som ska visas på den aktuella sidan
                .ToListAsync();

            // Om användaren är inloggad, uppdatera kundvagnens antal varor
            if (_signInManager.IsSignedIn(User))
            {
                var cart = _cartService.GetCart();
                ViewData["CartItemCount"] = cart.Items.Sum(item => item.Quantity);
            }
            else
            {
                ViewData["CartItemCount"] = 0; // Om användaren inte är inloggad, sätt antal varor till 0
            }
        }

        // Funktion för att lägga till ett spel i kundvagnen
        [Authorize] // Endast inloggade användare kan lägga till spel i kundvagnen
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

            _cartService.SaveCart(cart); // Spara kundvagnen

            // Återvänd till samma sida utan att ändra på sidnummer
            return RedirectToPage(new { pageIndex = CurrentPage });
        }
    }
}
