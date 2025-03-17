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
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;

        public CategoryModel(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public string CategoryName { get; set; }
        public IList<Game> Games { get; set; } = new List<Game>();

        public async Task<IActionResult> OnGetAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToPage("/Index");
            }

            CategoryName = category;

            Games = await _context.Games
                .Where(g => g.Category == category)
                .ToListAsync();

            return Page();
        }

        [Authorize]
        public IActionResult OnPostAddToCart(int id, string title, decimal price, int quantity, string category)
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

            // Redirect back to the same category page
            return RedirectToPage(new { category = category });
        }
    }
}