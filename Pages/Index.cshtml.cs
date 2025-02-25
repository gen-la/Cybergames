using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cybergames.Models;
using Cybergames.Data;
using Microsoft.EntityFrameworkCore;

namespace Cybergames.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly CartService _cartService; // Inject CartService
        private readonly ApplicationDbContext _context; // Inject ApplicationDbContext

        // List of games
        public List<Game> Games { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, CartService cartService, ApplicationDbContext context)
        {
            _logger = logger;
            _cartService = cartService; // Inject CartService
            _context = context; // Inject ApplicationDbContext
        }

        public async Task OnGetAsync()
        {
            // Fetch games from the database
            Games = await _context.Games.ToListAsync();

            var cart = _cartService.GetCart();
            ViewData["CartItemCount"] = cart.Items.Sum(item => item.Quantity);
        }

        // Handle adding a game to the cart
        public IActionResult OnPostAddToCart(int id, string title, decimal price, int quantity)
        {
            var cart = _cartService.GetCart();
            cart.AddItem(new CartItem
            {
                Id = id,
                Name = title,
                Price = price,
                Quantity = quantity
            });
            _cartService.SaveCart(cart);

            return RedirectToPage(); // Refresh the page
        }
    }
}