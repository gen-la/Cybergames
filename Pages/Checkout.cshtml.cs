using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Cybergames.Models;

public class CheckoutModel : PageModel
{
    private readonly CartService _cartService; // Tjänst för att hantera kundvagnen
    private readonly GameService _gameService; // Tjänst för att hantera spelköp
    private readonly UserManager<ApplicationUser> _userManager; // Hanterar användarinformation

    public ShoppingCart Cart { get; set; } // Kundvagnsobjektet

    [BindProperty]
    public bool ShowConfirmationMessage { get; set; } = false; // Indikerar om bekräftelsemeddelandet ska visas

    // Konstruktor som injicerar nödvändiga tjänster
    public CheckoutModel(
        CartService cartService,
        GameService gameService,
        UserManager<ApplicationUser> userManager)
    {
        _cartService = cartService;
        _gameService = gameService;
        _userManager = userManager;
    }

    // Körs när sidan laddas
    public void OnGet()
    {
        Cart = _cartService.GetCart(); // Hämtar kundvagnen
        ViewData["CartItemCount"] = Cart.Items.Sum(item => item.Quantity); // Uppdaterar antal spel i vyn
    }

    // Hanterar köp av varor i kundvagnen
    public async Task<IActionResult> OnPostAsync()
    {
        // Hämtar den inloggade användaren
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToPage("/Account/Login"); // Omdirigerar till inloggningssidan om användaren inte är inloggad
        }

        // Hämtar kundvagnen
        var cart = _cartService.GetCart();

        // Lägger till varje spel i användarens bibliotek
        foreach (var item in cart.Items)
        {
            await _gameService.AddGameToUserAsync(item.Id, User); // Lägger till spelet för användaren
        }

        // Tömmer kundvagnen efter genomfört köp
        cart.Clear(); 
        _cartService.SaveCart(cart); // Sparar den uppdaterade (tömda) kundvagnen

        // Visar bekräftelsemeddelandet
        ShowConfirmationMessage = true;

        // Uppdaterar kundvagnen för att visa att den är tom
        Cart = _cartService.GetCart();
        ViewData["CartItemCount"] = Cart.Items.Sum(item => item.Quantity);

        // Returnerar samma sida för att visa bekräftelsen
        return Page();
    }
}
