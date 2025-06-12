using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize] // Kräv inloggning för att komma åt denna sida
public class CartModel : PageModel
{
    private readonly CartService _cartService;

    public ShoppingCart Cart { get; set; }

    // Konstruktor för att injicera CartService
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public CartModel(CartService cartService)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        _cartService = cartService;
    }

    // Hämtar kundvagnen och beräknar totala antalet varor
    public void OnGet()
    {
        Cart = _cartService.GetCart();
        ViewData["CartItemCount"] = Cart.Items.Sum(item => item.Quantity);
    }

#pragma warning disable MVC1001 // Filters cannot be applied to page handler methods
    [Authorize] // Kräv inloggning för att lägga till spel i kundvagnen
#pragma warning restore MVC1001 // Filters cannot be applied to page handler methods
    public IActionResult OnPostAddToCart(int id, string name, decimal price, int quantity)
    {
        var cart = _cartService.GetCart();
        cart.AddItem(new CartItem { Id = id, Name = name, Price = price, Quantity = quantity });
        _cartService.SaveCart(cart);
        return RedirectToPage(); // Omdirigerar tillbaka till samma sida
    }

#pragma warning disable MVC1001 // Filters cannot be applied to page handler methods
    [Authorize] // Kräv inloggning för att ta bort spel från kundvagnen
#pragma warning restore MVC1001 // Filters cannot be applied to page handler methods
    public IActionResult OnPostRemoveFromCart(int itemId)
    {
        var cart = _cartService.GetCart();
        cart.RemoveItem(itemId);
        _cartService.SaveCart(cart);
        return RedirectToPage(); // Omdirigerar tillbaka till samma sida
    }
}